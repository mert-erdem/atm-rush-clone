using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : Singleton<MoneyManager>
{
    private int currentAmount, totalAmount;


    private void Start()
    {
        currentAmount = 0;
        totalAmount = PlayerPrefs.GetInt("TOTAL_MONEY", 0);
    }

    public void Deposit(int itemLevel)
    {
        currentAmount += (itemLevel + 1) * 2;
    }

    public void SaveTheAmount()
    {
        totalAmount += currentAmount;
        PlayerPrefs.SetInt("TOTAL_MONEY", totalAmount);
    }
}
