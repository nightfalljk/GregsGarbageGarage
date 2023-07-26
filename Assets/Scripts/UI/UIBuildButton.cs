using UnityEngine;
using UnityEngine.EventSystems;

public class UIBuildButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject tooltip;
    [SerializeField] private GameObject subMenu;
    
    private Tooltip tooltipScript;
    // Start is called before the first frame update
    void Start()
    {
        tooltipScript = tooltip.GetComponent<Tooltip>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (prefab == null)
        {
            if (subMenu != null)
            {
                tooltipScript.GenerateSubMenuTooltip(subMenu.GetComponent<BuildMenuOptions>());
            }
            else
            {
                tooltipScript.GenerateSellTootip();
            }
        }
        else
        {
            tooltipScript.GenerateTooltip(prefab);
        }
        
        tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
    }

    public void DeactivateTooltip()
    {
        tooltipScript.Deactivate();
    }


    public void StartBuilding()
    {
        FindObjectOfType<GridBasedBuilding>().SetBuildmode(GridBasedBuilding.BuildMode.build, prefab);
    }

    public void Delete()
    {
        FindObjectOfType<GridBasedBuilding>().SetBuildmode(GridBasedBuilding.BuildMode.delete);
    }
}
