﻿using System.Collections.Generic;
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
        private Vector3 _curDestination;
        private float _changePosTime;

        [Header("Behavior")]
        [SerializeField] private bool _aggressive;
        [SerializeField] private float _rewardExp;
        [SerializeField] private float _viewDistance = 5f;
        [SerializeField] private float _agroDistance = 5f;
        [SerializeField] private float _reviveDelay = 5f;



        private float _reviveTime;
        // список персонажей, атаковавших монстра
        private List<Character> _enemies = new List<Character>();

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
            if (_focus == null)
            {
                // блуждание
                Wandering(Time.deltaTime);
                // поиск цели если монстр агресивный
                if (_aggressive) FindEnemy();
            }
            else
            {
                var distance = Vector3.Distance(_focus.InteractionTransform.position, transform.position);
                if (distance > _viewDistance || !_focus.HasInteract)
                {
                    // если цель далеко перестаём преследовать
                    RemoveFocus();
                }
                else if (distance <= InteractDistance)
                {
                    // действие если цель в зоне взаимодействия
                    if (!_focus.Interact(gameObject)) RemoveFocus();
                }
            }
        }

        protected override void Revive()
        {
            base.Revive();
            transform.position = _startPosition;
            if (isServer)
            {
                _motor.MoveToPoint(_startPosition);
            }
        }

        protected override void Die()
        {
            base.Die();
            if (!isServer) return;
            foreach (var enemy in _enemies)
            {
                enemy.Player.Progress.AddExp(_rewardExp / _enemies.Count);
            }
            _enemies.Clear();
        }

        private void FindEnemy()
        {
            var colliders = Physics.OverlapSphere(transform.position, _agroDistance, 1 << LayerMask.NameToLayer("Player"));
            foreach (var t in colliders)
            {
                var interactable = t.GetComponent<Interactable>();
                if (interactable == null || !interactable.HasInteract) continue;
                SetFocus(interactable);
                break;
            }
        }

        private void Wandering(float deltaTime)
        {
            _changePosTime -= deltaTime;
            if (_changePosTime > 0) return;
            RandomMove();
            _changePosTime = Random.Range(_minMoveDelay, _maxMoveDelay);
        }

        private void RandomMove()
        {
            _curDestination = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up) * new Vector3(_moveRadius, 0, 0) + _startPosition;
            _motor.MoveToPoint(_curDestination);
        }
        
        protected override void DamageWithCombat(GameObject user)
        {
            base.DamageWithCombat(user);
            Unit enemy = user.GetComponent<Unit>();
            if (enemy != null)
            {
                SetFocus(enemy.GetComponent<Interactable>());
                Character character = enemy as Character;
                if (character != null && !_enemies.Contains(character))
                    _enemies.Add(character);
            }
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _viewDistance);
        }
    }
}