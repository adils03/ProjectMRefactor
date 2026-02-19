using UnityEngine;

public class SynergyPort
{
    private GridObject gridObject;
    private IPlaced owner;
    private IPlaced connectedTo;

    public SynergyPort(IPlaced owner, GridObject gridObject)
    {
        this.owner = owner;
        this.gridObject = gridObject;
    }
    public IPlaced GetConnectedTo()
    {
        return connectedTo;
    }
    public void SetConnectedTo(IPlaced item)
    {
        connectedTo = item;
    }   

    public void ClearConnection()
    {
        connectedTo = null;
    }

    public IPlaced GetOwner()
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