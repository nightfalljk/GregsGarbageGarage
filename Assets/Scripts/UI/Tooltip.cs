using UnityEngine;
using TMPro;
public class Tooltip : MonoBehaviour
{
    [SerializeField] private GameObject tooltip;

    private TextMeshProUGUI text;
    
    void Awake()
    {
        text = tooltip.GetComponentInChildren<TextMeshProUGUI>();
        text.text = "";
        tooltip.SetActive(false);
    }

    public void GenerateTooltip(GameObject prefab)
    {
        BuildingIdentifier buildingUnit = prefab.GetComponent<BuildingIdentifier>();

        string desc = string.Format("<b>{0}</b>\n{1}\n\n<b>Cost: {2}</b>", buildingUnit.GetTitle(),buildingUnit.GetDescription(),buildingUnit.GetCost().ToString());

        //string formattedDesc = string.Format();
        
        text.text = desc;
    }

    public void GenerateSellTootip()
    {
        text.text = "<b>Sell</b>\nSells the building for a fraction of it's price.";
    }

    public void Deactivate()
    {
        tooltip.SetActive(false);
    }

    public void GenerateSubMenuTooltip(BuildMenuOptions options)
    {
        text.text = string.Format("<b>{0}</b>\n{1}",options.title,options.description);
    }
    
}
