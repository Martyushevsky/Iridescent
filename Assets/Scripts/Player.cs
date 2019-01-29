using UnityEngine;

namespace Geekbrains
{
	public class Player : MonoBehaviour
	{
		[SerializeField] Character _character;
		[SerializeField] Inventory _inventory;
		[SerializeField] Equipment _equipment;

		public Character Character => _character;
		public Inventory Inventory => _inventory;
		public Equipment Equipment => _equipment;

		public void Setup(Character character, Inventory inventory, Equipment equipment, bool isLocalPlayer)
		{
			_character = character;
			_inventory = inventory;
			_equipment = equipment;
			_character.Player = this;
			_inventory.Player = this;
			_equipment.Player = this;

			if (isLocalPlayer)
			{
				InventoryUI.Instance.SetInventory(_inventory);
				EquipmentUI.Instance.SetEquipment(_equipment);
			}
		}
	}
}