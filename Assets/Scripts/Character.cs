using UnityEngine;

namespace Geekbrains
{
    [RequireComponent(typeof(UnitMotor), typeof(PlayerStats))]
    public class Character : Unit
    {
        [SerializeField] float _reviveDelay = 5f;
        [SerializeField] GameObject _gfx;

        private Vector3 _startPosition;
        private float _reviveTime;

        public new PlayerStats Stats => _stats as PlayerStats;

        public Player Player;

        void Start()
        {
            _startPosition = Vector3.zero;
            _reviveTime = _reviveDelay;

            if (Stats.CurHealth == 0)
            {
                transform.position = _startPosition;
                if (isServer)
                {
                    Stats.SetHealthRate(1);
                    _motor.MoveToPoint(_startPosition);
                }
            }
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
            if (_focus == null) return;
            if (!_focus.HasInteract)
            {
                RemoveFocus();
            }
            else
            {
                var distance = Vector3.Distance(_focus.InteractionTransform.position, transform.position);
                if (distance <= InteractDistance)
                {
                    if (!_focus.Interact(gameObject)) RemoveFocus();
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
                _motor.MoveToPoint(_startPosition);
            }
        }

        public void SetMovePoint(Vector3 point)
        {
            if (!IsDead)
            {
                RemoveFocus();
                _motor.MoveToPoint(point);
            }
        }

        public void SetNewFocus(Interactable newFocus)
        {
            if (IsDead) return;
            if (newFocus.HasInteract) SetFocus(newFocus);
        }
    }
}