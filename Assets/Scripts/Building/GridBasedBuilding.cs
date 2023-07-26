using System;
using System.Collections;
using System.Collections.Generic;
using Trash.Gameplay;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

//using UnityEngine.Experimental.In

public class GridBasedBuilding : MonoBehaviour
{
    [SerializeField] private int gridSize;
    [SerializeField] private Camera mainCam;
    //Size of one Cell
    [SerializeField] private float cellSize = 1;
    [SerializeField] private Vector3 origin;
    //[SerializeField] private GameObject buildPlanePrefab;
    [SerializeField] private LayerMask colliding;
    [SerializeField] private GameObject SpawnerPrefab;
    [SerializeField] private GameObject DestroyerPrefab;
    
    [SerializeField] private Material allow;
    [SerializeField] private Material notAllow;
    [SerializeField] private Material normal;
    
    private GameObject selectedBlueprintPrefab;
    public GameObject[] blueprints;
    public int selectPrefab = 0;
    
    MachineB _spawnedHoverMachineB;
    
    [SerializeField] private GameObject buildPlane;
    private GameObject blueprintHoverObject;
    
    //[x,y] x to right. y up
    private Cell[,] grid;
    //private bool buildMode = false;

    private BuildMode buildmode = BuildMode.off;
    
    bool doReplace = false;
    private bool allowBuild = true;

    private MachineB lastToDelete;
    
    public enum BuildMode
    {
        off,
        build,
        delete
    }
    
