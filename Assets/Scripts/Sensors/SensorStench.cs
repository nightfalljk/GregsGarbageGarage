using Trash;
using Trash.Properties;

public class SensorStench : Sensor
{
    public override bool TestTrash(TrashStack _trashStack)
    {
        // don't detect trash that is being moved by machine except conveyorbelt
        TrashMover trashMover = _trashStack.GetComponent<TrashMover>();
        if (trashMover.GetMoveState() == TrashMover.MoveState.MovedByMachine)
            return false;

        // check if there is one stenchy item
        foreach (Trash.Trash currTrash in _trashStack.Stack)
        {
            foreach (TrashProperty currProperty in currTrash.Properties)
            {
                if (currProperty.GetType() == typeof(TrashStench)
                    && ((TrashStench)currProperty).IsStenchy)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
