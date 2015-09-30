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
		
        public static void Interrupter_OnInterruptableSpell(Obj_AI_Base unit, Interrupter.InterruptableSpellEventArgs spell)
        {
            float getDist = EloBuddy.SDK.Extensions.Distance(_Player, unit) / 2.0f; // formula of swag
            if (Program.MiscMenu["interrupt"].Cast<CheckBox>().CurrentValue && Program.W.IsReady() && Program.Q.IsReady() && unit.IsValidTarget(Program.W.Range))
            {
                if (unit.IsValidTarget(220))
                {
                    Program.W.Cast(unit);
                    Program.Q.Cast();
                }
                else
                {
                    Program.W.Cast(unit);
                    EloBuddy.SDK.Core.DelayAction(() => { Program.Q.Cast(); }, (int)getDist);
                }
            }
            if (Program.MiscMenu["interrupt"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady())
            {
                if (unit.Distance(_Player.ServerPosition, true) <= Program.Q.Range)
                {
                    Program.Q.Cast(unit);
                }
            }
            if (Program.MiscMenu["interrupt"].Cast<CheckBox>().CurrentValue && Program.W.IsReady())
            {
                if (unit.Distance(_Player.ServerPosition, true) <= Program.W.Range)
                {
                    Program.W.Cast(unit);
                }
            }
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

