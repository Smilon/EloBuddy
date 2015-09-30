using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using SharpDX;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using extent = EloBuddy.SDK.Extensions;

namespace NerdLeona
{
    class StateHandler
    {
        public static AIHeroClient _Player { get { return ObjectManager.Player; } }
        public static Spell.Targeted ignite;
        static Vector3 mousePos { get { return Game.CursorPos; } }

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
            if (Program.MiscMenu["interrupt"].Cast<CheckBox>().CurrentValue && Program.E.IsReady())
            {
                if (unit.Distance(_Player.ServerPosition, true) <= Program.E.Range)
                {
                    Program.E.Cast(unit);
                }
            }
            if (Program.MiscMenu["interrupt"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady())
            {
                if (unit.Distance(_Player.ServerPosition, true) <= Program.Q.Range)
                {
                    Program.Q.Cast(unit);
                }
            }
            if (Program.MiscMenu["interrupt"].Cast<CheckBox>().CurrentValue && Program.R.IsReady())
            {
                if (unit.Distance(_Player.ServerPosition, true) <= Program.R.Range)
                {
                    Program.R.Cast(unit);
                }
            }
        }

        public static void Combo()
        {
            var target = TargetSelector2.GetTarget(GetDynamicRange() + 100, DamageType.Magical);
            var t = TargetSelector2.GetTarget(Program.R.Range, DamageType.Magical);
            var summonerIgnite = Player.Spells.FirstOrDefault(o => o.SData.Name == "summonerdot"); // Thanks finn

            if (target == null) return;

            var manaPre = _Player.ManaPercent > Program.MinNumberManaC;

            if (!manaPre)
            {
                return;
            }

            if (summonerIgnite != null)
            {
                SpellSlot igSlot = extent.GetSpellSlotFromName(_Player, "summonerdot");
                ignite = new Spell.Targeted(igSlot, 600);
                if (Program.ComboMenu["igniteKill"].Cast<CheckBox>().CurrentValue && ignite.IsReady())
                {
                    if (getIgniteDamage() >= target.Health - 5) //-5 on enemy health for safecheck & HP regen
                    {
                        ignite.Cast(target);
                    }
                }
                else if (Program.ComboMenu["igniteAlways"].Cast<CheckBox>().CurrentValue && ignite.IsReady())
                {
                    ignite.Cast(target);
                }
            }

            if (Program.ComboMenu["useWCombo"].Cast<CheckBox>().CurrentValue && Program.W.IsReady())
            {
                Program.W.Cast();
            }
            if (Program.ComboMenu["useECombo"].Cast<CheckBox>().CurrentValue && Program.E.IsReady() && target.IsValidTarget(Program.E.Range))
            {
                Program.E.Cast(target);
            }
            if (Program.ComboMenu["useQCombo"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady())
            {
                Program.Q.Cast();
            }
            if (Program.ComboMenu["useRCombo"].Cast<CheckBox>().CurrentValue && Program.R.IsReady() && target.IsValidTarget(Program.R.Range) && t.CountEnemiesInRange(Program.R.Range) >= Program.MinNumberR)
            {
                Program.R.Cast(t);
            }
        }

        public static void Harass()
        {
            var manaPre = _Player.ManaPercent > Program.MinNumberManaH;
            if (!manaPre)
            {
                return;
            }

            var target = TargetSelector2.GetTarget(GetDynamicRange() + 100, DamageType.Magical);
            if (target == null) return;
            if (Program.HarassMenu["useQHarass"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady() && target.IsValidTarget(Program.Q.Range))
            {
                Program.Q.Cast(target);
            }
            if (Program.HarassMenu["useEHarass"].Cast<CheckBox>().CurrentValue && Program.E.IsReady() && target.IsValidTarget(Program.E.Range))
            {
                Program.E.Cast(target);
            }
        }

        public static int getIgniteDamage()
        {
            return 50 + 20 * _Player.Level;
        }
    }
}
