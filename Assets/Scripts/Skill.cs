using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Geekbrains
{
    public class Skill : NetworkBehaviour
    {
        public Sprite icon;

        [SerializeField] protected float castTime = 1f;
        [SerializeField] protected float cooldown = 1f;

        [HideInInspector] public float castDelay, cooldownDelay;
        protected Unit unit;
        protected Interactable target;

        List<Skill> _skills;
        int _skillIndex;

        protected virtual void Start()
        {
            _skills = new List<Skill>(GetComponents<Skill>());
            _skillIndex = _skills.FindIndex(x => x == this);
        }

        void Update()
        {
            if (castDelay > 0)
            {
                castDelay -= Time.deltaTime;
                if (castDelay <= 0)
                {
                    castDelay = 0;
                    if (isServer) OnCastComplete();
                }
            }
            if (cooldownDelay > 0)
            {
                cooldownDelay -= Time.deltaTime;
                if (cooldownDelay <= 0)
                {
                    cooldownDelay = 0;
                    if (isServer) OnCooldownComplete();
                }
            }
        }

        public void Use(Unit unit)
        {
            if (castDelay == 0 && cooldownDelay == 0)
            {
                this.unit = unit;
                target = unit.Focus;
                OnUse();
            }
        }

        protected virtual void OnUse()
        {
            if (isServer)
            {
                RpcOnUse(_skillIndex, unit.gameObject, target != null ? target.gameObject : null);
                if (castTime > 0) castDelay = castTime;
                else OnCastComplete();
            }
            else
            {
                if (castTime > 0) castDelay = castTime;
            }
        }

        protected virtual void OnCastComplete()
        {
            if (isServer)
            {
                RpcOnCastComplete(_skillIndex);
                if (cooldown > 0) cooldownDelay = cooldown;
                else OnCooldownComplete();
            }
            else
            {
                if (cooldown > 0) cooldownDelay = cooldown;
            }
        }

        protected virtual void OnCooldownComplete()
        {
            if (isServer) RpcOnCooldownComplete(_skillIndex);
        }

        [ClientRpc]
        void RpcOnUse(int skillIndex, GameObject unitGo, GameObject targetGo)
        {
            _skills[skillIndex].unit = unitGo.GetComponent<Unit>();
            if (targetGo != null) _skills[skillIndex].target = targetGo.GetComponent<Interactable>();
            _skills[skillIndex].OnUse();
        }

        [ClientRpc]
        void RpcOnCastComplete(int skillIndex)
        {
            _skills[skillIndex].OnCastComplete();
        }

        [ClientRpc]
        void RpcOnCooldownComplete(int skillIndex)
        {
            _skills[skillIndex].OnCooldownComplete();
        }
    }
}