using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Events;
using MetalSpace.Objects;
using MetalSpace.Textures;
using MetalSpace.Managers;
using MetalSpace.Settings;
using MetalSpace.GameComponents;

namespace MetalSpace.GameScreens
{
    /// <summary>
    /// The <c>InventoryScreen</c> class represents the inventory of the player,
    /// including equipped and non-equipped objects.
    /// </summary>
    class InventoryScreen: GameScreen
    {
        #region Fields

        /// <summary>
        /// Horizontal pad.
        /// </summary>
        public const int HPad = 16;

        /// <summary>
        /// Vertical pad.
        /// </summary>
        public const int VPad = 16;

        /// <summary>
        /// Enum of the parts in the inventory screen.
        /// </summary>
        public enum InventoryParts
        {
            /// <summary>
            /// Equipment part.
            /// </summary>
            Equipment, 
            /// <summary>
            /// Inventory part.
            /// </summary>
            Inventory,
            /// <summary>
            /// Close part.
            /// </summary>
            Exit
        }

        /// <summary>
        /// Reference to the main game screen.
        /// </summary>
        private MainGameScreen _gameScreen;

        /// <summary>
        /// Reference to the player.
        /// </summary>
        private Player _player;

        /// <summary>
        /// Store for the Equipment property.
        /// </summary>
        private Equipment _equipment;

        /// <summary>
        /// Store for the Inventory property.
        /// </summary>
        private Inventory _inventory;

        /// <summary>
        /// Name of the background of the inventory screen.
        /// </summary>
        private string _backgroundTextureName;

        /// <summary>
        /// true if the user is inside the equipment or the inventory, false otherwise.
        /// </summary>
        private bool _inside;

        /// <summary>
        /// Current element selected by the user.
        /// </summary>
        private InventoryParts _currentElement;

        /// <summary>
        /// Equipment position.
        /// </summary>
        private Vector2 _equipmentPosition;

        /// <summary>
        /// Inventory position.
        /// </summary>
        private Vector2 _inventoryPosition;

        /// <summary>
        /// Close button position.
        /// </summary>
        private Vector2 _closeButtonPosition;

        /// <summary>
        /// Background position.
        /// </summary>
        private Vector2 _backgroundPosition;

        #endregion

        #region Properties

        /// <summary>
        /// Reference to the equipment part of the inventory screen.
        /// </summary>
        public Equipment Equipment
        {
            get { return _equipment; }
            set { _equipment = value; }
        }

