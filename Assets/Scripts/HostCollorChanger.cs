using UnityEngine;
using UnityEngine.Networking;

public class HostCollorChanger : NetworkBehaviour {

    [SerializeField] Renderer _renderer;
    [SerializeField] Material clientMaterial;
    [SerializeField] Material hostMaterial;

    [SyncVar] bool isHost;

    private void Update()
    {
	    if (isServer)
        {
	        isHost = isLocalPlayer;
        }

	    _renderer.material = isHost ? hostMaterial : clientMaterial;
    }
}
