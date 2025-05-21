using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace KBH {

    #region Decorator
    public class UntilSuccessNode : BTNode {
        public UntilSuccessNode(string name) : base(name) { }

        // Behaviour �� �����ϸ� Ʈ���� ���¸� �����ϰ� ���� ���� ��ȯ
        public override Status Process() {
            if (children[0].Process() == Status.Success) {
                Reset();
                return Status.Success;
            }

            // ���� ���°� �ƴ϶�� ������ ��ȯ
            return Status.Running;
        }
    }

    // �ڽĳ�尡 ��ȯ�� ���¸� ����
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
    public class RandomSelectorNode : PrioritySelectorNode {
        protected override List<BTNode> SortChildren() => children.Shuffle().ToList();
        public RandomSelectorNode(string name) : base(name) { }
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
    // �ڽ� ������ �ϳ��� ���� ���¸� ��ȯ�Ѵٸ� ���������� ���� ���� ��ȯ
    public class SelectorNode : BTNode {
        public SelectorNode(string name, int priority = 0) : base(name, priority) { }

        public override Status Process() {
            if (currentChild < children.Count) {
                switch (children[currentChild].Process()) {
                    case Status.Running:
                        return Status.Running;
                    case Status.Success:
                        Reset();
                        return Status.Success;
                    default: // ���� �ڽĳ�尡 ���� ���¸� ��ȯ�ϸ� ���� �ڽ��� �Ǵ�
                        currentChild++;
                        return Status.Running;
                }
            }

            Reset();
            return Status.Failure;
        }
    }

    // Logical AND
    // ��� �ڽ� ������ ����, �ڽ� ������ �ϳ��� ���� ���¸� ��ȯ�Ѵٸ� ���������� ���� ���¸� ��ȯ
    public class SequenceNode : BTNode {
        public SequenceNode(string name, int priority = 0) : base(name, priority) { }

        public override Status Process() {
            if (currentChild < children.Count) {
                switch (children[currentChild].Process()) {
                    case Status.Running:
                        return Status.Running;
                    case Status.Failure:
                        return Status.Failure;
                    default:
                        currentChild++;
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
                var status = children[currentChild].Process();

                if (status != Status.Success) {
                    return status;
                }
                currentChild++;
            }
            return Status.Success;
        }
    }

    // ��� �ٳ��� �����ؾ��� Behaviour�� ����
    // �ش� �ൿ�� ĸ��ȭ �Ͽ� Strategy���� ����
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