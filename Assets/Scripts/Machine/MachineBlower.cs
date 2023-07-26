using UnityEngine;
using Trash;
using Trash.Properties;

public class MachineBlower : Machine
{
    public float pushDistance = 1f;
    public float pushSpeed = 1f;

    public void activate()
    {
        if (dontWork)
            return;

        trashStack = sensor.GetCurrentTrashStack();

        TrashMover trashMover = trashStack.GetComponentInChildren<TrashMover>();
        Vector3 machineToTrash = trashMover.transform.position - this.transform.position;
        Vector3 machineToTrash2D = getHighestDirectionXZ(machineToTrash);

        // if on top is medium/light item, split stack and push it
        Trash.Trash currTrash = trashStack.Stack.Peek();
        TrashWeight currWeight = (TrashWeight)currTrash.PropertiesDictionary[typeof(TrashWeight)];

        float blowDistance = 0f;
        if (currWeight.Value < 2.1f)
            blowDistance = 1f;
        if (currWeight.Value < 1.1f)
            blowDistance = 2f;

        if (blowDistance > 0f)
        {
            // split only if stack > 1
            TrashMover topTrashStackMover;
            if (trashStack.Stack.Count > 1)
            {
                TrashStack topTrashStack;
                trashStack.TrySplit(trashStack.Stack.Count - 1, out topTrashStack);
                topTrashStackMover = topTrashStack.GetComponent<TrashMover>();
            }
            else
            {
                topTrashStackMover = trashStack.GetComponent<TrashMover>();
            }

            // push away
            topTrashStackMover.Push(machineToTrash2D * pushDistance * blowDistance, pushSpeed * blowDistance);

            int a = 0;
        }
    }
}
