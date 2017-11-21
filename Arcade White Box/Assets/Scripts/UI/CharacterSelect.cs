using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour {

    private CharacterManager characterManager;

    [SerializeField]
    private InputField nameLetter;

    int charIndex = 0;

    public string[] nameChar;

    void Start()
    {
        charIndex = 0;
        nameLetter.text = nameChar[charIndex];
    }
        

    public void CharUp()
    {
        if(charIndex == nameChar.Length - 1)
        {
            charIndex = 0;
        }
        else
        {
            charIndex++;
        }

        nameLetter.text = nameChar[charIndex];
    }

    public void CharDown()
    {
        if (charIndex == 0)
        {
            charIndex = nameChar.Length - 1;
        }
        else
        {
            charIndex--;
        }

        nameLetter.text = nameChar[charIndex];
    }


    public string getCurrentChar()
    {
        return nameLetter.text;
    }
}
