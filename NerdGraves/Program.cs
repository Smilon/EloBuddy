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

namespace NerdGraves
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
        public static Spell.Skillshot E;
        public static Spell.Skillshot R;

        public static Menu GravesMenu, ComboMenu, HarassMenu, FleeMenu, MiscMenu;

        private static Slider manaH;
        public static int MinManaH { get { return manaH.CurrentValue; } }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            TargetSelector2.init();
            Bootstrap.Init(null);

            Q = new Spell.Skillshot(SpellSlot.Q, 700, SkillShotType.Linear);
            W = new Spell.Skillshot(SpellSlot.W, 950, SkillShotType.Circular);
            E = new Spell.Skillshot(SpellSlot.E, 32767, SkillShotType.Linear);
            R = new Spell.Skillshot(SpellSlot.R, 1000, SkillShotType.Linear);

            GravesMenu = MainMenu.AddMenu("NerdGraves", "NerdGraves");
            GravesMenu.AddGroupLabel("NerdGraves");
            GravesMenu.AddSeparator();
            GravesMenu.AddLabel("Nerd Series - Downloading More Ram");
            GravesMenu.AddLabel("Berb @ EloBuddy");
            GravesMenu.AddSeparator();
            GravesMenu.AddLabel("TO-DO ::");

            ComboMenu = GravesMenu.AddSubMenu("Combo", "Combo");
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.AddSeparator();
            ComboMenu.Add("useQCombo", new CheckBox("Use Q"));
            ComboMenu.Add("useWCombo", new CheckBox("Use W"));
            ComboMenu.Add("useECombo", new CheckBox("Use E (to mouse position)"));
            ComboMenu.AddSeparator();
            ComboMenu.Add("useRKill", new CheckBox("Use R If Target is Killable"));
            ComboMenu.Add("useRCKill", new CheckBox("Use R if Target is Killable by Combo Dmg"));
            ComboMenu.AddSeparator();
            ComboMenu.Add("autoR", new Slider("AutoR if Min Targets is in R range", 2, 1, 5));

            HarassMenu = GravesMenu.AddSubMenu("Harass", "Harass");
            HarassMenu.AddGroupLabel("Harass Settings");
            HarassMenu.Add("useQHarass", new CheckBox("Use Q"));
            HarassMenu.Add("useWHarass", new CheckBox("Use W"));
            HarassMenu.AddSeparator();
            manaH = HarassMenu.Add("manaHarass", new Slider("Minimum Mana to harass (%)", 60, 1, 100));

            FleeMenu = GravesMenu.AddSubMenu("Flee", "Flee");
            FleeMenu.AddGroupLabel("Flee Settings");
            FleeMenu.Add("useE", new CheckBox("Use E (to mouse position)"));

            Game.OnTick += Game_OnTick;

            EloBuddy.Chat.Print("NerdGraves : Thanks for using my script! Enjoy the game!");
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
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                StateHandler.Flee();
            }
        }
    }
}
