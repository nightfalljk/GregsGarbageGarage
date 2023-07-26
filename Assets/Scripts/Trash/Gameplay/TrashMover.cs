using System;
using System.Collections;
using ConveyorBelt;
using Trash;
using UnityEngine;

public class TrashMover : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_rigidbody;

    [SerializeField]
    private TrashConfig m_config;

    private Vector3 m_startPosition;
    private Vector3 m_movementDirection;

    private Vector3 m_targetPosition;

    private float m_timePassed = 0.0f;

    [SerializeField]
    private MoveState m_currentMoveState;

    [SerializeField]
    private bool m_mergeable = true;

    public enum MoveState
    {
        ConveyorBelt,
        Dropping,
        MovedByMachine,
        Grabbed,
        ThrownAway
    }

    private void Awake()
    {
        CalcNewPosition();
    }

    private void Update()
    {
        if (m_currentMoveState == MoveState.ConveyorBelt)
        {
            m_timePassed += Time.deltaTime;

            Vector3 newPos = m_startPosition + m_movementDirection * m_config.DistancePerSecond.Evaluate(m_timePassed);

            if (newPos == transform.position)
            {
                CalcNewPosition();
            }

            m_rigidbody.MovePosition(newPos);
        }
    }

    private void SetNewTargetPos(Transform tileTransform)
    {
        m_timePassed = 0.0f;
        transform.position = (tileTransform.position + new Vector3(0, 0.266f, 0));
        m_movementDirection = tileTransform.forward;
        m_startPosition = transform.position;
    }

    public void CalcNewPosition()
    {
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out RaycastHit hit, 2.0f, LayerMask.GetMask("ConveyorBelt")))
        {
            ConveyorBeltTile tile = hit.collider.GetComponent<ConveyorBeltTile>();

            transform.localRotation = Quaternion.identity;

            SetNewTargetPos(hit.transform);
            SwapState(MoveState.ConveyorBelt);
        }
        else
        {
            SwapState(MoveState.ThrownAway);
            m_rigidbody.AddForce(m_movementDirection * m_config.FallingOfConveyorBeltSpeed);
        }
    }

    public void Push(Vector3 _v3Direction, float _speed, Action<float> perFrameUpdate = null, Action onDone = null)
    {
        StartCoroutine(Pushing(_v3Direction, _speed,  perFrameUpdate,  onDone));
    }

    private float m_totalDistance;
    
    private IEnumerator Pushing(Vector3 _v3Direction, float _speed,Action<float> perFrameUpdate = null, Action onDone = null)
    {
        SwapState(MoveState.MovedByMachine);
        m_targetPosition = transform.position + _v3Direction;

        float remainingDistance = Vector3.Distance(transform.position, m_targetPosition);

        m_totalDistance = remainingDistance;
        
        while (remainingDistance > 0.01f)
        {
            transform.position += _v3Direction * Mathf.Min(Time.deltaTime * _speed, remainingDistance);

            // check if overshoot
            if (Vector3.Dot(transform.position - m_targetPosition, _v3Direction) > 0f)
                transform.position = m_targetPosition;

            remainingDistance = Vector3.Distance(transform.position, m_targetPosition);
            perFrameUpdate?.Invoke((m_totalDistance - remainingDistance)/m_totalDistance);
            yield return null;
        }

        onDone?.Invoke();
        CalcNewPosition();
    }

    public void SwapState(MoveState state)
    {
        if (state == m_currentMoveState)
        {
            return;
        }

        if (state == MoveState.ThrownAway)
        {
            m_mergeable = false;
        }

        if (state == MoveState.Grabbed)
        {
            m_mergeable = true;
        }

        if (state == MoveState.ConveyorBelt || state == MoveState.MovedByMachine || state == MoveState.Grabbed)
        {
            m_rigidbody.isKinematic = true;

            if (state == MoveState.Grabbed)
            {
                m_rigidbody.detectCollisions = false;
            }
        }
        else
        {
            m_rigidbody.isKinematic = false;
        }

        if (state != MoveState.Grabbed)
        {
            m_rigidbody.detectCollisions = true;
        }

        m_currentMoveState = state;
    }

    public MoveState GetMoveState()
    {
        return m_currentMoveState;
    }

    private void OnCollisionStay(Collision other)
    {
        if (m_mergeable)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("ConveyorBelt"))
            {
                    CalcNewPosition();
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("TrashStack"))
            {
                TrashMover trashMover = other.gameObject.GetComponent<TrashMover>();

                if (!trashMover.m_mergeable)
                {
                    return;
                }

                if (trashMover.m_currentMoveState == MoveState.Dropping)
                {
                    if (transform.position.y < trashMover.transform.position.y)
                    {
                        GetComponent<TrashStack>().TryMerge(other.gameObject.GetComponent<TrashStack>());
                    }
                }
                else if (trashMover.m_currentMoveState == MoveState.ConveyorBelt)
                {
                    if ((other.transform.position - transform.position).normalized == (m_targetPosition - transform.position).normalized)
                    {
                        GetComponent<TrashStack>().TryMerge(other.gameObject.GetComponent<TrashStack>());
                    }
                }
            }
        }
    }
}