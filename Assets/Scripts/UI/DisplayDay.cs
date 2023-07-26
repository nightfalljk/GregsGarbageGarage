using TMPro;
using UnityEngine;

public class DisplayDay : MonoBehaviour
{
    [SerializeField] private GameObject dayObject;
    
    private TextMeshProUGUI day;
    private int currentDay;
 
    void Awake()
    {
        day = dayObject.GetComponent<TextMeshProUGUI>();
        day.text = "Day: " + 0;
    }

    public void UpdateDay(int dayNumber)
    {
        day.text = "Day: " + dayNumber;
    }

    public void AddDay()
    {
        currentDay++;
    }
}
