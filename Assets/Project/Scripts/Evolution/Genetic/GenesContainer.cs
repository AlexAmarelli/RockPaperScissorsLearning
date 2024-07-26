using System;
using System.Collections.Generic;
using UnityEngine;

public class GenesContainer : MonoBehaviour
{
    [field: SerializeReference]
    // ReSharper disable once UseArrayEmptyMethod
    public List<Gene> Genes { get; set; } = new();

    public bool HasGene<T>(string key) where T : Gene
    {
        foreach (var gene in Genes)
        {
            if (gene.Key != key)
            {
                continue;
            }

            if (gene is not T castedGene)
            {
                continue;
            }

            return true;
        }

        return false;
    }

    public T GetGene<T>(string key) where T : Gene
    {
        foreach (var gene in Genes)
        {
            if (gene.Key != key)
            {
                continue;
            }

            if (gene is not T castedGene)
            {
                continue;
            }

            return castedGene;
        }

        return null;
    }

    public float GetGenesTotalValue()
    {
        float total = 0f;

        foreach (var gene in Genes)
        {
            total += gene.Value;
        }

        return total;
    }
}