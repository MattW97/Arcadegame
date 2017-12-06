using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameGenerator : MonoBehaviour {

    private string[] firstNames, lastNames;
    [SerializeField]
    private TextAsset firstName, lastName;

	// Use this for initialization
	void Start () {
        string first = firstName.text;
        firstNames = splitStrings(first);

        string last = lastName.text;
        lastNames = splitStrings(last);
		
	}

    private string[] splitStrings(string stringToSplit)
    {
        string[] array = stringToSplit.Split('\n');
        return array;
    }

    public string GenerateName()
    {
        string first = null;
        string last = null;
        first = firstNames[Random.Range(0, firstNames.Length)];
        last = lastNames[Random.Range(0, lastNames.Length)];
        string name = first + " " + last;
        return name;
    }
}
