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

namespace NerdBlitz
{
    class StateHandler
    {
        public static AIHeroClient _Player { get { return ObjectManager.Player; } }
        public static Spell.Targeted ignite;
        public static Spell.Skillshot flash;
        public static Vector2 oWp;
        public static Vector2 nWp;

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
            var hpPre = _Player.HealthPercent > Program.MinHQNoQ;
            if (Program.MiscMenu["interrupt"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady() && hpPre && Program.MiscMenu["grab" + unit.BaseSkinName].Cast<CheckBox>().CurrentValue)
            {
                if (unit.Distance(_Player.ServerPosition, true) <= Program.Q.Range)
                {
                    CheckCollisionAndCastQ(unit, HitChance.High);
                }
            }
            if (Program.MiscMenu["interrupt"].Cast<CheckBox>().CurrentValue && Program.E.IsReady())
            {
                if (unit.Distance(_Player.ServerPosition, true) <= Program.E.Range)
                {
                    Program.E.Cast();
                }
            }
            if (Program.MiscMenu["interrupt"].Cast<CheckBox>().CurrentValue && Program.R.IsReady())
            {
                if (unit.Distance(_Player.ServerPosition, true) <= Program.R.Range)
                {
                    Program.R.Cast();
                }
            }
        }

        public static void immobileQ()
        {
            var target = TargetSelector2.GetTarget(GetDynamicRange() + 100, DamageType.Magical);
            if (target == null) return;
            if (target.Distance(_Player.ServerPosition) >= Program.Q.Range && Program.MiscMenu["grab" + target.ChampionName].Cast<CheckBox>().CurrentValue)
            {
                if (Program.Q.GetPrediction(target).HitChance == HitChance.Immobile)
                {
                    Program.Q.Cast(target);
                }
            }
        }

        public static void Clear()
        {
            var manaPre = _Player.ManaPercent > Program.MinNumberManaCL;

            if (manaPre == false)
            {
                return;
            }

            if (Program.clearMenu["useRClear"].Cast<CheckBox>().CurrentValue && Program.R.IsReady())
            {
                var minions = ObjectManager.Get<Obj_AI_Minion>().Where(min => min.IsEnemy).Where(min => Program.R.IsInRange(min));
                foreach (var minion in minions)
                {
                    var minionc = EntityManager.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition.To2D(), Program.R.Range);
                    if (minionc.Count >= Program.getMinMin)
                    {
                        Program.R.Cast();
                    }
                }
            }
        }

