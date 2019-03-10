using UnityEngine;

namespace Geekbrains
{
    public class AimingShotSkill : UpgradeableSkill
    {
        [SerializeField] float range = 7f;
        [SerializeField] int damage = 25;
        [SerializeField] ParticleSystem castEffect;
        [SerializeField] ParticleSystem aimingShotEffect;

        public override int level
        {
            set
            {
                base.level = value;
                damage = 25 + 5 * level;
            }
        }

        protected override void Start()
        {
            base.Start();
            aimingShotEffect.transform.SetParent(null);
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
            else
            {
                castEffect.Play();
                base.OnUse();
            }
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
                castEffect.Stop();
                aimingShotEffect.transform.position = enemy.transform.position;
                aimingShotEffect.transform.rotation = Quaternion.LookRotation(enemy.transform.position - unit.transform.position);
                aimingShotEffect.Play();
            }
            base.OnCastComplete();
        }

        private void OnDestroy()
        {
            if (isServer) Destroy(aimingShotEffect.gameObject);
        }
    }
}
