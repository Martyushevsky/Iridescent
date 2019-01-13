using UnityEngine;

namespace Geekbrains
{
	[RequireComponent(typeof(UnitMotor), typeof(PlayerStats))]
	public class Character : Unit
	{
		[SerializeField] float _reviveDelay = 5f;
		[SerializeField] GameObject _gfx;

		Vector3 _startPosition;
		float _reviveTime;

		void Start()
		{
			_startPosition = transform.position;
			_reviveTime = _reviveDelay;
		}

		void Update()
		{
			OnUpdate();
		}

		protected override void OnDeadUpdate()
		{
			base.OnDeadUpdate();
			if (_reviveTime > 0)
			{
				_reviveTime -= Time.deltaTime;
			}
			else
			{
				_reviveTime = _reviveDelay;
				Revive();
			}
		}

		protected override void Die()
		{
			base.Die();
			_gfx.SetActive(false);
		}

		protected override void Revive()
		{
			base.Revive();
			transform.position = _startPosition;
			_gfx.SetActive(true);
			if (isServer)
			{
				motor.MoveToPoint(_startPosition);
			}
		}

		public void SetMovePoint(Vector3 point)
		{
			if (!isDead)
			{
				motor.MoveToPoint(point);
			}
		}
	}
}