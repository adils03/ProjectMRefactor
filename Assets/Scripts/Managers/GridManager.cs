using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{

    [SerializeField] private float cellSize = 10f;
    public float CellSize => cellSize;

    private Dictionary<GridSystem<GridObject>, InventoryRuntime> itemGrids
        = new();

    private Dictionary<GridSystem<GridObject>, GridSystem<GridObject>> slotGrids
        = new();

    public void RegisterItemGrid(GridSystem<GridObject> grid, InventoryRuntime owner)
    {
        if (grid == null) return;

        itemGrids[grid] = owner;

        grid.Name = "ItemGrid";

        // TODO: Clone meodu yapılacak
        // // var slotClone = grid.Clone();
        // slotClone.Name = "SlotGrid";

        // slotGrids[grid] = slotClone;
    }

    public void UnregisterItemGrid(GridSystem<GridObject> grid)
    {
        if (grid == null) return;
        itemGrids.Remove(grid);
        slotGrids.Remove(grid);
    }

    public GridSystem<GridObject> GetSlotGrid(GridSystem<GridObject> itemGrid)
    {
        if (itemGrid == null) return null;
        slotGrids.TryGetValue(itemGrid, out var slotGrid);
        return slotGrid;
    }

    public InventoryRuntime GetInventoryFromGrid(GridSystem<GridObject> grid)
    {
        itemGrids.TryGetValue(grid, out var inv);
        return inv;
    }

    public InventoryRuntime GetInventoryFromSlotGrid(GridSystem<GridObject> slotGrid)
    {
        foreach (var kvp in slotGrids)
        {
            if (kvp.Value == slotGrid)
            {
                itemGrids.TryGetValue(kvp.Key, out var inv);
                return inv;
            }
        }
        return null;
    }

    private List<IPlaced> GetPlacedItems(GridSystem<GridObject> grid)
    {
        var result = new List<IPlaced>();
        if (grid == null) return result;

        var seen = new HashSet<IPlaced>();

        foreach (var gridObj in grid.GetGridArray())
        {
            var placed = gridObj.GetPlacedObject();
            if (placed != null && !seen.Contains(placed))
            {
                seen.Add(placed);
                result.Add(placed);
            }
        }

        return result;
    }

    public GridSystem<GridObject> GetGridFromWorldPosition(Vector3 worldPosition)
    {
        foreach (var kvp in itemGrids)
        {
            if (kvp.Key.IsWorldPositionInside(worldPosition))
                return kvp.Key;
        }
        return null;
    }

    public void DestroyGrid(GridSystem<GridObject> grid)
    {
        foreach (var placed in GetPlacedItems(grid))
        {
            placed.DestroySelf();
        }

        UnregisterItemGrid(grid);
    }


    public void MoveGrid(GridSystem<GridObject> grid, Vector3 newOrigin)
    {
        if (grid == null) return;
        grid.SetOriginPosition(newOrigin);
    }
}
