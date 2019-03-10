using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
    [RequireComponent(typeof(StatsManager), typeof(PlayerProgress), typeof(NetworkIdentity))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private StatsManager _statsManager;
        [SerializeField] private PlayerProgress _progress;
        [SerializeField] private Character _character;
        [SerializeField] private Inventory _inventory;
        [SerializeField] private Equipment _equipment;

        public Character Character => _character;
        public Inventory Inventory => _inventory;
        public Equipment Equipment => _equipment;
        public PlayerProgress Progress => _progress;

        public NetworkConnection Conn
        {
            get
            {
                if (_conn == null)
                    _conn = GetComponent<NetworkIdentity>().connectionToClient;
                return _conn;
            }
        }

        NetworkConnection _conn;

        public void Setup(Character character, Inventory inventory, Equipment equipment, bool isLocalPlayer)
        {
            Debug.Log("setup");
            _statsManager = GetComponent<StatsManager>();
            _progress = GetComponent<PlayerProgress>();
            _character = character;
            _inventory = inventory;
            _equipment = equipment;
            _character.Player = this;
            _inventory.Player = this;
            _equipment.Player = this;
            _statsManager.Player = this;

            if (GetComponent<NetworkIdentity>().isServer)
            {
                UserAccount account = AccountManager.GetAccount(GetComponent<NetworkIdentity>().connectionToClient);
                _character.Stats.Load(account.data);
                _character.unitSkills.Load(account.data);
                _progress.Load(account.data);
                _inventory.Load(account.data);
                _equipment.Load(account.data);

                _character.Stats.Manager = _statsManager;
                _progress.Manager = _statsManager;
            }

            if (isLocalPlayer)
            {
                Debug.Log("local");
                InventoryUI.Instance.SetInventory(_inventory);
                EquipmentUI.Instance.SetEquipment(_equipment);
                StatsUI.instance.SetManager(_statsManager);
                SkillsPanel.instance.SetSkills(character.unitSkills);
                SkillTree.instance.SetCharacter(character);
                SkillTree.instance.SetManager(_statsManager);

                PlayerChat playerChat = GetComponent<PlayerChat>();
                if (playerChat != null)
                {
                    if (GlobalChatChannel.instance != null)
                        playerChat.RegisterChannel(GlobalChatChannel.instance);
                    ChatChannel localChannel = _character.GetComponent<ChatChannel>();
                    if (localChannel != null) playerChat.RegisterChannel(localChannel);
                    ChatUI.instance.SetPlayerChat(playerChat);
                }
            }
        }
    }
}