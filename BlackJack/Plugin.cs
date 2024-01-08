using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using TerminalApi;
using static TerminalApi.TerminalApi;
using TerminalApi.Events;
using BlackJack.Patches;
using TerminalApi.Classes;
using static TerminalApi.Events.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    [BepInPlugin(modGUID,modName,modVersion)]
    [BepInDependency("atomic.terminalapi")]
    public class BlackJackBase : BaseUnityPlugin
    {
        public const string modGUID = "godzilla.blackjack";
        public const string modName = "BlackJack";
        public const string modVersion = "1.0.0";
        
        private readonly Harmony harmony = new Harmony(modGUID);

        public static BlackJackBase _instance;

        internal ManualLogSource mls;

        public BlackJackGameManager currentGame;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            mls.LogInfo("BlackJack Loaded");

            harmony.PatchAll(typeof(BlackJackBase));


            TerminalNode BlackjackNode = CreateTerminalNode("BlackJackNode");



            
            AddCommand("blackjack", new CommandInfo()
            {
                DisplayTextSupplier = () => 
                {
                    Logger.LogWarning("Starting a game of Blackjack");
                    currentGame = new BlackJackGameManager();
                    currentGame.StartGame(currentGame);
                    return currentGame.GameToString();
                },
                Category = "blackjack"
            });


            AddCommand("hit", new CommandInfo()
            {
                DisplayTextSupplier = () =>
                {
                    Logger.LogWarning("Hit");
                    return currentGame.Hit();
                },
                Category = "blackjack"
            });






        }

        private void OnTerminalExit(object sender, TerminalEventArgs e)
        {
            BlackJackBase._instance.currentGame = null;
        }


    }

}
