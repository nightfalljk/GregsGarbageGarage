using UnityEngine;
using TMPro;

public class Report : MonoBehaviour
{

    [SerializeField] private Money money;

    [SerializeField] private GameObject report;

    private TextMeshProUGUI content;
    // Start is called before the first frame update
    void Start()
    {
        content = report.GetComponentInChildren<TextMeshProUGUI>();
        content.richText = true;
        report.SetActive(false);
    }

    public void CreateReport()
    {
        string title = "Report";

        string content = "<b>Report</b>\n\n\nCity Budget: <color=#599E3C>"
                         + money.GetCityPayment()
                         + "</color>\n\nPurchases: <color=#E2433B>"
                         + money.GetNewMachineCost()
                         + "</color>\nUpkeep: <color=#E2433B>"
                         + money.GetMachineUpkeep()
                         + "</color>\n\nRevenue: <color=#599E3C>"
                         + money.GetRecycleRevenue()
                         + "</color>\n\nPollution Fine: <color=#E2433B>"
                         + money.GetMiscycledCost()
                         + "</color>\nCleaning Cost: <color=#E2433B>"
                         + money.GetCleanUpCost()
                         + "</color>\n\nBalance: "
                         + money.GetMoney()
                         + "\n\n\nClick to Continue";

        this.content.text = content;
        report.SetActive(true);
    }

    public void Continue()
    {
        report.SetActive(false);
    }
    
}
