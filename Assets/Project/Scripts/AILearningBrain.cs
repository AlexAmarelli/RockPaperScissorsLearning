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
        ChooseMove();

        _inputs.Add(input);        
    }

    private void ChooseMove()
    {
        NGram();
    }

    private void NGram()
    {
        if(ngramOrder == 1 || _inputs.Count < ngramOrder)
        {
            aiChoiceText.text = GetRandomMove();
        }
        else
        {
            currentWindowLength = ngramOrder - 1;

            HashSet<string> possiblePatterns = new HashSet<string>();

            GetInputPatterns(_inputs, possiblePatterns, ngramOrder);

            Debug.Log(PrintPatterns(possiblePatterns));

            string window = GetWindowPattern();

            string playerPattern = GetCorrectPattern(window, possiblePatterns);

            Debug.Log($"PATTERN CHOSEN: {playerPattern}");

            if (string.IsNullOrEmpty(playerPattern))
            {
                aiChoiceText.text = GetRandomMove();
                return;
            }

            string aiMove = GetNextMove(playerPattern);

            if (string.IsNullOrEmpty(aiMove))
            {
                aiChoiceText.text = GetRandomMove();
                return;
            }

            aiChoiceText.text = aiMove;
        }       
    }

    private string GetRandomMove()
    {
        float randomNumber = UnityEngine.Random.Range(0f, 1f);

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

    private string GetCorrectPattern(string window, HashSet<string> possiblePatterns)
    {
        foreach(string pattern in possiblePatterns)
        {
            if(pattern.Substring(0, window.Length) == window)
            {
                return pattern;
            }
        }
        return "";
    }

    private static void GetInputPatterns(List<string> inputs, HashSet<string> possiblePatterns, int ngramLength)
    {
        for(int i = 0; i <= inputs.Count - ngramLength; i++)
        {
            var pattern = inputs.GetRange(i, ngramLength);
            var stringPattern = "";

            foreach (var c in pattern)
            {
                stringPattern += c;
            }

            possiblePatterns.Add(stringPattern);
        }
    }

    private static string GetNextMove(string playerPattern)
    {
        string playerNextMove = playerPattern[playerPattern.Length - 1].ToString();

        switch (playerNextMove) 
        {
            case "R":
                return "P";
            case "P": 
                return "S";
            case "S":
                return "R";
        }

        return "";
    }

    private string PrintPatterns(HashSet<string> inputs)
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
