using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;

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

    void Start()
    {
        string posText = positiveNotifications.text;
        posArray = splitStrings(posText, PosArray);

        string negText = negativeNotifications.text;
        negArray = splitStrings(negText, NegArray);

        string specText = specialNotifications.text;
        specArray = splitStrings(specText, SpecArray);
    }

    void Update()
    {
    }

    private string[] splitStrings(string stringToSplit, string[] array)
    {
        array = stringToSplit.Split('\n');
        //PrintAllNotifications(array);
        return array;
    }

   
    private void PrintAllNotifications(string[] array)
    {
        foreach (string str in array)
        {
            print(str);
        }
    }

    
}

