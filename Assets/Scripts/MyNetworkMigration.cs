using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class MyNetworkMigration : NetworkMigrationManager
{
    override protected void OnAuthorityUpdated(GameObject go, int connectionId, bool authorityState)
    {
        base.OnAuthorityUpdated(go, connectionId, authorityState);
        Debug.Log(nameof(OnAuthorityUpdated));
    }
    override protected void OnClientDisconnectedFromHost(NetworkConnection conn, out SceneChangeOption sceneChange)
    {
        base.OnClientDisconnectedFromHost(conn, out sceneChange);
        Debug.Log(nameof(OnClientDisconnectedFromHost));
    }
    override protected void OnPeersUpdated(PeerListMessage peers)
    {
        base.OnPeersUpdated(peers);
        Debug.Log(nameof(OnPeersUpdated));
    }
    override protected void OnServerHostShutdown()
    {
        base.OnServerHostShutdown();
        Debug.Log(nameof(OnServerHostShutdown));
    }
    override protected void OnServerReconnectObject(NetworkConnection newConnection, GameObject oldObject, int oldConnectionId)
    {
        OnServerReconnectObject(newConnection, oldObject, oldConnectionId);
        Debug.Log(nameof(OnServerReconnectObject));
    }
    override protected void OnServerReconnectPlayer(NetworkConnection newConnection, GameObject oldPlayer, int oldConnectionId, short playerControllerId)
    {
        base.OnServerReconnectPlayer(newConnection, oldPlayer, oldConnectionId, playerControllerId);
        Debug.Log(nameof(OnServerReconnectPlayer));
    }
}
