using System;
using System.Collections.Generic;
using System.Text;
using MetalSpace.Interfaces;
using MetalSpace.Managers;
using MetalSpace.GameScreens;

namespace MetalSpace.Events
{
    /// <summary>
    /// The <c>EventListener_ChangePlayerObjects</c> class represents the behaviour
    /// of the player when it's necessary to change the state of an object. 
    /// 
    /// Respond to: EventData_EquipObject, EventData_UnequipObject,
    ///             EventData_ThrowAway, EventData_ReloadGun
    /// </summary>
    class EventListener_ChangePlayerObjects : IEventListener
    {
        #region Constructor

        /// <summary>
        /// Constructor of the <c>EventListener_ChangePlayerObjects</c> class.
        /// </summary>
        public EventListener_ChangePlayerObjects()
        {
            
        }

        #endregion

        /// <summary>
        /// Get the name of the event listener.
        /// </summary>
        /// <returns>Name of the event listener.</returns>
        public string GetName() { return "Change_Player_Objects_Event_Listener"; }
        
        /// <summary>
        /// Handle the behaviour of the listener for the different types of events.
        /// </summary>
        /// <param name="producedEvent"><c>IEventData</c> that contains the produced event.</param>
        /// <returns>True if the event has been processed and false otherwise.</returns>
        public bool HandleEvent(IEventData producedEvent) 
        {
            // The player equip an object
            if (producedEvent is EventData_EquipObject)
            {
                ((EventData_EquipObject)producedEvent).InventoryScreen.Inventory.RemoveObject(
                    ((EventData_EquipObject)producedEvent).NewObject);

                ((EventData_EquipObject)producedEvent).InventoryScreen.Equipment.AddObject(
                    ((EventData_EquipObject)producedEvent).NewObject);

                ((EventData_EquipObject)producedEvent).Player.ChangeState(
                    ((EventData_EquipObject)producedEvent).NewObject, true);

                return true;
            }
            // The player unequip an object
            else if (producedEvent is EventData_UnequipObject)
            {
                ((EventData_UnequipObject)producedEvent).InventoryScreen.Equipment.RemoveObject(
                    ((EventData_UnequipObject)producedEvent).NewObject);

                ((EventData_UnequipObject)producedEvent).InventoryScreen.Inventory.AddObject(
                    ((EventData_UnequipObject)producedEvent).NewObject);

                ((EventData_UnequipObject)producedEvent).Player.ChangeState(
                    ((EventData_UnequipObject)producedEvent).NewObject, false);

                return true;
            }
            // The player throw away an object
            else if (producedEvent is EventData_ThrowAway)
            {
                ((EventData_ThrowAway)producedEvent).InventoryScreen.Equipment.RemoveObject(
                    ((EventData_ThrowAway)producedEvent).NewObject);

                ((EventData_ThrowAway)producedEvent).InventoryScreen.Inventory.RemoveObject(
                    ((EventData_ThrowAway)producedEvent).NewObject);

                ((EventData_ThrowAway)producedEvent).Player.RemoveObject(
                    ((EventData_ThrowAway)producedEvent).NewObject);

                return true;
            }
            // The player reload his gun
            else if (producedEvent is EventData_ReloadGun)
            {
                int maxAmmo = ((EventData_ReloadGun)producedEvent).Player.PlayerGun.MaxAmmo;
                int currentAmmo = ((EventData_ReloadGun)producedEvent).Player.PlayerGun.CurrentAmmo;
                int availableAmmo = ((Objects.Ammo)((EventData_ReloadGun)producedEvent).NewObject).Amount;

                if (availableAmmo > (maxAmmo - currentAmmo))
                {
                    ((Objects.Ammo)((EventData_ReloadGun)producedEvent).NewObject).Amount -= (maxAmmo - currentAmmo);
                    ((EventData_ReloadGun)producedEvent).Player.PlayerGun.CurrentAmmo = maxAmmo;
                }
                else
                {
                    ((EventData_ReloadGun)producedEvent).Player.PlayerGun.CurrentAmmo += availableAmmo;

                    if (ScreenManager.GetScreen("Inventory") != null)
                    {
                        ((InventoryScreen) ScreenManager.GetScreen("Inventory")).Inventory.RemoveObject(
                                ((EventData_ReloadGun)producedEvent).NewObject);
                    }
                    
                    ((EventData_ReloadGun)producedEvent).Player.RemoveObject(
                        ((EventData_ReloadGun)producedEvent).NewObject);
                }

                return true;
            }

            return false;
        }
    }
}
