using Trash;
using Trash.Properties;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Money : MonoBehaviour
{
    [SerializeField]
    private int money;

    [SerializeField]
    private int m_threshold = -10000;

    [SerializeField]
    private int m_cityMoney = 1000;
    
    [SerializeField]
    private UnityEvent OnMoneyLoss;

    [SerializeField]
    private UnityEvent OnMoneyGain;

    private int machineUpkeep;
    private int machinePurchasesCost;
    private int miscycledCost;
    private int cleanUpCost;
    private int recycleRevenue;
    private int machineSellingRevenue;
    private int cityPayment;

    public DisplayBalance displayBalance;
    
    public void DecreaseMoney(int amt)
    {
        money -= amt;
        OnMoneyLoss.Invoke();
        if (money < m_threshold)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void IncreaseMoney(int amt)
    {
        money += amt;
        OnMoneyGain.Invoke();
    }

    public void PayMachineUpkeep(int amt)
    {
        DecreaseMoney(amt);
        machineUpkeep += amt;
    }

    public void PayForMachinePurchase(int amt)
    {
        DecreaseMoney(amt);
        machinePurchasesCost += amt;
    }

    public void PayForCleanUp(int amt)
    {
        money -= amt;
        cleanUpCost += amt;
    }

    public void PayForMiscycling(int amt)
    {
        DecreaseMoney(amt);
        miscycledCost += amt;
    }

    public void GainRecycleRevenue(int amt)
    {
        IncreaseMoney(amt);
        recycleRevenue += amt;
    }

    public void MachineSoldRevenue(int amt)
    {
        IncreaseMoney(amt);
        machineSellingRevenue += amt;
    }

    public void AddCityPayment()
    {
        IncreaseMoney(m_cityMoney);
        cityPayment += m_cityMoney;
    }

    public bool CanBuy(int value)
    {
        if (value <= money)
        {
            return true;
        }
        displayBalance.BlinkRed();
        return false;
    }

    public void ResetDailyCostAndRevenue()
    {
        machinePurchasesCost = 0;
        machineUpkeep = 0;
        miscycledCost = 0;
        cleanUpCost = 0;
        recycleRevenue = 0;
        cityPayment = 0;
        OnMoneyGain.Invoke();
        OnMoneyLoss.Invoke();
    }

    public void PayAllCosts()
    {
        foreach (BuildingIdentifier building in FindObjectsOfType<BuildingIdentifier>())
        {
            PayMachineUpkeep(building.UpkeepCosts);
        }

        foreach (TrashStack trashstack in FindObjectsOfType<TrashStack>())
        {
            foreach (Trash.Trash trash in trashstack.Stack)
            {
                PayForCleanUp((int) (((MoneyLossOnFailedRecycle) trash.PropertiesDictionary[typeof(MoneyLossOnFailedRecycle)]).Amount * 1.5f));
            }
            Destroy(trashstack.gameObject);
        }
    }

    public int GetNewMachineCost()
    {
        return machinePurchasesCost;
    }

    public int GetMachineUpkeep()
    {
        return machineUpkeep;
    }

    public int GetMiscycledCost()
    {
        return miscycledCost;
    }

    public int GetCleanUpCost()
    {
        return cleanUpCost;
    }

    public int GetRecycleRevenue()
    {
        return recycleRevenue;
    }

    public int GetCityPayment()
    {
        return m_cityMoney;
    }

    public int GetMoney()
    {
        return money;
    }
}