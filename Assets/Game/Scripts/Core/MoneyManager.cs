using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : Singleton<MoneyManager>
{
    public int CurrentMultiplier
    {
        set => currentMultiplier = value;
    }

    private int currentAmount, totalAmount, currentMultiplier;


    private void Start()
    {
        currentAmount = 0;
        totalAmount = PlayerPrefs.GetInt("TOTAL_MONEY", 0);
    }

    public void Deposit(int itemLevel)
    {
        currentAmount += itemLevel + 1;
    }

    public void SaveTheAmount()// level finish action
    {
        totalAmount += currentAmount * currentMultiplier;
        PlayerPrefs.SetInt("TOTAL_MONEY", totalAmount);
    }
}
