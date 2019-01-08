using UnityEngine;
using UnityEngine.Networking;

public class NetPlayerSetup : NetworkBehaviour
{
    [SerializeField] MonoBehaviour[] disableBehaviours;

    void Awake()
    {
        if (!hasAuthority)
        {
            for (int i = 0; i < disableBehaviours.Length; i++)
            {
                disableBehaviours[i].enabled = false;
            }
        }
    }

    public override void OnStartAuthority()
    {
        for (int i = 0; i < disableBehaviours.Length; i++)
        {
            disableBehaviours[i].enabled = true;
        }
    }
}
