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

namespace NerdGath
{
    class StateHandler
    {
        public static AIHeroClient _Player { get { return ObjectManager.Player; } }
        public static Spell.Targeted ignite;

        public static float GetDynamicRange()
        {
            if (Program.Q.IsReady())
            {
                return Program.Q.Range;
            }
            return _Player.GetAutoAttackRange();
        }

        public static void Interrupter_OnInterruptableSpell(Obj_AI_Base unit, InterruptableSpellEventArgs spell)
        {
            var target = TargetSelector2.GetTarget(GetDynamicRange() + 100, DamageType.Magical);
            if (Program.MiscMenu["interrupt"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady())
            {
                if (unit.Distance(_Player.ServerPosition, true) <= Program.Q.Range)
                {
                    Program.Q.Cast(target);
                }
            }
        }

        public static void Combo()
        {
            var target = TargetSelector2.GetTarget(GetDynamicRange() + 100, DamageType.Magical);
            var t = TargetSelector2.GetTarget(Program.R.Range, DamageType.Magical);
            var summonerIgnite = Player.Spells.FirstOrDefault(o => o.SData.Name == "summonerdot"); // Thanks finn

            float rdmg = EloBuddy.SDK.DamageLibrary.GetSpellDamage(_Player, target, EloBuddy.SpellSlot.R); // damage of cho R

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
                    if (getIgniteDamage() > target.Health - 10) //-10 on enemy health for safecheck
                    {
                        ignite.Cast(target);
                    }
                }
            }

            if (Program.ComboMenu["useQCombo"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady() && target.IsValidTarget(Program.Q.Range))
            {
                var qPred = Program.Q.GetPrediction(target);
                if (qPred.HitChance >= HitChance.High)
                {
                    Program.Q.Cast(target);
                }
            }
            if (Program.ComboMenu["useWCombo"].Cast<CheckBox>().CurrentValue && Program.W.IsReady() && target.IsValidTarget(Program.W.Range))
            {
                Program.W.Cast(target);
            }
            if (Program.ComboMenu["useRCombo"].Cast<CheckBox>().CurrentValue && Program.ComboMenu["useRKill"].Cast<CheckBox>().CurrentValue && Program.R.IsReady() && rdmg > target.Health - 10)
            {
                Program.R.Cast(target);
            }
            if (Program.ComboMenu["useRCombo"].Cast<CheckBox>().CurrentValue && !Program.ComboMenu["useRKill"].Cast<CheckBox>().CurrentValue && Program.R.IsReady())
            {
                Program.R.Cast(target);
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
                var qPred = Program.Q.GetPrediction(target);
                if (qPred.HitChance >= HitChance.High)
                {
                    Program.Q.Cast(target);
                }
            }

            if (Program.HarassMenu["useWHarass"].Cast<CheckBox>().CurrentValue && Program.W.IsReady() && target.IsValidTarget(Program.W.Range))
            {
                Program.W.Cast(target);
            }
        }