        public static void Combo()
        {
            var target = TargetSelector2.GetTarget(GetDynamicRange() + 100, DamageType.Magical);
            var t = TargetSelector2.GetTarget(Program.R.Range, DamageType.Magical);
            var summonerIgnite = Player.Spells.FirstOrDefault(o => o.SData.Name == "summonerdot"); // Thanks finn
            var hpPre = _Player.HealthPercent > Program.MinHQNoQ;
            var manaPre = _Player.ManaPercent > Program.MinNumberManaC;

            if (target == null) return;

            if (manaPre == false)
            {
                return;
            }

            if (summonerIgnite != null)
            {
                SpellSlot igSlot = extent.GetSpellSlotFromName(_Player, "summonerdot");
                ignite = new Spell.Targeted(igSlot, 600);
                if (Program.ComboMenu["igniteKill"].Cast<CheckBox>().CurrentValue && ignite.IsReady())
                {
                    if (getIgniteDamage() >= target.Health - 5) //-5 on enemy health for safecheck
                    {
                        ignite.Cast(target);
                    }
                }
                if (Program.ComboMenu["igniteAlways"].Cast<CheckBox>().CurrentValue && ignite.IsReady())
                {
                    if (summonerIgnite != null)
                    {
                        ignite.Cast(target);
                    }
                }
            }

            if (Program.ComboMenu["useWCombo"].Cast<CheckBox>().CurrentValue && Program.W.IsReady())
            {
                Program.W.Cast();
            }
            if (Program.ComboMenu["useQCombo"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady() && target.IsValidTarget(Program.Q.Range) && hpPre && Program.MiscMenu["grab" + target.ChampionName].Cast<CheckBox>().CurrentValue)
            {
                CheckCollisionAndCastQ(target, HitChance.High);
            }
            if (Program.ComboMenu["useECombo"].Cast<CheckBox>().CurrentValue && Program.E.IsReady() && target.IsValidTarget(Program.E.Range))
            {
                Program.E.Cast();
                Orbwalker.ResetAutoAttack();
            }
            if (Program.ComboMenu["useRCombo"].Cast<CheckBox>().CurrentValue && Program.R.IsReady() && t.CountEnemiesInRange(Program.R.Range) >= Program.MinNumberR)
            {
                Program.R.Cast();
            }
        }

        public static void Harass()
        {
            var manaPre = _Player.ManaPercent > Program.MinNumberManaH;
            if (manaPre == false)
            {
                return;
            }

            var target = TargetSelector2.GetTarget(GetDynamicRange() + 100, DamageType.Magical);
            if (target == null) return;
            if (Program.HarassMenu["useQHarass"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady() && target.IsValidTarget(Program.Q.Range) && Program.MiscMenu["grab" + target.ChampionName].Cast<CheckBox>().CurrentValue)
            {
                CheckCollisionAndCastQ(target, HitChance.Medium);
            }
            if (Program.HarassMenu["useEHarass"].Cast<CheckBox>().CurrentValue && Program.E.IsReady() && target.IsValidTarget(Program.E.Range))
            {
                Program.E.Cast();
            }
        }

        public static void Flee()
        {
            if (Program.FleeMenu["useWFlee"].Cast<CheckBox>().CurrentValue && Program.W.IsReady())
            {
                Program.W.Cast();
            }
        }

        public static int getIgniteDamage()
        {
            return 50 + 20 * _Player.Level;
        }

        private static void CheckCollisionAndCastQ(Obj_AI_Base tar, HitChance chance)
        {
            var qPred = Program.Q.GetPrediction(tar);
            var closestMinion = qPred.CollisionObjects.Count(h => h.IsEnemy && !h.IsDead && h is Obj_AI_Minion) < 1;

            if (qPred.HitChance != HitChance.Collision)
            {
                if (qPred.HitChance >= chance && closestMinion)
                {
                    Program.Q.Cast(tar);
                }

            }
        }

        internal static void FlashQCombo()
        {
            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);

            var target = TargetSelector2.GetTarget(Program.Q.Range + 425, DamageType.Magical);

            var qPred = Program.Q.GetPrediction(target);
            var closestMinion = qPred.CollisionObjects.Count(h => h.IsEnemy && !h.IsDead && h is Obj_AI_Minion) < 1;

            var FlashSlot = Player.Spells.FirstOrDefault(o => o.SData.Name == "summonerflash"); // Thanks finn
            SpellSlot flSlot = extent.GetSpellSlotFromName(_Player, "summonerflash");
            flash = new Spell.Skillshot(flSlot, 32767, SkillShotType.Linear);

            if (target == null) return;

            var x = target.Position.Extend(target, 425f);
            if (qPred.HitChance != HitChance.Collision)
            {
                if (qPred.HitChance >= HitChance.High && closestMinion && flash.IsReady())
                {
                    flash.Cast((Vector3)x);
                    Program.Q.Cast((Vector3)x);
                }

            }
        }

        internal static void KSR()
        {
            var target = TargetSelector2.GetTarget(GetDynamicRange() + 100, DamageType.Magical);
            if (target == null) return;

            if (Program.MiscMenu["KSR"].Cast<CheckBox>().CurrentValue)
            {
                if (_Player.CalculateDamageOnUnit(target, DamageType.Magical, (float)(new[] { 250, 375, 500 }[Program.R.Level] + 1.0 * _Player.TotalMagicalDamage)) > target.Health && target.IsValidTarget(Program.R.Range))
                {
                    Program.R.Cast();
                }
            }
        }
    }
}
