using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Windows;

public class AILearningBrain : MonoBehaviour
{
    [SerializeField]
    private TMP_Text aiChoiceText;

    private List<string> _inputs = new List<string>();

    [SerializeField]
    private int ngramOrder = 1;

    [SerializeField]
    private int currentWindowLength = 1;

    public void AddInput(string input)
    {
        _inputs.Add(input);

        ChooseMove();
    }

    private void ChooseMove()
    {
        if(_inputs.Count >= 2)
        {
            if(_inputs.Count > currentWindowLength + 1)
            {
                ngramOrder++;
            }
        }

        NGram();
    }

    private void NGram()
    {
        if(ngramOrder == 1)
        {
            aiChoiceText.text = GetRandomMove();
        }
        else
        {
            HashSet<string> possibleCombinations = new HashSet<string>();

            GenerateCombinations(_inputs, possibleCombinations, ngramOrder);

            Debug.Log(PrintCombinations(possibleCombinations));

            string window = GetWindowPattern();

            string nextChoice = GetCorrectCombination(window, possibleCombinations);

            Debug.Log($"PATTERN CHOSEN: {nextChoice}");

            if(string.IsNullOrEmpty(nextChoice))
            {
                return;
            }

            aiChoiceText.text = nextChoice[nextChoice.Length - 1].ToString();
        }       
    }

    private string GetRandomMove()
    {
        float randomNumber = UnityEngine.Random.Range(0, 1);

        if(randomNumber <= 0.33f)
        {
            return "R";
        }
        else if (randomNumber > 0.33f && randomNumber <= 0.66f)
        {
            return "S";
        }
        else
        {
            return "P";
        }
    }

    private string GetWindowPattern()
    {
        List<string> windowCharacters = _inputs.GetRange(_inputs.Count - currentWindowLength, currentWindowLength);

        string s = "";

        foreach (string c in windowCharacters)
        {
            s += c;
        }

        return s;
    }

    private string GetCorrectCombination(string window, HashSet<string> possibleCombinations)
    {
        foreach(string combination in possibleCombinations)
        {
            if(combination.Substring(0, window.Length) == window)
            {
                return combination;
            }
        }
        return "";
    }

    private static void GenerateCombinations(List<string> items, HashSet<string> possibleCombinations, int length, string current = "", int start = 0)
    {
        if (current.Length == length)
        {
            possibleCombinations.Add(current);
            return;
        }

        for (int i = 0; i < items.Count; i++)
        {
            GenerateCombinations(items, possibleCombinations, length, current + items[i], i + 1);
        }
    }

    private string PrintCombinations(HashSet<string> inputs)
    {
        string s = "{";

        foreach (string input in inputs)
        {
            s += $"{input},";
        }

        s += "}";

        return s;
    }
}