        /// <summary>
        /// Reference to the inventory part of the inventory screen.
        /// </summary>
        public Inventory Inventory
        {
            get { return _inventory; }
            set { _inventory = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>InventoryScreen</c> class.
        /// </summary>
        /// <param name="gameScreen">Reference to the main screen of the game.</param>
        /// <param name="name">Name of the screen.</param>
        /// <param name="backgroundTextureName">Name of the background texture.</param>
        /// <param name="player">Reference to the player.</param>
        public InventoryScreen(MainGameScreen gameScreen, string name, 
            string backgroundTextureName, Player player)
        {
            GameGraphicsDevice = EngineManager.GameGraphicsDevice;

            Name = name;

            _gameScreen = gameScreen;

            _player = player;

            TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            TransitionOffTime = TimeSpan.FromSeconds(0.5f);

            Input.Continuous = false;

            IsPopup = true;

            _backgroundTextureName = backgroundTextureName;

            _inside = false;
            _currentElement = InventoryParts.Exit;

            Vector2 viewportSize = new Vector2(
                GameSettings.DefaultInstance.ResolutionWidth, 
                GameSettings.DefaultInstance.ResolutionHeight);

            _backgroundPosition = new Vector2();
            _backgroundPosition.X = viewportSize.X / 8f;
            _backgroundPosition.Y = viewportSize.Y / 8f;

            _closeButtonPosition = new Vector2();
            _closeButtonPosition.X = viewportSize.X * 6f / 8f;
            _closeButtonPosition.Y = viewportSize.Y * 6f / 8f;
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load and calculate all the necessary content of the inventory screen.
        /// </summary>
        public override void Load()
        {
            base.Load();

            List<Objects.Object> equippedObjects = new List<Objects.Object>();
            List<Objects.Object> notEquippedObjects = new List<Objects.Object>();
            foreach (Objects.Object playerObject in _player.Objects)
            {
                if (playerObject.IsEquipped)
                    equippedObjects.Add(playerObject);
                else
                    notEquippedObjects.Add(playerObject);
            }

            Vector2 viewportSize = new Vector2(
                GameSettings.DefaultInstance.ResolutionWidth,
                GameSettings.DefaultInstance.ResolutionHeight);

            _equipmentPosition = new Vector2();
            _equipmentPosition.X = (viewportSize.X / 8f) + HPad;
            _equipmentPosition.Y = (viewportSize.Y / 8f) + VPad;
            _equipment = new Equipment(this, _player, equippedObjects, _equipmentPosition);
            _equipment.Focus = false;

            _inventoryPosition = new Vector2();
            _inventoryPosition.X = (viewportSize.X / 2f) + HPad;
            _inventoryPosition.Y = (viewportSize.Y / 8f) + VPad;
            _inventory = new Inventory(this, _player, notEquippedObjects, _inventoryPosition);
            _inventory.Focus = false;
        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload the necessary elements of the inventory screen.
        /// </summary>
        public override void Unload()
        {
            base.Unload();

            Input.Continuous = true;

            _equipment.Unload();
            _inventory.Unload();
        }

        #endregion

        #region Handle Input Method

        /// <summary>
        /// Handle the input of the user relative to the <c>InventoryScreen</c> screen.
        /// </summary>
        /// <param name="input">Current state of the player input.</param>
        /// <param name="gameTime">Current time of the game.</param>
        public override void HandleInput(GameComponents.Input input, GameTime gameTime)
        {
            base.HandleInput(input, gameTime);

            if (!_inside)
            {
                if (input.Up)
                {
                    if (_currentElement == InventoryParts.Equipment)
                        _currentElement = InventoryParts.Exit;
                    else if (_currentElement == InventoryParts.Inventory)
                        _currentElement = InventoryParts.Equipment;
                    else
                        _currentElement = InventoryParts.Inventory;
                }

                if (input.Down)
                {
                    if (_currentElement == InventoryParts.Equipment)
                        _currentElement = InventoryParts.Inventory;
                    else if (_currentElement == InventoryParts.Inventory)
                        _currentElement = InventoryParts.Exit;
                    else
                        _currentElement = InventoryParts.Equipment;
                }

                if (input.Left)
                {
                    if (_currentElement == InventoryParts.Inventory)
                        _currentElement = InventoryParts.Equipment;
                    else if (_currentElement == InventoryParts.Equipment)
                        _currentElement = InventoryParts.Inventory;
                    else
                        _currentElement = InventoryParts.Equipment;
                }

                if (input.Right)
                {
                    if (_currentElement == InventoryParts.Inventory)
                        _currentElement = InventoryParts.Equipment;
                    else if (_currentElement == InventoryParts.Equipment)
                        _currentElement = InventoryParts.Inventory;
                    else
                        _currentElement = InventoryParts.Equipment;
                }

                if (input.Action)
                {
                    switch (_currentElement)
                    {
                        case InventoryParts.Inventory:
                            _inside = true;
                            _equipment.HandleInput(input);
                            break;

                        case InventoryParts.Equipment:
                            _inside = true;
                            _inventory.HandleInput(input);
                            break;

                        case InventoryParts.Exit:
                            ScreenManager.RemoveScreen("Inventory");
                            break;
                    }
                }

                if (input.Cancel)
                {
                    ScreenManager.RemoveScreen("Inventory");
                }
            }
            else
            {
                bool isCurrentElementInside = false;
                if (_currentElement == InventoryParts.Equipment && _equipment.Inside)
                    isCurrentElementInside = true;
                else if (_currentElement == InventoryParts.Inventory && _inventory.Inside)
                    isCurrentElementInside = true;

                if (_currentElement == InventoryParts.Equipment)
                {
                    _equipment.Focus = true;
                    _equipment.HandleInput(input);
                }
                else if (_currentElement == InventoryParts.Inventory)
                {
                    _inventory.Focus = true;
                    _inventory.HandleInput(input);
                }

                if (input.Cancel)
                {
                    if(!isCurrentElementInside)
                    {
                        _inventory.Focus = false;
                        _equipment.Focus = false;
                        _inside = false;
                    }
                }
            }
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the state of the inventory screen.
        /// </summary>
        /// <param name="gameTime">Current time of the game.</param>
        /// <param name="otherScreenHasFocus">true if other screen has the focus, false otherwise.</param>
        /// <param name="coveredByOtherScreen">true if other screen cover this screen, false otherwise.</param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            _equipment.Update(gameTime);

            _inventory.Update(gameTime);
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the inventory screen.
        /// </summary>
        /// <param name="gameTime">Current time of the game.</param>
        public override void Draw(GameTime gameTime)
        {
            Vector2 viewportSize = new Vector2(
                GameSettings.DefaultInstance.ResolutionWidth,
                GameSettings.DefaultInstance.ResolutionHeight);

            Color color = new Color(255, 255, 255, 200);

            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullCounterClockwise, null, CanvasManager.ScaleMatrix);

            ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture(_backgroundTextureName).BaseTexture as Texture2D,
                new Rectangle((int)_backgroundPosition.X, (int)_backgroundPosition.Y,
                              (int)((viewportSize.X * 3f / 4f)),
                              (int)((viewportSize.Y * 3f / 4f))),
                color);

            ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
              StringHelper.DefaultInstance.Get("inventory_total_points") + _player.TotalPoints,
              new Vector2(
                  _inventoryPosition.X,
                  _inventoryPosition.Y + viewportSize.Y * 3.85f / 8f - InventoryScreen.VPad),
              Color.White);

            ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                StringHelper.DefaultInstance.Get("inventory_material_aerogel") + _player.Aerogel,
                new Vector2(
                    _inventoryPosition.X,
                    _inventoryPosition.Y + viewportSize.Y * 4.3f / 8f - InventoryScreen.VPad),
                Color.White);

            ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                StringHelper.DefaultInstance.Get("inventory_material_debolio") + _player.Debolio,
                new Vector2(
                    _inventoryPosition.X,
                    _inventoryPosition.Y + viewportSize.Y * 4.75f / 8f - InventoryScreen.VPad),
                Color.White);

            ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
              StringHelper.DefaultInstance.Get("inventory_material_fulereno") + _player.Fulereno,
              new Vector2(
                  _inventoryPosition.X,
                  _inventoryPosition.Y + viewportSize.Y * 5.2f / 8f - InventoryScreen.VPad),
              Color.White);

            ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("CloseButton").BaseTexture as Texture2D,
                new Rectangle((int)_closeButtonPosition.X, (int)_closeButtonPosition.Y,
                              (int)((viewportSize.X * 7f / 8f) - HPad * 3f - _closeButtonPosition.X),
                              (int)((viewportSize.Y * 7f / 8f) - VPad * 2f - _closeButtonPosition.Y)),
                color);

