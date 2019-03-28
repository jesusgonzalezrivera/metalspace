using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Events;
using MetalSpace.Objects;
using MetalSpace.Managers;
using MetalSpace.Settings;
using MetalSpace.GameComponents;

namespace MetalSpace.GameScreens
{
    /// <summary>
    /// The <c>Equipment</c> class represents the list of equipped
    /// objects that the player has.
    /// </summary>
    class Equipment
    {
        #region Fields

        /// <summary>
        /// Store for the Focus property.
        /// </summary>
        private bool _focus;

        /// <summary>
        /// Store for the Inside property.
        /// </summary>
        private bool _inside;

        /// <summary>
        /// Store for the SelectedObject property.
        /// </summary>
        private int _selectedObject;

        /// <summary>
        /// Selected menu entry.
        /// </summary>
        private int _selectedEntryMenu;
        
        /// <summary>
        /// Initial position of the entries.
        /// </summary>
        private Vector2 _initialPosition;

        /// <summary>
        /// Reference to the player.
        /// </summary>
        private Player _player;

        /// <summary>
        /// Reference to the main <c>InventoryScreen</c>.
        /// </summary>
        private InventoryScreen _inventoryScreen;

        /// <summary>
        /// List of equipped objects of the player.
        /// </summary>
        private List<Objects.Object> _equippedObjects;

        #endregion

        #region Properties

        /// <summary>
        /// Inside property
        /// </summary>
        /// <value>
        /// true if the player selects one equipped object.
        /// </value>
        public bool Inside
        {
            get { return _inside; }
            set { _inside = value; }
        }

        /// <summary>
        /// Focus property
        /// </summary>
        /// <value>
        /// true if the current equipment screen has the focus and false otherwise.
        /// </value>
        public bool Focus
        {
            get { return _focus; }
            set { _focus = value; }
        }

