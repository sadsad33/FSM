using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace KBH {

    #region Decorator
    public class CheckConditionNode : BTNode {
        readonly Func<bool> condition;
        public CheckConditionNode(string name, Func<bool> condition) : base(name) {
            this.condition = condition;
        }

        public override Status Process() {
            if (condition()) return children[0].Process();
            return Status.Failure;
        }
    }

    public class WaitUntilNode : BTNode {
        readonly Func<bool> condition;
        readonly float maximumWaitTime;
        float elapsedTime;
        public WaitUntilNode(string name, Func<bool> condition, float maximumWaitTime) : base(name) {
            this.condition = condition;
            this.maximumWaitTime = maximumWaitTime;
            elapsedTime = 0;
        }

        public override Status Process() {
            //Debug.Log("잠시 대기");
            if (condition()) {
                elapsedTime = 0f; // 리셋
                return children[0].Process();
            }

            elapsedTime += Time.deltaTime;
            Debug.Log("elapsedTime : " + elapsedTime);
            if (elapsedTime >= maximumWaitTime) {
                elapsedTime = 0f;
                return Status.Failure;
            }

            return Status.Running;
        }
    }

    public class UntilSuccessNode : BTNode {
        public UntilSuccessNode(string name, int priority = 0) : base(name, priority) { }

        // Behaviour 가 성공하면 트리의 상태를 리셋하고 성공 상태 반환
        public override Status Process() {
            //Debug.Log("실행되는 노드 이름 : " + children[0].name);
            if (children[0].Process() == Status.Success) {
                Reset();
                return Status.Success;
            }

            // 성공 상태가 아니라면 실행중 반환
            return Status.Running;
        }
    }

    // 자식노드가 반환한 상태를 반전
    public class InverterNode : BTNode {
        public InverterNode(string name) : base(name) { }

        public override Status Process() {
            switch (children[0].Process()) {
                case Status.Running:
                    return Status.Running;
                case Status.Failure:
                    return Status.Success;
                default:
                    return Status.Failure;
            }
        }
    }
    #endregion

    #region Composite

    public class RandomOnceSelectorNode : BTNode {
        private List<BTNode> shuffledChildren;
        private bool hasShuffled;

        public RandomOnceSelectorNode(string name, int priority = 0) : base(name, priority) { }

        public override Status Process() {
            if (!hasShuffled) {
                shuffledChildren = children.Shuffle().ToList();
                hasShuffled = true;
            }

            //Debug.Log("실행되는 노드 이름 : " + shuffledChildren[0].name);
            foreach (var child in shuffledChildren) {
                var status = child.Process();
                if (status == Status.Running || status == Status.Success)
                    return status;
            }

            Reset();
            return Status.Failure;
        }

        public override void Reset() {
            base.Reset();
            hasShuffled = false;
            shuffledChildren = null;
        }
    }

    public class RandomSelectorNode : PrioritySelectorNode {
        private List<BTNode> shuffledChildren;

        public RandomSelectorNode(string name) : base(name) { }

        // 실행할때마다 무조건 랜덤한 자식을 선택
        public override Status Process() {
            shuffledChildren = children.Shuffle().ToList();
            //Debug.Log(shuffledChildren[0].name);
            Status ret = shuffledChildren[0].Process();
            Reset();
            return ret;
        }

        public override void Reset() {
            base.Reset();
            shuffledChildren = null;
        }
    }

    public class PrioritySelectorNode : SelectorNode {
        List<BTNode> sortedChildren;
        List<BTNode> SortedChildren => sortedChildren ??= SortChildren();
        protected virtual List<BTNode> SortChildren() => children.OrderByDescending(child => child.priority).ToList();

        public PrioritySelectorNode(string name) : base(name) { }

        public override void Reset() {
            base.Reset();
            sortedChildren = null;
        }

        public override Status Process() {
            foreach (var child in SortedChildren) {
                switch (child.Process()) {
                    case Status.Running:
                        return Status.Running;
                    case Status.Success:
                        return Status.Success;
                    default:
                        continue;
                }
            }

            return Status.Failure;
        }
    }

    // Logical OR
    // 자식 노드들중 하나라도 성공 상태를 반환한다면 최종적으로 성공 상태 반환
    public class SelectorNode : BTNode {
        public SelectorNode(string name, int priority = 0) : base(name, priority) { }

        public override Status Process() {
            if (currentChild < children.Count) {
                //Debug.Log("실행되는 노드 이름 : " + children[currentChild].name);
                switch (children[currentChild].Process()) {
                    case Status.Running:
                        return Status.Running;
                    case Status.Success:
                        Reset();
                        return Status.Success;
                    default: // 현재 자식노드가 실패 상태를 반환하면 다음 자식을 판단
                        currentChild++;
                        return Status.Running;
                }
            }

            Reset();
            return Status.Failure;
        }
    }

    public class UntilFailureSequence : SequenceNode {
        public UntilFailureSequence(string name, int priority = 0) : base(name, priority) { }

        public override Status Process() {
            if (currentChild < children.Count) {
                if (children[currentChild].Process() == Status.Failure) {
                    return Status.Failure;
                } else {
                    currentChild++;
                    return currentChild == children.Count ? Status.Success : Status.Running;
                }
            }

            Reset();
            return Status.Success;
        }
    }

    // Logical AND
    // 모든 자식 노드들을 실행, 자식 노드들중 하나라도 실패 상태를 반환한다면 최종적으로 실패 상태를 반환
    public class SequenceNode : BTNode {
        public SequenceNode(string name, int priority = 0) : base(name, priority) { }

        public override Status Process() {
            if (currentChild < children.Count) {
                //Debug.Log("LightAttackSequence 자식 인덱스 : " + currentChild);
                switch (children[currentChild].Process()) {
                    case Status.Running:
                        return Status.Running;
                    case Status.Failure:
                        return Status.Failure;
                    default:
                        currentChild++;
                        //Debug.Log("다음 " + currentChild + " 번 검사 준비");
                        return currentChild == children.Count ? Status.Success : Status.Running;
                }
            }

            Reset();
            return Status.Success;
        }
    }
    #endregion

    public class BehaviourTree : BTNode {
        public BehaviourTree(string name) : base(name) { }

        public override Status Process() {
            while (currentChild < children.Count) {
                //Debug.Log("실행되는 노드 이름 : " + children[currentChild].name);
                var status = children[currentChild].Process();

                if (status != Status.Success) {
                    return status;
                }
                currentChild++;
            }
            return Status.Success;
        }
    }

    // 모든 잎노드는 실행해야할 Behaviour을 가짐
    // 해당 행동을 캡슐화 하여 Strategy으로 만듬
    public class BTLeaf : BTNode {
        readonly IStrategy strategy;
        public BTLeaf(string name, IStrategy strategy, int priority = 0) : base(name, priority) {
            this.strategy = strategy;
        }

        public override Status Process() => strategy.Process();
        public override void Reset() => strategy.Reset();
    }

    public class BTNode {
        public enum Status { Success, Failure, Running }

        public readonly string name;
        public readonly int priority;

        public readonly List<BTNode> children = new();
        protected int currentChild;

        public BTNode(string name = "Node", int priority = 0) {
            this.name = name;
            this.priority = priority;
        }

        public void AddChild(BTNode child) => children.Add(child);

        public virtual Status Process() => children[currentChild].Process();

        public virtual void Reset() {
            currentChild = 0;
            foreach (var child in children) {
                child.Reset();
            }
        }
    }
}