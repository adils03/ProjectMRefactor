using UnityEngine;

public static class ItemConnectorService
{
    public static void HandlePlace(IEntityRuntime placedItem,IEntityRuntime toConnectTo)
    {
        if(placedItem is ItemRuntime itemruntime && toConnectTo is ItemRuntime toConnectToRuntime)
        {
            itemruntime.ConnectTo(toConnectToRuntime);
        }
    }

    public static void HandleRemove(IEntityRuntime placedItem,IEntityRuntime connectedItem)
    {
        if(placedItem is ItemRuntime itemruntime && connectedItem is ItemRuntime toConnectToRuntime)
        {
            itemruntime.DisconnectFrom(toConnectToRuntime);
        }
    }
}
