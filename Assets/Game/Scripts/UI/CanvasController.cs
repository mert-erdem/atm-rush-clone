using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasController : Singleton<CanvasController>
{
    [SerializeField] private GameObject panelMenu, panelInGame, panelEndGame;
    [SerializeField] private MoneyIndicator moneyIndicator;
    [SerializeField] private TextMeshProUGUI textTotalMoney;

    private void Start()
    {
        GameManager.ActionGameStart += SetInGameUi;
        GameManager.ActionMiniGame += SetMiniGameUi;
        GameManager.ActionGameEnd += SetEndGameUi;       
    }

    private void SetInGameUi()
    {
        StartCoroutine(SetInGameUiRoutine());
    }
    private IEnumerator SetInGameUiRoutine()
    {
        panelMenu.SetActive(false);

        yield return new WaitForSeconds(1f);
       
        panelInGame.SetActive(true);
        moneyIndicator.enabled = true;
    }

    private void SetMiniGameUi()
    {
        panelInGame.SetActive(false);
    }

    private void SetEndGameUi()
    {
        panelEndGame.SetActive(true);
    }

    public void UpdateTotalMoneyText(int value)
    {
        textTotalMoney.text = value.ToString();
    }

    #region UI Button's Methods
    public void ButtonStartPressed()
    {
        GameManager.ActionGameStart?.Invoke();
    }

    public void ButtonNextLevelPressed()
    {
        GameManager.Instance.LoadNextLevel();
    }

    public void ButtonRestartPressed()
    {
        GameManager.Instance.RestartLevel();
    }
    #endregion

    private void OnDisable()
    {
        GameManager.ActionGameStart -= SetInGameUi;
        GameManager.ActionMiniGame -= SetMiniGameUi;
        GameManager.ActionGameEnd -= SetEndGameUi;
    }
}
