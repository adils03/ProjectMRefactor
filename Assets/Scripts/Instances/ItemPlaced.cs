using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlaced : PlacedBase
{

    public ItemPlaced(ItemSO itemSO, IEntityRuntime runtime) : base(itemSO, runtime)
    {

    }

    protected override void BindToGridSlots()
    {
        var positions = GetGridPositionList();

        foreach (var p in positions)
        {
            var obj = grid.GetGridObject(p.x, p.y);
            if (obj == null) continue;

            obj.SetPlacedObject(this);
            slots.Add(obj);
        }
        SetupSynergy();
    }

    private void SetupSynergy()
    {
        var synergyLocs = GetSynergyLocations();

        foreach (var loc in synergyLocs)
        {
            var port = new SynergyPort(this, loc);
            loc.AddSynergyPort(port);
            port.SetConnectedTo(loc.GetPlacedObject());
        }
    }

    public override void ClearSlots()
    {
        foreach (var s in slots)
        {
            SlotRuntime slot = s.GetPlacedSlot()?.GetRuntime() as SlotRuntime;
            slot?.DisconnectItem(GetRuntime() as ItemRuntime);
            s.ClearPlacedObject();
        }

        slots.Clear();
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

}
