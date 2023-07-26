using Trash;

public class SensorConstant : Sensor
{
    public override bool TestTrash(TrashStack _trashStack)
    {
        // don't detect trash that is being moved by machine except conveyorbelt
        TrashMover trashMover = _trashStack.GetComponent<TrashMover>();
        if (trashMover.GetMoveState() == TrashMover.MoveState.MovedByMachine)
            return false;

        return true;
    }
}
