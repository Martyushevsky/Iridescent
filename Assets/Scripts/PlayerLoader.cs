using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
	public class PlayerLoader : NetworkBehaviour
	{
		[SerializeField] GameObject _unitPrefab;
		[SerializeField] PlayerController _controller;

		[SyncVar(hook = nameof(HookUnitIdentity))] NetworkIdentity _unitIdentity;


		private void Start()
		{
			//Debug.Log($"{nameof(isServer)} =  {isServer}");
		}

		public override void OnStartAuthority()
		{
			if (isServer)
			{
				SpawnPlayer(true);
			}
			else
			{
				CmdCreatePlayer();
			}
		}

		[Command]
		public void CmdCreatePlayer()
		{
			SpawnPlayer(false);
		}

		[ClientCallback]
		private void HookUnitIdentity(NetworkIdentity unit)
		{
			if (!isLocalPlayer) return;
			_unitIdentity = unit;
			_controller.SetCharacter(unit.GetComponent<Character>(), true);
		}

		public void SpawnPlayer(bool isLocalPlayer)
		{
			var unit = Instantiate(_unitPrefab, transform.position, Quaternion.identity);
			NetworkServer.Spawn(unit);
			_unitIdentity = unit.GetComponent<NetworkIdentity>();
			_controller.SetCharacter(unit.GetComponent<Character>(), isLocalPlayer);
		}
	}
}