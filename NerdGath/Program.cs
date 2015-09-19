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

namespace NerdGath
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static AIHeroClient _Player { get { return ObjectManager.Player; } }
        public static Spell.Skillshot Q;
        public static Spell.Skillshot W;
        public static Spell.Active E;
        public static Spell.Targeted R;

        private static Slider manaC;
        public static int MinNumberManaC { get { return manaC.CurrentValue; } }

        private static Slider manaH;
        public static int MinNumberManaH { get { return manaH.CurrentValue; } }

        public static Menu GathMenu, ComboMenu, HarassMenu, FleeMenu, MiscMenu, FarmMenu, KSMenu;

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            TargetSelector2.init();
            Bootstrap.Init(null);

            Q = new Spell.Skillshot(SpellSlot.Q, 940, SkillShotType.Circular);
            W = new Spell.Skillshot(SpellSlot.W, 290, SkillShotType.Cone);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Targeted(SpellSlot.R, 175);

            GathMenu = MainMenu.AddMenu("NerdGath", "NerdGath");
            GathMenu.AddGroupLabel("NerdGath");
            GathMenu.AddSeparator();
            GathMenu.AddLabel("Nerd Series - Downloading More Ram");
            GathMenu.AddLabel("Berb @ EloBuddy");
            GathMenu.AddSeparator();
            GathMenu.AddLabel("TO-DO ::");

            ComboMenu = GathMenu.AddSubMenu("Combo", "Combo");
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.AddSeparator();
            ComboMenu.Add("useQCombo", new CheckBox("Use Q"));
            ComboMenu.Add("useWCombo", new CheckBox("Use W"));
            ComboMenu.Add("useRCombo", new CheckBox("Use R"));
            ComboMenu.Add("useRKill", new CheckBox("Ult only if killable (-Use R- MUST be enabled!)"));
            ComboMenu.Add("igniteKill", new CheckBox("Ignite if Killable"));
            ComboMenu.AddSeparator();
            manaC = ComboMenu.Add("manamanager", new Slider("Minimum mana to do combo (%)", 20, 1, 100));

            HarassMenu = GathMenu.AddSubMenu("Harass", "Harass");
            HarassMenu.AddGroupLabel("Harass Settings");
            HarassMenu.AddSeparator();
            HarassMenu.Add("useQHarass", new CheckBox("Use Q"));
            HarassMenu.Add("useWHarass", new CheckBox("Use W"));
            HarassMenu.AddSeparator();
            manaH = HarassMenu.Add("manamanager", new Slider("Minimum mana to harass (%)", 20, 1, 100));

            FleeMenu = GathMenu.AddSubMenu("Flee", "Flee");
            FleeMenu.AddGroupLabel("Flee Settings");
            FleeMenu.AddSeparator();
            FleeMenu.Add("useQFlee", new CheckBox("Use Q"));

            KSMenu = GathMenu.AddSubMenu("KSMenu", "KSMenu");
            KSMenu.AddGroupLabel("Kill Steal Settings");
            KSMenu.AddSeparator();
            KSMenu.Add("doKS", new CheckBox("Enable KS"));
            KSMenu.Add("qKS", new CheckBox("Use Q to KS"));
            KSMenu.Add("wKS", new CheckBox("Use W to KS"));
            KSMenu.Add("rKS", new CheckBox("Use R to KS"));
            KSMenu.Add("igniteKill", new CheckBox("Use Ignite to KS"));

            FarmMenu = GathMenu.AddSubMenu("Farm", "Farm");
            FarmMenu.AddGroupLabel("Farming Settings");
            FarmMenu.AddLabel("NOTE : The R/Ult usage on farming is only if the mob is killable");
            FarmMenu.AddSeparator();
            FarmMenu.Add("qLaneClear", new CheckBox("Use Q to Lane Clear"));
            FarmMenu.Add("wLaneClear", new CheckBox("Use W to Lane Clear"));
            FarmMenu.Add("rLaneClear", new CheckBox("Use R to Lane Clear"));
            FarmMenu.AddSeparator();
            FarmMenu.Add("qJG", new CheckBox("Use Q to Jungle Clear"));
            FarmMenu.Add("wJG", new CheckBox("Use W to Jungle Clear"));
            FarmMenu.Add("rJG", new CheckBox("Use R to Jungle Clear"));
            FarmMenu.AddSeparator();
            FarmMenu.Add("rLastHit", new CheckBox("Use R to Last Hit"));

            MiscMenu = GathMenu.AddSubMenu("Misc", "Misc");
            MiscMenu.AddGroupLabel("Misc. Settings");
            MiscMenu.AddSeparator();
            MiscMenu.Add("interrupt", new CheckBox("Use Q to Interrupt"));

            Game.OnTick += Game_OnTick;
            Interrupter.OnInterruptableSpell += StateHandler.Interrupter_OnInterruptableSpell;

            EloBuddy.Chat.Print("NerdGath : Thanks for using my script! Enjoy the game!");
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                StateHandler.Combo();
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                StateHandler.Harass();
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                StateHandler.Farm("Jungle");
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                StateHandler.Farm("Lane");
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                StateHandler.Farm("LastHit");
            }
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                StateHandler.Flee();
            }

            if (Program.FleeMenu["doKS"].Cast<CheckBox>().CurrentValue)
            {
                StateHandler.KS();
            }
        }
    }
}
