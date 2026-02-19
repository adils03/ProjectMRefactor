using System.Collections.Generic;
using UnityEngine;

public class GridObject
{

    private GridSystem<GridObject> grid;
    private int x;
    private int y;
    private IPlaced placedItem;
    private IPlaced placedSlot;
    List<SynergyPort> synergyPorts;


    public GridObject(GridSystem<GridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        placedItem = null;
        placedSlot = null;
        synergyPorts = new List<SynergyPort>();
    }


    public override string ToString()
    {
        return x + ", " + y + "\n" + placedItem;
    }

    public void SetPlacedObject(IPlaced placedObject)
    {
        this.placedItem = placedObject;
        grid.TriggerGridObjectChanged(x, y);
    }

    public void ClearPlacedObject()
    {
        placedItem = null;
        grid.TriggerGridObjectChanged(x, y);
    }

    public void SetPlacedSlot(IPlaced placedSlot)
    {
        this.placedSlot = placedSlot;
        grid.TriggerGridObjectChanged(x, y);
    }

    public void ClearPlacedSlot()
    {
        placedSlot = null;
        grid.TriggerGridObjectChanged(x, y);
    }

    public IPlaced GetPlacedObject()
    {
        return placedItem;
    }

    public IPlaced GetPlacedSlot()
    {
        return placedSlot;
    }
    public Vector2Int GetGridPosition()
    {
        return new Vector2Int(x, y);
    }
    public bool CanBuild()
    {
        return placedItem == null;
    }

    public bool IsSlot()
    {
        return placedSlot != null;
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