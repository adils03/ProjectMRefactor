using System;
using System.Collections.Generic;

public class ItemRuntime : BaseRuntime
{
    public event EventHandler OnTickEvent;
    public event EventHandler OnCooldownChanged;
    

    public StatsInstance Stats { get; private set; }

    private readonly List<SynergyRule> runtimeSynergyRules = new();

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

   
}
