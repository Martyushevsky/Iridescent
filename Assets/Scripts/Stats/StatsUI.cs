using UnityEngine;

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

		private StatsManager _manager;
		private int _curDamage, _curArmor, _curMoveSpeed;

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
			}
		}

		public void SetManager(StatsManager statsManager)
		{
			_manager = statsManager;
			CheckManagerChanges();
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
		}
	}
}