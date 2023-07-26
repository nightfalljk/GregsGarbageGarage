using UnityEngine;

namespace Trash.Properties
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Trash/Type")]
    public class TrashType : ScriptableObject
    {
        public string Name;

        public GameObject[] Models;

        public GameObject GetRandomModel(Transform parent, int stackCount)
        {
            GameObject g = Instantiate(Models[Random.Range(0,Models.Length)]);
            
            g.transform.rotation = Quaternion.Euler(0, stackCount * 30, 0);

            return g;
        }
    }
}