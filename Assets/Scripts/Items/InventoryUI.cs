using UnityEngine;
using UnityEngine.EventSystems;

namespace Geekbrains
{
	public class InventoryUI : MonoBehaviour
	{
		#region Singleton
		public static InventoryUI Instance;

		private void Awake()
		{
			_inventoryUi.SetActive(false);
			if (Instance != null)
			{
				Debug.LogError("More than one instance of InventoryUI found!");
				return;
			}
			Instance = this;
		}
		#endregion

		[SerializeField] private GameObject _inventoryUi;
		[SerializeField] private Transform _itemsParent;
		[SerializeField] private InventorySlot _slotPrefab;

		private InventorySlot[] _slots;
		private Inventory _inventory;

		private void Update()
		{
			if (Input.GetButtonDown("Inventory") && EventSystem.current.currentSelectedGameObject == null)
			{
				_inventoryUi.SetActive(!_inventoryUi.activeSelf);
			}
		}

		public void SetInventory(Inventory newInventory)
		{
			_inventory = newInventory;
			_inventory.OnItemChanged += ItemChanged;
			var childs = _itemsParent.GetComponentsInChildren<InventorySlot>();
			foreach (var child in childs)
				Destroy(child.gameObject);

			_slots = new InventorySlot[_inventory.Space];
			for (var i = 0; i < _inventory.Space; i++)
			{
				_slots[i] = Instantiate(_slotPrefab, _itemsParent);
				_slots[i].Inventory = _inventory;
				if (i < _inventory.Items.Count) _slots[i].SetItem(_inventory.Items[i]);
				else _slots[i].ClearSlot();
			}
		}

		private void ItemChanged(UnityEngine.Networking.SyncList<Item>.Operation op, int itemIndex)
		{
			for (var i = 0; i < _slots.Length; i++)
			{
				if (i < _inventory.Items.Count) _slots[i].SetItem(_inventory.Items[i]);
				else _slots[i].ClearSlot();
			}
		}
	}

}