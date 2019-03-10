using UnityEngine;

namespace Geekbrains
{
    public class SoulStrikeSkill : UpgradeableSkill
    {

        [SerializeField] float range = 7f;
        [SerializeField] int damage = 25;
        [SerializeField] ParticleSystem castEffect;
        [SerializeField] ParticleSystem soulStrikeEffect;

        public override int level
        {
            set
            {
                base.level = value;
                damage = 25 + 5 * level;
                range = level < 3 ? 7f : 10f;
            }
        }

        protected override void Start()
        {
            base.Start();
            soulStrikeEffect.transform.SetParent(null);
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
                soulStrikeEffect.transform.position = enemy.transform.position;
                soulStrikeEffect.transform.rotation = Quaternion.LookRotation(enemy.transform.position - unit.transform.position);
                soulStrikeEffect.Play();
            }
            base.OnCastComplete();
        }

        private void OnDestroy()
        {
            if (isServer) Destroy(soulStrikeEffect.gameObject);
        }
    }
}