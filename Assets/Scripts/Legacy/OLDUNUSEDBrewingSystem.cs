using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

//this is a test change


public class BrewingSystem : MonoBehaviour
{
    public List<string[]> recipeList = new List<string[]>();
    private int[] _currentPotion = new int[] {0,0,0};

    // Start is called before the first frame update
    void Start()
    {
        LoadCSV();
    }

    void LoadCSV()
    {
        // imports the csv
        TextAsset textFile = Resources.Load<TextAsset>("recipies");
        Debug.Log(textFile.text);

        // splits into lines
        string[] splittedLines = textFile.text.Split("\n");

        // splits into items
        for(int i = 1; i < splittedLines.Length; i++)
        {
            //Debug.Log(splittedLines[i]);
            string[] splittedValues = splittedLines[i].Split(',');
            // Debug.Log(splittedValues[0]);
            recipeList.Add(splittedValues);
        }

        foreach (string[] a in recipeList)
        {
            Debug.Log(a[0]);
        }

    }

    public void AddIngredient(string Input)
    {
        switch (Input)
        {
        case "brown leaf":
            _currentPotion[0] ++;
            break;
        case "stick":
            _currentPotion[1] ++;
            break;
        case "fancy leaf":
            _currentPotion[2] ++;
            break;
        default:
            Debug.Log(Input + " Is not a valid item");
            break;
        }       
    }

    public void Brew()
    {
        string potion = "Potion contains: " + _currentPotion[0] + " ," + _currentPotion[1] + " ," + _currentPotion[2];
        Debug.Log(potion);

        // basically what we need to check now is if the ingredients we have match that of a recpipie
        // Find the matching array in the list
        string[] resultArray = FindArray(_currentPotion);

        if (resultArray != null)
        {
            // Access the "Health" value (assuming it's always the first element in the array)
            string potionType = resultArray[0];
            Debug.Log("Potion: " + potionType);
        }
        else
        {
            Debug.Log("Invalid potion recipe");
        }
    }

    private string[] FindArray(int[] targetArray)
    {
        // Use LINQ to find the array that matches the target values
        return recipeList.FirstOrDefault(array => array.Skip(1).Take(3).Take(4).Select(int.Parse).SequenceEqual(targetArray));
    }

    public void DiscardPotion()
    {
        _currentPotion = new int[] {0,0,0};
    }
}
