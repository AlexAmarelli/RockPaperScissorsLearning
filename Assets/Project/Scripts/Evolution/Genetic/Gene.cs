using System;
using UnityEngine;

[Serializable]
public class Gene
{
    [field: SerializeField]
    public string Key { get; set; }

    [field: SerializeField]
    public float Value { get; set; }

    [field: SerializeField]
    public float Min { get; set; }

    [field: SerializeField]
    public float Max { get; set; }

    public Gene Mutate(float mutation)
    {
        return new Gene
        {
            Key = Key,
            Min = Min,
            Max = Max,
            Value = Math.Clamp(Value + Mathf.Lerp(Min, Max, mutation), Min, Max)
        };
    }
}