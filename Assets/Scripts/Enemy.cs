using UnityEngine;

namespace Geekbrains
{
	[RequireComponent(typeof(UnitMotor), typeof(EnemyStats))]
	public class Enemy : Unit
	{
		[Header("Movement")]
		[SerializeField] private float _moveRadius = 10f;
		[SerializeField] private float _minMoveDelay = 4f;
		[SerializeField] private float _maxMoveDelay = 12f;
		private Vector3 _startPosition;
		private Vector3 _curDistanation;
		private float _changePosTime;

		[Header("Behavior")]
		[SerializeField] private bool _aggressive;
		[SerializeField] private float _viewDistance = 5f;
		[SerializeField] private float _reviveDelay = 5f;
		private float _reviveTime;

		private void Start()
		{
			_startPosition = transform.position;
			_changePosTime = Random.Range(_minMoveDelay, _maxMoveDelay);
			_reviveTime = _reviveDelay;
		}

		private void Update()
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

		protected override void OnAliveUpdate()
		{
			base.OnAliveUpdate();
			Wandering(Time.deltaTime);
		}

		protected override void Revive()
		{
			base.Revive();
			transform.position = _startPosition;
			if (isServer)
			{
				motor.MoveToPoint(_startPosition);
			}
		}

		private void Wandering(float deltaTime)
		{
			_changePosTime -= deltaTime;
			if (!(_changePosTime <= 0)) return;
			RandomMove();
			_changePosTime = Random.Range(_minMoveDelay, _maxMoveDelay);
		}

		private void RandomMove()
		{
			_curDistanation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up) * new Vector3(_moveRadius, 0, 0) + _startPosition;
			motor.MoveToPoint(_curDistanation);
		}
	}
}