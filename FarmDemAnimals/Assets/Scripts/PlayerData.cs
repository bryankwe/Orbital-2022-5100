using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : Manager<PlayerData> {
    public int Money { get; private set; }
    public int Lives { get; private set; }
    public int Trophies { get; private set; }
    public int TurnNumber { get; private set; }

    private int NumberOfLivesToLose = 0;
    private int NumberOfTrophiesToWin = 5;

    public System.Action OnUpdateMoney;
    public System.Action OnUpdateLives;
    public System.Action OnUpdateTrophies;
    public System.Action OnUpdateTurnNumber;

    private void Start() {
        Money = 10;
        Lives = 2;
        Trophies = 0;
        TurnNumber = 1;
    }

    public bool CanAfford(int amount)
    {
        return amount <= Money;
    }

    public void SpendMoney(int amount)
    {
        Money -= amount;
        OnUpdateMoney?.Invoke(); //Use this to update UI
    }

    public void AddMoney(int amount) {
        Money += amount;
        OnUpdateMoney?.Invoke();
    }

    public bool IsGameOver() {
        return Lives <= NumberOfLivesToLose;
    }

    public void LoseLife() {
        Lives -= 1;
        OnUpdateLives?.Invoke();
    }

    public bool HasWon() {
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
