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

        private UserData data;

        public void Load(UserData data)
        {
            this.data = data;
            for (int i = 0; i < data.inventory.Count; i++)
            {
                Items.Add(ItemBase.GetItem(data.inventory[i]));
            }
        }

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
                data.inventory.Add(ItemBase.GetItemId(item));
                return true;
            }

            return false;
        }

        public void RemoveItem(Item item)
        {
            Items.Remove(item);
            data.inventory.Remove(ItemBase.GetItemId(item));
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
            RemoveItem(Items[index]);
        }

        private void Drop(Item item)
        {
            var pickupItem = Instantiate(item.PickupPrefab, Player.Character.transform.position,
                Quaternion.Euler(0, Random.Range(0, 360f), 0));
            pickupItem.Item = item;
            NetworkServer.Spawn(pickupItem.gameObject);
        }
    }
}