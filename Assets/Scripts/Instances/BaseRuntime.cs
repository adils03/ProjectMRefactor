using System;
using UnityEngine;

public abstract class BaseRuntime : IEntityRuntime
{
    public ItemSO Data { get; private set; }
    public IEntity Owner { get; private set; }

    protected BaseRuntime(ItemSO data, IEntity owner)
    {
        Data = data;
        Owner = owner;
    }

    public virtual void ChangeOwner(IEntity owner)
    {
        this.Owner = owner;
    }

    public virtual ItemContext CreateContext(IEntity target = null)
    {
        return new ItemContext
        {
            owner = Owner,
            target = target,
            item = this
        };
    }

}