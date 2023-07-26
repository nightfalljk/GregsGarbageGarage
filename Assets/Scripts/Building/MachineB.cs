using System;
using System.Collections.Generic;
using UnityEngine;

public class MachineB : MonoBehaviour
{
    private int _id = -1;
    public int Id
    {
        get {
            if (_id != -1)
            {
                return _id;
            }
            else
            {
                _id = gameObject.GetComponent<BuildingIdentifier>().GetId();
                return _id;
            }
        }
        set { _id = value; }
    }

    //x is y 
    public bool[,] shape
    {
        get {
            if (_shape != null)
            {
                return _shape;
            }
            else
            {
                Init();
                return _shape;
            }
        }
        set { _shape = value; }
    }

    private bool[,] _shape = null;
    //defines pivot corner
    //0 -> left bottom
    //1 -> left top
    //2 -> right top
    //3 -> right bottom
    public int rotation;

    public List<BoolList> rowsInXRight;
    [SerializeField] private bool allowDestroy = true;
    
    [Serializable]
    public struct BoolList
    {
        public List<bool> columnsInZUp;
    }

    public void Awake()
    {
        //Init();
    }

    public void Init()
    {
        int sizeX = rowsInXRight.Count;
        int sizeY = rowsInXRight[0].columnsInZUp.Count;
        shape = new bool[sizeX, sizeY];
        for (int indexX = 0; indexX < sizeX; indexX++)
        {
            for (int indexY = 0; indexY < sizeY; indexY++)
            {
                shape[indexX,indexY] = rowsInXRight[indexX].columnsInZUp[indexY];
            }
        }
        //Debug.Log("Layout of "+gameObject.name+": ");
        //PrintLayout(shape);
    }

    public static void PrintLayout(bool[,] shape)
    {
        int sizeX = shape.GetLength(0);
        int sizeY = shape.GetLength(1);
        String s = "";
//        Debug.Log("sizeX: "+sizeX);
//        Debug.Log("sizeY: "+sizeY);
        for (int indexY = sizeY-1; indexY >=0; indexY--)
        {
            for (int indexX = 0; indexX < sizeX; indexX++)
            {
                if (shape[indexX, indexY])
                {
                    s += "x ";
                }
                else
                {
                    s += "o ";
                }
            }

            s += "\n";
        }
        //Debug.Log(s);
    }

    public bool AllowDestroy()
    {
        return allowDestroy;
    }
}
