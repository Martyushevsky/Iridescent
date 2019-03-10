using UnityEngine;

namespace Geekbrains
{
    public class ElectroShieldSkill : UpgradeableSkill
    {
        [SerializeField] int damage;
        [SerializeField] float radius;
        [SerializeField] LayerMask enemyMask;
        [SerializeField] ParticleSystem electroEffect;

        public override int level
        {
            set
            {
                base.level = value;
                damage = 25 + 5 * level;
            }
        }

        protected override void OnUse()
        {
            if (isServer)
            {
                unit.Motor.StopFollowingTarget();
            }
            base.OnUse();
        }

        protected override void OnCastComplete()
        {
            if (isServer)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, radius, enemyMask);
                for (int i = 0; i < colliders.Length; i++)
                {
                    Unit enemy = colliders[i].GetComponent<Unit>();
                    if (enemy != null && enemy.HasInteract) enemy.TakeDamage(unit.gameObject, damage);
                }
            }
            else
            {
                electroEffect.Play();
            }
            base.OnCastComplete();
        }

        protected void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
