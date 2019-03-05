using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Geekbrains
{
    public class HealSkill : Skill
    {
        [SerializeField] int healAmount = 10;
        [SerializeField] ParticleSystem particle;

        protected override void OnCastComplete()
        {
            if (isServer) unit.Stats.AddHealth(healAmount);

            else particle.Play();
            base.OnCastComplete();
        }
    }
}