    public void Start()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }
        Init(gridSize);
        SetBuildmode(BuildMode.off);

        BuildMenuOptions.OnBuildOptionsSetActive.AddListener(PreventBuild);
    }

    public void Update()
    {
        if (Keyboard.current.bKey.wasReleasedThisFrame)
        {
            SetBuildmode(BuildMode.build, blueprints[selectPrefab]);
        }
        if (Keyboard.current.nKey.wasReleasedThisFrame)
        {
            SetBuildmode(BuildMode.delete);
        }
        if (Keyboard.current.mKey.wasReleasedThisFrame)
        {
            SetBuildmode(BuildMode.off);
        }
        
        if (buildmode == BuildMode.build)
        {
            Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            bool canBuildNow = false;
            if (Physics.Raycast(ray, out hit,100,colliding))
            {
                if (Keyboard.current.rKey.wasReleasedThisFrame)
                {
                    //visual
                    blueprintHoverObject.transform.Rotate(Vector3.up,90,Space.Self);
                    //logical
                    _spawnedHoverMachineB.shape = RotateShape90Left(_spawnedHoverMachineB.shape);
                    _spawnedHoverMachineB.shape = RotateShape90Left(_spawnedHoverMachineB.shape);
                    _spawnedHoverMachineB.shape = RotateShape90Left(_spawnedHoverMachineB.shape);
                    MachineB.PrintLayout(_spawnedHoverMachineB.shape);
                    _spawnedHoverMachineB.rotation += 1;
                    _spawnedHoverMachineB.rotation = _spawnedHoverMachineB.rotation % 4;
                    
                }
                Vector3 posInWorld = new Vector3(0,0,0);
                if (hit.collider.gameObject == buildPlane)
                {
                    posInWorld = hit.point;
                    OnBuildingMousePos(posInWorld);
                    Vector2Int inGrid = ToGridPos(posInWorld);
                    canBuildNow = CanBuildHere(inGrid, _spawnedHoverMachineB);
                }
                else
                {
                    canBuildNow = false;
                }

                if (Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    Vector2Int pivotInGrid = ToGridPos(posInWorld);
                    if (canBuildNow && allowBuild)
                    {
                        if (!EnoughMoney(_spawnedHoverMachineB))//TODO test EnoughMoney
                        {
                            //Debug.Log("Not enough Money to build");
                        }
                        else
                        {
                            if (doReplace)
                            {
                                Debug.Log("is replacing");
                                Vector2Int inGrid = ToGridPos(posInWorld);
                                MachineB toDelete = grid[inGrid.x, inGrid.y].GetMain().MachineB;
                                Destroy(toDelete.gameObject);
                                //TODO Trash inside?
                                //Free all Spaces
                                FreeSpaces(toDelete, pivotInGrid);
                                
                                
                                bool[,] shape = _spawnedHoverMachineB.shape.Clone() as bool[,];
                                SpawnPrefab(pivotInGrid, selectedBlueprintPrefab,
                                    blueprintHoverObject.transform.position, blueprintHoverObject.transform.rotation, shape);
                            }
                            else
                            {
                                Buy(_spawnedHoverMachineB);
                                bool[,] shape = _spawnedHoverMachineB.shape.Clone() as bool[,];
                                SpawnPrefab(pivotInGrid, selectedBlueprintPrefab,
                                    blueprintHoverObject.transform.position, blueprintHoverObject.transform.rotation, shape);
                            }

                        }
                        
                    }
                    else
                    {
                        Debug.Log("can't build here or now: ");
                    }
                        
                }

                if (Mouse.current.rightButton.wasReleasedThisFrame)
                {
                    SetBuildmode(BuildMode.off);
                }
            }
        }
        else if(buildmode == BuildMode.delete)
        {
            Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            MachineB toDelete = null;
            if (Physics.Raycast(ray, out hit, 100, colliding))
            {
                bool canDelete = false;
                
                Vector3 posInWorld = new Vector3(0,0,0);
                if (hit.collider.gameObject == buildPlane)
                {
                    posInWorld = hit.point;
                    //OnBuildingMousePos(posInWorld);
                    Vector2Int inGrid = ToGridPos(posInWorld);
                    if (grid[inGrid.x, inGrid.y].occuppied)
                    {
                        MachineB lookedAt = grid[inGrid.x, inGrid.y].GetMain().MachineB;
                        if (lookedAt.AllowDestroy())
                        {
                            canDelete = true;
                            toDelete = grid[inGrid.x, inGrid.y].GetMain().MachineB;
                            if (toDelete != lastToDelete)
                            {
                                if (lastToDelete != null)
                                {
                                    DefaultColorEffect(lastToDelete);
                                }
                                ColorEffect(false, toDelete);
                                lastToDelete = toDelete;
                            }
                        }
                        else
                        {
                            canDelete = false;
                            toDelete = null;
                        }
                    }
                    else
                    {
                        canDelete = false;
                        toDelete = null;
                    }
                }
                else
                {
                    canDelete = false;
                }

                Vector2Int pivotInGrid = ToGridPos(posInWorld);
                
                if (Mouse.current.leftButton.wasReleasedThisFrame && canDelete)
                {
                    Sell(toDelete);
                    //Delete Object
                    Destroy(toDelete.gameObject);
                    //TODO Trash inside?
                    //Free all Spaces
                    FreeSpaces(toDelete, pivotInGrid);
                    //PrintLayout(grid);
                }
                if (Mouse.current.rightButton.wasReleasedThisFrame)
                {
                    SetBuildmode(BuildMode.off);
                }
            }
            if (lastToDelete != null && lastToDelete != toDelete)
            {
                DefaultColorEffect(lastToDelete);
                lastToDelete = null;
            }
        }
    }

    public void Init(int size)
    {
        grid = new Cell[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                grid[i,j] = new Cell();
            }
        }
        //buildPlane = Instantiate(buildPlanePrefab,new Vector3(0,0,0),Quaternion.identity);
        
        
