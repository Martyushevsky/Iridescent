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
			if (Focus == null)
			{
				// блуждание
				Wandering(Time.deltaTime);
				// поиск цели если монстр агресивный
				if (_aggressive) FindEnemy();
			}
			else
			{
				var distance = Vector3.Distance(Focus.InteractionTransform.position, transform.position);
				if (distance > _viewDistance || !Focus.HasInteract)
				{
					// если цель далеко перестаём приследовать
					RemoveFocus();
				}
				else if (distance <= Focus.Radius)
				{
					// действие если цель взоне взаимодействия
					Focus.Interact(gameObject);
				}
			}
		}

		private void FindEnemy()
		{
			var colliders = Physics.OverlapSphere(transform.position, _viewDistance, 1 << LayerMask.NameToLayer("Player"));
			foreach (var t in colliders)
			{
				var interactable = t.GetComponent<Interactable>();
				if (interactable == null || !interactable.HasInteract) continue;
				SetFocus(interactable);
				break;
			}
		}

		protected override void Revive()
		{
			base.Revive();
			transform.position = _startPosition;
			if (isServer)
			{
				Motor.MoveToPoint(_startPosition);
			}
		}

		public override bool Interact(GameObject user)
		{
			if (!base.Interact(user)) return false;
			SetFocus(user.GetComponent<Interactable>());
			return true;

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
			Motor.MoveToPoint(_curDistanation);
		}

		protected override void OnDrawGizmosSelected()
		{
			base.OnDrawGizmosSelected();
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, _viewDistance);
		}
	}
}