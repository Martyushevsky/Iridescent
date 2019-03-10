using UnityEngine;

namespace Geekbrains
{
    public class DoubleShotSkill : UpgradeableSkill
    {

        [SerializeField] float range = 7f;
        [SerializeField] int damage = 25;
        [SerializeField] ParticleSystem doubleShotEffect;

        public override int level
        {
            set
            {
                base.level = value;
                damage = 25 + 3 * level;
            }
        }

        protected override void Start()
        {
            base.Start();
            doubleShotEffect.transform.SetParent(null);
        }

        protected override void OnUse()
        {
            if (isServer)
            {
                if (target != null && target.GetComponent<Unit>() != null)
                {
                    if (Vector3.Distance(target.transform.position, unit.transform.position) <= range)
                    {
                        unit.RemoveFocus();
                        base.OnUse();
                    }
                }
            }
            else base.OnUse();
        }

        protected override void OnCastComplete()
        {
            Unit enemy = target.GetComponent<Unit>();
            if (isServer)
            {
                if (enemy.HasInteract)
                {
                    enemy.TakeDamage(unit.gameObject, damage);
                    unit.SetFocus(enemy);
                }
            }
            else
            {
                doubleShotEffect.transform.position = enemy.transform.position;
                doubleShotEffect.transform.rotation = Quaternion.LookRotation(enemy.transform.position - unit.transform.position);
                doubleShotEffect.Play();
            }
            base.OnCastComplete();
        }

        private void OnDestroy()
        {
            if (isServer) Destroy(doubleShotEffect.gameObject);
        }
    }

}