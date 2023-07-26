using System.Collections;
using TMPro;
using UnityEngine;

public class DisplayBalance : MonoBehaviour
{
    [SerializeField] private GameObject balanceObject;
    [SerializeField] private Money money;
    private TextMeshProUGUI balance;
    private Color32 defaultColor;
    void Awake()
    {
        balance = balanceObject.GetComponent<TextMeshProUGUI>();
        UpdateBalance();
        defaultColor = balance.color;
        money.displayBalance = this;
    }

    public void UpdateBalance()
    {
        balance.text = "Balance: " + money.GetMoney();
    }

    public void BlinkRed()
    {
        StartCoroutine(BlinkCo());
    }

    IEnumerator BlinkCo()
    {
        balance.color = Color.red;
        yield return new WaitForSecondsRealtime(0.3f);
        balance.color = defaultColor;
        yield return new WaitForSecondsRealtime(0.3f);
        balance.color = Color.red;
        yield return new WaitForSecondsRealtime(0.3f);
        balance.color = defaultColor;
    }

}
