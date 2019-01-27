using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
	public class PlayerLoader : NetworkBehaviour
	{
		[SerializeField] GameObject unitPrefab;
		[SerializeField] PlayerController controller;

		[SyncVar(hook = "HookUnitIdentity")] NetworkIdentity unitIdentity;

		public override void OnStartAuthority()
		{
			if (isServer)
			{
				Character character = CreateCharacter();
				controller.SetCharacter(character, true);
				InventoryUI.Instance.SetInventory(character.Inventory);
			}
			else CmdCreatePlayer();
		}

		[Command]
		public void CmdCreatePlayer()
		{
			controller.SetCharacter(CreateCharacter(), false);
		}

		[ClientCallback]
		void HookUnitIdentity(NetworkIdentity unit)
		{
			if (isLocalPlayer)
			{
				unitIdentity = unit;
				Character character = unit.GetComponent<Character>();
				controller.SetCharacter(character, true);
				character.SetInventory(GetComponent<Inventory>());
				InventoryUI.Instance.SetInventory(character.Inventory);
			}
		}

		public Character CreateCharacter()
		{
			GameObject unit = Instantiate(unitPrefab);
			NetworkServer.Spawn(unit);
			unitIdentity = unit.GetComponent<NetworkIdentity>();
			unit.GetComponent<Character>().SetInventory(GetComponent<Inventory>());
			return unit.GetComponent<Character>();
		}

		public override bool OnCheckObserver(NetworkConnection connection)
		{
			return false;
		}
	}
}