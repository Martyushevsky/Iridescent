using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
	[RequireComponent(typeof(StatsManager), typeof(NetworkIdentity))]
	public class Player : MonoBehaviour
	{
		[SerializeField] private StatsManager _statsManager;
		[SerializeField] private Character _character;
		[SerializeField] private Inventory _inventory;
		[SerializeField] private Equipment _equipment;

		public Character Character => _character;
		public Inventory Inventory => _inventory;
		public Equipment Equipment => _equipment;

		public void Setup(Character character, Inventory inventory, Equipment equipment, bool isLocalPlayer)
		{
			_statsManager = GetComponent<StatsManager>();
			_character = character;
			_inventory = inventory;
			_equipment = equipment;
			_character.Player = this;
			_inventory.Player = this;
			_equipment.Player = this;

			if (GetComponent<NetworkIdentity>().isServer)
			{
				_character.Stats.Manager = _statsManager;
			}

			if (isLocalPlayer)
			{
				InventoryUI.Instance.SetInventory(_inventory);
				EquipmentUI.Instance.SetEquipment(_equipment);
				StatsUI.instance.SetManager(_statsManager);
			}
		}
	}
}