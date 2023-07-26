using System;
using UnityEngine;
using Trash;
using Trash.Properties;

public class SensorColor : Sensor
{
    public int m_checkColorIndex = 0;

    private Color m_checkColor;

    [SerializeField]
    private EyePusherAnimator m_eyePusherAnimator;

    public ColorReference[] m_colorReferences;
    
    private void Start()
    {
        m_eyePusherAnimator.SetColor(m_colorReferences[m_checkColorIndex].Color);
    }

    public override bool TestTrash(TrashStack _trashStack)
    {
        // don't detect trash that is being moved by machine except conveyorbelt
        TrashMover trashMover = _trashStack.GetComponent<TrashMover>();
        if (trashMover.GetMoveState() == TrashMover.MoveState.MovedByMachine)
            return false;

        // check color of top item
        Trash.Trash topTrash = _trashStack.Stack.Peek();
        Color currColor = Color.white;
        bool colorFound = false;
        foreach (TrashProperty currProperty in topTrash.Properties)
        {
            if (currProperty.GetType() == typeof(ColorReference))
            {
                currColor = ((ColorReference) currProperty).Color;
                colorFound = true;
                break;
            }
        }
        
        if (colorFound && currColor == m_checkColor)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RotateColor()
    {
        m_checkColorIndex = (m_checkColorIndex + 1) % m_colorReferences.Length;
        
        m_eyePusherAnimator.SetColor(m_colorReferences[m_checkColorIndex].Color);
        m_checkColor = m_colorReferences[m_checkColorIndex].Color;
    }

    public void SetColorAgain()
    {
        m_eyePusherAnimator.SetColor(m_colorReferences[m_checkColorIndex].Color);
    }
}