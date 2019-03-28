using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MetalSpace.Models;
using MetalSpace.Objects;
using MetalSpace.Managers;
using MetalSpace.Settings;
using MetalSpace.Interfaces;
using MetalSpace.GameScreens;
using MetalSpace.ArtificialIntelligence;

namespace MetalSpace.Events
{
    /// <summary>
    /// The <c>EventListener_CharactersAttack</c> class represents the behaviour
    /// of the characters when one hit the other.
    /// 
    /// Respond to: EventData_CharactersAttack
    /// </summary>
    class EventListener_CharactersAttack : IEventListener
    {
        #region Constructor

        /// <summary>
        /// Constructor of the <c>EventListener_CharactersAttack</c> class.
        /// </summary>
        public EventListener_CharactersAttack()
        {
            
        }

        #endregion

        /// <summary>
        /// Get the name of the event listener.
        /// </summary>
        /// <returns>Name of the event listener.</returns>
        public string GetName() { return "Characters_Attack_Event_Listener"; }

        /// <summary>
        /// Handle the behaviour of the listener when one character attacks the other.
        /// </summary>
        /// <param name="producedEvent"><c>IEventData</c> that contains the produced event.</param>
        /// <returns>True if the event has been processed and false otherwise.</returns>
        public bool HandleEvent(IEventData producedEvent) 
        {
            if (!(producedEvent is EventData_CharactersAttack))
                return false;

            // If the enemy is unconscious, waking up, attacked or dying, it is not
            // necessary to process the event.
            if(((EventData_CharactersAttack)producedEvent).Attacked is Enemy &&
               ( ((Enemy) ((EventData_CharactersAttack)producedEvent).Attacked).Context.CurrentState == Unconscious.Instance ||
                 ((Enemy) ((EventData_CharactersAttack)producedEvent).Attacked).Context.CurrentState == WakeUp.Instance ||
                 ((Enemy) ((EventData_CharactersAttack)producedEvent).Attacked).Context.CurrentState == Attacked.Instance ||
                 ((Enemy)((EventData_CharactersAttack)producedEvent).Attacked).Context.CurrentState == Dying.Instance ))
                 return false;

            // The attacked unit is going to dead
            if (((EventData_CharactersAttack)producedEvent).Attacked.Life <= 0)
            {
                if (((EventData_CharactersAttack)producedEvent).Attacked is Enemy)
                {
                    SoundManager.GetSound("EnemyDead").Volume = GameSettings.DefaultInstance.SoundVolume;
                    SoundManager.GetSound("EnemyDead").Play(true, false);

                    // Reduce the life of the enemy
                    ((EventData_CharactersAttack)producedEvent).Attacked.Life -= 
                        ((Player) ((EventData_CharactersAttack)producedEvent).Attacker).PlayerGun.Attack;
                    ((AnimatedModel)((EventData_CharactersAttack)producedEvent).Attacked.DModel).Damaged = false;

                    // Change the behaviour of the enemy depending of the shot type
                    if(((Player)((EventData_CharactersAttack)producedEvent).Attacker).PlayerGun.GunType == Gun.ShotType.Normal)
                        ((Enemy)((EventData_CharactersAttack)producedEvent).Attacked).Context.ChangeState(Unconscious.Instance);
                    else
                        ((Enemy)((EventData_CharactersAttack)producedEvent).Attacked).Context.ChangeState(Dying.Instance);

                    ((Player)((EventData_CharactersAttack)producedEvent).Attacker).TotalPoints += 100;
                }
                else if (((EventData_CharactersAttack)producedEvent).Attacked is Player)
                {
                    SoundManager.GetSound("PlayerDead").Volume = GameSettings.DefaultInstance.SoundVolume;
                    SoundManager.GetSound("PlayerDead").Play(true, false);

                    // The player is dead, so it's necessary to show the dead screen
                    ScreenManager.AddScreen("DeadScreen", new DeadScreen(
                        (MainGameScreen)ScreenManager.GetScreen("ContinueGame"), "DeadScreen"));
                }
            }
            // The attacked unit is not going to dead
            else
            {
                if (((EventData_CharactersAttack)producedEvent).Attacked is Enemy)
                {
                    SoundManager.GetSound("EnemyAttacked").Volume = GameSettings.DefaultInstance.SoundVolume;
                    SoundManager.GetSound("EnemyAttacked").Play(true, false);

                    // Reduce the life of the enemy
                    ((EventData_CharactersAttack)producedEvent).Attacked.Life -=
                        ((Player)((EventData_CharactersAttack)producedEvent).Attacker).PlayerGun.Attack;
                    ((AnimatedModel)((EventData_CharactersAttack)producedEvent).Attacked.DModel).Damaged = true;

                    // Change its behaviour to attacked
                    ((Enemy)((EventData_CharactersAttack)producedEvent).Attacked).Context.ChangeState(Attacked.Instance);
                }
                else if (((EventData_CharactersAttack)producedEvent).Attacked is Player)
                {
                    SoundManager.GetSound("Hurt").Volume = GameSettings.DefaultInstance.SoundVolume;
                    SoundManager.GetSound("Hurt").Play(true, false);

                    // Reduce the life of the enemy
                    ((EventData_CharactersAttack)producedEvent).Attacked.Life -=
                        ((Enemy)((EventData_CharactersAttack)producedEvent).Attacker).Attack;
                    ((Player)((EventData_CharactersAttack)producedEvent).Attacked).LifeBar.CurrentLife =
                        ((EventData_CharactersAttack)producedEvent).Attacked.Life;
                    
                    ((AnimatedModel)((EventData_CharactersAttack)producedEvent).Attacked.DModel).Damaged = true;
                }
            }
            
            return false;
        }
    }
}
