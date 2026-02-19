using UnityEngine;

public interface IEntityRuntime 
{
    public ItemSO Data { get; }
    public IEntity Owner { get; }
    public void ChangeOwner(IEntity owner);
    public ItemContext CreateContext(IEntity target = null);
}
