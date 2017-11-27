using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMachine : Machine {

    public enum GameGenre { Puzzle, Platformer, Shooter, Sports, Fighting, Racing, Physical }
    public enum PlayerCount { One, Two, Four }

    [SerializeField] private GameGenre machineGenre; // The genre of the game machine
    [SerializeField] private PlayerCount playerCount; // How many players can use the machine
    [SerializeField] private float attractionRatingStart; // How attractive the machine is, how often it draws customers to it.
    [SerializeField] private float attractionRatingMinimum; // How low the attraction rating can fall to.
    [SerializeField] private float attractionRatingFallOff; // How fast the attraction rating falls off. (Percentage)

    private int currentNumberOfPlayers;

    public override void OnUse()
    {
        if (CheckIfMachineIsFull())
        {
            base.OnUse();
            GameManager.Instance.SceneManagerLink.GetComponent<EconomyManager>().MoneyEarnedFromArcade(this);
            // Give Happiness, TotalHappiness()
        }
    }

    public void IncreaseCurrentNumberOfPlayers()
    {
        currentNumberOfPlayers++;
    }

    public void DecreaseCurrentNumberOfPlayers()
    {
        currentNumberOfPlayers--;
    }

    public float TotalHappiness()
    {
        float hapGiven = statAdjustment;
        int winRoll = Random.Range(0, 1);
        if (winRoll == 1)
            hapGiven += (statAdjustment / 2);
        return hapGiven;
    }

    /// <summary>
    /// Checks if the machine has the necessary number of the players to be used
    /// @return the bool
    /// </summary>
    /// <returns></returns>
    public bool CheckIfMachineIsFull()
    {
        if (currentNumberOfPlayers == SwitchStringToInt(playerCount))
            return true;
        else
            return false;
    }

    protected override void DailyReset()
    {
        base.DailyReset();
        CalculateAttractionRating();
    }
    private void CalculateAttractionRating()
    {
        if (attractionRatingStart > attractionRatingMinimum)
            attractionRatingStart = attractionRatingMinimum;
        else
        {
            attractionRatingStart -= attractionRatingFallOff;
        }
    }

    public int SwitchStringToInt(PlayerCount value)
    {
        if (value == PlayerCount.One)
            return 1;
        else if (value == PlayerCount.Two)
            return 2;
        else
            return 4;
    }
}
