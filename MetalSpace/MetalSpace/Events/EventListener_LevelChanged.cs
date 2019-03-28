using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetalSpace.Objects;
using MetalSpace.Managers;
using MetalSpace.GameScreens;
using MetalSpace.Interfaces;

namespace MetalSpace.Events
{
    /// <summary>
    /// The <c>EventListener_LevelChanged</c> class represents the behaviour
    /// when the player go to another level.
    /// 
    /// Respond to: EventListener_LevelChanged
    /// </summary>
    class EventListener_LevelChanged : IEventListener
    {
        #region Constructor

        /// <summary>
        /// Constructor of the <c>EventListener_LevelChanged</c> class.
        /// </summary>
        public EventListener_LevelChanged()
        {
            
        }

        #endregion

        /// <summary>
        /// Get the name of the event listener.
        /// </summary>
        /// <returns>Name of the event listener.</returns>
        public string GetName() { return "Level_Changed_Event_Listener"; }

        /// <summary>
        /// Handle the behaviour of the listener when the player go to another level.
        /// </summary>
        /// <param name="producedEvent"><c>IEventData</c> that contains the produced event.</param>
        /// <returns>True if the event has been processed and false otherwise.</returns>
        public bool HandleEvent(IEventData producedEvent)
        {
            if (!(producedEvent is EventData_LevelChanged))
                return false;

            //ScreenManager.RemoveScreen("ContinueGame");
            // El primero es el bueno
            /*ScreenManager.AddScreen("LoadingScreen", new ChangingGame(
                ((EventData_LevelChanged)producedEvent).MapName,
                ((EventData_LevelChanged)producedEvent).Player,
                ((EventData_LevelChanged)producedEvent).NewPosition));*/

            /*ScreenManager.AddScreen("ContinueGame", new MainGameScreen(
                "ContinueGame", ((EventData_LevelChanged)producedEvent).MapName, 
                FileHelper.readMapInformation(((EventData_LevelChanged)producedEvent).MapName),
                ((EventData_LevelChanged)producedEvent).Player, 
                ((EventData_LevelChanged)producedEvent).NewPosition));*/
            ScreenManager.AddScreen("LoadingScreen", new ChangingGame2(
                ((MainGameScreen) ScreenManager.GetScreen("ContinueGame")).MapName,
                LevelManager.GetLevel(((MainGameScreen)ScreenManager.GetScreen("ContinueGame")).MapName),
                ((MainGameScreen) ScreenManager.GetScreen("ContinueGame")).MainPlayer,
                ((EventData_LevelChanged)producedEvent).NewPosition,
                ((EventData_LevelChanged)producedEvent).MapName));
            ScreenManager.RemoveScreen("ContinueGame");
            return true;
        }
    }
}
