﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour {
      

    [SerializeField]
    private InputField char1, char2, char3, char4, char5, char6, char7, char8, char9, char10, char11, char12, char13, char14, char15;

    public void stringconcat()
    {
        string arcadeName = (char1.text + char2.text + char3.text + char4.text + char5.text + char6.text + char7.text + char8.text + char9.text + char10.text + char11.text + char12.text + char13.text + char14.text + char15.text);
    }
}