//        //TODO Predefined machines on the map
//        Vector2Int destroyer1 = new Vector2Int(5,10);
//        bool[,] shape1 = DestroyerPrefab.GetComponent<MachineB>().shape.Clone() as bool[,];
//        SpawnPrefab(destroyer1,DestroyerPrefab,ToWorldPos(destroyer1), DestroyerPrefab.transform.rotation, shape1);
//        
//        Vector2Int destroyer2 = new Vector2Int(2,10);
//        bool[,] shape2 = DestroyerPrefab.GetComponent<MachineB>().shape.Clone() as bool[,];
//        SpawnPrefab(destroyer2,DestroyerPrefab,ToWorldPos(destroyer2), DestroyerPrefab.transform.rotation, shape2);
//        
//        Vector2Int destroyer3 = new Vector2Int(8,10);
//        bool[,] shape3 = DestroyerPrefab.GetComponent<MachineB>().shape.Clone() as bool[,];
//        SpawnPrefab(destroyer3,DestroyerPrefab,ToWorldPos(destroyer3), DestroyerPrefab.transform.rotation, shape3);
    }

    private void OnBuildingMousePos(Vector3 pos)
    {
        Vector2 inGrid = ToGridPos(pos);
        Vector3 snapped = ToWorldPos(inGrid);
        //Debug.Log("snapped" + snapped);
        blueprintHoverObject.transform.position = snapped;
    }

    private Vector2Int ToGridPos(Vector3 worldPos)
    {
        float halfSize = (float)gridSize / 2;
        float halfCell = cellSize / 2;
        Vector3 v = new Vector3();
        
        
        v -= new Vector3(1, 0, 1);
        v -= new Vector3(-halfCell, 0, -halfCell);
        v += (worldPos - origin-new Vector3(-halfCell, 0, -halfCell)) / (cellSize);
        v += new Vector3(halfSize, 0, halfSize);
        //Debug.Log("worldpos: " + v);
        Vector2Int result = new Vector2Int(RoundToZero(v.x),RoundToZero(v.z));
        //result *= -1;
        return result;
    }

    private Vector3 ToWorldPos(Vector2 pos)
    {
        float halfSize = (float)gridSize / 2;
        float halfCell = cellSize / 2;
        Vector3 result = new Vector3();
        //pos *= -1;
        //pos += new Vector2(-1, -1);
        result += origin + (cellSize * new Vector3(pos.x, buildPlane.transform.position.y, pos.y));
        result -= new Vector3(halfSize,0,halfSize);
        result += new Vector3(-halfCell, 0, -halfCell);
        result += new Vector3(1, 0, 1);
        return result;
    }

    private int RoundToZero(float input)
    {
        if (input >= 0)
        {
            return (int) input;
        }
        else
        {
            return ((int) input)-1;
        }
    }

    private bool CanBuildHere(Vector2Int pos, MachineB blueprintMachineB)
    {
        bool result = false;
        if (grid[pos.x, pos.y].occuppied)
        {
            //test if both conveyer
            if (grid[pos.x, pos.y].MachineB.Id == blueprintMachineB.Id && blueprintMachineB.Id == 1)//TODO conveyorbelt hardcoded
            {
                result = true;
                doReplace = true;
            }
            else
            {
                result = false;
                doReplace = false;
            }
        }
        else
        {
            doReplace = false;
            if (fitGrid(blueprintMachineB.shape, blueprintMachineB.rotation, pos))
            {
                
                result = true;
            }
            else
            {
                result = false;
            }
            
            //return true;
        }

        ColorEffect(result, _spawnedHoverMachineB);
        
        return result;
    }

    private bool fitGrid(bool[,] shape, int rot, Vector2Int pos)
    {
        bool result = true;
        
        int inX = shape.GetLength(0);
        int inY = shape.GetLength(1);

        int addx = 0;
        int addz = 0;
        
        //offset for rest of cells viewd on fised pivot after rotation
        switch (rot)
        {
            case 0:
                break;
            case 1:
                addz = -(inY - 1);
                break;
            case 2:
                addx = -(inX - 1);
                addz = -(inY - 1);
                break;
            case 3:
                addx = -(inX - 1);
                break;
            default:
                Debug.Log("rot is: "+rot);
                break;
        }
        
        //check for outside of grid
        //offset is bottom left corner of shape (not mattering how it is rotated)
        Vector2Int offset = new Vector2Int(addx,addz);
        //RectInt gridRect = new RectInt(new Vector2Int(0,0), new Vector2Int(gridSize, gridSize) );
        RectInt shapeRect = new RectInt(pos+offset,new Vector2Int(inX,inY));

        if (shapeRect.min.x < 0 || shapeRect.min.y < 0 || shapeRect.max.x > gridSize || shapeRect.max.y > gridSize)
        {
            Debug.Log("Size does not fit");
            return false;
        }

        //Check for all tiles space
        for (int i = 0; i < shapeRect.width; i++)
        {
            for (int j = 0; j < shapeRect.height; j++)
            {
                int xInGridSpace = shapeRect.x + i;
                int yInGridSpace = shapeRect.y + j;
                if (shape[i, j])
                {
                    if (grid[xInGridSpace, yInGridSpace].occuppied)
                    {
                        return false;
                    }
                }
            }
        }

        return true;

    }

    private bool[,] RotateShape90Left(bool[,] shape)
    {
        bool[,] newMatrix = new bool[shape.GetLength(1), shape.GetLength(0)];
        int newColumn, newRow = 0;
        for (int oldColumn = shape.GetLength(1) - 1; oldColumn >= 0; oldColumn--)
        {
            newColumn = 0;
            for (int oldRow = 0; oldRow < shape.GetLength(0); oldRow++)
            {
                newMatrix[newRow, newColumn] = shape[oldRow, oldColumn];
                newColumn++;
            }
            newRow++;
        }
        return newMatrix;
    }
    
    
    public void SetMachineBlueprint(GameObject prefab, GameObject spawnedHover)
    {
        selectedBlueprintPrefab = prefab;
        _spawnedHoverMachineB = spawnedHover.GetComponent<MachineB>();
    }

    public void SetBuildmode(BuildMode b, GameObject prefab = null)
    {
        switch (b)
        {
            case BuildMode.off:
                FindObjectOfType<MouseInteraction>().allowGrabbing = true;
                EndHover();
                break;
            case BuildMode.build:
                FindObjectOfType<MouseInteraction>().allowGrabbing = false;
                EndHover();
                if (prefab == null)
                {
                    Debug.Log("No Prefab set to build");
                    return;
                }

                selectedBlueprintPrefab = prefab;//blueprints[0];
                blueprintHoverObject = Instantiate(selectedBlueprintPrefab);
                blueprintHoverObject.GetComponentInChildren<Machine>()?.TurnOff();
                SetMachineBlueprint(blueprintHoverObject, blueprintHoverObject);
                break;
            case BuildMode.delete:
                FindObjectOfType<MouseInteraction>().allowGrabbing = false;
                EndHover();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        buildmode = b;
    }

    public void EndHover()
    {
        if (blueprintHoverObject != null)
        {
            Destroy(blueprintHoverObject);
            blueprintHoverObject = null;
        }
    }
    
    public static void PrintLayout(Cell[,] cells)
    {
        bool[,] b = new bool[cells.GetLength(0),cells.GetLength(1)];
        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                b[i,j] = cells[i,j].occuppied;
            }
        }
        MachineB.PrintLayout(b);
    }

    public void FreeSpaces(MachineB m, Vector2Int pos)
    {
        int inX = m.shape.GetLength(0);
        int inY = m.shape.GetLength(1);

        int addx = 0;
        int addz = 0;
        
        //offset for rest of cells viewd on fised pivot after rotation
        switch (m.rotation)
        {
            case 0:
                break;
            case 1:
                addz = -(inY - 1);
                break;
            case 2:
                addx = -(inX - 1);
                addz = -(inY - 1);
                break;
            case 3:
                addx = -(inX - 1);
                break;
            default:
                Debug.Log("rot is: "+m.rotation);
                break;
        }
        
        //check for outside of grid
        //offset is bottom left corner of shape (not mattering how it is rotated)
        Vector2Int offset = new Vector2Int(addx,addz);
        RectInt shapeRect = new RectInt(pos+offset,new Vector2Int(inX,inY));
        for (int i = 0; i < shapeRect.width; i++)
        {
            for (int j = 0; j < shapeRect.height; j++)
            {
                int xInGridSpace = shapeRect.x + i;// -rotationOffset.x;
                int yInGridSpace = shapeRect.y + j;// -rotationOffset.y;
                if (m.shape[i, j])
                {
                    grid[xInGridSpace, yInGridSpace].Free();
                }
            }
        }
        PrintLayout(grid);
    }

    public void ColorEffect(bool b, MachineB go)
    {
        if (b)
        {
            foreach (Renderer rendererInChild in go.gameObject.GetComponentsInChildren<Renderer>())
            {
                List<Material> newMats = new List<Material>();
                foreach (var mat in rendererInChild.materials)
                {
                    if (mat.name.Contains("Environment"))
                    {
                        newMats.Add(allow);
                    }
                    else
                    {
                        newMats.Add(mat);
                    }
                }
                rendererInChild.materials = newMats.ToArray();
            }
        }
        else
        {
            foreach (Renderer rendererInChild in go.gameObject.GetComponentsInChildren<Renderer>())
            {
                List<Material> newMats = new List<Material>();
                foreach (var mat in rendererInChild.materials)
                {
                    if (mat.name.Contains("Environment"))
                    {
                        newMats.Add(notAllow);
                    }
                    else
                    {
                        newMats.Add(mat);
                    }
                }
                rendererInChild.materials = newMats.ToArray();
            }
        }
    }

    public void DefaultColorEffect(MachineB go)
    {
        foreach (Renderer rendererInChild in go.gameObject.GetComponentsInChildren<Renderer>())
        {
            List<Material> newMats = new List<Material>();
            foreach (var mat in rendererInChild.materials)
            {
                if (mat.name.Contains("Environment"))
                {
                    newMats.Add(normal);
                }
                else
                {
                    newMats.Add(mat);
                }
            }
            rendererInChild.materials = newMats.ToArray();
        }
        if(go.gameObject.GetComponentInChildren<SensorColor>())
        {
            go.gameObject.GetComponentInChildren<SensorColor>().SetColorAgain();
        }
    }

    private bool EnoughMoney(MachineB m)
    {
        return m.gameObject.GetComponentInParent<BuildingIdentifier>().EnoughMoney();
    }

    private void Buy(MachineB m)
    {
        m.gameObject.GetComponentInParent<BuildingIdentifier>().Buy();
    }

    private void Sell(MachineB m)
    {
        m.gameObject.GetComponentInParent<BuildingIdentifier>().Sell();
    }

    private void SpawnPrefab(Vector2Int pivotInGridLocal, GameObject selectedBlueprintPrefabLocal, Vector3 position,
        Quaternion rotation, bool[,] shape)
    {
        //Debug.Log(pivotInGrid);
        GameObject go = Instantiate(selectedBlueprintPrefabLocal, position, rotation);
        MachineB spawnedMachineB = go.GetComponent<MachineB>();
        go.GetComponentInChildren<Machine>()?.TurnOn();
        DefaultColorEffect(spawnedMachineB);
        spawnedMachineB.shape = shape;
        grid[pivotInGridLocal.x,pivotInGridLocal.y].SetMachine(go, spawnedMachineB);
        

        int inX = spawnedMachineB.shape.GetLength(0);
        int inY = spawnedMachineB.shape.GetLength(1);

        int addx = 0;
        int addz = 0;

        
        
        //offset for rest of cells viewd on fised pivot after rotation
        switch (spawnedMachineB.rotation)
        {
            case 0:
                break;
            case 1:
                addz = -(inY - 1);
                break;
            case 2:
                addx = -(inX - 1);
                addz = -(inY - 1);
                break;
            case 3:
                addx = -(inX - 1);
                break;
            default:
                Debug.Log("rot is: "+spawnedMachineB.rotation);
                break;
        }

        //offset is bottom left corner of shape (not mattering how it is rotated)
        Vector2Int rotationOffset = new Vector2Int(addx,addz);
        //Debug.Log("rotationOffset:" + rotationOffset);
        RectInt shapeRect = new RectInt(pivotInGridLocal+rotationOffset,new Vector2Int(inX,inY));

        for (int i = 0; i < shapeRect.width; i++)
        {
            for (int j = 0; j < shapeRect.height; j++)
            {
                int xInGridSpace = shapeRect.x + i;
                int yInGridSpace = shapeRect.y + j;
                if (spawnedMachineB.shape[i, j])
                {
                    grid[xInGridSpace,yInGridSpace].SetMachineOuterpart(grid[pivotInGridLocal.x, pivotInGridLocal.y]);
                }
            }
        }
        //PrintLayout(grid);
    }

    private void PreventBuild(bool b)
    {
        allowBuild = !b;
    }

    public BuildMode GetBuildMode()
    {
        return buildmode;
    }
}
