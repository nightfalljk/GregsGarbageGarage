using UnityEngine;

public class Cell
{
    public bool occuppied;
    private Cell containingBaseOfObject;
    private GameObject go;
    public MachineB MachineB;

    public void SetMachine(GameObject gameObject, MachineB machineB)
    {
        go = gameObject;
        occuppied = true;
        this.MachineB = machineB;
    }

    public void SetMachineOuterpart(Cell baseOfObject)
    {
        occuppied = true;
        containingBaseOfObject = baseOfObject;
    }

    public Cell GetMain()
    {
        return containingBaseOfObject;
    }

    public void Free()
    {
        occuppied = false;
        containingBaseOfObject = null;
        go = null;
        MachineB = null;
    }
}
