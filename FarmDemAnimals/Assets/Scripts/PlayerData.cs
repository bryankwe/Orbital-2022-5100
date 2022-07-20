using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : SingletonManager<PlayerData> {
    public int Money { get; private set; }
    public int Lives { get; private set; }
    public int Trophies { get; private set; }
    public int TurnNumber { get; private set; }

    private int numberOfLivesToLose = 0;
    private int numberOfTrophiesToWin = 5;
    public int maxLives = 2;
    public int maxTrophies = 5;

    public System.Action OnUpdateMoney;

    private void Start() {
        //Debug.Log("Enter PlayerData Start()");
        Money = 10;
        Lives = 2;
        Trophies = 0;
        TurnNumber = 1;
    }

    /// <summary>
    /// Call this function when the game has ended
    /// Resets all stats back to normal
    /// </summary>
    public void ResetAllStats() {
        Instance.Money = 10;
        Instance.Lives = 2;
        Instance.Trophies = 0;
        Instance.TurnNumber = 1;
    }
    
    /// <summary>
    /// Checks if player can afford to spend input amount of money
    /// </summary>
    /// <param name="amount">The amount of money desired to spend</param>
    public bool CanAfford(int amount)
    {
        return amount <= Money;
    }

    /// <summary>
    /// Reduces player's money by input amount
    /// </summary>
    /// <param name="amount">The amount of money desired to spend</param>
    public void SpendMoney(int amount)
    {
        Money -= amount;
        SoundManager.Instance.Play("Coins");
        OnUpdateMoney?.Invoke(); //Use this to update UI
    }

    /// <summary>
    /// Increases player's money by input amount
    /// </summary>
    /// <param name="amount">The amount of money desired to add</param>
    public void AddMoney(int amount) {
        Money += amount;
        SoundManager.Instance.Play("Coins");
        OnUpdateMoney?.Invoke();
    }

    /// <summary>
    /// Resets player's money back to 10
    /// Called at the start of every Preparation Phase
    /// </summary>
    public void ResetMoney() {
        Money = 10;
        OnUpdateMoney?.Invoke();
    }

    /// <summary>
    /// Checks if the player has lost the game
    /// Compares current lives to the set number of lives a player can lose
    /// </summary>
    public bool HasLostGame() {
        return Lives <= numberOfLivesToLose;
    }

    /// <summary>
    /// Reduces the player's number of lives by 1
    /// </summary>
    public void LoseLife() {
        Lives -= 1;
    }

    /// <summary>
    /// Checks if the player has won the game
    /// Compares current trophies to the set number of trophies a player is required to win
    /// </summary>
    public bool HasWonGame() {
        return Trophies >= numberOfTrophiesToWin;
    }

    /// <summary>
    /// Increases the player's number of trophies by 1
    /// </summary>
    public void IncreaseTrophies() {
        Trophies += 1;
    }

    /// <summary>
    /// Increases the player's turn number by 1
    /// </summary>
    public void IncreaseTurnNumber() {
        TurnNumber += 1;
    }

    // ------------------------------------ OLD FUNCTIONS -------------------------------
    /// <summary>
    /// Call this function if BATTLE outcome is Victory
    /// </summary>
    /*public void Victory() {
        Instance.ResetMoney();
        Instance.IncreaseTrophies();
    }

    /// <summary>
    /// Call this function if BATTLE outcome is Draw
    /// </summary>
    public void Draw() {
        Instance.ResetMoney();
    }

    /// <summary>
    /// Call this function if BATTLE outcome is Lose
    /// </summary>
    public void Lose() {
        Instance.ResetMoney();
        Instance.LoseLife();
    }*/
}
