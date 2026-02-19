using UnityEngine;

public class SlotPlaced : PlacedBase
{
    public SlotPlaced(ItemSO itemSO, IEntityRuntime runtime) : base(itemSO, runtime)
    {
    }

    protected override void BindToGridSlots()
    {
        var positions = GetGridPositionList();

        foreach (var p in positions)
        {
            var obj = grid.GetGridObject(p.x, p.y);
            if (obj == null) continue;

            obj.SetPlacedSlot(this);
            slots.Add(obj);
        }
    }


    public override void ClearSlots()
    {
        foreach (var s in slots)
        {
            s.ClearPlacedSlot();
        }

        slots.Clear();
    }
}
