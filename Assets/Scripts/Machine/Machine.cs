using UnityEngine;
using Trash;

public class Machine : MonoBehaviour
{
    public Sensor sensor; // that activates the machine
    protected TrashStack trashStack; // the trash that the machine is working on
    protected bool dontWork = false;

    public void TurnOn()
    {
        dontWork = false;
    }

    public void TurnOff()
    {
        dontWork = true;
    }

    public void TurnOnOff(bool turnOn)
    {
        dontWork = !turnOn;
    }

    public void ToggleOnOff()
    {
        dontWork = !dontWork;
    }

    // =============== HELPERS ===============

    public Vector3 getHighestDirection(Vector3 _v3Input)
    {
        int iMaxDirection = 0;
        if (Mathf.Abs(_v3Input[1]) > Mathf.Abs(_v3Input[0]) && Mathf.Abs(_v3Input[1]) > Mathf.Abs(_v3Input[2]))
            iMaxDirection = 1;
        if (Mathf.Abs(_v3Input[2]) > Mathf.Abs(_v3Input[0]) && Mathf.Abs(_v3Input[2]) > Mathf.Abs(_v3Input[1]))
            iMaxDirection = 2;

        Vector3 outputDir = Vector3.zero;
        outputDir[iMaxDirection] = 1f * Mathf.Sign(outputDir[iMaxDirection]);

        return outputDir;
    }

    public Vector3 getHighestDirectionXZ(Vector3 _v3Input)
    {
        int iMaxDirection = 0;
        if (Mathf.Abs(_v3Input[2]) > Mathf.Abs(_v3Input[0]))
            iMaxDirection = 2;

        Vector3 outputDir = Vector3.zero;
        outputDir[iMaxDirection] = 1f * Mathf.Sign(_v3Input[iMaxDirection]);

        return outputDir;
    }
}


