using UnityEngine;
using UnityEngine.EventSystems;

namespace Geekbrains
{
    public class EquipmentUI : MonoBehaviour
    {

        #region Singleton
        public static EquipmentUI Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("More than one instance of InventoryUI found!");
                return;
            }
            Instance = this;
        }
        #endregion

        [SerializeField] private GameObject _equipmentUi;
        [Space, SerializeField] private EquipmentSlot _headSlot;
        [SerializeField] private EquipmentSlot _chestSlot;
        [SerializeField] private EquipmentSlot _legsSlot;
        [SerializeField] private EquipmentSlot _righHandSlot;
        [SerializeField] private EquipmentSlot _leftHandSlot;

        private Equipment _equipment;
        private EquipmentSlot[] _slots;

        private void Start()
        {
            _equipmentUi.SetActive(false);
            _slots = new EquipmentSlot[System.Enum.GetValues(typeof(EquipmentSlotType)).Length];
            _slots[(int)EquipmentSlotType.Chest] = _chestSlot;
            _slots[(int)EquipmentSlotType.Head] = _headSlot;
            _slots[(int)EquipmentSlotType.LeftHand] = _leftHandSlot;
            _slots[(int)EquipmentSlotType.Legs] = _legsSlot;
            _slots[(int)EquipmentSlotType.RighHand] = _righHandSlot;
        }

        private void Update()
        {
            if (Input.GetButtonDown("Equipment") && EventSystem.current.currentSelectedGameObject == null)
            {
                _equipmentUi.SetActive(!_equipmentUi.activeSelf);
            }
        }

        public void SetEquipment(Equipment newEquipment)
        {
            _equipment = newEquipment;
            _equipment.OnItemChanged += ItemChanged;
            foreach (var slot in _slots)
            {
                if (slot != null)
                {
                    slot.Equipment = _equipment;
                }
            }

            ItemChanged(0, 0);
        }

        private void ItemChanged(UnityEngine.Networking.SyncList<Item>.Operation op, int itemIndex)
        {
            foreach (var slot in _slots)
            {
                slot.ClearSlot();
            }

            foreach (var item in _equipment.Items)
            {
                _slots[(int)((EquipmentItem)item).EquipSlot].SetItem(item);
            }
        }
    }
}