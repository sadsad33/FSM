using System;
using System.Collections.Generic;
using System.Linq;

public static class ListExtensions {
    static Random range;
    // 확장 메서드
    public static IList<T> Shuffle<T>(this IList<T> list) {
        if (range == null) range = new Random();
        int count = list.Count;
        while (count > 1) {
            --count;
            int index = range.Next(count + 1);
            // 튜플을 이용한 두 변수의 값 교환
            (list[index], list[count]) = (list[count], list[index]);
        }

        return list;
    }
}
