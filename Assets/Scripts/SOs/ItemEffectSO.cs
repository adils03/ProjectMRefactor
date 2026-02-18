using UnityEngine;

public abstract class ItemEffectSO : ScriptableObject
{
    public virtual void OnFirstPlaced(ItemContext context) { }
    public virtual void OnTrashed(ItemContext context) { }
    public virtual void OnUsed(ItemContext context) { }
    public virtual void OnSelled(ItemContext context) { }
    public virtual void OnTick(ItemContext context) { }
}


