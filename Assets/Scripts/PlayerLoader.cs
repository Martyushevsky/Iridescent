using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
    public class PlayerLoader : NetworkBehaviour
    {
        [SerializeField] private PlayerController _controller;
        [SerializeField] private Player _player;

        [TargetRpc]
        void TargetLinkCharacter(NetworkConnection target, NetworkIdentity unit)
        {
            Character character = unit.GetComponent<Character>();
            _player.Setup(character, GetComponent<Inventory>(), GetComponent<Equipment>(), true);
            _controller.SetCharacter(character, true);
        }

        public override void OnStartAuthority()
        {
            CmdCreatePlayer();
        }

        [Command]
        public void CmdCreatePlayer()
        {
            Character character = CreateCharacter();
            _player.Setup(character, GetComponent<Inventory>(), GetComponent<Equipment>(), isLocalPlayer);
            _controller.SetCharacter(character, isLocalPlayer);
        }

        public Character CreateCharacter()
        {
            UserAccount acc = AccountManager.GetAccount(connectionToClient);
            GameObject unitPrefab = NetworkManager.singleton.spawnPrefabs.Find(x => x.GetComponent<NetworkIdentity>().assetId.Equals(acc.data.characterHash));
            GameObject unit = Instantiate(unitPrefab, acc.data.posCharacter, Quaternion.identity);
            // указываем объект игрока для определения видимости персонажа
            Character character = unit.GetComponent<Character>();
            character.Player = _player;
            // реплицируем персонажа
            NetworkServer.Spawn(unit);
            // настраиваем персонажа на клиенте, которому он принадлежит
            TargetLinkCharacter(connectionToClient, unit.GetComponent<NetworkIdentity>());
            return character;
        }

        public override bool OnCheckObserver(NetworkConnection connection) => false;

        private void OnDestroy()
        {
            if (isServer && _player.Character != null)
            {
                UserAccount acc = AccountManager.GetAccount(connectionToClient);
                acc.data.posCharacter = _player.Character.transform.position;
                Destroy(_player.Character.gameObject);
                NetworkManager.singleton.StartCoroutine(acc.Quit());
            }
        }
    }
}