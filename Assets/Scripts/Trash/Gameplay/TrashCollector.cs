using System.Linq;
using Trash.Properties;
using UnityEngine;
using UnityEngine.Events;


public class TrashCollector : MonoBehaviour
{
    [SerializeField] private TrashType trashType;
    [SerializeField] private Money money;
    private Trash.Trash[] trashCheckArray;

    [SerializeField]
    private UnityEvent m_onPositive;
    [SerializeField]
    private UnityEvent m_onNegative;
    
    private void Awake()
    {
        money = FindObjectOfType<Money>();
    }

    void OnTriggerEnter(Collider other)
    {
        trashCheckArray = other.gameObject.GetComponentsInChildren<Trash.Trash>();

        foreach (Trash.Trash trash in trashCheckArray)
        {
            if (trash.Type.Equals(trashType))
            {
                int amt = ((MoneyGainOnSuccessfulRecycle) trash.Properties.Single(prop =>
                    prop.GetType() == typeof(MoneyGainOnSuccessfulRecycle))).Amount;
                money.GainRecycleRevenue(amt);
                m_onPositive?.Invoke();
            }
            else
            {
                int amt = ((MoneyLossOnFailedRecycle) trash.Properties.Single(prop =>
                    prop.GetType() == typeof(MoneyLossOnFailedRecycle))).Amount;
                money.PayForMiscycling(amt);
                m_onNegative?.Invoke();
            }
        }
        Destroy(other.gameObject);
    }
}