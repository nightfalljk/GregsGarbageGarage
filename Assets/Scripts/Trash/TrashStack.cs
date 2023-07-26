using System.Collections.Generic;
using Trash.Spawning;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Trash
{
    public class TrashStack : MonoBehaviour
    {
        public Stack<Trash> Stack = new Stack<Trash>();

        [SerializeField]
        private int m_maxStack;

        [SerializeField]
        private TrashStackSetting m_settings;

        [SerializeField]
        private float m_throwAwayForce;

        [SerializeField]
        private BoxCollider m_collider;

        /// <summary>
        /// Index is included
        /// </summary>
        public bool TrySplit(int indexToStart, out TrashStack splittedTrashStack)
        {
            splittedTrashStack = null;
            if (indexToStart < 0 || indexToStart > Stack.Count)
            {
                return false;
            }

            if (indexToStart == 0)
            {
                splittedTrashStack = this;
                return true;
            }

            List<Trash> splitted = new List<Trash>();

            splittedTrashStack = Instantiate(m_settings.TrashStackPrefab, transform.position, transform.rotation).GetComponent<TrashStack>();

            while (Stack.Count > indexToStart)
            {
                Trash t = Stack.Pop();
                splitted.Add(t);
            }

            UpdateCollider();

            splitted.Reverse();
            for (int i = 0; i < splitted.Count; i++)
            {
                Trash t = splitted[i];
                splittedTrashStack.Push(t);
                t.transform.localPosition = new Vector3(0, Spawner.YOffsetPerObject * i, 0);
                t.transform.localRotation = Quaternion.Euler(0, Spawner.RotationPerObject * i, 0);
            }

            if (Stack.Count == 0)
            {
                Destroy(gameObject);
            }

            return true;
        }

        public bool TryMerge(TrashStack otherStack)
        {
            if (otherStack == null)
            {
                return false;
            }

            if (otherStack.Stack.Count == 1 && Stack.Count >= m_maxStack)
            {
                otherStack.GetComponent<TrashMover>().SwapState(TrashMover.MoveState.ThrownAway);
                otherStack.GetComponent<Rigidbody>().velocity = (Random.Range(-1.0f, 1.0f) < 0 ? -1 : 1) * m_throwAwayForce * otherStack.transform.right + Vector3.up;
            }
            else
            {
                List<Trash> listToAdd = new List<Trash>();
                List<Trash> listToRemove = new List<Trash>();
                while (otherStack.Stack.Count > 0)
                {
                    Trash trash = otherStack.Pop();

                    if (Stack.Count + listToAdd.Count < m_maxStack)
                    {
                        listToAdd.Add(trash);
                    }
                    else
                    {
                        listToRemove.Add(trash);
                    }
                }

                listToAdd.Reverse();
                foreach (Trash trash in listToAdd)
                {
                    Stack.Push(trash);
                    trash.transform.parent = transform;
                    trash.transform.localPosition = new Vector3(0, Spawner.YOffsetPerObject * (Stack.Count - 1), 0);
                    trash.transform.localRotation = Quaternion.Euler(0, Spawner.RotationPerObject * (Stack.Count - 1), 0);
                }

                UpdateCollider();
                
                foreach (Trash trash in listToRemove)
                {
                    //throw away
                    Vector3 spawnPos = Stack.Peek().transform.position + new Vector3(0, Spawner.YOffsetPerObject, 0);
                    TrashStack stack = Instantiate(m_settings.TrashStackPrefab, spawnPos , otherStack.transform.rotation).GetComponent<TrashStack>();

                    stack.transform.position = spawnPos;
                    stack.Push(trash);
                    trash.transform.localPosition = new Vector3(0, 0, 0);
                    trash.transform.localRotation = Quaternion.Euler(0, 0, 0);

                    stack.GetComponent<TrashMover>().SwapState(TrashMover.MoveState.ThrownAway);
                    stack.GetComponent<Rigidbody>().velocity = (Random.Range(-1.0f, 1.0f) < 0 ? -1 : 1) * m_throwAwayForce * stack.transform.right + Vector3.up;
                }

                Destroy(otherStack.gameObject);
            }

            return true;
        }

        public void Push(Trash t)
        {
            Stack.Push(t);
            t.transform.parent = transform;
            UpdateCollider();
        }

        public Trash Pop()
        {
            Trash t = Stack.Pop();
            UpdateCollider();
            return t;
        }

        private void UpdateCollider()
        {
            m_collider.center = new Vector3(0, (Spawner.YOffsetPerObject * Stack.Count) / 2, 0);
            m_collider.size = new Vector3(m_collider.size.x, Spawner.YOffsetPerObject * Stack.Count, m_collider.size.z);
        }
    }
}