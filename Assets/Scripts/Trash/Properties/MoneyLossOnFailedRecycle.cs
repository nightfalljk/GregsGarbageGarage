using UnityEngine;

namespace Trash.Properties
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Trash/Properties/Money/Loss")]
    public class MoneyLossOnFailedRecycle : TrashProperty
    {
        public int Amount;
    }
}