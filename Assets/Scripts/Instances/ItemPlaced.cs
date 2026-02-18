using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlaced : MonoBehaviour
{
    public event EventHandler OnMoved;

    [SerializeField] private Transform cellPrefab;
    [SerializeField] private Transform cellsParent;

    public List<Transform> CellInstances { get; private set; }

    private ItemSO itemSO;
    private Vector2Int origin;
    private Direction dir;
    private GridSystem<GridObject> grid;
    private List<GridObject> slots;

    private ItemView itemView;
    public ItemView ItemView => itemView;

    public void Setup(
        ItemSO itemSO,
        Vector2Int requestedOrigin,
        Direction dir,
        GridSystem<GridObject> grid,
        ItemView itemView)
    {
        name = itemSO.itemName;

        this.itemSO = itemSO;
        this.dir = dir;
        this.grid = grid;
        this.itemView = itemView;

        SetPlacement(requestedOrigin, dir, grid);
        SetCollidersAndCells();
        BindToGridSlots();
    }

    private void SetPlacement(Vector2Int requestedOrigin, Direction dir, GridSystem<GridObject> grid)
    {
        origin = ItemPlacementHelper.ChooseAnchorPosition(
            requestedOrigin.x,
            requestedOrigin.y,
            dir);

        transform.position = grid.GetWorldPosition(origin.x, origin.y);

        float rot = ItemPlacementHelper.GetRotationAngle(dir);
        transform.rotation = Quaternion.Euler(0, 0, -rot);
    }

    void SetCollidersAndCells()
    {
        var offsets = GetCellOffsets();
        float size = grid.GetCellSize();

        CellInstances = new List<Transform>();

        foreach (var off in offsets)
        {
            var col = gameObject.AddComponent<BoxCollider2D>();
            col.size = Vector2.one * size;
            col.offset = new Vector2(
                off.x * size + size * .5f,
                off.y * size + size * .5f);

            var cell = Instantiate(cellPrefab, cellsParent);
            cell.localScale = new Vector3(size, size, 1);
            cell.localPosition = new Vector3(
                off.x * size + size * .5f,
                off.y * size + size * .5f,
                0);

            CellInstances.Add(cell);
        }
    }

    void BindToGridSlots()
    {
        var positions = GetGridPositionList();
        slots = new List<GridObject>();

        foreach (var p in positions)
        {
            var obj = grid.GetGridObject(p.x, p.y);
            if (obj == null) continue;

            obj.SetPlacedObject(this);
            slots.Add(obj);
        }

        SetupSynergy();
        TriggerOnMoved();
    }

    void SetupSynergy()
    {
        var synergyLocs = GetSynergyLocations();

        foreach (var loc in synergyLocs)
        {
            var port = new SynergyPort(this, loc);
            loc.AddSynergyPort(port);
            port.SetConnectedTo(loc.GetPlacedObject());
        }
    }

    public void ClearSlots()
    {
        if (slots == null) return;

        foreach (var s in slots)
            s.ClearPlacedObject();

        slots.Clear();
    }

    public void RotateAroundPivot(Vector3 pivot, float angle)
    {
        transform.RotateAround(
            new Vector3(pivot.x, pivot.y, transform.position.z),
            Vector3.forward,
            angle);

        dir = ItemPlacementHelper.GetNextDir(dir);
        RebindAfterTransformChange();
    }

    void RebindAfterTransformChange()
    {
        ClearSlots();
        BindToGridSlots();
    }

    public List<Vector2Int> GetGridPositionList()
    {
        return ItemPlacementHelper.GetGridPositionList(
            origin,
            dir,
            itemSO.itemShape.itemCells);
    }

    public List<Vector2Int> GetCellOffsets()
    {
        return ItemPlacementHelper.GetGridPositionList(
            Vector2Int.zero,
            Direction.Down,
            itemSO.itemShape.itemCells);
    }

    public List<GridObject> GetSynergyLocations()
    {
        var pos = ItemPlacementHelper.GetSynergyGridPositionList(
            origin,
            dir,
            itemSO.itemSynergy.itemOrigin,
            itemSO.itemSynergy.syngergyCells);

        var list = new List<GridObject>();

        foreach (var p in pos)
        {
            var g = grid.GetGridObject(p.x, p.y);
            if (g != null) list.Add(g);
        }

        return list;
    }

    public void TriggerOnMoved()
    {
        OnMoved?.Invoke(this, EventArgs.Empty);
    }

    public void DestroySelf()
    {
        ClearSlots();
        Destroy(gameObject);
    }

    public override string ToString()
    {
        return itemSO.itemName;
    }
}
