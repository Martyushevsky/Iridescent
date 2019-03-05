using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
    public class Unit : Interactable
    {
        [SerializeField] protected UnitMotor _motor;
        [SerializeField] protected UnitStats _stats;
        public UnitStats Stats => _stats;
        public UnitMotor Motor => _motor;

        protected Interactable _focus;
        public Interactable Focus => _focus;

        public UnitSkills unitSkills;

        protected float InteractDistance;

        protected bool IsDead;

        public delegate void UnitDelegate();
        public event UnitDelegate EventOnDamage;
        public event UnitDelegate EventOnDie;
        public event UnitDelegate EventOnRevive;

        public override void OnStartServer()
        {
            _motor.SetMoveSpeed(_stats.MoveSpeed.GetValue());
            _stats.MoveSpeed.OnStatChanged += _motor.SetMoveSpeed;
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

        public override float GetInteractDistance(GameObject user)
        {
            Combat combat = user.GetComponent<Combat>();
            return base.GetInteractDistance(user) + (combat != null ? combat.attackDistance : 0f);
        }

        protected virtual void DamageWithCombat(GameObject user)
        {
            EventOnDamage?.Invoke();
        }

        public void TakeDamage(GameObject user, int damage)
        {
            _stats.TakeDamage(damage);
            DamageWithCombat(user);
        }

        public virtual void SetFocus(Interactable newFocus)
        {
            if (newFocus == _focus) return;
            _focus = newFocus;
            InteractDistance = _focus.GetInteractDistance(gameObject);
            _motor.FollowTarget(newFocus, InteractDistance);
        }

        public virtual void RemoveFocus()
        {
            _focus = null;
            _motor.StopFollowingTarget();
        }

        public void UseSkill(int skillNum)
        {
            if (!IsDead && skillNum < unitSkills.Count)
            {
                unitSkills[skillNum].Use(this);
            }
        }

        protected virtual void Die()
        {
            IsDead = true;
            GetComponent<Collider>().enabled = false;
            EventOnDie?.Invoke();
            if (!isServer) return;
            HasInteract = false; // с объектом нельзя взаимодействовать
            RemoveFocus();
            _motor.MoveToPoint(transform.position);
            RpcDie();
        }

        [ClientRpc]
        private void RpcDie()
        {
            if (!isServer) Die();
        }

        protected virtual void Revive()
        {
            IsDead = false;
            GetComponent<Collider>().enabled = true;
            EventOnRevive?.Invoke();
            if (!isServer) return;
            HasInteract = true; // с объектом можно взаимодействовать
            _stats.SetHealthRate(1);
            RpcRevive();
        }

        [ClientRpc]
        private void RpcRevive()
        {
            if (!isServer) Revive();
        }
    }
}