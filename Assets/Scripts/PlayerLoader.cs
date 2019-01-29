using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
	public class PlayerLoader : NetworkBehaviour
	{
		[SerializeField] private GameObject _unitPrefab;
		[SerializeField] private PlayerController _controller;
		[SerializeField] private Player _player;

		[SyncVar(hook = nameof(HookUnitIdentity))]
		private NetworkIdentity _unitIdentity;

		public override void OnStartAuthority()
		{
			if (isServer)
			{
				var character = CreateCharacter();
				_player.Setup(character, GetComponent<Inventory>(), GetComponent<Equipment>(), true);
				_controller.SetCharacter(character, true);
			}
			else
			{
				CmdCreatePlayer();
			}
		}

		[Command]
		public void CmdCreatePlayer()
		{
			var character = CreateCharacter();
			_player.Setup(character, GetComponent<Inventory>(), GetComponent<Equipment>(), false);
			_controller.SetCharacter(character, false);
		}

		[ClientCallback]
		private void HookUnitIdentity(NetworkIdentity unit)
		{
			if (isLocalPlayer)
			{
				_unitIdentity = unit;
				var character = unit.GetComponent<Character>();
				//_controller.SetCharacter(character, true);
				_player.Setup(character, GetComponent<Inventory>(), GetComponent<Equipment>(), true);
				_controller.SetCharacter(character, true);
			}
		}

		public Character CreateCharacter()
		{
			var unit = Instantiate(_unitPrefab);
			NetworkServer.Spawn(unit);
			_unitIdentity = unit.GetComponent<NetworkIdentity>();
			return unit.GetComponent<Character>();
		}

		public override bool OnCheckObserver(NetworkConnection connection) => false;
	}
}