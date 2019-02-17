using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
    public class PlayerLoader : NetworkBehaviour
    {
        [SerializeField] private GameObject _unitPrefab;
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
            // получаем аккаунт из менеджера аккаунтов по связи с клиентом
            UserAccount acc = AccountManager.GetAccount(connectionToClient);

            // создаем персонажа в позиции из пользовательских данных
            GameObject unit = Instantiate(_unitPrefab, acc.data.posCharacter, Quaternion.identity);

            NetworkServer.Spawn(unit);
            TargetLinkCharacter(connectionToClient, unit.GetComponent<NetworkIdentity>());
            return unit.GetComponent<Character>();
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