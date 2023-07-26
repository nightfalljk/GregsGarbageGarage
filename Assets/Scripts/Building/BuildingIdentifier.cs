using UnityEngine;

public class BuildingIdentifier : MonoBehaviour
{
    [SerializeField] private int id;

    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] private int cost;
    [SerializeField] private float sellFactor = 0.5f;
    public int UpkeepCosts = 50;

    
    private Money money;

    private void Awake()
    {
        money = FindObjectOfType<Money>();
    }

    public bool EnoughMoney()
    {
        return money.CanBuy(cost);
    }

    public int GetId()
    {
        return id;
    }
    
    public void Buy()
    {
        money.PayForMachinePurchase(cost);
    }

    public void Sell()
    {
        money.MachineSoldRevenue( (int)(cost*sellFactor));
    }

    public int GetCost()
    {
        return cost;
    }

    public string GetDescription()
    {
        return description;
    }

    public string GetTitle()
    {
        return title;
    }
}
