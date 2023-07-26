using UnityEngine;

namespace Trash.Spawning
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Spawning/ScalingSettings")]
    public class ScalingSpawnChances : ScriptableObject
    {
        [SerializeField]
        private SpawnChances m_copyDay;

        [SerializeField]
        private SpawnChances m_copyNight;

        public SpawnChances GetRandomScaledSpawnChancesDay(int day)
        {
            SpawnChances chances = Instantiate(m_copyDay);

            chances.StackSpawnChancePerSecond += day * 0.05f;

            return chances;
        }

        public SpawnChances GetRandomScaledSpawnChancesNight(int night)
        {
            SpawnChances chances = Instantiate(m_copyNight);

            chances.StackSpawnChancePerSecond += night * 0.01f;

            return chances;
        }
    }
}