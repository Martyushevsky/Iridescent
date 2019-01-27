﻿using UnityEngine;

namespace Geekbrains
{
	[RequireComponent(typeof(UnitMotor), typeof(PlayerStats))]
	public class Character : Unit
	{
		[SerializeField] float _reviveDelay = 5f;
		[SerializeField] GameObject _gfx;

		private Vector3 _startPosition;
		private float _reviveTime;
		public Inventory Inventory;

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

		protected override void OnAliveUpdate()
		{
			base.OnAliveUpdate();
			if (Focus == null) return;
			if (!Focus.HasInteract)
			{
				RemoveFocus();
			}
			else
			{
				var distance = Vector3.Distance(Focus.InteractionTransform.position, transform.position);
				if (distance <= Focus.Radius)
				{
					if (!Focus.Interact(gameObject)) RemoveFocus();
				}
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
				Motor.MoveToPoint(_startPosition);
			}
		}

		public void SetMovePoint(Vector3 point)
		{
			if (!IsDead)
			{
				RemoveFocus();
				Motor.MoveToPoint(point);
			}
		}

		public void SetInventory(Inventory inventory)
		{
			Inventory = inventory;
			inventory.DropPoint = transform;
		}

		public void SetNewFocus(Interactable newFocus)
		{
			if (IsDead) return;
			if (newFocus.HasInteract) SetFocus(newFocus);
		}
	}
}