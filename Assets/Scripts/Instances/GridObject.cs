using System.Collections.Generic;
using UnityEngine;

public class GridObject
{

    private GridSystem<GridObject> grid;
    private int x;
    private int y;
    private ItemPlaced placedObject;
    List<SynergyPort> synergyPorts;

    public GridObject(GridSystem<GridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        placedObject = null;
        synergyPorts = new List<SynergyPort>();
    }


    public override string ToString()
    {
        return x + ", " + y + "\n" + placedObject;
    }

    public void SetPlacedObject(ItemPlaced placedObject)
    {
        this.placedObject = placedObject;
        grid.TriggerGridObjectChanged(x, y);
    }

    public void ClearPlacedObject()
    {
        placedObject = null;
        grid.TriggerGridObjectChanged(x, y);
    }

    public ItemPlaced GetPlacedObject()
    {
        return placedObject;
    }
    public Vector2Int GetGridPosition()
    {
        return new Vector2Int(x, y);
    }
    public bool CanBuild()
    {
        return placedObject == null;
    }

    public void AddSynergyPort(SynergyPort port)
    {
        synergyPorts.Add(port);
    }

    public List<SynergyPort> GetSynergyPorts()
    {
        return synergyPorts;
    }

    public void RemoveSynergyPort(SynergyPort port)
    {
        synergyPorts.Remove(port);
    }
    
    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }
}