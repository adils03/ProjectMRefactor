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
        if (itemSO.itemSynergy == null) return;

        foreach (var loc in slots)
        {
            var existingPorts = loc.GetSynergyPorts();
            foreach (var port in existingPorts)
            {
                port.SetConnectedTo(GetRuntime());
            }
        }

        var synergyLocs = GetSynergyLocations();
        if (synergyLocs == null) return;
        foreach (var loc in synergyLocs)
        {
            var port = new SynergyPort(GetRuntime(), loc);
            loc.AddSynergyPort(port);
            IPlaced existingItem = loc.GetPlacedObject();
            if (existingItem != null)
                port.SetConnectedTo(existingItem.GetRuntime());
        }
    }

    private void ClearSynergy()
    {
        foreach (var loc in slots)
        {
            var ports = loc.GetSynergyPorts();
            if (ports == null) continue;
            foreach (var port in ports)
            {
                if (port.GetOwner() == GetRuntime())
                {
                    port.ClearConnection();
                }
            }
        }

        var synergyLocs = GetSynergyLocations();
        foreach (var loc in synergyLocs)
        {
            var ports = loc.GetSynergyPorts();
            if (ports == null) continue;
            var portsToRemove = new List<SynergyPort>();
            foreach (var port in ports)
            {
                if (port.GetOwner() == GetRuntime())
                {
                    portsToRemove.Add(port);
                }
            }
            
            foreach (var port in portsToRemove)
            {
                loc.RemoveSynergyPort(port);
            }
        }
    }

    public override void ClearSlots()
    {
        if(grid == null) return;
        ClearSynergy();
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
