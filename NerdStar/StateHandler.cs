using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using extent = EloBuddy.SDK.Extensions;

namespace NerdStar
{
    class StateHandler
    {
        public static AIHeroClient _Player { get { return ObjectManager.Player; } }

        public static float GetDynamicRange()
        {
            if (Program.Q.IsReady())
            {
                return Program.Q.Range;
            }
            return _Player.GetAutoAttackRange();
        }

        public static void Combo()
        {
            var target = TargetSelector2.GetTarget(GetDynamicRange() + 100, DamageType.Magical);
            float getDist = EloBuddy.SDK.Extensions.Distance(_Player, target) / 2.0f; // formula of swag
            if (target == null) return;
            if (Program.W.IsReady() && Program.Q.IsReady() && target.IsValidTarget(Program.W.Range))
            {
                if (target.IsValidTarget(220)) // if target is close then apply combo fast NO DELAY
                {
                    Program.W.Cast(target);
                    Program.Q.Cast();
                }
                else // if target is a bit farther then apply combo with delay
                {
                    Program.W.Cast(target);
                    EloBuddy.SDK.Core.DelayAction(() => { Program.Q.Cast(); }, (int)getDist);
                }
            }
        }
    }
}

