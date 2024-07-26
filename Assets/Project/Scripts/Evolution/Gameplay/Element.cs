using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Element : MonoBehaviour
{
    [SerializeField]
    private List<TMP_Text> labels = new List<TMP_Text>();

    public GenesContainer GenesContainer { get; set; }

    private void Awake()
    {
        GenesContainer = GetComponent<GenesContainer>();
    }

    private void Start()
    {
        UpdateLabels();
    }

    public void UpdateLabels()
    {
        foreach (var gene in GenesContainer.Genes)
        {
            var key = gene.Key;

            var label = labels.Find(x => x.name.Contains(key));
            label.text = $"{key}: {GenesContainer.GetGene<Gene>(key).Value:0.0}";
        }
    }
}
