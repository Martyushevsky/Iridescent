using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Geekbrains
{

    public class FrontWarpSkill : UpgradeableSkill
    {
        [SerializeField] float warpDistance = 7f;

        public override int level
        {
            set
            {
                base.level = value;
                warpDistance = 7f + 0.5f * level;
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
                unit.transform.Translate(Vector3.forward * warpDistance);
                unit.Motor.StopFollowingTarget();
            }
            base.OnCastComplete();
        }

    }
}