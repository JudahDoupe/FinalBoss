using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extentions {

    public static TValue RandomValue<TKey, TValue>(this IDictionary<TKey, TValue> dict)
    {
        var values = Enumerable.ToList(dict.Values);
        int size = dict.Count;
        return values[Random.Range(0,size)];
    }
}