        /// <summary>
        /// SelectedObject property
        /// </summary>
        /// <value>
        /// Selected equipped object.
        /// </value>
        public int SelectedObject
        {
            get { return _selectedObject; }
            set { _selectedObject = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>Equipment</c> class.
        /// </summary>
        /// <param name="inventoryScreen">Reference to the inventory screen.</param>
        /// <param name="player">Reference to the player.</param>
        /// <param name="equippedObjects">List of equipped objects to be displayed.</param>
        /// <param name="initialPosition">Initial position of the equipment section.</param>
        public Equipment(InventoryScreen inventoryScreen, Player player,
            List<Objects.Object> equippedObjects, Vector2 initialPosition)
        {
            _equippedObjects = equippedObjects;

            _inside = false;
            _selectedObject = 0;
            _selectedEntryMenu = 0;

            _inventoryScreen = inventoryScreen;

            _player = player;

            _initialPosition = initialPosition;
        }

        #endregion

        #region Add/Remove Methods

        /// <summary>
        /// Add a new object to the equipped objects list.
        /// </summary>
        /// <param name="newObject">Object to be added.</param>
        /// <returns>true if the object is added, false otherwise.</returns>
        public bool AddObject(Objects.Object newObject)
        {
            if (_equippedObjects.Count == 5)
                return false;

            foreach(Objects.Object playerObject in _equippedObjects)
            {
                if (playerObject is Objects.Armature && newObject is Objects.Armature &&
                   ((Objects.Armature)playerObject).Type == ((Objects.Armature)newObject).Type)
                    return false;
                else if (playerObject is Objects.Weapon && newObject is Objects.Weapon)
                    return false;
            }

            _equippedObjects.Add(newObject);

            return true;
        }

        /// <summary>
        /// Remove an object from the equipped objects list.
        /// </summary>
        /// <param name="index">Index of the object to be removed.</param>
        /// <returns>true if the object is removed, false otherwise.</returns>
        public bool RemoveObject(int index)
        {
            if (index >= _equippedObjects.Count)
                return false;

            _equippedObjects.RemoveAt(index);

            if (_selectedObject >= _equippedObjects.Count)
            {
                _selectedObject = _equippedObjects.Count - 1;
                if (_selectedObject < 0)
                    _selectedObject = 0;
            }

            _inside = false;

            return true;
        }

        /// <summary>
        /// Remove an object from the equipped objects list.
        /// </summary>
        /// <param name="removableObject">Object to be removed.</param>
        /// <returns>true if the object is removed, false otherwise.</returns>
        public bool RemoveObject(Objects.Object removableObject)
        {
            bool exits = false;
            foreach (Objects.Object playerObject in _equippedObjects)
                if (playerObject == removableObject)
                    exits = true;

            if (exits == false)
                return false;
            else
            {
                _equippedObjects.Remove(removableObject);

                if (_selectedObject >= _equippedObjects.Count)
                {
                    _selectedObject = _equippedObjects.Count - 1;
                    if (_selectedObject < 0)
                        _selectedObject = 0;
                }

                _inside = false;

                return true;
            }
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load the necessary elements of the equipment screen.
        /// </summary>
        public void Load()
        {

        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload the necessary elements of the equipment screen.
        /// </summary>
        public void Unload()
        {

        }

        #endregion

        #region HandleInput Method

        /// <summary>
        /// Handle the input of the user in the equipment screen.
        /// </summary>
        /// <param name="input">Current state of the player input.</param>
        public void HandleInput(GameComponents.Input input)
        {
            if (input.Down)
            {
                if (!_inside)
                {
                    if (_selectedObject == 0)
                        _selectedObject = 2;
                    else if (_selectedObject == 1)
                        _selectedObject = 3;
                    else if (_selectedObject == 2)
                        _selectedObject = 4;
                    else if (_selectedObject == 3)
                        _selectedObject = 1;
                    else
                        _selectedObject = 0;
                }
                else
                    _selectedEntryMenu = _selectedEntryMenu - 1 < 0 ? 1 : 0;
            }

            if (input.Up)
            {
                if (!_inside)
                {
                    if (_selectedObject == 0)
                        _selectedObject = 4;
                    else if (_selectedObject == 1)
                        _selectedObject = 3;
                    else if (_selectedObject == 2)
                        _selectedObject = 0;
                    else if (_selectedObject == 3)
                        _selectedObject = 1;
                    else
                        _selectedObject = 2;
                }
                else
                    _selectedEntryMenu = _selectedEntryMenu + 1 > 1 ? 0 : 1;
            }

            if (input.Left)
            {
                if (!_inside)
                {
                    if (_selectedObject == 0)
                        _selectedObject = 1;
                    else if (_selectedObject == 1)
                        _selectedObject = 0;
                    else if (_selectedObject == 2)
                        _selectedObject = 3;
                    else if (_selectedObject == 3)
                        _selectedObject = 2;
                }
            }

            if (input.Right)
            {
                if (!_inside)
                {
                    if (_selectedObject == 0)
                        _selectedObject = 1;
                    else if (_selectedObject == 1)
                        _selectedObject = 0;
                    else if (_selectedObject == 2)
                        _selectedObject = 3;
                    else if (_selectedObject == 3)
                        _selectedObject = 2;
                }
            }

            if (input.Action)
            {
                if (!_inside)
                {
                    //_inside = true;
                    foreach (Objects.Object playerObject in _equippedObjects)
                    {
                        if (playerObject is Objects.Weapon && _selectedObject == 1)
                            _inside = true;
                        else if (playerObject is Objects.Armature && _selectedObject == 0 &&
                            ((Objects.Armature)playerObject).Type == Armature.ArmatureType.Helmet)
                            _inside = true;
                        else if (playerObject is Objects.Armature && _selectedObject == 2 &&
                            ((Objects.Armature)playerObject).Type == Armature.ArmatureType.Body)
                            _inside = true;
                        else if (playerObject is Objects.Armature && _selectedObject == 3 &&
                            ((Objects.Armature)playerObject).Type == Armature.ArmatureType.Hand)
                            _inside = true;
                        else if (playerObject is Objects.Armature && _selectedObject == 4 &&
                            ((Objects.Armature)playerObject).Type == Armature.ArmatureType.Foot)
                            _inside = true;
                    }
                }
                else
                {
                    if (_selectedEntryMenu == 0)
                    {
                        Objects.Object element = null;
                        for (int i = 0; i < _equippedObjects.Count && element == null; i++)
                        {
                            if (_selectedObject == 0 && _equippedObjects[i] is Objects.Armature &&
                               ((Objects.Armature)_equippedObjects[i]).Type == Armature.ArmatureType.Helmet)
                                element = _equippedObjects[i];
                            else if (_selectedObject == 1 && _equippedObjects[i] is Objects.Weapon)
                                element = _equippedObjects[i];
                            else if (_selectedObject == 2 && _equippedObjects[i] is Objects.Armature &&
                               ((Objects.Armature)_equippedObjects[i]).Type == Armature.ArmatureType.Body)
                                element = _equippedObjects[i];
                            else if (_selectedObject == 3 && _equippedObjects[i] is Objects.Armature &&
                               ((Objects.Armature)_equippedObjects[i]).Type == Armature.ArmatureType.Hand)
                                element = _equippedObjects[i];
                            else if (_selectedObject == 4 && _equippedObjects[i] is Objects.Armature &&
                               ((Objects.Armature)_equippedObjects[i]).Type == Armature.ArmatureType.Foot)
                                element = _equippedObjects[i];
                        }

                        if(element != null)
                            EventManager.Trigger(new EventData_UnequipObject(_player, element, _inventoryScreen));
                    }
                    else
                    {
                        Objects.Object element = null;
                        for (int i = 0; i < _equippedObjects.Count && element == null; i++)
                        {
                            if (_selectedObject == 0 && _equippedObjects[i] is Objects.Armature &&
                               ((Objects.Armature)_equippedObjects[i]).Type == Armature.ArmatureType.Helmet)
                                element = _equippedObjects[i];
                            else if (_selectedObject == 1 && _equippedObjects[i] is Objects.Weapon)
                                element = _equippedObjects[i];
                            else if (_selectedObject == 2 && _equippedObjects[i] is Objects.Armature &&
                               ((Objects.Armature)_equippedObjects[i]).Type == Armature.ArmatureType.Body)
                                element = _equippedObjects[i];
                            else if (_selectedObject == 3 && _equippedObjects[i] is Objects.Armature &&
                               ((Objects.Armature)_equippedObjects[i]).Type == Armature.ArmatureType.Hand)
                                element = _equippedObjects[i];
                            else if (_selectedObject == 4 && _equippedObjects[i] is Objects.Armature &&
                               ((Objects.Armature)_equippedObjects[i]).Type == Armature.ArmatureType.Foot)
                                element = _equippedObjects[i];
                        }

                        if (element != null)
                            EventManager.Trigger(new EventData_ThrowAway(_player, element, _inventoryScreen));
                    }
                }
            }

            if (input.Cancel)
            {
                _inside = false;
            }
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the elements of the equipment screen.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {

        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the equipped objects of the equipment screen.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public void Draw(GameTime gameTime)
        {
            Vector2 viewportSize = new Vector2(
                GameSettings.DefaultInstance.ResolutionWidth,
                GameSettings.DefaultInstance.ResolutionHeight);

            // Draw the background rectangle.
            ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("EquipmentBackground").BaseTexture as Texture2D,
                new Rectangle((int)_initialPosition.X, (int)_initialPosition.Y,
                    (int)(viewportSize.X * 3f / 8f - InventoryScreen.HPad),
                    (int)(viewportSize.Y * 6f / 8f - InventoryScreen.VPad * 2f)),
                new Color(255, 255, 255, 200));

            // Draw each equipped object
            foreach (Objects.Object playerObject in _equippedObjects)
                playerObject.Draw(gameTime);

            if (!_focus || _equippedObjects.Count == 0) return;

            Objects.Object element = null;
            foreach (Objects.Object equippedObject in _equippedObjects)
            {
                if ((_selectedObject == 1 && equippedObject is Objects.Weapon) ||
                    (_selectedObject == 0 && equippedObject is Objects.Armature && ((Objects.Armature)equippedObject).Type == Objects.Armature.ArmatureType.Helmet) ||
                    (_selectedObject == 2 && equippedObject is Objects.Armature && ((Objects.Armature)equippedObject).Type == Objects.Armature.ArmatureType.Body) ||
                    (_selectedObject == 3 && equippedObject is Objects.Armature && ((Objects.Armature)equippedObject).Type == Objects.Armature.ArmatureType.Hand) ||
                    (_selectedObject == 4 && equippedObject is Objects.Armature && ((Objects.Armature)equippedObject).Type == Objects.Armature.ArmatureType.Foot))
                    element = equippedObject;
            }

            if (_selectedObject == 0)
            {
                ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                    new Rectangle(
                        (int)(viewportSize.X * 2.025f / 8f), (int)(viewportSize.Y * 1.38f / 8f),
                        (int)(viewportSize.X * 0.73f / 8f), (int)(viewportSize.Y * 0.875f / 8f)),
                    Color.White);

                if (_inside)
                {
                    ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DialogBackground").BaseTexture as Texture2D, 
                        new Rectangle(
                            (int)(viewportSize.X * 1.675f / 8f), (int)(viewportSize.Y * 2.255f / 8f),
                            (int)(viewportSize.X * 1.5f / 8f), (int)(viewportSize.Y * 2.6f / 8f)), 
                        Color.White);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_armature_type") + ": " +
                      StringHelper.DefaultInstance.Get("equipment_armature_type_helmet"),
                      new Vector2(viewportSize.X * 1.8f / 8f, viewportSize.Y * 2.5f / 8f),
                      Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_armature_defense") + ": " + 
                      ((Objects.Armature)element).Defense,
                      new Vector2(viewportSize.X * 1.8f / 8f, viewportSize.Y * 2.95f / 8f),
                      Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_armature_skill") + ": " +
                      ((Objects.Armature)element).Skill,
                      new Vector2(viewportSize.X * 1.8f / 8f, viewportSize.Y * 3.4f / 8f),
                      Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("SeparationBar").BaseTexture,
                        new Rectangle(
                            (int)(viewportSize.X * 1.8f / 8f), (int)(viewportSize.Y * 3.75f / 8f),
                            (int)(viewportSize.X * 1.25f / 8f), 3),
                        Color.White);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_select_object_unequip"),
                      new Vector2(viewportSize.X * 1.8f / 8f, viewportSize.Y * 3.85f / 8f),
                      _selectedEntryMenu == 0 ? Color.White : Color.SlateGray, 
                      0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_select_object_throw_away"),
                      new Vector2(viewportSize.X * 1.8f / 8f, viewportSize.Y * 4.3f / 8f),
                      _selectedEntryMenu == 1 ? Color.White : Color.SlateGray, 
                      0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
                }
            }
            else if (_selectedObject == 1)
            {
                ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                    new Rectangle(
                        (int)(viewportSize.X * 3.105f / 8f), (int)(viewportSize.Y * 1.41f / 8f),
                        (int)(viewportSize.X * 0.725f / 8f), (int)(viewportSize.Y * 2.28f / 8f)),
                    Color.White);

                if (_inside)
                {
                    ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DialogBackground").BaseTexture as Texture2D,
                        new Rectangle(
                            (int)(viewportSize.X * 2.75 / 8f), (int)(viewportSize.Y * 2.2f / 8f),
                            (int)(viewportSize.X * 1.6f / 8f), (int)(viewportSize.Y * 2.6f / 8f)),
                        Color.White);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_weapon_type") + ": " +
                      StringHelper.DefaultInstance.Get("equipment_weapon"),
                      new Vector2(viewportSize.X * 2.875f / 8f, viewportSize.Y * 2.45f / 8f),
                      Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_weapon_ammo") + ": " +
                      ((Objects.Weapon)element).CurrentAmmo,
                      new Vector2(viewportSize.X * 2.875f / 8f, viewportSize.Y * 2.905f / 8f),
                      Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_weapon_power") + ": " +
                      ((Objects.Weapon)element).Power,
                      new Vector2(viewportSize.X * 2.875f / 8f, viewportSize.Y * 3.36f / 8f),
                      Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("SeparationBar").BaseTexture,
                        new Rectangle(
                            (int)(viewportSize.X * 2.875f / 8f), (int)(viewportSize.Y * 3.71f / 8f),
                            (int)(viewportSize.X * 1.25f / 8f), 3),
                        Color.White);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_select_object_unequip"),
                      new Vector2(
                          viewportSize.X * 2.875f / 8f, viewportSize.Y * 3.81f / 8f),
                      _selectedEntryMenu == 0 ? Color.White : Color.SlateGray,
                      0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_select_object_throw_away"),
                      new Vector2(
                          viewportSize.X * 2.875f / 8f, viewportSize.Y * 4.265f / 8f),
                      _selectedEntryMenu == 1 ? Color.White : Color.SlateGray,
                      0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
                }
            }
            else if (_selectedObject == 2)
            {
                ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                    new Rectangle(
                        (int)(viewportSize.X * 1.825f / 8f), (int)(viewportSize.Y * 2.35f / 8f),
                        (int)(viewportSize.X * 1.185f / 8f), (int)(viewportSize.Y * 3.5f / 8f)),
                    Color.White);

