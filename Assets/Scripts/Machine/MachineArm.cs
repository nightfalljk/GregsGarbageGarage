using System.Collections;
using UnityEngine;
using Trash;

public class MachineArm : Machine
{
    public float sideSpeed = 1f;
    public float cooldown = 2f;

    private bool onCooldown = false;

    public void activate()
    {
        if (dontWork)
            return;

        if (onCooldown)
            return;
        onCooldown = true;
        StartCoroutine(CooldownWait());

        trashStack = sensor.GetCurrentTrashStack();
        TrashMover trashMover = trashStack.GetComponentInChildren<TrashMover>();

        // take upper element and move it
        TrashStack topTrashStack;
        trashStack.TrySplit(trashStack.Stack.Count - 1, out topTrashStack);

        // move to side and drop stack
        TrashMover topTrashStackMover;
        topTrashStackMover = topTrashStack.GetComponent<TrashMover>();
        topTrashStackMover.Push(-transform.forward * 2f, sideSpeed);

    }

    private IEnumerator CooldownWait()
    {
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }
}
