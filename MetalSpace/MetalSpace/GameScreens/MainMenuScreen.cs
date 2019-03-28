using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Managers;
using MetalSpace.GameScreens;
using MetalSpace.Events;
using MetalSpace.GameComponents;

namespace MetalSpace.GameScreens
{
    /// <summary>
    /// The <c>MainMenuScreen</c> class represents the menu that appears
    /// on the screen when the user execute the application.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        #region Constructor

        /// <summary>
        /// Constructor of the <c>MainMenuScreen</c> class.
        /// </summary>
        /// <param name="name">Name of the screen.</param>
        public MainMenuScreen(string name)
            : base(name, "Metal Space")
        {
            Input.Continuous = false;

            MenuEntry continueMenuEntry = new MenuEntry(StringHelper.DefaultInstance.Get("menu_continue"));
            MenuEntry newMenuEntry = new MenuEntry(StringHelper.DefaultInstance.Get("menu_new"));
            MenuEntry loadMenuEntry = new MenuEntry(StringHelper.DefaultInstance.Get("menu_load"));
            MenuEntry optionsMenuEntry = new MenuEntry(StringHelper.DefaultInstance.Get("menu_options"));
            MenuEntry exitMenuEntry = new MenuEntry(StringHelper.DefaultInstance.Get("menu_exit"));

            continueMenuEntry.Selected += ContinueMenuEntrySelected;
            newMenuEntry.Selected += NewMenuEntrySelected;
            loadMenuEntry.Selected += LoadMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += ExitMenuEntrySelected;

            MenuEntries.Add(continueMenuEntry);
            MenuEntries.Add(newMenuEntry);
            MenuEntries.Add(loadMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        #endregion

        #region ContinueMenuEntrySelected Method

        /// <summary>
        /// EventHandler that permits to continue with the last saved game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ContinueMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.RemoveScreen("Background");
            ScreenManager.RemoveScreen("MainMenu");
            ScreenManager.AddScreen("LoadingScreen", new LoadingGame("Map_1_5.txt", true));
            /*int index = 0;
            string[] files = FileHelper.GetFilesInDirectory();
            if (files.Length == 0)
            {
                EventManager.Trigger(new EventData_LoadSavedGame(null));
            }
            else
            {
                bool finish = false;
                for (int i = 0; i < files.Length && !finish; i++)
                {
                    if (files[i].Contains("GameSetting"))
                        continue;

                    string extension = files[i].Split(' ')[2].Split('.')[1];
                    if (extension != "png")
                    {
                        index = i;
                        finish = true;
                    }
                }

                for (int i = 1; i < files.Length; i++)
                {
                    if (files[i].Contains("GameSetting"))
                        continue;

                    string extension = files[i].Split(' ')[2].Split('.')[1];
                    if (extension != "png")
                    {
                        int date = Convert.ToInt32(files[i].Split(' ')[1]);
                        int hour = Convert.ToInt32(files[i].Split(' ')[2].Split('.')[0]);
                        if (date > Convert.ToInt32(files[index].Split(' ')[1]))
                            index = i;
                        else if (date == Convert.ToInt32(files[index].Split(' ')[1]) &&
                            hour > Convert.ToInt32(files[index].Split(' ')[2].Split('.')[0]))
                            index = i;
                    }
                }

                EventManager.Trigger(new EventData_LoadSavedGame(files[index]));
            }*/
        }

        #endregion

        #region NewMenuEntrySelected Method

        /// <summary>
        /// EventHandler that permits to start a new game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NewMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen("NewGame", new NewGameScreen("NewGame"));
        }

        #endregion

        #region LoadMenuEntrySelected Method

        /// <summary>
        /// EventHandler that permits to load a saved game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LoadMenuEntrySelected(object sender, EventArgs e)
        {
            string[] files = FileHelper.GetFilesInDirectory();
            if (files.Length != 0)
                ScreenManager.AddScreen("LoadGames", new LoadGameScreen("LoadGames"));
            else
            {
                MessageBoxScreen messageBox = new MessageBoxScreen("MessageBox",
                    StringHelper.DefaultInstance.Get("menu_load_empty"), true);
                ScreenManager.AddScreen("MessageBox", messageBox);
            }
        }

        #endregion

        #region OptionsMenuEntrySelected Method

        /// <summary>
        /// EventHandler that permits modify the options of the game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OptionsMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen("OptionsMenu", new OptionsMenu("OptionsMenu"));
        }

        #endregion

        #region ExitMenuEntrySelected Method

        /// <summary>
        /// EventHandler that permits show a message box to ask the user if he/she
        /// wants to close the game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ExitMenuEntrySelected(object sender, EventArgs e)
        {
            MessageBoxScreen messageBox = new MessageBoxScreen("MessageBox",
                StringHelper.DefaultInstance.Get("menu_exit_message"), false);
            messageBox.Accepted += ExitMessageBoxAccepted;
            ScreenManager.AddScreen("MessageBox", messageBox);
        }

        #endregion

        #region ExitMessageBoxAccepted Method

        /// <summary>
        /// EventHandler that permits close the game when the user accept the
        /// message box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ExitMessageBoxAccepted(object sender, EventArgs e)
        {
            EngineManager.Game.Exit();
        }

        #endregion
    }
}