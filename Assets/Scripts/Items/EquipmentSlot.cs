using UnityEngine;
using UnityEngine.UI;

namespace Geekbrains
{
	public class EquipmentSlot : MonoBehaviour
	{
		public Image Icon;
		public Button UnequipButton;
		public Equipment Equipment;

		private Item _item;

		public void SetItem(Item newItem)
		{
			_item = newItem;
			Icon.sprite = _item.Icon;
			Icon.enabled = true;
			UnequipButton.interactable = true;
		}

		public void ClearSlot()
		{
			_item = null;
			Icon.sprite = null;
			Icon.enabled = false;
			UnequipButton.interactable = false;
		}

		public void Unequip()
		{
			Equipment.UnequipItem(_item);
		}
	}
}