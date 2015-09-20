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

namespace Killability
{
    class DamageLib
    {
        public static AIHeroClient _Player { get { return ObjectManager.Player; } }

        public static Boolean calcDamage(Obj_AI_Base targ)
        {
            var qdmg = 0.0f;
            var wdmg = 0.0f;
            var edmg = 0.0f;
            var rdmg = 0.0f;

            if (_Player.ChampionName == "Aatrox")
            {
                qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Physical, (float)(new[] { 70, 115, 160, 205, 250 }[Program.Q.Level] + 0.6 * _Player.BaseAttackDamage));
                if (HaveAatroxWDmg)
                {
                    wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Physical, (float)(new[] { 60, 95, 130, 165, 200 }[Program.W.Level] + 1.0 * _Player.BaseAttackDamage));
                }
                edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Mixed, (float)(new[] { 75, 110, 145, 180, 215 }[Program.E.Level] + (0.6 * _Player.BaseAbilityDamage) + (0.6 * _Player.BaseAttackDamage)));
                rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 200, 300, 400 }[Program.R.Level] + 1.0 * _Player.TotalMagicalDamage));
            }
            else if (_Player.ChampionName == "Ahri")
            {
                qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 40, 65, 90, 115, 140 }[Program.Q.Level] + 0.35 * _Player.TotalMagicalDamage));
                qdmg = qdmg + _Player.CalculateDamageOnUnit(targ, DamageType.True, (float)(new[] { 40, 65, 90, 115, 140 }[Program.Q.Level] + 0.35 * _Player.TotalMagicalDamage));
                wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 40, 65, 90, 115, 140 }[Program.W.Level] + 0.4 * _Player.TotalMagicalDamage));
                edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 60, 90, 120, 150, 200 }[Program.E.Level] + 0.5 * _Player.TotalMagicalDamage));
                rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 70, 110, 150 }[Program.R.Level] + 0.3 * _Player.TotalMagicalDamage) * 2); //2 spirit rushes?
            }

            if (qdmg + wdmg + rdmg > targ.Health)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private static bool HaveAatroxWDmg
        {
            get { return _Player.HasBuff("AatroxWPower"); }
        }
    }
}
