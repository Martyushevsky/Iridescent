using UnityEngine.Networking;

namespace Geekbrains
{
    public class Equipment : NetworkBehaviour
    {
        public event SyncList<Item>.SyncListChanged OnItemChanged;
        public SyncListItem Items = new SyncListItem();

        public Player Player;

        private UserData data;

        public void Load(UserData data)
        {
            this.data = data;
            for (int i = 0; i < data.equipment.Count; i++)
            {
                EquipmentItem item = (EquipmentItem)ItemBase.GetItem(data.equipment[i]);
                Items.Add(item);
                item.Equip(Player);
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

        public EquipmentItem EquipItem(EquipmentItem item)
        {
            EquipmentItem oldItem = null;
            for (var i = 0; i < Items.Count; i++)
            {
                if (((EquipmentItem)Items[i]).EquipSlot == item.EquipSlot)
                {
                    oldItem = (EquipmentItem)Items[i];
                    oldItem.Unequip(Player);
                    data.equipment.Remove(ItemBase.GetItemId(Items[i]));
                    Items.RemoveAt(i);
                    break;
                }
            }
            Items.Add(item);
            item.Equip(Player);
            data.equipment.Add(ItemBase.GetItemId(item));
            return oldItem;
        }

        public void UnequipItem(Item item)
        {
            CmdUnequipItem(Items.IndexOf(item));
        }

        [Command]
        void CmdUnequipItem(int index)
        {
            if (Items[index] != null && Player.Inventory.AddItem(Items[index]))
            {
                ((EquipmentItem)Items[index]).Unequip(Player);
                data.equipment.Remove(ItemBase.GetItemId(Items[index]));
                Items.RemoveAt(index);
            }
        }
    }
}