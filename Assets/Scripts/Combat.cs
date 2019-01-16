using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
	[RequireComponent(typeof(UnitStats))]
	public class Combat : NetworkBehaviour
	{
		[SerializeField] private float _attackSpeed = 1f;

		private UnitStats _myStats;
		private float _attackCooldown = 0f;

		public delegate void CombatDenegate();
		[SyncEvent] public event CombatDenegate EventOnAttack;

		void Start()
		{
			_myStats = GetComponent<UnitStats>();
		}

		private void Update()
		{
			if (_attackCooldown > 0) _attackCooldown -= Time.deltaTime;
		}

		public bool Attack(UnitStats targetStats)
		{
			if (!(_attackCooldown <= 0)) return false;
			targetStats.TakeDamage(_myStats.Damage.GetValue());
			EventOnAttack?.Invoke();
			_attackCooldown = 1f / _attackSpeed;
			return true;
		}
	}
}