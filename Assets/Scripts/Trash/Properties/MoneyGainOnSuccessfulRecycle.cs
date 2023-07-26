using UnityEngine;

namespace Trash.Properties
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Trash/Properties/Money/Gain")]
    public class MoneyGainOnSuccessfulRecycle : TrashProperty
    {
        public int Amount;
    }
}