            _inventory.Draw(gameTime);

            _equipment.Draw(gameTime);

            if (!_inside)
            {
                if (_currentElement == InventoryParts.Equipment)
                {
                    ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                        new Rectangle((int)_equipmentPosition.X, (int)_equipmentPosition.Y, 
                            (int)(viewportSize.X * 3f / 8f - InventoryScreen.HPad),
                            (int)(viewportSize.Y * 6f / 8f - InventoryScreen.VPad * 2f)),
                        Color.White);
                }
                else if (_currentElement == InventoryParts.Inventory)
                {
                    ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                        new Rectangle((int)_inventoryPosition.X, (int)_inventoryPosition.Y,
                            (int)(viewportSize.X * 2.85f / 8f - InventoryScreen.HPad),
                            (int)(viewportSize.Y * 4f / 8f - InventoryScreen.VPad * 2f)),
                        Color.White);
                }
                else
                {
                    ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                        new Rectangle((int)_closeButtonPosition.X, (int)_closeButtonPosition.Y,
                            (int)((viewportSize.X * 7f / 8f) - HPad * 3f - _closeButtonPosition.X),
                            (int)((viewportSize.Y * 7f / 8f) - VPad * 2f - _closeButtonPosition.Y)),
                        Color.White);
                }
            }

            ScreenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}
