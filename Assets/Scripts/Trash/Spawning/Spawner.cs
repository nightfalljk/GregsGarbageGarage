using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Trash.Spawning
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        private Vector3 m_spawnPoint;

        public const float YOffsetPerObject = 0.17f;

        public const float RotationPerObject = 30.0f;

        [SerializeField]
        private GameObject m_stackPrefab;

        private SpawnChances m_spawnChances;

        private Coroutine m_spawning;

        [SerializeField]
        private ScalingSpawnChances m_scalingSpawnChances;

        private IEnumerator StartSpawning()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);

                if (Random.Range(0.0f, 1.0f) <= m_spawnChances.StackSpawnChancePerSecond)
                {
                    GameObject stack = Instantiate(m_stackPrefab, m_spawnPoint, Quaternion.identity);
                    TrashStack trashStack = stack.GetComponent<TrashStack>();
                    int stackSize = 1;

                    float r = Random.Range(0.0f, 1.0f);

                    foreach (SpawnChances.StackSizeChance stackSizeChance in m_spawnChances.StackSizeChances)
                    {
                        if (r >= stackSizeChance.ChanceStart && r < stackSizeChance.ChanceEnd)
                        {
                            stackSize = stackSizeChance.StackSize;
                            break;
                        }
                    }

                    for (int i = 0; i < stackSize; i++)
                    {
                        r = Random.Range(0.0f, 1.0f);

                        foreach (SpawnChances.TypeSpawnChance typeSpawnChance in m_spawnChances.TypeSpawnChances)
                        {
                            if (r >= typeSpawnChance.ChanceStart && r < typeSpawnChance.ChanceEnd)
                            {
                                GameObject prefab = typeSpawnChance.Type.Models[Random.Range(0, typeSpawnChance.Type.Models.Length)];
                                Trash trash = Instantiate(prefab, stack.transform.position + i * YOffsetPerObject * Vector3.up, Quaternion.Euler(0, RotationPerObject * i, 0), stack.transform).GetComponent<Trash>();
                                trashStack.Push(trash);
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void SetChances(SpawnChances chances)
        {
            m_spawnChances = chances;
        }

        public void OnDayStartTest(int dayNumber)
        {
            m_spawnChances = m_scalingSpawnChances.GetRandomScaledSpawnChancesDay(dayNumber);
            m_spawning = StartCoroutine(StartSpawning());
        }

        public void OnDayEndTest(int dayNumber)
        {
            StopCoroutine(m_spawning);
        }

        public void OnNightStartTest(int nightNumber)
        {
            m_spawnChances = m_scalingSpawnChances.GetRandomScaledSpawnChancesNight(nightNumber);
            m_spawning = StartCoroutine(StartSpawning());
        }

        public void OnNightEndTest(int nightNumber)
        {
            StopCoroutine(m_spawning);
        }
    }
}