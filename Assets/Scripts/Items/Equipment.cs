using UnityEngine.Networking;

namespace Geekbrains
{
	public class Equipment : NetworkBehaviour
	{
		public event SyncList<Item>.SyncListChanged onItemChanged;
		public SyncListItem items = new SyncListItem();

		public Player Player;

		public override void OnStartLocalPlayer()
		{
			items.Callback += ItemChanged;
		}

		private void ItemChanged(SyncList<Item>.Operation op, int itemIndex)
		{
			onItemChanged?.Invoke(op, itemIndex);
		}

		public EquipmentItem EquipItem(EquipmentItem item)
		{
			EquipmentItem oldItem = null;
			for (var i = 0; i < items.Count; i++)
			{
				if (((EquipmentItem)items[i]).EquipSlot == item.EquipSlot)
				{
					oldItem = (EquipmentItem)items[i];
					oldItem.Unequip(Player); 
					items.RemoveAt(i);
					break;
				}
			}
			items.Add(item);
			item.Equip(Player);
			return oldItem;
		}

		public void UnequipItem(Item item)
		{
			CmdUnequipItem(items.IndexOf(item));
		}

		[Command]
		void CmdUnequipItem(int index)
		{
			if (items[index] != null && Player.Inventory.AddItem(items[index]))
			{
				((EquipmentItem)items[index]).Unequip(Player);
				items.RemoveAt(index);
			}
		}
	}
}