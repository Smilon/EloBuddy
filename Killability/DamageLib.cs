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

        /**
         * Note : This returns the bool results of the damage calculations on the targ and if they're killable regardless of cooldown or charges.
         * This calculates the damage of the FULL combo, FULL. Meaning, all charges of a certain champions skill. Also, meaning every skillshot was hit.
         * Ex : 3 Ahri R's (Spirit Rush)
         * Ex : 3 Akali R's (Shadow Dance)
         * Ex : Akali Q damange AND the detonation damage (auto attack detonation)
         * Ex : Nunu's R FULL charge
         * Ex : All rounds of GangPlanks ULT
        */
        public static bool checkForBuffAura = false; // true to check to see if player has skill buff or aura on to calc
        public static bool checkCDtoCalc = false; //check if spell has cd between adding calc
        public static AIHeroClient _Player { get { return ObjectManager.Player; } }

        public static Boolean calcDamage(Obj_AI_Base targ)
        {
            var qdmg = 0.0f;
            var wdmg = 0.0f;
            var edmg = 0.0f;
            var rdmg = 0.0f;

            if (_Player.ChampionName == "Aatrox")
            {
                if (checkCDtoCalc) { if (Program.Q.IsReady()) { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Physical, (float)(new[] { 70, 115, 160, 205, 250 }[Program.Q.Level] + 0.6 * _Player.BaseAttackDamage)); } } else { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Physical, (float)(new[] { 70, 115, 160, 205, 250 }[Program.Q.Level] + 0.6 * _Player.BaseAttackDamage)); }
                if (checkForBuffAura) { if (HaveAatroxWDmg) { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Physical, (float)(new[] { 60, 95, 130, 165, 200 }[Program.W.Level] + 1.0 * _Player.BaseAttackDamage)); } } else { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Physical, (float)(new[] { 60, 95, 130, 165, 200 }[Program.W.Level] + 1.0 * _Player.BaseAttackDamage)); }
                if (checkCDtoCalc) { if (Program.E.IsReady()) { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Mixed, (float)(new[] { 75, 110, 145, 180, 215 }[Program.E.Level] + (0.6 * _Player.BaseAbilityDamage) + (0.6 * _Player.BaseAttackDamage))); } } else { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Mixed, (float)(new[] { 75, 110, 145, 180, 215 }[Program.E.Level] + (0.6 * _Player.BaseAbilityDamage) + (0.6 * _Player.BaseAttackDamage))); }
                if (checkCDtoCalc) { if (Program.R.IsReady()) { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 200, 300, 400 }[Program.R.Level] + 1.0 * _Player.TotalMagicalDamage)); } } else { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 200, 300, 400 }[Program.R.Level] + 1.0 * _Player.TotalMagicalDamage)); }
            }
            else if (_Player.ChampionName == "Ahri")
            {
                if (checkCDtoCalc) { if (Program.Q.IsReady()) { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 40, 65, 90, 115, 140 }[Program.Q.Level] + 0.35 * _Player.TotalMagicalDamage)) + _Player.CalculateDamageOnUnit(targ, DamageType.True, (float)(new[] { 40, 65, 90, 115, 140 }[Program.Q.Level] + 0.35 * _Player.TotalMagicalDamage)); } } else { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 40, 65, 90, 115, 140 }[Program.Q.Level] + 0.35 * _Player.TotalMagicalDamage)) + _Player.CalculateDamageOnUnit(targ, DamageType.True, (float)(new[] { 40, 65, 90, 115, 140 }[Program.Q.Level] + 0.35 * _Player.TotalMagicalDamage)); }
                if (checkCDtoCalc) { if (Program.W.IsReady()) { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 40, 65, 90, 115, 140 }[Program.W.Level] + 0.4 * _Player.TotalMagicalDamage)); } } else { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 40, 65, 90, 115, 140 }[Program.W.Level] + 0.4 * _Player.TotalMagicalDamage)); }
                if (checkCDtoCalc) { if (Program.E.IsReady()) { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 60, 90, 120, 150, 200 }[Program.E.Level] + 0.5 * _Player.TotalMagicalDamage)); } } else { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 60, 90, 120, 150, 200 }[Program.E.Level] + 0.5 * _Player.TotalMagicalDamage)); }
                if (checkCDtoCalc) { if (Program.R.IsReady()) { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 70, 110, 150 }[Program.R.Level] + 0.3 * _Player.TotalMagicalDamage) * 3); } } else { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 70, 110, 150 }[Program.R.Level] + 0.3 * _Player.TotalMagicalDamage) * 3); }
            }
            else if (_Player.ChampionName == "Akali")
            {
                if (checkCDtoCalc) { if (Program.Q.IsReady()) { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 35, 55, 75, 95, 115 }[Program.Q.Level] + 0.4 * _Player.TotalMagicalDamage) + (float)(new[] { 45, 70, 95, 120, 145 }[Program.Q.Level] + 0.5 * _Player.TotalMagicalDamage)); } } else { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 35, 55, 75, 95, 115 }[Program.Q.Level] + 0.4 * _Player.TotalMagicalDamage) + (float)(new[] { 45, 70, 95, 120, 145 }[Program.Q.Level] + 0.5 * _Player.TotalMagicalDamage)); }
                if (checkCDtoCalc) { if (Program.E.IsReady()) { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Mixed, (float)(new[] { 30, 55, 80, 105, 130 }[Program.E.Level] + (0.6 * _Player.TotalAttackDamage) + (0.4 * _Player.TotalMagicalDamage))); } } else { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Mixed, (float)(new[] { 30, 55, 80, 105, 130 }[Program.E.Level] + (0.6 * _Player.TotalAttackDamage) + (0.4 * _Player.TotalMagicalDamage))); }
                if (checkCDtoCalc) { if (Program.R.IsReady()) { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 100, 175, 250 }[Program.R.Level] + 0.5 * _Player.TotalMagicalDamage) * 3); } } else { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 100, 175, 250 }[Program.R.Level] + 0.5 * _Player.TotalMagicalDamage) * 3); }
            }
            else if (_Player.ChampionName == "Alistar")
            {
                if (checkCDtoCalc) { if (Program.Q.IsReady()) { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 60, 105, 150, 195, 240 }[Program.Q.Level] + 0.5 * _Player.TotalMagicalDamage)); } } else { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 60, 105, 150, 195, 240 }[Program.Q.Level] + 0.5 * _Player.TotalMagicalDamage)); }
                if (checkCDtoCalc) { if (Program.W.IsReady()) { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 55, 110, 165, 220, 275 }[Program.W.Level] + 0.7 * _Player.TotalMagicalDamage)); } } else { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 55, 110, 165, 220, 275 }[Program.W.Level] + 0.7 * _Player.TotalMagicalDamage)); }
            }
            else if (_Player.ChampionName == "Amumu")
            {
                if (checkCDtoCalc) { if (Program.Q.IsReady()) { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 80, 130, 180, 230, 280 }[Program.Q.Level] + 0.7 * _Player.TotalMagicalDamage)); } } else { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 80, 130, 180, 230, 280 }[Program.Q.Level] + 0.7 * _Player.TotalMagicalDamage)); }
                if (checkForBuffAura) { if (HaveAmumuWDmg) { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 8, 12, 16, 20, 24 }[Program.W.Level] + new[] { 1, 1.5, 2, 2.5, 3 }[Program.W.Level]) * targ.MaxHealth / 100); } } else { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 8, 12, 16, 20, 24 }[Program.W.Level] + new[] { 1, 1.5, 2, 2.5, 3 }[Program.W.Level]) * targ.MaxHealth / 100); }
                if (checkCDtoCalc) { if (Program.E.IsReady()) { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 75, 100, 125, 150, 175 }[Program.E.Level] + 0.5 * _Player.TotalMagicalDamage)); } } else { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 75, 100, 125, 150, 175 }[Program.E.Level] + 0.5 * _Player.TotalMagicalDamage)); }
                if (checkCDtoCalc) { if (Program.R.IsReady()) { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 150, 250, 350 }[Program.R.Level] + 0.8 * _Player.TotalMagicalDamage)); } } else { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 150, 250, 350 }[Program.R.Level] + 0.8 * _Player.TotalMagicalDamage)); }
            }
            else if (_Player.ChampionName == "Anivia")
            {
                if (checkCDtoCalc) { if (Program.Q.IsReady()) { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 60, 90, 120, 150, 180 }[Program.Q.Level] + new[] { 60, 90, 120, 150, 180 }[Program.Q.Level] + 1.0 * _Player.TotalMagicalDamage)); } } else { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 60, 90, 120, 150, 180 }[Program.Q.Level] + new[] { 60, 90, 120, 150, 180 }[Program.Q.Level] + 1.0 * _Player.TotalMagicalDamage)); }
                if (checkCDtoCalc) { if (Program.E.IsReady()) { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 55, 85, 115, 145, 175 }[Program.E.Level] + 0.5 * _Player.TotalMagicalDamage)); } } else { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 55, 85, 115, 145, 175 }[Program.E.Level] + 0.5 * _Player.TotalMagicalDamage)); } if (targ.HasBuff("chilled")) { wdmg = wdmg * 2; }
                //Improve Anivia R logic, check if obj "cryo_storm" was made
                if (checkCDtoCalc) { if (Program.R.IsReady()) { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 80, 120, 160 }[Program.R.Level] + 0.25 * _Player.TotalMagicalDamage)); } } else { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 80, 120, 160 }[Program.R.Level] + 0.25 * _Player.TotalMagicalDamage)); }
            }
            else if (_Player.ChampionName == "Annie")
            {
                if (checkCDtoCalc) { if (Program.Q.IsReady()) { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 80, 115, 150, 185, 220 }[Program.Q.Level] + 0.8 * _Player.TotalMagicalDamage)); } } else { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 80, 115, 150, 185, 220 }[Program.Q.Level] + 0.8 * _Player.TotalMagicalDamage)); }
                if (checkCDtoCalc) { if (Program.W.IsReady()) { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 70, 115, 160, 205, 250 }[Program.W.Level] + 0.85 * _Player.TotalMagicalDamage)); } } else { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 70, 115, 160, 205, 250 }[Program.W.Level] + 0.85 * _Player.TotalMagicalDamage)); }
                if (checkCDtoCalc) { if (Program.R.IsReady()) { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 175, 300, 425 }[Program.R.Level] + 0.8 * _Player.TotalMagicalDamage)); } } else { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 175, 300, 425 }[Program.R.Level] + 0.8 * _Player.TotalMagicalDamage)); }
            }

            /*
             * Small manual "generator" :----------------------------------------------------------------------------------------------------------------------------------
             * 
             *  if (checkCDtoCalc) { if (Program.a.IsReady()) { } } else { }
             * admg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { }[Program.a.Level] + 0.1 * _Player.TotalMagicalDamage));
             * 
             * 
             * "Work Area" :----------------------------------------------------------------------------------------------------------------------------------
             * 
             * admg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { }[Program.a.Level] + 0.1 * _Player.TotalMagicalDamage));
             * 
             * "Finished" :----------------------------------------------------------------------------------------------------------------------------------
             *
             *
            */

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

        private static bool HaveAmumuWDmg
        {
            get { return _Player.HasBuff("AuraofDespair"); }
        }
    }
}
