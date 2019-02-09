using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
    public class Unit : Interactable
    {
        [SerializeField] protected UnitMotor Motor;
        [SerializeField] protected UnitStats _stats;

        public UnitStats Stats => _stats;

        protected Interactable Focus;

        protected bool IsDead;

        public delegate void UnitDelegate();
        public event UnitDelegate EventOnDamage;
        public event UnitDelegate EventOnDie;
        public event UnitDelegate EventOnRevive;

        public override void OnStartServer()
        {
            Motor.SetMoveSpeed(_stats.MoveSpeed.GetValue());
            _stats.MoveSpeed.OnStatChanged += Motor.SetMoveSpeed;
        }

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
            if (!IsDead)
            {
                if (_stats.CurHealth == 0) Die();
                else OnAliveUpdate();
            }
            else
            {
                OnDeadUpdate();
            }
        }

        /// <summary>
        /// Установка нового объекта в фокус
        /// </summary>
        /// <param name="newFocus"></param>
        protected virtual void SetFocus(Interactable newFocus)
        {
            if (newFocus == Focus) return;
            Focus = newFocus;
            Motor.FollowTarget(newFocus);
        }

        /// <summary>
        /// Удаление фокуса
        /// </summary>
        protected virtual void RemoveFocus()
        {
            Focus = null;
            Motor.StopFollowingTarget();
        }

        [ClientCallback]
        protected virtual void Die()
        {
            IsDead = true;
            GetComponent<Collider>().enabled = false;
            if (!isServer) return;
            HasInteract = false; // с объектом нельзя взаимодействовать
            RemoveFocus();
            Motor.MoveToPoint(transform.position);
            EventOnDie?.Invoke();
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
            IsDead = false;
            GetComponent<Collider>().enabled = true;
            if (!isServer) return;
            HasInteract = true; // с объектом можно взаимодействовать
            _stats.SetHealthRate(1);
            EventOnRevive?.Invoke();
            RpcRevive();
        }

        [ClientRpc]
        private void RpcRevive()
        {
            if (!isServer) Revive();
        }

        public override bool Interact(GameObject user)
        {
            Debug.Log(gameObject.name + " ineracted with " + user.name);
            Combat combat = user.GetComponent<Combat>();
            if (combat != null)
            {
                if (combat.Attack(_stats))
                {
                    DamageWithCombat(user);
                }
                return true;
            }
            return base.Interact(user);
        }

        protected virtual void DamageWithCombat(GameObject user)
        {
            EventOnDamage?.Invoke();
        }
    }
}