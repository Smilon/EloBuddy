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

namespace Winderupper
{
    class Program
    {

        public static Menu WindMenu, SettingsM;
        public static bool alreadyPosted = false;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Bootstrap.Init(null);

            WindMenu = MainMenu.AddMenu("Winderupper", "Winderupper");
            WindMenu.AddGroupLabel("Winderupper");
            WindMenu.AddSeparator();
            WindMenu.AddLabel("Nerd Utility Series - Downloading More Ram");
            WindMenu.AddLabel("Berb @ EloBuddy");
            WindMenu.AddSeparator();
            WindMenu.AddLabel("What is this?");
            WindMenu.AddLabel("This calculates the optimal # of your windup time.");

            SettingsM = WindMenu.AddSubMenu("Settings", "Settings");
            SettingsM.AddGroupLabel("Settings");
            SettingsM.AddSeparator();
            SettingsM.Add("calcWindup", new KeyBind("Prints to chat your optimal windup", false, KeyBind.BindTypes.HoldActive, 'a'));

            Game.OnTick += Game_OnTick;

            EloBuddy.Chat.Print("Winderupper : Thanks for using my script! Enjoy the game!");
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Program.SettingsM["calcWindup"].Cast<KeyBind>().CurrentValue)
            {
                if (!alreadyPosted) {
                    var windup = Game.Ping * 1.5;
                    EloBuddy.Chat.Print("Your optimal windup time is : " + windup);
                }
                alreadyPosted = true;
                EloBuddy.SDK.Core.DelayAction(() => { alreadyPosted = false; }, 1200);
            }
        }
    }
}
