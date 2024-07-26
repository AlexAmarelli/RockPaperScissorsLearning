using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EvolutionBrain : MonoBehaviour
{
    [Serializable]
    public struct GenerationGroup
    {
        [Range(0,1)]
        public float GroupSize;

        [Range(0, 1)]
        public float MinMutation;
        
        [Range(0, 1)]
        public float MaxMutation;

        public GenerationGroup(float size, float minMutation, float maxMutation)
        {
            GroupSize = size;
            MinMutation = minMutation;
            MaxMutation = maxMutation;
        }
    }

    [SerializeField]
    private Transform parentPanel;

    [SerializeField]
    private Transform generationPanel;

    [SerializeField]
    private Transform selectionPanel;

    [SerializeField]
    private GameObject elementPrefab;

    [SerializeField, Range(1,25)]
    private int generatedElementsCount;

    [SerializeField]
    private GenerationGroup[] generationSettings;

    private List<GenesContainer> generatedElements;

    private Element parentElement;

    private int phaseCount = 0;

    private void Start()
    {
        GameObject elementObject = Instantiate(elementPrefab, parentPanel);

        parentElement = elementObject.GetComponent<Element>();    
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            switch (phaseCount)
            {
                case 0:
                    Generate();
                    break;
                case 1:
                    Evaluate();
                    break;
                case 2:
                    SetNewGeneration();
                    break;
            }

            phaseCount++;

            if(phaseCount > 2)
            {
                phaseCount = 0;
            }            
        }
    }

    private void Generate()
    {
        generatedElements = new List<GenesContainer>();

        var parentGenes = parentElement.GenesContainer;

        for(int i = 0; i < generationSettings.Length; i++)
        {
            var genSetting = generationSettings[i];
            int size = (int)(generatedElementsCount * genSetting.GroupSize);

            if(size <= 0)
            {
                size = 1;
            }

            for(int j = 0; j < size; j++)
            {
                List<Gene> generatedElementGenes = new List<Gene>();

                foreach (var gene in parentGenes.Genes)
                {
                    var mutation = UnityEngine.Random.Range(genSetting.MinMutation, genSetting.MaxMutation);
                    generatedElementGenes.Add(gene.Mutate(mutation));
                }

                GameObject generatedElementObject = Instantiate(elementPrefab, generationPanel);
                var generatedElement = generatedElementObject.GetComponent<Element>();
                generatedElement.GenesContainer.Genes = generatedElementGenes;

                generatedElements.Add(generatedElement.GenesContainer);
            }
        }
    }

    private void Evaluate()
    {
        var chosenElement = generatedElements.OrderByDescending(x => x.GetGenesTotalValue()).FirstOrDefault();

        GameObject generatedElementObject = Instantiate(elementPrefab, selectionPanel);
        var generatedElement = generatedElementObject.GetComponent<Element>();
        generatedElement.GenesContainer.Genes = chosenElement.Genes;

        parentElement.GenesContainer.Genes = chosenElement.Genes;
    }

    private void SetNewGeneration()
    {
        parentElement.UpdateLabels();

        //Cleans up generation panel
        foreach(Transform generationObject in generationPanel)
        {
            Destroy(generationObject.gameObject);
        }

        //Cleans up selection panel
        Destroy(selectionPanel.transform.GetChild(1).gameObject);
    }
}
