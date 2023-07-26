using UnityEngine;
using UnityEngine.InputSystem;

namespace Trash.Gameplay
{
    public class TrashStackMouseInteraction : MonoBehaviour, IInteractable
    {
        private bool m_grabbed;

        [SerializeField]
        private float m_distanceHover;

        [SerializeField]
        private TrashStack m_trashStack;

        [SerializeField]
        private TrashMover m_trashMover;

        [SerializeField]
        private Material m_hover;

        private GameObject m_grabbedObject;

        private void Update()
        {
            if (m_grabbed)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hit, Mathf.Infinity, ~LayerMask.GetMask("Grabbed")))
                {
                    m_grabbedObject.transform.position = hit.point + Vector3.up * m_distanceHover;
                }
            }
        }

        public void OnHoverEnter()
        {
            foreach (Trash trash in m_trashStack.Stack)
            {
                MeshRenderer mr = trash.GetComponentInChildren<MeshRenderer>();
                mr.material = m_hover;
            }
        }

        public void OnHoverExit()
        {
            foreach (Trash trash in m_trashStack.Stack)
            {
                if (trash == null)
                {
                    continue;
                }

                trash.GetComponentInChildren<MeshRenderer>().material = trash.OriginalMaterial;
            }
        }

        public void OnMouseDown()
        {
            if (m_trashStack.TrySplit(m_trashStack.Stack.Count - 1, out TrashStack newStack))
            {
                TrashStackMouseInteraction trashStackInteraction = newStack.GetComponent<TrashStackMouseInteraction>();

                trashStackInteraction.m_trashMover.SwapState(TrashMover.MoveState.Grabbed);
                trashStackInteraction.m_grabbedObject = newStack.gameObject;
                trashStackInteraction.m_grabbed = true;
                trashStackInteraction.m_grabbedObject.layer = LayerMask.NameToLayer("Grabbed");

                MouseInteraction mouseInteraction = FindObjectOfType<MouseInteraction>();
                mouseInteraction.SwapHoldingObject(trashStackInteraction);
            }
        }

        public void OnMouseUp()
        {
            m_grabbed = false;

            if (m_grabbedObject)
            {
                m_grabbedObject.GetComponent<TrashMover>().SwapState(TrashMover.MoveState.Dropping);
                m_grabbedObject.layer = LayerMask.NameToLayer("TrashStack");
            }

            m_grabbedObject = null;
        }
    }
}