using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
	public class Unit : NetworkBehaviour
	{
		[SerializeField] protected UnitMotor motor;
		[SerializeField] protected UnitStats myStats;

		protected bool isDead;

		private void Update()
		{
			OnUpdate();
		}

		protected virtual void OnAliveUpdate()
		{
		}

		protected virtual void OnDeadUpdate()
		{
		}

		protected void OnUpdate()
		{
			if (!isServer) return;
			if (!isDead)
			{
				if (myStats.CurHealth == 0) Die();
				else OnAliveUpdate();
			}
			else
			{
				OnDeadUpdate();
			}
		}

		[ClientCallback]
		protected virtual void Die()
		{
			isDead = true;
			if (!isServer) return;
			motor.MoveToPoint(transform.position);
			RpcDie();
		}

		[ClientRpc]
		private void RpcDie()
		{
			if (!isServer) Die();
		}

		[ClientCallback]
		protected virtual void Revive()
		{
			isDead = false;
			if (!isServer) return;
			myStats.SetHealthRate(1);
			RpcRevive();
		}

		[ClientRpc]
		private void RpcRevive()
		{
			if (!isServer) Revive();
		}
	}
}