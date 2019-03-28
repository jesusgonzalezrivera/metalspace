using System;
using System.Collections.Generic;
using System.Text;
using MetalSpace.Objects;
using MetalSpace.Managers;
using MetalSpace.Interfaces;
using MetalSpace.GameScreens;

namespace MetalSpace.Events
{
    /// <summary>
    /// The <c>EventListener_PickedObject</c> class represents the behaviour
    /// when the player pick up an object.
    /// 
    /// Respond to: EventData_PickedObject
    /// </summary>
    class EventListener_PickedObject : IEventListener
    {
        #region Constructor

        /// <summary>
        /// Constructor of the <c>EventListener_PickedObject</c> class.
        /// </summary>
        public EventListener_PickedObject()
        {
            
        }

        #endregion

        /// <summary>
        /// Get the name of the event listener.
        /// </summary>
        /// <returns>Name of the event listener.</returns>
        public string GetName() { return "Picked_Object_Event_Listener"; }

        /// <summary>
        /// Handle the behaviour of the listener when the player picks up an object.
        /// </summary>
        /// <param name="producedEvent"><c>IEventData</c> that contains the produced event.</param>
        /// <returns>True if the event has been processed and false otherwise.</returns>
        public bool HandleEvent(IEventData producedEvent) 
        {
            if (producedEvent is EventData_PickedObject)
            {
                if (((EventData_PickedObject)producedEvent).MObject.DModel.Model.FileName.Contains("Armor"))
                {
                    return ((Player)((EventData_PickedObject)producedEvent).Player).AddObject(new
                        Objects.Armature("Armature1", "ArmorObjectTexture", 0, false, 10, 10, Armature.ArmatureType.Body));
                }
                else if (((EventData_PickedObject)producedEvent).MObject.DModel.Model.FileName.Contains("Helmet"))
                {
                    return ((Player)((EventData_PickedObject)producedEvent).Player).AddObject(new
                        Objects.Armature("Helmet", "HelmetObjectTexture", 0, false, 10, 10, Armature.ArmatureType.Helmet));
                }
                else if (((EventData_PickedObject)producedEvent).MObject.DModel.Model.FileName.Contains("Boots"))
                {
                    return ((Player)((EventData_PickedObject)producedEvent).Player).AddObject(new
                        Objects.Armature("Boots1", "BootsObjectTexture", 0, false, 10, 10, Armature.ArmatureType.Foot));
                }
                else if (((EventData_PickedObject)producedEvent).MObject.DModel.Model.FileName.Contains("Gun"))
                {
                    return ((Player)((EventData_PickedObject)producedEvent).Player).AddObject(new
                        Weapon("GunNormal", "GunNormalObjectTexture", 0, false, Gun.ShotType.Normal, 10, 25));
                }
                else if (((EventData_PickedObject)producedEvent).MObject.DModel.Model.FileName.Contains("AmmoLaser"))
                {
                    return ((Player)((EventData_PickedObject)producedEvent).Player).AddObject(new
                        Ammo("LaserShot", "LaserShotObjectTexture", 4, false, 25, Gun.ShotType.Laser));
                }
                else if (((EventData_PickedObject)producedEvent).MObject.DModel.Model.FileName.Contains("AmmoNormal"))
                {
                    return ((Player)((EventData_PickedObject)producedEvent).Player).AddObject(new
                        Ammo("NormalShot", "NormalShotObjectTexture", 4, false, 25, Gun.ShotType.Normal));
                }
                else if (((EventData_PickedObject)producedEvent).MObject.DModel.Model.FileName.Contains("CardKey"))
                {
                    return ((Player)((EventData_PickedObject)producedEvent).Player).AddObject(new
                        Objects.Object("CardKey", "CardKey", 4, false));
                }

                ((MainGameScreen)ScreenManager.GetScreen("ContinueGame")).ThrownObjectsManager.RemoveObject(
                    ((EventData_PickedObject)producedEvent).MObject);

                return false;
            }
            
            return false;
        }
    }
}
