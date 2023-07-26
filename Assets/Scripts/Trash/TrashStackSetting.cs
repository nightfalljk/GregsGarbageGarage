using UnityEngine;

namespace Trash
{
    [CreateAssetMenu(menuName = "ScriptableObjects/TrashStack/Setting")]
    public class TrashStackSetting : ScriptableObject
    {
        public GameObject TrashStackPrefab;
    }
}