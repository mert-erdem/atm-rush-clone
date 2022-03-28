using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMoney;
    [SerializeField] private Image imageBackground;
    [SerializeField] private Camera mainCamera;
    private Vector3 newPos;

    private void OnEnable()
    {
        GameManager.ActionMiniGame += DisableThis;

        textMoney.transform.position = mainCamera.WorldToScreenPoint(this.transform.position);
        imageBackground.transform.position = textMoney.transform.position;
    }

    private void LateUpdate()
    {
        FollowPlayer();
        SetCoinText();
    }

    private void SetCoinText()
    {
        textMoney.text = StackManager.Instance.CurrentStackValue.ToString();
    }

    private void FollowPlayer()
    {
        newPos = mainCamera.WorldToScreenPoint(this.transform.position);
        textMoney.transform.position = new Vector3(
            Mathf.Lerp(textMoney.transform.position.x, newPos.x, 2f),
            textMoney.transform.position.y,
            textMoney.transform.position.z);
        imageBackground.transform.position = textMoney.transform.position;
    }

    private void DisableThis()
    {
        Destroy(textMoney.gameObject);
        Destroy(imageBackground.gameObject);
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        GameManager.ActionMiniGame -= DisableThis;
    }
}
