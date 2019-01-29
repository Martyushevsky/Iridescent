using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
	public class Inventory : NetworkBehaviour
	{
		public Player Player;
		public int Space = 20;
		public event SyncList<Item>.SyncListChanged OnItemChanged;

		public SyncListItem Items = new SyncListItem();

		public override void OnStartLocalPlayer()
		{
			Items.Callback += ItemChanged;
		}

		private void ItemChanged(SyncList<Item>.Operation op, int itemIndex)
		{
			OnItemChanged?.Invoke(op, itemIndex);
		}

		public bool AddItem(Item item)
		{
			if (Items.Count < Space)
			{
				Items.Add(item);
				return true;
			}

			return false;
		}

		public void UseItem(Item item)
		{
			CmdUseItem(Items.IndexOf(item));
		}

		[Command]
		void CmdUseItem(int index)
		{
			if (Items[index] != null)
			{
				Items[index].Use(Player);
			}
		}

		public void DropItem(Item item)
		{
			CmdDropItem(Items.IndexOf(item));
		}

		[Command]
		void CmdDropItem(int index)
		{
			if (Items[index] == null) return;
			Drop(Items[index]);
			Items.RemoveAt(index);
		}

		private void Drop(Item item)
		{
			var pickupItem = Instantiate(item.PickupPrefab, Player.Character.transform.position, 
				Quaternion.Euler(0, Random.Range(0, 360f), 0));
			pickupItem.Item = item;
			NetworkServer.Spawn(pickupItem.gameObject);
		}

		public void RemoveItem(Item item)
		{
			Items.Remove(item);
		}
	}
}