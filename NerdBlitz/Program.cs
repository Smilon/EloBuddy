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

namespace NerdBlitz
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static AIHeroClient _Player { get { return ObjectManager.Player; } }
        public static Spell.Skillshot Q;
        public static Spell.Active W;
        public static Spell.Active E;
        public static Spell.Active R;

        private static Slider _numR;
        public static int MinNumberR { get { return _numR.CurrentValue; } }

        private static Slider manaC;
        public static int MinNumberManaC { get { return manaC.CurrentValue; } }

        private static Slider manaH;
        public static int MinNumberManaH { get { return manaH.CurrentValue; } }

        private static Slider manaCL;
        public static int MinNumberManaCL { get { return manaCL.CurrentValue; } }

        private static Slider lowQ;
        public static int MinHQNoQ { get { return lowQ.CurrentValue; } }

        private static Slider rRange;
        public static int getRRange { get { return rRange.CurrentValue; } }

        private static Slider minMin;
        public static int getMinMin { get { return minMin.CurrentValue; } }

        public static Menu BlitzMenu, ComboMenu, HarassMenu, FleeMenu, MiscMenu, clearMenu;

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            TargetSelector2.init();
            Bootstrap.Init(null);

            BlitzMenu = MainMenu.AddMenu("NerdBlitz", "nerdblitz");
            BlitzMenu.AddGroupLabel("NerdBlitz");
            BlitzMenu.AddSeparator();
            BlitzMenu.AddLabel("Nerd Series - Downloading More Ram");
            BlitzMenu.AddLabel("Berb @ EloBuddy");
            BlitzMenu.AddSeparator();
            BlitzMenu.AddLabel("TO-DO ::");
            BlitzMenu.AddLabel("- [Give me suggestions!]");

            ComboMenu = BlitzMenu.AddSubMenu("Combo", "Combo");
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

            HarassMenu = BlitzMenu.AddSubMenu("Harass", "Harass");
            HarassMenu.AddGroupLabel("Harass Settings");
            HarassMenu.AddSeparator();
            HarassMenu.Add("useQHarass", new CheckBox("Use Q"));
            HarassMenu.Add("useEHarass", new CheckBox("Use E"));
            HarassMenu.AddSeparator();
            manaH = HarassMenu.Add("manamanager", new Slider("Minimum mana to harass (%)", 20, 1, 100));

            clearMenu = BlitzMenu.AddSubMenu("Clear", "Clear");
            clearMenu.AddGroupLabel("Lane Clear Settings");
            clearMenu.AddSeparator();
            clearMenu.Add("useRClear", new CheckBox("Use R"));
            clearMenu.AddSeparator();
            minMin = clearMenu.Add("minmin", new Slider("Minimum minions to R", 6, 1, 20));
            clearMenu.AddSeparator();
            manaCL = clearMenu.Add("manamanager", new Slider("Minimum mana to clear (%)", 20, 1, 100));

            FleeMenu = BlitzMenu.AddSubMenu("Flee", "Flee");
            FleeMenu.AddGroupLabel("Flee Settings");
            FleeMenu.AddSeparator();
            FleeMenu.Add("useWFlee", new CheckBox("Use W"));

            MiscMenu = BlitzMenu.AddSubMenu("Misc", "Misc");
            MiscMenu.AddGroupLabel("Misc. Settings");
            MiscMenu.AddSeparator();
            MiscMenu.Add("interrupt", new CheckBox("Use Spells to Interrupt"));
            MiscMenu.Add("immobile", new CheckBox("Auto Q on immobile"));
            MiscMenu.AddSeparator();
            lowQ = MiscMenu.Add("hpcheck", new Slider("Don't pull if below HP (%)", 30, 1, 100));
            rRange = MiscMenu.Add("rrange", new Slider("R cast range (if changed, please reload addon)", 590, 125, 600));
            MiscMenu.Add("KSR", new CheckBox("KS w/ R"));
            MiscMenu.AddSeparator();
            foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(enemy => enemy.Team != _Player.Team))
            {
                MiscMenu.Add("grab" + enemy.ChampionName, new CheckBox("Grab : " + enemy.ChampionName + "?"));
            }
            MiscMenu.AddSeparator();
            MiscMenu.Add("doFlashQ", new KeyBind("Do Flash Q", false, KeyBind.BindTypes.HoldActive, 't'));

            Game.OnTick += Game_OnTick;
            Interrupter.OnInterruptableSpell += StateHandler.Interrupter_OnInterruptableSpell;

            EloBuddy.Chat.Print("NerdBlitz : Thanks for using my script! Enjoy the game!");

            Q = new Spell.Skillshot(SpellSlot.Q, 910, SkillShotType.Linear); // shortened range for better grabs instead of 100% range grabs
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E, 125);
            R = new Spell.Active(SpellSlot.R, (uint)getRRange);
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Program.MiscMenu["KSR"].Cast<CheckBox>().CurrentValue)
            {
                StateHandler.KSR();
            }
            if (Program.MiscMenu["immobile"].Cast<CheckBox>().CurrentValue)
            {
                StateHandler.immobileQ();
            }
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
            else if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                StateHandler.Clear();
            }
            if (Program.MiscMenu["doFlashQ"].Cast<KeyBind>().CurrentValue)
            {
                StateHandler.FlashQCombo();
            }
        }
    }
}
