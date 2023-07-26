using UnityEngine;
using UnityEngine.InputSystem;

namespace Trash.Gameplay
{
    public class MouseInteraction : MonoBehaviour
    {
        [SerializeField]
        private Camera m_camera;

        private IInteractable m_interactable;
        private MouseState m_state = MouseState.Nothing;

        private enum MouseState
        {
            Nothing,
            HoveringObject,
            HoldingObject,
            SelectedObject,
            SellingMode
        }

        [SerializeField]
        private Texture2D m_handIdle;

        [SerializeField]
        private Texture2D m_handHover;

        [SerializeField]
        private Texture2D m_handGrab;

        [SerializeField] private Texture2D m_handSell;
        
        private Vector2 m_pivot;

        public bool allowGrabbing = true;
        private GridBasedBuilding gbb;
        
        private void Awake()
        {
            m_pivot = new Vector2(m_handIdle.width / 2.0f, m_handIdle.height / 2.0f);
            Cursor.SetCursor(m_handIdle,m_pivot , CursorMode.Auto);
            gbb = FindObjectOfType<GridBasedBuilding>();
        }

        private void Update()
        {
            bool hitted = Physics.Raycast(m_camera.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hit);

            if (Mouse.current.rightButton.wasPressedThisFrame && hitted)
            {
                if (hit.transform.CompareTag("ColorMachine"))
                {
                    SensorColor sensor = hit.transform.GetComponentInChildren<SensorColor>();
                    if(sensor)
                    {
                        sensor.RotateColor();
                    }
                }
            }

            switch (m_state)
            {
                case MouseState.Nothing:
                {
                    if (hitted && allowGrabbing)
                    {
                        IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                        if (interactable == null)
                        {
                            break;
                        }

                        m_interactable?.OnHoverExit();
                        interactable.OnHoverEnter();
                        m_interactable = interactable;
                        m_state = MouseState.HoveringObject;

                        Cursor.SetCursor(m_handHover, m_pivot, CursorMode.Auto);

                        if (Mouse.current.leftButton.wasPressedThisFrame)
                        {
                            m_interactable.OnMouseDown();
                            m_state = MouseState.HoldingObject;
                            Cursor.SetCursor(m_handGrab, Vector3.zero, CursorMode.Auto);
                        }
                    }
                    else
                    {
                        if (gbb.GetBuildMode() == GridBasedBuilding.BuildMode.delete)
                        {
                            m_state = MouseState.SellingMode;
                            Cursor.SetCursor(m_handSell, m_pivot, CursorMode.Auto);
                        }
                    }
                    break;
                }

                case MouseState.HoveringObject:
                {
                    if (!hitted)
                    {
                        m_interactable?.OnHoverExit();
                        m_interactable = null;
                        m_state = MouseState.Nothing;
                        Cursor.SetCursor(m_handIdle,m_pivot, CursorMode.Auto);
                    }
                    else
                    {
                        IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                        if (interactable == null)
                        {
                            m_interactable?.OnHoverExit();
                            m_interactable = null;
                            m_state = MouseState.Nothing;
                            Cursor.SetCursor(m_handIdle, m_pivot, CursorMode.Auto);
                            break;
                        }

                        if (interactable != m_interactable)
                        {
                            m_interactable?.OnHoverExit();
                            interactable.OnHoverEnter();
                            m_interactable = interactable;
                        }

                        if (Mouse.current.leftButton.wasPressedThisFrame)
                        {
                            m_interactable.OnMouseDown();
                            m_state = MouseState.HoldingObject;
                            Cursor.SetCursor(m_handGrab, Vector3.zero, CursorMode.Auto);
                        }
                    }

                    break;
                }
                case MouseState.HoldingObject:
                {
                    if (Mouse.current.leftButton.wasReleasedThisFrame)
                    {
                        m_state = MouseState.Nothing;
                        Cursor.SetCursor(m_handIdle, m_pivot, CursorMode.Auto);
                        m_interactable?.OnHoverExit();
                        m_interactable?.OnMouseUp();
                        m_interactable = null;
                    }

                    break;
                }
                case MouseState.SellingMode:
                    if (gbb.GetBuildMode() != GridBasedBuilding.BuildMode.delete)
                    {
                        m_state = MouseState.Nothing;
                        Cursor.SetCursor(m_handIdle, m_pivot,CursorMode.Auto);
                    }
                    break;
            }
        }

        public void SwapHoldingObject(IInteractable interactable)
        {
            if (interactable == m_interactable)
            {
                return;
            }

            m_interactable.OnHoverExit();
            m_interactable.OnMouseUp();
            m_interactable = interactable;
            m_interactable.OnHoverEnter();
        }
    }
}