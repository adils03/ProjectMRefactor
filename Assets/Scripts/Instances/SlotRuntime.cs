using System.Collections.Generic;
using UnityEngine;

public class SlotRuntime : BaseRuntime
{
    private readonly List<ItemRuntime> connectedItems;

    public SlotRuntime(ItemSO data, IEntity owner) : base(data, owner)
    {
        connectedItems = new List<ItemRuntime>();
    }

    public void ConnectItem(ItemRuntime item)
    {
        if (!connectedItems.Contains(item))
        {
            connectedItems.Add(item);
            Debug.Log($"Item {item.Data.itemName} connected to slot {Data.itemName}");
        }
        ApplySlotEffects();
    }

    public void DisconnectItem(ItemRuntime item)
    {
        if (connectedItems.Contains(item))
        {
            DeleteSlotEffects(item);
            connectedItems.Remove(item);
        }
    }


    private void DisconnectAllItems()
    {
        foreach (var item in connectedItems)
        {
            DeleteSlotEffects(item);
            item.ChangeOwner(null);
        }

        connectedItems.Clear();
    }

    private void ApplySlotEffects()
    {
        // Implement logic to apply slot effects to connected items if needed
    }

    private void DeleteSlotEffects(ItemRuntime item)
    {
        // Implement logic to remove slot effects from connected items if needed
    }
}
