using System.Collections.Generic;
using UnityEngine;

public class ConnectedItemTickSynergyRule : SynergyRule
{
    private List<ItemRuntime> connectedItems = new List<ItemRuntime>();

    protected override void OnInit()
    {

    }

    protected override void Rebind()
    {
        Unbind();
        connectedItems.Clear();
        connectedItems = owner.GetConnectedItems();
        Debug.Log("Rebinding ConnectedItemTickSynergyRule. Connected items count: " + connectedItems.Count);
        foreach (var item in connectedItems)
        {
            if (item != null)
                item.OnTickEvent += HandleConnectedItemTick;
        }
    }
    private void HandleConnectedItemTick(object sender, System.EventArgs e)
    {
        Debug.Log("Connected item ticked: " + ((ItemRuntime)sender).Data.itemName);
    }

    public override void Dispose()
    {
        base.Dispose();
        Unbind();
        connectedItems.Clear();
    }

    private void Unbind()
    {
        foreach (var item in connectedItems)
        {
            if (item != null)
                item.OnTickEvent -= HandleConnectedItemTick;
        }
        connectedItems.Clear();
    }
}
