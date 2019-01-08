using UnityEngine;
using UnityEngine.Networking;

public class PlayerStats : NetworkBehaviour
{

    [SerializeField] int maxHealth;
    [SyncVar] int curHealth;

    public override void OnStartAuthority()
    {
        curHealth = maxHealth;
    }
}