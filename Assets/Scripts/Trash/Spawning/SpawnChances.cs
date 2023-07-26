using System;
using Trash.Properties;
using UnityEngine;

namespace Trash.Spawning
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Spawning/SpawnChanceSetting")]
    public class SpawnChances : ScriptableObject
    {
        public float StackSpawnChancePerSecond;

        public StackSizeChance[] StackSizeChances;

        public TypeSpawnChance[] TypeSpawnChances;

        [Serializable]
        public struct StackSizeChance
        {
            public int StackSize;
            [Header("Included")]
            public float ChanceStart;
            [Header("Excluded")]
            public float ChanceEnd;
        }
        
        [Serializable]
        public struct TypeSpawnChance
        {
            public TrashType Type;
            [Header("Included")]
            public float ChanceStart;
            [Header("Excluded")]
            public float ChanceEnd;
        }
    }
}