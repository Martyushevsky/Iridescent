using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
	public class UnitStats : NetworkBehaviour
	{
		[SerializeField] private int _maxHealth;
		[SyncVar] private int _curHealth;

		public Stat Damage;

		public int CurHealth => _curHealth;

		public override void OnStartServer()
		{
			SetHealthRate(1);
		}

		public void SetHealthRate(float rate)
		{
			_curHealth = rate == 0 ? 0 : (int)(_maxHealth * rate);
		}
	}
}