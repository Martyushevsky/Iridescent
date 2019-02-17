using UnityEngine;
using UnityEngine.UI;

namespace Geekbrains
{
    public class StatsUI : MonoBehaviour
    {
        #region Singleton
        public static StatsUI instance;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("More than one instance of StatsUI found!");
                return;
            }
            instance = this;
        }
        #endregion

        [SerializeField] private GameObject _statsUi;
        [SerializeField] private StatItem _damageStat;
        [SerializeField] private StatItem _armorStat;
        [SerializeField] private StatItem _moveSpeedStat;
        // поля для вывода уровня и очков характеристик
        [SerializeField] private Text _levelText;
        [SerializeField] private Text _statPointsText;

        [SerializeField] private Slider _expSlider;
        [SerializeField] private Slider _healthSlider;

        private StatsManager _manager;
        private int _curDamage, _curArmor, _curMoveSpeed;
        private int _curLevel, _curStatPoints;
        private float _curExp, _nextLevelExp, _curHealth, _maxHealth;

        void Start()
        {
            _statsUi.SetActive(false);
        }

        void Update()
        {
            if (Input.GetButtonDown("Stats"))
            {
                _statsUi.SetActive(!_statsUi.activeSelf);
            }
            if (_manager != null)
            {
                CheckManagerChanges();

                _curHealth = _manager.Player.Character.Stats.CurHealth;
                _healthSlider.value = _curHealth / _maxHealth;
                Debug.Log(_curHealth);
                Debug.Log(_manager.Player.Character.Stats.CurHealth);
                Debug.Log(_healthSlider.value);
            }
        }

        public void SetManager(StatsManager statsManager)
        {
            _manager = statsManager;
            CheckManagerChanges();

            _expSlider.value = _manager.Exp / _manager.NextLevelExp;

            _curHealth = _manager.Player.Character.Stats.CurHealth;
            _maxHealth = _manager.Player.Character.Stats.MaxHealth;
            _healthSlider.value = _curHealth / _maxHealth;
        }

        private void CheckManagerChanges()
        {
            // stat changes
            if (_curDamage != _manager.Damage)
            {
                _curDamage = _manager.Damage;
                _damageStat.ChangeStat(_curDamage);
            }
            if (_curArmor != _manager.Armor)
            {
                _curArmor = _manager.Armor;
                _armorStat.ChangeStat(_curArmor);
            }
            if (_curMoveSpeed != _manager.MoveSpeed)
            {
                _curMoveSpeed = _manager.MoveSpeed;
                _moveSpeedStat.ChangeStat(_curMoveSpeed);
            }

            if (_curLevel != _manager.Level)
            {
                _curLevel = _manager.Level;
                _levelText.text = _curLevel.ToString();
            }
            if (_curExp != _manager.Exp)
            {
                _expSlider.value = _manager.Exp / _manager.NextLevelExp;
                _curExp = _manager.Exp;

            }
            if (_nextLevelExp != _manager.NextLevelExp)
            {
                _nextLevelExp = _manager.NextLevelExp;
            }
            if (_curStatPoints != _manager.StatPoints)
            {
                _curStatPoints = _manager.StatPoints;
                _statPointsText.text = _curStatPoints.ToString();
                SetUpgradableStats(_curStatPoints > 0);
            }
        }

        // установка возможности апгрейда для всех статов
        private void SetUpgradableStats(bool active)
        {
            _damageStat.SetUpgradable(active);
            _armorStat.SetUpgradable(active);
            _moveSpeedStat.SetUpgradable(active);
        }

        public void UpgradeStat(StatItem stat)
        {
            if (stat == _damageStat) _manager.CmdUpgradeStat((int)StatType.Damage);
            else if (stat == _armorStat) _manager.CmdUpgradeStat((int)StatType.Armor);
            else if (stat == _moveSpeedStat) _manager.CmdUpgradeStat((int)StatType.MoveSpeed);
        }
    }
}