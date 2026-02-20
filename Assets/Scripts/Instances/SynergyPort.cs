using UnityEngine;

public class SynergyPort
{
    private GridObject gridObject;
    private IEntityRuntime owner;
    private IEntityRuntime connectedTo;

    public SynergyPort(IEntityRuntime owner, GridObject gridObject)
    {
        this.owner = owner;
        this.gridObject = gridObject;
    }
    public IEntityRuntime GetConnectedTo()
    {
        return connectedTo;
    }
    public void SetConnectedTo(IEntityRuntime item)
    {
        Debug.Log($"{item.Data.itemName}");
        connectedTo = item;
    }   

    public void ClearConnection()
    {
        connectedTo = null;
    }

    public IEntityRuntime GetOwner()
    {
        return owner;
    }

    public override bool Equals(object obj)
    {
        if (obj is SynergyPort otherPort)
        {
            return owner == otherPort.owner && gridObject == otherPort.gridObject;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return owner.GetHashCode() ^ gridObject.GetHashCode();
    }
}