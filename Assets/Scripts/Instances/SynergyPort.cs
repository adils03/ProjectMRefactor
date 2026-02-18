using UnityEngine;

public class SynergyPort
{
    private GridObject gridObject;
    private ItemPlaced owner;
    private ItemPlaced connectedTo;

    public SynergyPort(ItemPlaced owner, GridObject gridObject)
    {
        this.owner = owner;
        this.gridObject = gridObject;
    }
    public ItemPlaced GetConnectedTo()
    {
        return connectedTo;
    }
    public void SetConnectedTo(ItemPlaced item)
    {
        connectedTo = item;
    }   

    public void ClearConnection()
    {
        connectedTo = null;
    }

    public ItemPlaced GetOwner()
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