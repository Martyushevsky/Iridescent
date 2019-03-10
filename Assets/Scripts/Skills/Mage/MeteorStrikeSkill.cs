using UnityEngine;

namespace Geekbrains
{
    public class MeteorStrikeSkill : UpgradeableSkill
    {

        [SerializeField] float range = 7f;
        [SerializeField] float radius = 3f;
        [SerializeField] int damage = 25;
        [SerializeField] LayerMask enemyMask;
        [SerializeField] ParticleSystem castEffect;
        [SerializeField] ParticleSystem meteorStrikeEffect;

        public override int level
        {
            set
            {
                base.level = value;
                damage = 25 + 7 * level;
                range = level < 3 ? 7f : 10f;
            }
        }

        protected override void Start()
        {
            base.Start();
            meteorStrikeEffect.transform.SetParent(null);
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
            if (isServer)
            {
                Collider[] colliders = Physics.OverlapSphere(target.transform.position, radius, enemyMask);
                for (int i = 0; i < colliders.Length; i++)
                {
                    Unit enemy = colliders[i].GetComponent<Unit>();
                    if (enemy != null && enemy.HasInteract) enemy.TakeDamage(unit.gameObject, damage);
                }
            }
            else
            {
                castEffect.Stop();
                meteorStrikeEffect.transform.position = target.transform.position;
                meteorStrikeEffect.transform.rotation = Quaternion.LookRotation(target.transform.position - unit.transform.position);
                meteorStrikeEffect.Play();
            }
            base.OnCastComplete();
        }

        private void OnDestroy()
        {
            if (isServer) Destroy(meteorStrikeEffect.gameObject);
        }
    }
}