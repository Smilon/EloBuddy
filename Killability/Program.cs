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
    class Program
    {

        public static Menu KillMenu, SettingsM;
        public static bool alreadyPosted = false;
        public static AIHeroClient _Player { get { return ObjectManager.Player; } }

        public static Spell.Active Q;
        public static Spell.Active W;
        public static Spell.Active E;
        public static Spell.Active R;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Bootstrap.Init(null);

            KillMenu = MainMenu.AddMenu("Killability", "Killability");
            KillMenu.AddGroupLabel("Killability");
            KillMenu.AddSeparator();
            KillMenu.AddLabel("Nerd Utility Series - Downloading More Ram");
            KillMenu.AddLabel("Berb @ EloBuddy");
            KillMenu.AddSeparator();
            KillMenu.AddLabel("What is this?");
            KillMenu.AddLabel("It shows you what skills are needed to kill the enemy.");

            SettingsM = KillMenu.AddSubMenu("Settings", "Settings");
            SettingsM.AddGroupLabel("Settings");
            SettingsM.AddSeparator();
            SettingsM.Add("enabled", new KeyBind("Enable?", false, KeyBind.BindTypes.PressToggle, 'a'));

            Game.OnTick += Game_OnTick;

            EloBuddy.Chat.Print("Killability : Thanks for using my script! Enjoy the game!");
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Program.SettingsM["enabled"].Cast<KeyBind>().CurrentValue)
            {

            }
        }
    }
}
