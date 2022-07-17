using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : SingletonManager<PlayerData> {
    public int Money { get; private set; }
    public int Lives { get; private set; }
    public int Trophies { get; private set; }
    public int TurnNumber { get; private set; }

    private int NumberOfLivesToLose = 0;
    private int NumberOfTrophiesToWin = 5;
    public int maxLives = 2;
    public int maxTrophies = 5;

    public System.Action OnUpdateMoney;
    public System.Action OnUpdateLives;
    public System.Action OnUpdateTrophies;
    public System.Action OnUpdateTurnNumber;

    private void Start() {
        //Debug.Log("Enter PlayerData Start()");
        Money = 10;
        Lives = 2;
        Trophies = 0;
        TurnNumber = 1;
    }

    /// <summary>
    /// Call this function if BATTLE outcome is Victory
    /// </summary>
    public void Victory() {
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
    }

    /// <summary>
    /// Call this function when the game has ended
    /// </summary>
    public void ResetAllStats() {
        Instance.Money = 10;
        Instance.Lives = 2;
        Instance.Trophies = 0;
        Instance.TurnNumber = 1;
    }
    
    public bool CanAfford(int amount)
    {
        return amount <= Money;
    }

    public void SpendMoney(int amount)
    {
        Money -= amount;
        SoundManager.Instance.Play("Coins");
        OnUpdateMoney?.Invoke(); //Use this to update UI
    }

    public void AddMoney(int amount) {
        Money += amount;
        SoundManager.Instance.Play("Coins");
        OnUpdateMoney?.Invoke();
    }

    public void ResetMoney() {
        Money = 10;
        OnUpdateMoney?.Invoke();
    }

    public bool HasLostGame() {
        return Lives <= NumberOfLivesToLose;
    }

    public void LoseLife() {
        Lives -= 1;
        OnUpdateLives?.Invoke();
    }

    public bool HasWonGame() {
        return Trophies >= NumberOfTrophiesToWin;
    }

    public void IncreaseTrophies() {
        Trophies += 1;
        OnUpdateTrophies?.Invoke();
    }

    public void IncreaseTurnNumber() {
        TurnNumber += 1;
        OnUpdateTurnNumber?.Invoke();
    }
}
