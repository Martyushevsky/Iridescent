using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Geekbrains;

public class PlayerVisibility : NetworkBehaviour
{
    [SerializeField] private float visRange = 10f;
    [SerializeField] private float visUpdateInterval = 1f;
    [SerializeField] private LayerMask visMask;

    private Transform _transform;
    private float _visUpdateTime;

    public override void OnStartServer()
    {
        _transform = transform;
    }

    void Update()
    {
        if (isServer)
        {
            if (Time.time - _visUpdateTime > visUpdateInterval)
            {
                GetComponent<NetworkIdentity>().RebuildObservers(false);
                _visUpdateTime = Time.time;
            }
        }
    }

    public override bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool initialize)
    {
        Collider[] hits = Physics.OverlapSphere(_transform.position, visRange, visMask);
        foreach (Collider hit in hits)
        {
            Character character = hit.GetComponent<Character>();
            if (character != null && character.Player != null)
            {
                NetworkIdentity identity = character.Player.GetComponent<NetworkIdentity>();
                if (identity != null && identity.connectionToClient != null)
                {
                    observers.Add(identity.connectionToClient);
                }
            }
        }
        // если это персонаж, он всегда видим для своего игрока
        Character m_character = GetComponent<Character>();
        if (m_character != null && !observers.Contains(m_character.Player.Conn))
        {
            observers.Add(m_character.Player.Conn);
        }
        return true;
    }

    public override bool OnCheckObserver(NetworkConnection connection)
    {
        // если это персонаж, он всегда видим для своего игрока
        Character character = GetComponent<Character>();
        if (character != null && connection == character.Player.Conn)
        {
            return true;
        }
        // находим объект игрока по коннекту
        Player player = null;
        foreach (UnityEngine.Networking.PlayerController controller in connection.playerControllers)
        {
            if (controller != null)
            {
                player = controller.gameObject.GetComponent<Player>();
                if (player != null) break;
            }
        }
        // если игрок в зоне видимости, объект видим для него
        if (player != null && player.Character != null)
        {
            return (player.Character.transform.position - _transform.position).magnitude < visRange;
        }
        else return false;
    }
}
