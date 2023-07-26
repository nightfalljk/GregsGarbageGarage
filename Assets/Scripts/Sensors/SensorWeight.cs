using UnityEngine;
using Trash;
using Trash.Properties;

public class SensorWeight : Sensor
{
    public float m_maxWeight = 1f;

    [SerializeField]
    private WeightPusherAnimator m_weightPusherAnimator;

    public override bool TestTrash(TrashStack _trashStack)
    {
        // don't detect trash that is being moved by machine except conveyorbelt
        TrashMover trashMover = _trashStack.GetComponent<TrashMover>();
        if (trashMover.GetMoveState() == TrashMover.MoveState.MovedByMachine)
            return false;

        // check accumulated weight of all trash in stack
        float totalWeight = 0;
        foreach (Trash.Trash currTrash in _trashStack.Stack)
        {
            foreach (TrashProperty currProperty in currTrash.Properties)
            {
                if (currProperty.GetType() == typeof(TrashWeight))
                    totalWeight += ((TrashWeight) currProperty).Value;
            }
        }

        if (totalWeight > m_maxWeight)
        {
            m_weightPusherAnimator.Weight = 0.5f + (_trashStack.Stack.Count-1)*0.2f;
            return true;
        }
        else
        {
            m_weightPusherAnimator.Weight = (_trashStack.Stack.Count-1)*0.05f;
            return false;
        }
    }
}