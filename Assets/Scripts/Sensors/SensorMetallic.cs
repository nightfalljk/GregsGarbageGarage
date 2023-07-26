using Trash;
using Trash.Properties;

public class SensorMetallic : Sensor
{
    public override bool TestTrash(TrashStack _trashStack)
    {
        // don't detect trash that is being moved by machine except conveyorbelt
        TrashMover trashMover = _trashStack.GetComponent<TrashMover>();
        if (trashMover.GetMoveState() == TrashMover.MoveState.MovedByMachine)
            return false;

        // check if there is one metallic item
        foreach (Trash.Trash currTrash in _trashStack.Stack)
        {
            TrashMagnetism currMagnetism = (TrashMagnetism)currTrash.PropertiesDictionary[typeof(TrashMagnetism)];
            if (currMagnetism.IsMagnetism)
                return true;
        }

        return false;
    }
}
