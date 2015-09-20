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
        public static bool checkForBuffA = false; // true to check to see if player has skill buff or aura on to calc
        public static bool checkCDtoCalc = true; //check if spell has cd between adding calc

        public static Spell.Targeted ignite;
        public static AIHeroClient _Player { get { return ObjectManager.Player; } }

        public static Boolean calcDamage(Obj_AI_Base targ)
        {
            var qdmg = 0.0f;
            var wdmg = 0.0f;
            var edmg = 0.0f;
            var rdmg = 0.0f;
            var igdmg = 0.0f;
            var summonerIgnite = Player.Spells.FirstOrDefault(o => o.SData.Name == "summonerdot"); // Thanks finn

            float[] aatrox = new float[] {
                (float)(new[] { 70, 115, 160, 205, 250 }[Program.Q.Level] + 0.6 * _Player.BaseAttackDamage),
                (float)(new[] { 60, 95, 130, 165, 200 }[Program.W.Level] + 1.0 * _Player.BaseAttackDamage),
                (float)(new[] { 75, 110, 145, 180, 215 }[Program.E.Level] + (0.6 * _Player.BaseAbilityDamage) + (0.6 * _Player.BaseAttackDamage)),
                (float)(new[] { 200, 300, 400 }[Program.R.Level] + 1.0 * _Player.TotalMagicalDamage)
            };

            float[] ahri = new float[] {
                (float)(new[] { 40, 65, 90, 115, 140 }[Program.Q.Level] + 0.35 * _Player.TotalMagicalDamage) + _Player.CalculateDamageOnUnit(targ, DamageType.True, (float)(new[] { 40, 65, 90, 115, 140 }[Program.Q.Level] + 0.35 * _Player.TotalMagicalDamage)),
                (float)(new[] { 40, 65, 90, 115, 140 }[Program.W.Level] + 0.4 * _Player.TotalMagicalDamage),
                (float)(new[] { 60, 90, 120, 150, 200 }[Program.E.Level] + 0.5 * _Player.TotalMagicalDamage),
                (float)(new[] { 70, 110, 150 }[Program.R.Level] + 0.3 * _Player.TotalMagicalDamage) * 3
            };

            float[] akali = new float[] {
                (float)(new[] { 35, 55, 75, 95, 115 }[Program.Q.Level] + 0.4 * _Player.TotalMagicalDamage) + (float)(new[] { 45, 70, 95, 120, 145 }[Program.Q.Level] + 0.5 * _Player.TotalMagicalDamage),
                (float)(new[] { 30, 55, 80, 105, 130 }[Program.E.Level] + (0.6 * _Player.TotalAttackDamage) + (0.4 * _Player.TotalMagicalDamage)),
                (float)(new[] { 100, 175, 250 }[Program.R.Level] + 0.5 * _Player.TotalMagicalDamage) * 3
            };

            float[] alistar = new float[] {
                (float)(new[] { 60, 105, 150, 195, 240 }[Program.Q.Level] + 0.5 * _Player.TotalMagicalDamage),
                (float)(new[] { 55, 110, 165, 220, 275 }[Program.W.Level] + 0.7 * _Player.TotalMagicalDamage)
            };

            float[] amumu = new float[] {
              (float)(new[] { 80, 130, 180, 230, 280 }[Program.Q.Level] + 0.7 * _Player.TotalMagicalDamage),
              (float)(new[] { 8, 12, 16, 20, 24 }[Program.W.Level] + new[] { 1, 1.5, 2, 2.5, 3 }[Program.W.Level]) * targ.MaxHealth / 100,
              (float)(new[] { 75, 100, 125, 150, 175 }[Program.E.Level] + 0.5 * _Player.TotalMagicalDamage),
              (float)(new[] { 150, 250, 350 }[Program.R.Level] + 0.8 * _Player.TotalMagicalDamage)
            };

            float[] anivia = new float[] {
                (float)(new[] { 60, 90, 120, 150, 180 }[Program.Q.Level] + new[] { 60, 90, 120, 150, 180 }[Program.Q.Level] + 1.0 * _Player.TotalMagicalDamage),
                (float)(new[] { 55, 85, 115, 145, 175 }[Program.E.Level] + 0.5 * _Player.TotalMagicalDamage)
                (float)(new[] { 80, 120, 160 }[Program.R.Level] + 0.25 * _Player.TotalMagicalDamage)
            };

            if (_Player.ChampionName == "Aatrox")
            {
                if (checkCDtoCalc) { if (Program.Q.IsReady()) { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Physical, aatrox[0]); } } else { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Physical, aatrox[0]); }
                if (checkForBuffA) { if (UserHaveAatroxWDmgB) { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Physical, aatrox[1]); } } else { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Physical, aatrox[1]); }
                if (checkCDtoCalc) { if (Program.E.IsReady()) { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Mixed, aatrox[2]); } } else { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Mixed, aatrox[2]); }
                if (checkCDtoCalc) { if (Program.R.IsReady()) { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, aatrox[3]); } } else { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, aatrox[3]); }
            }
            else if (_Player.ChampionName == "Ahri")
            {
                if (checkCDtoCalc) { if (Program.Q.IsReady()) { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, ahri[0]); } } else { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, ahri[0]); }
                if (checkCDtoCalc) { if (Program.W.IsReady()) { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, ahri[1]); } } else { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, ahri[1]); }
                if (checkCDtoCalc) { if (Program.E.IsReady()) { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, ahri[2]); } } else { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, ahri[2]); }
                if (checkCDtoCalc) { if (Program.R.IsReady()) { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, ahri[3]); } } else { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, ahri[3]); }
            }
            else if (_Player.ChampionName == "Akali")
            {
                if (checkCDtoCalc) { if (Program.Q.IsReady()) { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, akali[0]); } } else { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, akali[0]); }
                if (checkCDtoCalc) { if (Program.E.IsReady()) { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Mixed, akali[1]); } } else { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Mixed, akali[1]); }
                if (checkCDtoCalc) { if (Program.R.IsReady()) { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, akali[2]); } } else { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, akali[2]); }
            }
            else if (_Player.ChampionName == "Alistar")
            {
                if (checkCDtoCalc) { if (Program.Q.IsReady()) { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, alistar[0]); } } else { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, alistar[0]); }
                if (checkCDtoCalc) { if (Program.W.IsReady()) { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, alistar[1]); } } else { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, alistar[1]); }
            }
            else if (_Player.ChampionName == "Amumu")
            {
                if (checkCDtoCalc) { if (Program.Q.IsReady()) { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, amumu[0]); } } else { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, amumu[0]); }
                if (checkForBuffA) { if (PlayerHaveAmumuWDmg) { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, amumu[1]); } } else { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, amumu[1]); }
                if (checkCDtoCalc) { if (Program.E.IsReady()) { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, amumu[2]); } } else { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, amumu[2]); }
                if (checkCDtoCalc) { if (Program.R.IsReady()) { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, amumu[3]); } } else { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, amumu[3]); }
            }
            else if (_Player.ChampionName == "Anivia")
            {
                if (checkCDtoCalc) { if (Program.Q.IsReady()) { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, anivia[0]); } } else { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, anivia[0]); }
                if (checkCDtoCalc) { if (Program.E.IsReady()) { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, anivia[1]); } } else { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, anivia[1]); } if (targ.HasBuff("chilled")) { edmg = edmg * 2; }
                if (checkCDtoCalc) { if (Program.R.IsReady()) { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, anivia[2]); } } else { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, anivia[2]); }
            }
            else if (_Player.ChampionName == "Annie")
            {
                if (checkCDtoCalc) { if (Program.Q.IsReady()) { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 80, 115, 150, 185, 220 }[Program.Q.Level] + 0.8 * _Player.TotalMagicalDamage)); } } else { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 80, 115, 150, 185, 220 }[Program.Q.Level] + 0.8 * _Player.TotalMagicalDamage)); }
                if (checkCDtoCalc) { if (Program.W.IsReady()) { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 70, 115, 160, 205, 250 }[Program.W.Level] + 0.85 * _Player.TotalMagicalDamage)); } } else { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 70, 115, 160, 205, 250 }[Program.W.Level] + 0.85 * _Player.TotalMagicalDamage)); }
                if (checkCDtoCalc) { if (Program.R.IsReady()) { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 175, 300, 425 }[Program.R.Level] + 0.8 * _Player.TotalMagicalDamage)); } } else { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 175, 300, 425 }[Program.R.Level] + 0.8 * _Player.TotalMagicalDamage)); }
            }
            else if (_Player.ChampionName == "Ashe")
            {
                if (checkCDtoCalc) { if (Program.W.IsReady()) { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Physical, (float)(new[] { 20, 35, 50, 65, 80 }[Program.W.Level] + 1.0 * _Player.TotalAttackDamage)); } } else { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Physical, (float)(new[] { 20, 35, 50, 65, 80 }[Program.W.Level] + 1.0 * _Player.TotalAttackDamage)); }
                if (checkCDtoCalc) { if (Program.R.IsReady()) { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 250, 425, 600 }[Program.R.Level] + 1.0 * _Player.TotalMagicalDamage)); } } else { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 250, 425, 600 }[Program.R.Level] + 1.0 * _Player.TotalMagicalDamage)); }
            }
            else if (_Player.ChampionName == "Azir")
            {
                if (checkCDtoCalc) { if (Program.Q.IsReady()) { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 65, 85, 105, 125, 145 }[Program.Q.Level] + 0.5 * _Player.TotalMagicalDamage)); } } else { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 65, 85, 105, 125, 145 }[Program.Q.Level] + 0.5 * _Player.TotalMagicalDamage)); }
                if (checkCDtoCalc) { if (Program.W.IsReady()) { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(0.6 * _Player.TotalMagicalDamage)); } } else { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(0.6 * _Player.TotalMagicalDamage)); } // fucking horrible formula, not sure if even working
                if (checkCDtoCalc) { if (Program.E.IsReady()) { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 60, 90, 120, 150, 180 }[Program.E.Level] + 0.4 * _Player.TotalMagicalDamage)); } } else { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 60, 90, 120, 150, 180 }[Program.E.Level] + 0.4 * _Player.TotalMagicalDamage)); }
                if (checkCDtoCalc) { if (Program.R.IsReady()) { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 150, 225, 300 }[Program.R.Level] + 0.6 * _Player.TotalMagicalDamage)); } } else { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 150, 225, 300 }[Program.R.Level] + 0.6 * _Player.TotalMagicalDamage)); }
            }
            else if (_Player.ChampionName == "Bard")
            {
                if (checkCDtoCalc) { if (Program.Q.IsReady()) { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 80, 125, 170, 215, 260 }[Program.Q.Level] + 0.65 * _Player.TotalMagicalDamage)); } } else { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 80, 125, 170, 215, 260 }[Program.Q.Level] + 0.65 * _Player.TotalMagicalDamage)); }
            }
            else if (_Player.ChampionName == "Blitzcrank")
            {
                if (checkCDtoCalc) { if (Program.Q.IsReady()) { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 80, 135, 190, 245, 300 }[Program.Q.Level] + 1.0 * _Player.TotalMagicalDamage)); } } else { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 80, 135, 190, 245, 300 }[Program.Q.Level] + 1.0 * _Player.TotalMagicalDamage)); }
                if (checkCDtoCalc) { if (Program.E.IsReady()) { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Physical, (float)2.0 * _Player.TotalAttackDamage); } } else { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Physical, (float)2.0 * _Player.TotalAttackDamage); }
                if (checkCDtoCalc) { if (Program.R.IsReady()) { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 250, 375, 500 }[Program.R.Level] + 1.0 * _Player.TotalMagicalDamage)); } } else { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 250, 375, 500 }[Program.R.Level] + 1.0 * _Player.TotalMagicalDamage)); }
            }
            else if (_Player.ChampionName == "Brand")
            {
                if (checkCDtoCalc) { if (Program.Q.IsReady()) { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 80, 120, 160, 200, 240 }[Program.Q.Level] + 0.65 * _Player.TotalMagicalDamage)); } } else { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 80, 120, 160, 200, 240 }[Program.Q.Level] + 0.65 * _Player.TotalMagicalDamage)); }
                if (checkCDtoCalc) { if (Program.W.IsReady()) { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 75, 120, 165, 210, 255 }[Program.W.Level] + 0.6 * _Player.TotalMagicalDamage)); } } else { wdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 75, 120, 165, 210, 255 }[Program.W.Level] + 0.6 * _Player.TotalMagicalDamage)); } if (IsAblazed(targ)) { wdmg = wdmg * (float)1.25; }
                if (checkCDtoCalc) { if (Program.E.IsReady()) { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 70, 105, 140, 175, 210 }[Program.E.Level] + 0.55 * _Player.TotalMagicalDamage)); } } else { edmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 70, 105, 140, 175, 210 }[Program.E.Level] + 0.55 * _Player.TotalMagicalDamage)); }
                if (checkCDtoCalc) { if (Program.R.IsReady()) { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 150, 250, 350 }[Program.R.Level] + 0.5 * _Player.TotalMagicalDamage)); } } else { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 150, 250, 350 }[Program.R.Level] + 0.5 * _Player.TotalMagicalDamage)); }
            }
            else if (_Player.ChampionName == "Braum")
            {
                if (checkCDtoCalc) { if (Program.Q.IsReady()) { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 70, 115, 160, 205, 250 }[Program.Q.Level])); } } else { qdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 70, 115, 160, 205, 250 }[Program.Q.Level])); }
                if (checkCDtoCalc) { if (Program.R.IsReady()) { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 150, 250, 350 }[Program.R.Level] + 0.6 * _Player.TotalMagicalDamage)); } } else { rdmg = _Player.CalculateDamageOnUnit(targ, DamageType.Magical, (float)(new[] { 150, 250, 350 }[Program.R.Level] + 0.6 * _Player.TotalMagicalDamage)); }
            }

            if (summonerIgnite != null)
            {
                SpellSlot igSlot = extent.GetSpellSlotFromName(_Player, "summonerdot");
                ignite = new Spell.Targeted(igSlot, 600);
                if (checkCDtoCalc) { if (ignite.IsReady()) { igdmg = _Player.CalculateDamageOnUnit(targ, DamageType.True, getIgniteDamage()); } } else { igdmg = _Player.CalculateDamageOnUnit(targ, DamageType.True, getIgniteDamage()); }
            }

            if (qdmg + wdmg + rdmg + igdmg > targ.Health)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static int getIgniteDamage()
        {
            return 50 + 20 * _Player.Level;
        }

        public static bool IsAblazed(this Obj_AI_Base target)
        {
            return target.HasBuff("brandablaze");
        }

        private static bool UserHaveAatroxWDmgB
        {
            get { return _Player.HasBuff("AatroxWPower"); }
        }

        private static bool PlayerHaveAmumuWDmg
        {
            get { return _Player.HasBuff("AuraofDespair"); }
        }

        public static bool HasRendBuff(this Obj_AI_Base target)
        {
            return target.Buffs.Find(b => b.Caster.IsMe && b.IsValid() && b.DisplayName == "KalistaExpungeMarker") != null; // GetRendBuff
        }

        public static class Damages // Hellsing's Kalista Damage Class
        {

            public static readonly Damage.DamageSourceBoundle QDamage = new Damage.DamageSourceBoundle();

            private static readonly float[] RawRendDamage = { 20, 30, 40, 50, 60 };
            private static readonly float[] RawRendDamageMultiplier = { 0.6f, 0.6f, 0.6f, 0.6f, 0.6f };
            private static readonly float[] RawRendDamagePerSpear = { 10, 14, 19, 25, 32 };
            private static readonly float[] RawRendDamagePerSpearMultiplier = { 0.2f, 0.225f, 0.25f, 0.275f, 0.3f };

            static Damages()
            {
                QDamage.Add(new Damage.DamageSource(SpellSlot.Q, DamageType.Physical)
                {
                    Damages = new float[] { 10, 70, 130, 190, 250 }
                });
                QDamage.Add(new Damage.BonusDamageSource(SpellSlot.Q, DamageType.Physical)
                {
                    DamagePercentages = new float[] { 1, 1, 1, 1, 1 }
                });
            }

            public static float GetRendDamage(AIHeroClient target)
            {
                return GetRendDamage(target, -1);
            }

            public static float GetRendDamage(Obj_AI_Base target, int customStacks = -1)
            {
                return (Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, GetRawRendDamage(target, customStacks)) - 20) * 0.98f;
            }

            public static float GetRawRendDamage(Obj_AI_Base target, int customStacks = -1)
            {
                var buff = target.Buffs.Find(b => b.Caster.IsMe && b.IsValid() && b.DisplayName == "KalistaExpungeMarker");

                if (buff != null || customStacks > -1)
                {
                    return (RawRendDamage[Program.E.Level - 1] + RawRendDamageMultiplier[Program.E.Level - 1] * Player.Instance.TotalAttackDamage) + // Base damage
                           ((customStacks < 0 ? buff.Count : customStacks) - 1) * // Spear count
                           (RawRendDamagePerSpear[Program.E.Level - 1] + RawRendDamagePerSpearMultiplier[Program.E.Level - 1] * Player.Instance.TotalAttackDamage); // Damage per spear
                }

                return 0;
            }

            public static float GetTotalDamage(AIHeroClient target)
            {
                var damage = Player.Instance.GetAutoAttackDamage(target);

                if (Program.Q.IsReady())
                {
                    damage += QDamage.GetDamage(target);
                }

                if (Program.E.IsReady())
                {
                    damage += GetRendDamage(target);
                }

                return damage;
            }
        }

        public static List<T> MakeUnique<T>(this List<T> list) where T : Obj_AI_Base, new()
        {
            var uniqueList = new List<T>();

            foreach (var entry in list)
            {
                if (uniqueList.All(e => e.NetworkId != entry.NetworkId))
                {
                    uniqueList.Add(entry);
                }
            }

            list.Clear();
            list.AddRange(uniqueList);

            return list;
        }
    }
}
