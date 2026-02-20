using System.Collections.Generic;
using UnityEngine;

public class ConnectedItemTickSynergyRule : SynergyRule
{
    private List<ItemRuntime> connectedItems = new List<ItemRuntime>();

    protected override void OnInit()
    {
        Rebind();
    }

    protected override void Rebind()
    {
        Unbind();

    }
    private void HandleConnectedItemTick(object sender, System.EventArgs e)
    {
        // Implement the effect that should happen when the connected item ticks.
        // Debug.Log($"Connected item {connectedItem.Data.name} ticked, triggering synergy effect on {owner.Data.name}.");
        // Example: owner.Stats.IncreaseStat("Damage", 10);
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
