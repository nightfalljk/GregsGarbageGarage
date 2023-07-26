using UnityEngine;
using VFX;

public class MachinePusher : Machine
{
    public float pushDistance = 1f;
    public float pushSpeed = 1f;
    
    [SerializeField]
    private PusherAnimator m_pusherAnimator;

    public bool IsColor;
    
    public void activate()
    {
        if (dontWork)
            return;

        trashStack = sensor.GetCurrentTrashStack();

        TrashMover trashMover = trashStack.GetComponentInChildren<TrashMover>();

        if (trashMover.GetMoveState() != TrashMover.MoveState.ThrownAway)
        {
            Vector3 machineToTrash = trashMover.transform.position - this.transform.position;
            Vector3 machineToTrash2D = getHighestDirectionXZ(machineToTrash);
            trashMover.Push(machineToTrash2D * pushDistance, pushSpeed, percentage => m_pusherAnimator.SetExtension(percentage), () => m_pusherAnimator.SetExtension(0));
        }
    }
}
