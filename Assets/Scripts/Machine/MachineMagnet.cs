using System.Collections;
using UnityEngine;
using Trash;
using Trash.Properties;

public class MachineMagnet : Machine
{
    public float pullUpHeight = 1f;
    public float pullUpSpeed = 1f;
    public float sideSpeed = 1f;

    public float cooldown = 2f;

    private bool onCooldown = false;

    [SerializeField]
    private MetalMoverBehaviour m_metalMoverBehaviour;
    
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

        // divide stack at metallic object
        // - get id of lowest metallic object
        int idMetallic = 99;
        Trash.Trash[] arTrash = trashStack.Stack.ToArray();
        for (int i = 0; i < arTrash.Length; i++)
        {
            TrashMagnetism currMagnetism = (TrashMagnetism) arTrash[i].PropertiesDictionary[typeof(TrashMagnetism)];
            if (currMagnetism.IsMagnetism)
            {
                idMetallic = i;
                break;
            }
        }
        if (idMetallic > 5)
            return;
        // - divide stack
        TrashStack topTrashStack;
        trashStack.TrySplit(idMetallic, out topTrashStack);

        // move to side and drop stack
        TrashMover topTrashStackMover;
        topTrashStackMover = topTrashStack.GetComponent<TrashMover>();
        topTrashStackMover.Push(-transform.forward * 2f, sideSpeed, null, () => m_metalMoverBehaviour.IsActive = false);

        // pull upper half up
        trashStack.GetComponent<Rigidbody>().MovePosition(transform.position + Vector3.up * pullUpHeight);

        m_metalMoverBehaviour.IsActive = true;
    }

    private IEnumerator CooldownWait()
    {
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }
}
