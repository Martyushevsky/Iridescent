using UnityEngine;
using UnityEngine.Networking;

public class UnitLoader : NetworkBehaviour
{

    [SerializeField] GameObject unitPrefab;

    public override void OnStartServer()
    {
        GameObject unit = Instantiate(unitPrefab);
        NetworkServer.SpawnWithClientAuthority(unit, gameObject);
    }
}