                if (_inside)
                {
                    ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DialogBackground").BaseTexture as Texture2D,
                        new Rectangle(
                            (int)(viewportSize.X * 1.675f / 8f), (int)(viewportSize.Y * 3.25f / 8f),
                            (int)(viewportSize.X * 1.6f / 8f), (int)(viewportSize.Y * 2.6f / 8f)),
                        Color.White);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_armature_type") + ": " +
                      StringHelper.DefaultInstance.Get("equipment_armature_type_suit"),
                      new Vector2(viewportSize.X * 1.8f / 8f, viewportSize.Y * 3.495f / 8f),
                      Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_armature_defense") + ": " +
                      ((Objects.Armature)element).Defense,
                      new Vector2(viewportSize.X * 1.8f / 8f, viewportSize.Y * 3.95f / 8f),
                      Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_armature_skill") + ": " +
                      ((Objects.Armature)element).Skill,
                      new Vector2(viewportSize.X * 1.8f / 8f, viewportSize.Y * 4.405f / 8f),
                      Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("SeparationBar").BaseTexture,
                        new Rectangle(
                            (int)(viewportSize.X * 1.8f / 8f), (int)(viewportSize.Y * 4.755f / 8f),
                            (int)(viewportSize.X * 1.25f / 8f), 3),
                        Color.White);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_select_object_unequip"),
                      new Vector2(
                          viewportSize.X * 1.8f / 8f, viewportSize.Y * 4.855f / 8f),
                      _selectedEntryMenu == 0 ? Color.White : Color.SlateGray,
                      0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_select_object_throw_away"),
                      new Vector2(
                          viewportSize.X * 1.8f / 8f, viewportSize.Y * 5.31f / 8f),
                      _selectedEntryMenu == 1 ? Color.White : Color.SlateGray,
                      0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
                }
            }
            else if (_selectedObject == 3)
            {
                ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                    new Rectangle(
                        (int)(viewportSize.X * 1.25f / 8f), (int)(viewportSize.Y * 3.87f / 8f),
                        (int)(viewportSize.X * 0.5f / 8f), (int)(viewportSize.Y * 0.7f / 8f)),
                    Color.White);

                ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                    new Rectangle(
                        (int)(viewportSize.X * 3.095f / 8f), (int)(viewportSize.Y * 3.87f / 8f),
                        (int)(viewportSize.X * 0.5f / 8f), (int)(viewportSize.Y * 0.7f / 8f)),
                    Color.White);

                if (_inside)
                {
                    ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DialogBackground").BaseTexture as Texture2D,
                        new Rectangle(
                            (int)(viewportSize.X * 1.7 / 8f), (int)(viewportSize.Y * 4.57f / 8f),
                            (int)(viewportSize.X * 1.6f / 8f), (int)(viewportSize.Y * 2.6f / 8f)),
                        Color.White);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_armature_type") + ": " +
                      StringHelper.DefaultInstance.Get("equipment_armature_type_gloves"),
                      new Vector2(viewportSize.X * 1.825f / 8f, viewportSize.Y * 4.815f / 8f),
                      Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_armature_defense") + ": " +
                      ((Objects.Armature)element).Defense,
                      new Vector2(viewportSize.X * 1.825f / 8f, viewportSize.Y * 5.27f / 8f),
                      Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_armature_skill") + ": " +
                      ((Objects.Armature)element).Skill,
                      new Vector2(viewportSize.X * 1.825f / 8f, viewportSize.Y * 5.725f / 8f),
                      Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("SeparationBar").BaseTexture,
                        new Rectangle(
                            (int)(viewportSize.X * 1.825f / 8f), (int)(viewportSize.Y * 6.075f / 8f),
                            (int)(viewportSize.X * 1.25f / 8f), 3),
                        Color.White);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_select_object_unequip"),
                      new Vector2(
                          viewportSize.X * 1.825f / 8f, viewportSize.Y * 6.175f / 8f),
                      _selectedEntryMenu == 0 ? Color.White : Color.SlateGray,
                      0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_select_object_throw_away"),
                      new Vector2(
                          viewportSize.X * 1.825f / 8f, viewportSize.Y * 6.63f / 8f),
                      _selectedEntryMenu == 1 ? Color.White : Color.SlateGray,
                      0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
                }
            }
            else
            {
                ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                    new Rectangle(
                        (int)(viewportSize.X * 1.825f / 8f), (int)(viewportSize.Y * 5.99f / 8f),
                        (int)(viewportSize.X * 0.555f / 8f), (int)(viewportSize.Y * 0.75f / 8f)),
                    Color.White);

                ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                    new Rectangle(
                        (int)(viewportSize.X * 2.45f / 8f), (int)(viewportSize.Y * 5.99f / 8f),
                        (int)(viewportSize.X * 0.555f / 8f), (int)(viewportSize.Y * 0.75f / 8f)),
                    Color.White);

                if (_inside)
                {
                    ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DialogBackground").BaseTexture as Texture2D,
                        new Rectangle(
                            (int)(viewportSize.X * 1.65 / 8f), (int)(viewportSize.Y * 3.25f / 8f),
                            (int)(viewportSize.X * 1.6f / 8f), (int)(viewportSize.Y * 2.75f / 8f)),
                        Color.White);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_armature_type") + ": " +
                      StringHelper.DefaultInstance.Get("equipment_armature_type_boots"),
                      new Vector2(viewportSize.X * 1.775f / 8f, viewportSize.Y * 3.52f / 8f),
                      Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_armature_defense") + ": " +
                      ((Objects.Armature)element).Defense,
                      new Vector2(viewportSize.X * 1.775f / 8f, viewportSize.Y * 3.975f / 8f),
                      Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_armature_skill") + ": " +
                      ((Objects.Armature)element).Skill,
                      new Vector2(viewportSize.X * 1.775f / 8f, viewportSize.Y * 4.43f / 8f),
                      Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("SeparationBar").BaseTexture,
                        new Rectangle(
                            (int)(viewportSize.X * 1.775f / 8f), (int)(viewportSize.Y * 4.885f / 8f),
                            (int)(viewportSize.X * 1.25f / 8f), 3),
                        Color.White);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_select_object_unequip"),
                      new Vector2(
                          viewportSize.X * 1.775f / 8f, viewportSize.Y * 4.985f / 8f),
                      _selectedEntryMenu == 0 ? Color.White : Color.SlateGray,
                      0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      StringHelper.DefaultInstance.Get("equipment_select_object_throw_away"),
                      new Vector2(
                          viewportSize.X * 1.775f / 8f, viewportSize.Y * 5.44f / 8f),
                      _selectedEntryMenu == 1 ? Color.White : Color.SlateGray,
                      0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
                }
            }
        }

        #endregion
    }
}