        public static void Flee()
        {
            var target = TargetSelector2.GetTarget(GetDynamicRange() + 100, DamageType.Magical);
            if (Program.FleeMenu["useQFlee"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady())
            {
                Program.Q.Cast(target);
            }
        }

        public static void KS()
        {
            var target = TargetSelector2.GetTarget(GetDynamicRange() + 100, DamageType.Magical);
            float qdmg = EloBuddy.SDK.DamageLibrary.GetSpellDamage(_Player, target, EloBuddy.SpellSlot.Q); // damage of cho Q
            float wdmg = EloBuddy.SDK.DamageLibrary.GetSpellDamage(_Player, target, EloBuddy.SpellSlot.W); // damage of cho W
            float rdmg = EloBuddy.SDK.DamageLibrary.GetSpellDamage(_Player, target, EloBuddy.SpellSlot.R); // damage of cho R
            var summonerIgnite = Player.Spells.FirstOrDefault(o => o.SData.Name == "summonerdot"); // Thanks finn

            if (summonerIgnite != null)
            {
                SpellSlot igSlot = extent.GetSpellSlotFromName(_Player, "summonerdot");
                ignite = new Spell.Targeted(igSlot, 600);
                if (Program.KSMenu["igniteKill"].Cast<CheckBox>().CurrentValue && ignite.IsReady())
                {
                    if (getIgniteDamage() > target.Health - 10) //-10 on enemy health for safecheck
                    {
                        ignite.Cast(target);
                    }
                }
            }

            if (Program.KSMenu["qKS"].Cast<CheckBox>().CurrentValue && qdmg >= target.Health - 10 && Program.Q.IsReady() && target.IsValidTarget(Program.Q.Range))
            {
                var qPred = Program.Q.GetPrediction(target);
                if (qPred.HitChance >= HitChance.High)
                {
                    Program.Q.Cast(target);
                }
            }
            else if (Program.KSMenu["wKS"].Cast<CheckBox>().CurrentValue && wdmg >= target.Health - 10 && Program.W.IsReady() && target.IsValidTarget(Program.W.Range))
            {
                Program.W.Cast(target);
            }
            else if (Program.KSMenu["rKS"].Cast<CheckBox>().CurrentValue && rdmg >= target.Health - 10 && Program.R.IsReady() && target.IsValidTarget(Program.R.Range))
            {
                Program.R.Cast(target);
            }
        }

        public static void Farm(String type)
        {
            if (type == "Jungle")
            {
                foreach (Obj_AI_Base minion in EntityManager.GetJungleMonsters())
                {
                    float rdmg = EloBuddy.SDK.DamageLibrary.GetSpellDamage(_Player, minion, EloBuddy.SpellSlot.R); // damage of cho R
                    if (minion.IsValidTarget(1000f))
                    {
                        if (Program.FarmMenu["qJG"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady() && minion.IsValidTarget(Program.Q.Range))
                        {
                            Program.Q.Cast(minion);
                        }
                        if (Program.FarmMenu["wJG"].Cast<CheckBox>().CurrentValue && Program.W.IsReady() && minion.IsValidTarget(Program.W.Range))
                        {
                            Program.W.Cast(minion);
                        }
                        if (Program.FarmMenu["rJG"].Cast<CheckBox>().CurrentValue && Program.R.IsReady() && minion.IsValidTarget(Program.R.Range) && rdmg > minion.Health)
                        {
                            Program.R.Cast(minion);
                        }
                    }
                }
            }
            else if (type == "Lane")
            {
                foreach (Obj_AI_Base minion in EntityManager.GetLaneMinions())
                {
                    float rdmg = EloBuddy.SDK.DamageLibrary.GetSpellDamage(_Player, minion, EloBuddy.SpellSlot.R); // damage of cho R
                    if (minion.IsValidTarget(1000f))
                    {
                        if (Program.FarmMenu["qLaneClear"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady() && minion.IsValidTarget(Program.Q.Range))
                        {
                            Program.Q.Cast(minion);
                        }
                        if (Program.FarmMenu["wLaneClear"].Cast<CheckBox>().CurrentValue && Program.W.IsReady() && minion.IsValidTarget(Program.W.Range))
                        {
                            Program.W.Cast(minion);
                        }
                        if (Program.FarmMenu["RLaneClear"].Cast<CheckBox>().CurrentValue && Program.R.IsReady() && minion.IsValidTarget(Program.R.Range) && rdmg > minion.Health)
                        {
                            Program.R.Cast(minion);
                        }
                    }
                }
            }
            else if (type == "LastHit")
            {
                foreach (Obj_AI_Base minion in EntityManager.GetLaneMinions())
                {
                    float rdmg = EloBuddy.SDK.DamageLibrary.GetSpellDamage(_Player, minion, EloBuddy.SpellSlot.R); // damage of cho R
                    if (minion.IsValidTarget(1000f))
                    {
                        if (Program.FarmMenu["rLastHit"].Cast<CheckBox>().CurrentValue && Program.R.IsReady() && minion.IsValidTarget(Program.R.Range) && rdmg > minion.Health)
                        {
                            Program.R.Cast(minion);
                        }
                    }
                }
            }
        }

        public static int getIgniteDamage()
        {
            return 50 + 20 * _Player.Level;
        }
    }
}
