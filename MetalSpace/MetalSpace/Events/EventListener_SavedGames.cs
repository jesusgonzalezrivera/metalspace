using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Events;
using MetalSpace.Objects;
using MetalSpace.Managers;
using MetalSpace.Interfaces;
using MetalSpace.GameScreens;

namespace MetalSpace.Events
{
    /// <summary>
    /// The <c>EventListener_SavedGames</c> class represents the behaviour
    /// when the user load a saved game or saved the current game.
    /// 
    /// Respond to: EventData_SaveGame, EventData_LoadSavedGame
    /// </summary>
    class EventListener_SavedGames : IEventListener
    {
        #region Constructor

        /// <summary>
        /// Constructor of the <c>EventListener_SavedGames</c> class.
        /// </summary>
        public EventListener_SavedGames()
        {
            
        }

        #endregion

        /// <summary>
        /// Get the name of the event listener.
        /// </summary>
        /// <returns>Name of the event listener.</returns>
        public string GetName() { return "Saved_Games_Event_Listener"; }

        /// <summary>
        /// Handle the behaviour of the listener when the user load a saved game or
        /// save the current game.
        /// </summary>
        /// <param name="producedEvent"><c>IEventData</c> that contains the produced event.</param>
        /// <returns>True if the event has been processed and false otherwise.</returns>
        public bool HandleEvent(IEventData producedEvent) 
        {
            if (producedEvent is EventData_SaveGame)
            {
                SavedGame savedGame = new SavedGame();

                savedGame.Date = DateTime.Now.ToString();
                savedGame.MapName = ((EventData_SaveGame) producedEvent).MapName;
                savedGame.Percent = 0.5f;
                savedGame.CapturedImage = null;
                savedGame.PlayerName = ((EventData_SaveGame) producedEvent).Player.DModel.Model.FileName;
                savedGame.PlayerPosition = ((EventData_SaveGame) producedEvent).Player.Position;
                savedGame.PlayerRotation = ((EventData_SaveGame) producedEvent).Player.Rotation;
                savedGame.PlayerScale = ((EventData_SaveGame) producedEvent).Player.Scale;
                savedGame.PlayerMaxSpeed = ((EventData_SaveGame) producedEvent).Player.MaxSpeed;
                savedGame.PlayerCurrentLife = ((EventData_SaveGame) producedEvent).Player.Life;
                savedGame.PlayerMaxLife = ((EventData_SaveGame) producedEvent).Player.MaxLife;
                savedGame.Debolio = ((Player)((EventData_SaveGame) producedEvent).Player).Debolio;
                savedGame.Aerogel = ((Player)((EventData_SaveGame) producedEvent).Player).Aerogel;
                savedGame.Fulereno = ((Player)((EventData_SaveGame) producedEvent).Player).Fulereno;
                savedGame.TotalPoints = ((Player)((EventData_SaveGame) producedEvent).Player).TotalPoints;

                savedGame.PlayerNumberOfObjects = ((Player)((EventData_SaveGame) producedEvent).Player).Objects.Count;
                savedGame.PlayerObjectType = new List<string>();
                savedGame.PlayerObjectName = new List<string>();
                savedGame.PlayerObjectTextureName = new List<string>();
                savedGame.PlayerObjectPosition = new List<int>();
                savedGame.PlayerObjectEquipped = new List<bool>();
                savedGame.PlayerObjectWeaponCurrentAmmo = new List<int>();
                savedGame.PlayerObjectWeaponTotalAmmo = new List<int>();
                savedGame.PlayerObjectWeaponPower = new List<int>();
                savedGame.PlayerObjectWeaponType = new List<Objects.Gun.ShotType>();
                savedGame.PlayerObjectArmatureSkill = new List<int>();
                savedGame.PlayerObjectArmatureDefense = new List<int>();
                savedGame.PlayerObjectArmatureType = new List<Armature.ArmatureType>();
                savedGame.PlayerObjectAmmoAmount = new List<int>();
                savedGame.PlayerObjectAmmoType = new List<Objects.Gun.ShotType>();
                foreach (Objects.Object playerObject in ((Player)((EventData_SaveGame)producedEvent).Player).Objects)
                {
                    savedGame.PlayerObjectName.Add(playerObject.Name);
                    savedGame.PlayerObjectTextureName.Add(playerObject.TextureName);
                    savedGame.PlayerObjectPosition.Add(playerObject.Position);
                    savedGame.PlayerObjectEquipped.Add(playerObject.IsEquipped);
                    if (playerObject is Armature)
                    {
                        savedGame.PlayerObjectType.Add("Armature");
                        savedGame.PlayerObjectArmatureSkill.Add(((Armature)playerObject).Skill);
                        savedGame.PlayerObjectArmatureDefense.Add(((Armature)playerObject).Defense);
                        savedGame.PlayerObjectArmatureType.Add(((Armature)playerObject).Type);
                    }
                    else if (playerObject is Weapon)
                    {
                        savedGame.PlayerObjectType.Add("Weapon");
                        savedGame.PlayerObjectWeaponCurrentAmmo.Add(((Weapon)playerObject).CurrentAmmo);
                        savedGame.PlayerObjectWeaponTotalAmmo.Add(((Weapon)playerObject).TotalAmmo);
                        savedGame.PlayerObjectWeaponPower.Add(((Weapon)playerObject).Power);
                        savedGame.PlayerObjectWeaponType.Add(((Weapon)playerObject).Type);
                    }
                    else if (playerObject is Ammo)
                    {
                        savedGame.PlayerObjectType.Add("Ammo");
                        savedGame.PlayerObjectAmmoAmount.Add(((Ammo)playerObject).Amount);
                        savedGame.PlayerObjectAmmoType.Add(((Ammo)playerObject).Type);
                    }
                    else
                    {
                        savedGame.PlayerObjectType.Add("Object");
                    }
                }

                savedGame.NumberOfEnemies = ((EventData_SaveGame)producedEvent).Enemies.Length;
                savedGame.EnemyName = new List<string>();
                savedGame.EnemyPosition = new List<Vector3>();
                savedGame.EnemyRotation = new List<Vector3>();
                savedGame.EnemyScale = new List<Vector3>();
                savedGame.EnemyMaxSpeed = new List<Vector2>();
                savedGame.EnemyLife = new List<int>();
                savedGame.EnemyMaxLife = new List<int>();
                savedGame.EnemyAttack = new List<int>();
                foreach (Objects.Enemy enemy in ((EventData_SaveGame)producedEvent).Enemies)
                {
                    savedGame.EnemyName.Add(enemy.DModel.Model.FileName);
                    savedGame.EnemyPosition.Add(enemy.Position);
                    savedGame.EnemyRotation.Add(enemy.Rotation);
                    savedGame.EnemyScale.Add(enemy.Scale);
                    savedGame.EnemyMaxSpeed.Add(enemy.MaxSpeed);
                    savedGame.EnemyLife.Add(enemy.Life);
                    savedGame.EnemyMaxLife.Add(enemy.MaxLife);
                    savedGame.EnemyAttack.Add(enemy.Attack);
                }

                DateTime dateTime = DateTime.Now;
                FileHelper.WriteSavedGame("MetalSpace " + dateTime.Year +
                    (dateTime.Month.ToString().Length == 1 ? "0" + dateTime.Month.ToString() : dateTime.Month.ToString()) +
                    (dateTime.Day.ToString().Length == 1 ? "0" + dateTime.Day.ToString() : dateTime.Day.ToString()) + " " +
                    (dateTime.Hour.ToString().Length == 1 ? "0" + dateTime.Hour.ToString() : dateTime.Hour.ToString()) +
                    (dateTime.Minute.ToString().Length == 1 ? "0" + dateTime.Minute.ToString() : dateTime.Minute.ToString()) +
                    (dateTime.Second.ToString().Length == 1 ? "0" + dateTime.Second.ToString() : dateTime.Second.ToString()) +
                    ".xml", savedGame);

                PresentationParameters parametros = EngineManager.GameGraphicsDevice.PresentationParameters;

                RenderTarget2D datosTextura = new RenderTarget2D(EngineManager.GameGraphicsDevice,
                    256, 256, true, SurfaceFormat.Bgr565, DepthFormat.Depth24);

                EngineManager.GameGraphicsDevice.SetRenderTarget(datosTextura);
                ScreenManager.GetScreen("ContinueGame").Draw(new GameTime());
                EngineManager.GameGraphicsDevice.SetRenderTarget(null);

                datosTextura.SaveAsPng(FileHelper.SaveGameContentFile("MetalSpace " + dateTime.Year + 
                    (dateTime.Month.ToString().Length == 1 ? "0" + dateTime.Month.ToString() : dateTime.Month.ToString()) + 
                    (dateTime.Day.ToString().Length == 1 ? "0" + dateTime.Day.ToString() : dateTime.Day.ToString()) + " " + 
                    (dateTime.Hour.ToString().Length == 1 ? "0" + dateTime.Hour.ToString() : dateTime.Hour.ToString()) + 
                    (dateTime.Minute.ToString().Length == 1 ? "0" + dateTime.Minute.ToString() : dateTime.Minute.ToString()) + 
                    (dateTime.Second.ToString().Length == 1 ? "0" + dateTime.Second.ToString() : dateTime.Second.ToString()) + 
                    ".png"), 256, 256);

                return true;
            }
            else if (producedEvent is EventData_LoadSavedGame)
            {
                if (ScreenManager.GetScreen("LoadGames") != null)
                {
                    ScreenManager.RemoveScreen("Background");
                    ScreenManager.RemoveScreen("MainMenu");
                    ScreenManager.RemoveScreen("LoadGames");

                    SavedGame savedGame = FileHelper.ReadSavedGame(((EventData_LoadSavedGame)producedEvent).Filename);

                    ScreenManager.AddScreen("LoadingScreen", new LoadingGame(savedGame.MapName, true, savedGame));

                    return true;
                }

                if (ScreenManager.GetScreen("MainMenu") != null)
                {
                    ScreenManager.RemoveScreen("Background");
                    ScreenManager.RemoveScreen("MainMenu");
                    
                    if (((EventData_LoadSavedGame)producedEvent).Filename != null)
                    {
                        SavedGame savedGame = FileHelper.ReadSavedGame(((EventData_LoadSavedGame)producedEvent).Filename);
                        ScreenManager.AddScreen("LoadingScreen", new LoadingGame(savedGame.MapName, true, savedGame));
                    }
                    else
                    {
                        ScreenManager.AddScreen("LoadingScreen", new LoadingGame("Map_1_2.txt", true, null));
                    }

                    return true;
                }

                if (ScreenManager.GetScreen("ContinueGame") != null)
                {
                    ScreenManager.RemoveScreen("ContinueGame");

                    if (((EventData_LoadSavedGame)producedEvent).Filename != null)
                    {
                        SavedGame savedGame = FileHelper.ReadSavedGame(((EventData_LoadSavedGame)producedEvent).Filename);
                        ScreenManager.AddScreen("LoadingScreen", new ChangingGame(savedGame.MapName, savedGame));
                    }
                    else
                    {
                        ScreenManager.AddScreen("LoadingScreen", new ChangingGame("Map_1_2.txt", null));
                    }

                    return true;
                }
            }
                
            return false;
        }
    }
}
