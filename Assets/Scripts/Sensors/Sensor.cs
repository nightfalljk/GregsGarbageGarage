using UnityEngine;
using Trash;
using UnityEngine.Events;

public class Sensor : MonoBehaviour
{
    public UnityEvent OnTriggered;

    private TrashStack currentTrashStack; // machine gets this one

    private Vector3 currTrashPosFlat;
    private Vector3 currPosFlat;

    public virtual bool TestTrash(TrashStack _trashStack)
    {
        // implement this in a child class
        return false;
    }

    public TrashStack GetCurrentTrashStack()
    {
        return currentTrashStack;
    }

    public void OnTriggerStay(Collider other)
    {
        currentTrashStack = other.GetComponent<TrashStack>();

        if (currentTrashStack == null)
            return;

        // make positions 2d (flat)
        currTrashPosFlat = currentTrashStack.transform.position;
        currTrashPosFlat.y = 0f;
        currPosFlat = this.transform.position;
        currPosFlat.y = 0f;

        // only sense if trash is in middle of sensor
        float distanceSensorToTrash = Vector3.Distance(currTrashPosFlat, currPosFlat);
        if (distanceSensorToTrash < 0.1f)
        {
            if (TestTrash(currentTrashStack))
            {
                OnTriggered.Invoke();
            }
        }
    }
}