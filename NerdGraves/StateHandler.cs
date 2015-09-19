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

namespace NerdGraves
{
    class StateHandler
    {
        public static AIHeroClient _Player { get { return ObjectManager.Player; } }
        public static Spell.Targeted ignite;
        public static Spell.Active flash;
        public static Vector2 oWp;
        private static Vector3 mousePos { get { return Game.CursorPos; } }
        public static Vector2 nWp;

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
            var qPred = Program.Q.GetPrediction(target);
            var wPred = Program.W.GetPrediction(target);
            var rPred = Program.R.GetPrediction(target);

            if (target == null) return;

            if (Program.ComboMenu["useRCKill"].Cast<CheckBox>().CurrentValue && Program.R.IsReady() && target.IsValidTarget(Program.R.Range) && CalcDamage(target) >= target.Health - 5)
            {
                if (rPred.HitChance >= HitChance.High) { Program.R.Cast(target); }
            }
            if (Program.ComboMenu["useECombo"].Cast<CheckBox>().CurrentValue && Program.E.IsReady() && target.IsValidTarget(Program.R.Range))
            {
                Program.E.Cast(mousePos);
            }
            if (Program.ComboMenu["useQCombo"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady() && target.IsValidTarget(Program.Q.Range))
            {
                if (qPred.HitChance >= HitChance.High) { Program.Q.Cast(target); }
            }
            if (Program.ComboMenu["useWCombo"].Cast<CheckBox>().CurrentValue && Program.W.IsReady() && target.IsValidTarget(Program.W.Range))
            {
                if (wPred.HitChance >= HitChance.Low) { Program.W.Cast(target); }
            }
            if (Program.ComboMenu["useRKill"].Cast<CheckBox>().CurrentValue && Program.R.IsReady() && target.IsValidTarget(Program.R.Range) && getRDmg(target) >= target.Health - 5)
            {
                if (rPred.HitChance >= HitChance.High) { Program.R.Cast(target); }
            }
        }

        public static void Harass()
        {
            var target = TargetSelector2.GetTarget(GetDynamicRange() + 100, DamageType.Magical);
            var qPred = Program.Q.GetPrediction(target);
            var wPred = Program.W.GetPrediction(target);
            if (target == null) return;

            var manaCheck = _Player.ManaPercent > Program.MinManaH;

            if (!manaCheck) {
                return;
            }

            if (Program.HarassMenu["useQHarass"].Cast<CheckBox>().CurrentValue && Program.Q.IsReady() && target.IsValidTarget(Program.Q.Range))
            {
                if (qPred.HitChance >= HitChance.High) { Program.Q.Cast(target); }
            }
            if (Program.HarassMenu["useWHarass"].Cast<CheckBox>().CurrentValue && Program.W.IsReady() && target.IsValidTarget(Program.W.Range))
            {
                if (wPred.HitChance >= HitChance.Low) { Program.W.Cast(target); }
            }
        }

        public static void Flee()
        {
            if (Program.FleeMenu["useE"].Cast<CheckBox>().CurrentValue && Program.E.IsReady())
            {
                Program.E.Cast(mousePos);
            }
        }

        public static float CalcDamage(Obj_AI_Base target)
        {
            var qdmg = 0.0f;
            var wdmg = 0.0f;
            var rdmg = 0.0f;
            if (Program.Q.IsReady()) {
                qdmg = _Player.CalculateDamageOnUnit(target, DamageType.Physical, (float)(new[] { 60, 90, 120, 150, 180 }[Program.Q.Level] + 0.8 * _Player.BaseAttackDamage));
            }
            if (Program.W.IsReady())
            {
                wdmg = _Player.CalculateDamageOnUnit(target, DamageType.Magical, (float)(new[] { 60, 110, 160, 210, 260 }[Program.W.Level] + 0.6 * _Player.TotalMagicalDamage));
            }
            if (Program.R.IsReady())
            {
                rdmg = _Player.CalculateDamageOnUnit(target, DamageType.Physical, (float)(new[] { 200, 320, 440 }[Program.R.Level] + 1.2 * _Player.BaseAttackDamage));
            }

            return qdmg + wdmg + rdmg;
        }

        public static float getRDmg(Obj_AI_Base target)
        {
            return _Player.CalculateDamageOnUnit(target, DamageType.Physical, (float)(new[] { 200, 320, 440 }[Program.R.Level] + 1.2 * _Player.BaseAttackDamage));
        }

    }
}
