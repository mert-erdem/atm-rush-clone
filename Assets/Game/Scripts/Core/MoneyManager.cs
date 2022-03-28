using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : Singleton<MoneyManager>
{
    public int CurrentMultiplier
    {
        set => currentMultiplier = value;
    }

    private int totalAmount, currentMultiplier;


    private void Start()
    {
        GameManager.ActionGameEnd += SaveTheAmount;

        totalAmount = PlayerPrefs.GetInt("TOTAL_MONEY", 0);
        CanvasController.Instance.UpdateTotalMoneyText(totalAmount);
    }

    public void Deposit(int itemLevel)// atm
    {
        totalAmount += itemLevel + 1;
    }

    private void SaveTheAmount()// level finish action
    {
        totalAmount += StackManager.Instance.CurrentStackValue * currentMultiplier;
        PlayerPrefs.SetInt("TOTAL_MONEY", totalAmount);
        CanvasController.Instance.UpdateTotalMoneyText(totalAmount);
    }

    private void OnDestroy()
    {
        GameManager.ActionGameEnd -= SaveTheAmount;
    }
}
