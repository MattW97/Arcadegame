using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NotificationLoader : MonoBehaviour {

    private string[] posArray, negArray, specArray;

    [SerializeField]
    private TextAsset positiveNotifications, negativeNotifications, specialNotifications;

    public string[] PosArray
    {
        get
        {
            return posArray;
        }

        set
        {
            posArray = value;
        }
    }

    public string[] NegArray
    {
        get
        {
            return negArray;
        }

        set
        {
            negArray = value;
        }
    }

    public string[] SpecArray
    {
        get
        {
            return specArray;
        }

        set
        {
            specArray = value;
        }
    }

    void Awake()
    {
        string posText = positiveNotifications.text;
        posArray = splitStrings(posText);

        string negText = negativeNotifications.text;
        negArray = splitStrings(negText);

        string specText = specialNotifications.text;
        specArray = splitStrings(specText);
    }

    private string[] splitStrings(string stringToSplit)
    {
       string[] array = stringToSplit.Split('\n');
        return array;
    }

   
    private void PrintAllNotifications(string[] array)
    {
        foreach (string str in array)
        {
            print(str);
        }
    }

    public string[] getSpecArray()
    {
        return specArray;
    }

    
}

