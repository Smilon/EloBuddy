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

namespace NerdLeona
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static AIHeroClient _Player { get { return ObjectManager.Player; } }
        public static Spell.Active Q;
        public static Spell.Active W;
        public static Spell.Skillshot E;
        public static Spell.Skillshot R;

        private static Slider _numR;
        public static int MinNumberR { get { return _numR.CurrentValue; } }

        private static Slider manaC;
        public static int MinNumberManaC { get { return manaC.CurrentValue; } }

        private static Slider manaH;
        public static int MinNumberManaH { get { return manaH.CurrentValue; } }

        public static Menu LeoMenu, ComboMenu, HarassMenu, FleeMenu, MiscMenu;

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            TargetSelector2.init();
            Bootstrap.Init(null);

            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 875, SkillShotType.Linear);
            R = new Spell.Skillshot(SpellSlot.R, 1200, SkillShotType.Circular);

            LeoMenu = MainMenu.AddMenu("NerdLeona", "NerdLeona");
            LeoMenu.AddGroupLabel("NerdLeona");
            LeoMenu.AddSeparator();
            LeoMenu.AddLabel("Nerd Series - Downloading More Ram");
            LeoMenu.AddLabel("Berb @ EloBuddy");
            LeoMenu.AddSeparator();
            LeoMenu.AddLabel("TO-DO ::");

            ComboMenu = LeoMenu.AddSubMenu("Combo", "Combo");
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.AddSeparator();
            ComboMenu.Add("useQCombo", new CheckBox("Use Q"));
            ComboMenu.Add("useWCombo", new CheckBox("Use W"));
            ComboMenu.Add("useECombo", new CheckBox("Use E"));
            ComboMenu.Add("useRCombo", new CheckBox("Use R"));
            ComboMenu.Add("igniteAlways", new CheckBox("Always use ignite"));
            ComboMenu.Add("igniteKill", new CheckBox("Ignite if Killable"));
            ComboMenu.AddSeparator();
            _numR = ComboMenu.Add("useRinEnemy", new Slider("Use R if enemy in range", 2, 1, 5));
            ComboMenu.AddSeparator();
            manaC = ComboMenu.Add("manamanager", new Slider("Minimum mana to do combo (%)", 20, 1, 100));

            HarassMenu = LeoMenu.AddSubMenu("Harass", "Harass");
            HarassMenu.AddGroupLabel("Harass Settings");
            HarassMenu.AddSeparator();
            HarassMenu.Add("useQHarass", new CheckBox("Use Q"));
            HarassMenu.Add("useEHarass", new CheckBox("Use E"));
            HarassMenu.AddSeparator();
            manaH = HarassMenu.Add("manamanager", new Slider("Minimum mana to harass (%)", 20, 1, 100));

            MiscMenu = LeoMenu.AddSubMenu("Misc", "Misc");
            MiscMenu.AddGroupLabel("Misc. Settings");
            MiscMenu.AddSeparator();
            MiscMenu.Add("interrupt", new CheckBox("Use Spells to Interrupt"));

            Game.OnTick += Game_OnTick;
            Interrupter.OnInterruptableSpell += StateHandler.Interrupter_OnInterruptableSpell;

            EloBuddy.Chat.Print("NerdLeona : Thanks for using my script! Enjoy the game!");
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
        }
    }
}
