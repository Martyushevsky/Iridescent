using UnityEngine;

namespace Geekbrains
{
	[CreateAssetMenu(fileName = "New equipment", menuName = "Inventory/Equipment")]
	public class EquipmentItem : Item
	{
		public EquipmentSlotType EquipSlot;

		public int DamageModifier;
		public int ArmorModifier;
		public int SpeedModifier;

		public override void Use(Player player)
		{
			player.Inventory.RemoveItem(this);
			var oldItem = player.Equipment.EquipItem(this);
			if (oldItem != null) player.Inventory.AddItem(oldItem);
			base.Use(player);
		}
	}

	public enum EquipmentSlotType { Head, Chest, Legs, RighHand, LeftHand }
}