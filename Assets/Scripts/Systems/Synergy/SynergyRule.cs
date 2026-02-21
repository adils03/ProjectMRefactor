using System;
using UnityEngine;

public abstract class SynergyRule
{
    protected ItemRuntime owner;

    public void Init(ItemRuntime owner)
    {
        this.owner = owner;
        owner.OnConnectionsChanged += HandleConnectionsChanged;
        OnInit();
        Rebind();
    }

    protected abstract void OnInit();

    protected virtual void Rebind() { }

    private void HandleConnectionsChanged(object sender, EventArgs e)
    {
        Rebind();
    }

    public virtual void Dispose()
    {
        if (owner != null)
            owner.OnConnectionsChanged -= HandleConnectionsChanged;
    }
}