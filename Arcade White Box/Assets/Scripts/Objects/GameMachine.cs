using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMachine : Machine {

    [SerializeField] private int gameLength; //how long it takes to complete a game
    [SerializeField] private int gameReplayabilityPercent; //how likely a user is to play the game again IN PERCENT
    [SerializeField] private float playCost; //how much it costs a user to play the game

    public int GameLength
    {
        get
        {
            return gameLength;
        }

        set
        {
            gameLength = value;
        }
    }

    public int GameReplayabilityPercent
    {
        get
        {
            return gameReplayabilityPercent;
        }

        set
        {
            gameReplayabilityPercent = value;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnInteraction()
    {
        
    }
}
