using UnityEngine;
using Trash;

public class MachineSpreader : Machine
{
    public float pullUpHeight = 0.1f;
    public float pullUpSpeed = 0.01f;

    public TrashStack holdingStack;

    public void activate()
    {
        if (dontWork)
            return;

        trashStack = sensor.GetCurrentTrashStack();

        if (trashStack.Stack.Count < 2)
            return;

        // split off all the upper elements and hold them up slowly
        TrashStack topTrashStack;
        trashStack.TrySplit(1, out topTrashStack);

        TrashMover topTrashStackMover;
        topTrashStackMover = topTrashStack.GetComponent<TrashMover>();
        topTrashStackMover.Push(Vector3.up * pullUpHeight, pullUpSpeed);

        /*
        if (holdingStack != null)
        {
            trashStack.TryMerge(holdingStack);
        }
        else
            holdingStack = trashStack;
        */

    }
}
