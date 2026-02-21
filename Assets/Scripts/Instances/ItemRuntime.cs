using System;
using System.Collections.Generic;
using UnityEngine;
public class ItemRuntime : BaseRuntime
{
    public event EventHandler OnTickEvent;
    public event EventHandler OnCooldownChanged;
    public event EventHandler OnConnectionsChanged;

    public StatsInstance Stats { get; private set; }

    private readonly List<SynergyRule> runtimeSynergyRules = new();
    private readonly List<ItemRuntime> connectedItems = new();

    private float currentCooldown;
    public float CurrentCooldown => currentCooldown;
    public bool IsOnCooldown => currentCooldown > 0f;
    public bool IsCooldownItem => Data.itemStats != null && Data.itemStats.cooldown > 0;

    public ItemRuntime(ItemSO data, IEntity owner) : base(data, owner)
    {
        if (data.itemStats != null)
            Stats = new StatsInstance();
        CreateRuntimeSynergyRules();
        ResetCooldown();
    }

    public void CreateRuntimeSynergyRules()
    {
        runtimeSynergyRules.Clear();
        if (Data.synergyRules == null) return;

        foreach (var so in Data.synergyRules)
        {
            var rule = so.CreateRuntimeRule();
            rule.Init(this);
            runtimeSynergyRules.Add(rule);
        }
    }

    public void DisposeRuntimeSynergyRules()
    {
        foreach (var rule in runtimeSynergyRules)
            rule.Dispose();

        runtimeSynergyRules.Clear();
    }

    public void UpdateCooldown(float dt)
    {
        if (currentCooldown <= 0f) return;
        currentCooldown -= dt;
        if (currentCooldown < 0f)
            currentCooldown = 0f;
        OnCooldownChanged?.Invoke(this, EventArgs.Empty);
    }

    public void StartCooldown()
    {
        if (!IsCooldownItem) return;
        currentCooldown = Data.itemStats.cooldown;
        OnCooldownChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ResetCooldown()
    {
        currentCooldown = 0f;
        OnCooldownChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Use(IEntity target = null)
    {
        if (Data.effects == null) return;

        var ctx = new ItemContext
        {
            owner = Owner,
            target = target,
            item = this
        };

        foreach (var e in Data.effects)
            e.OnUsed(ctx);
    }

    public void Tick(IEntity target = null)
    {
        Debug.Log($"Ticking item {Data.itemName}");
        if (IsOnCooldown || !IsCooldownItem) return;
        if (Data.effects == null) return;

        var ctx = new ItemContext
        {
            owner = Owner,
            target = target,
            item = this
        };

        foreach (var e in Data.effects)
            e.OnTick(ctx);

        OnTickEvent?.Invoke(this, EventArgs.Empty);
        StartCooldown();
    }

    public void Sell()
    {
        if (Data.effects == null) return;

        var ctx = new ItemContext
        {
            owner = Owner,
            target = null,
            item = this
        };

        foreach (var e in Data.effects)
            e.OnSelled(ctx);
    }

    public void ConnectTo(ItemRuntime other)
    {
        if (connectedItems.Contains(other)) return;
        connectedItems.Add(other);
        OnConnectionsChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public void DisconnectFrom(ItemRuntime other)
    {
        Debug.Log($"Disconnecting {other.Data.itemName} from {Data.itemName}");
        connectedItems.Remove(other);
        OnConnectionsChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<ItemRuntime> GetConnectedItems()
    {
        return new List<ItemRuntime>(connectedItems);
    }

    public void DebugPrintConnections()
    {
        string connections = $"Item {Data.itemName} connections: ";
        foreach (var item in connectedItems)
        {
            connections += $"{item.Data.itemName}, ";
        }
        Debug.Log(connections);
    }
}

