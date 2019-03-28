using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.GameComponents;
using MetalSpace.Managers;
using MetalSpace.Events;
using MetalSpace.Objects;
using MetalSpace.Settings;

namespace MetalSpace.GameScreens
{
    /// <summary>
    /// The <c>Inventory</c> class represents the list of not equipped
    /// objects that the player has.
    /// </summary>
    class Inventory
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
        /// List of not equipped objects of the player.
        /// </summary>
        private List<Objects.Object> _notEquippedObjects;

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

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>Inventory</c> class.
        /// </summary>
        /// <param name="inventoryScreen">Reference to the inventory screen.</param>
        /// <param name="player">Reference to the player.</param>
        /// <param name="notEquippedObjects">List of not equipped objects to be displayed.</param>
        /// <param name="initialPosition">Initial position of the equipment section.</param>
        public Inventory(InventoryScreen inventoryScreen, Player player, 
            List<Objects.Object> notEquippedObjects, Vector2 initialPosition)
        {
            _notEquippedObjects = notEquippedObjects;

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
        /// Add a new object to the not equipped objects list.
        /// </summary>
        /// <param name="newObject">Object to be added.</param>
        /// <returns>true if the object is added, false otherwise.</returns>
        public bool AddObject(Objects.Object newObject)
        {
            if (_notEquippedObjects.Count == 9)
                return false;

            _notEquippedObjects.Add(newObject);

            for(int i=0; i < _notEquippedObjects.Count; i++)
                if(_notEquippedObjects[i].Position != i)
                    _notEquippedObjects[i].Position = i;

            return true;
        }

        /// <summary>
        /// Remove an object from the not equipped objects list.
        /// </summary>
        /// <param name="index">Index of the object to be removed.</param>
        /// <returns>true if the object is removed, false otherwise.</returns>
        public bool RemoveObject(int index)
        {
            if (index >= _notEquippedObjects.Count)
                return false;

            _notEquippedObjects.RemoveAt(index);

            // Order position of elements
            for (int i = 0; i < _notEquippedObjects.Count; i++)
                if (_notEquippedObjects[i].Position != i)
                    _notEquippedObjects[i].Position = i;

            if (_selectedObject >= _notEquippedObjects.Count)
            {
                _selectedObject = _notEquippedObjects.Count - 1;
                if (_selectedObject < 0)
                    _selectedObject = 0;
            }

            return true;
        }

        /// <summary>
        /// Remove an object from the not equipped objects list.
        /// </summary>
        /// <param name="removableObject">Object to be removed.</param>
        /// <returns>true if the object is removed, false otherwise.</returns>
        public bool RemoveObject(Objects.Object removableObject)
        {
            bool exits = false;
            foreach (Objects.Object playerObject in _notEquippedObjects)
                if (playerObject == removableObject)
                    exits = true;

            if (exits == false)
                return false;
            else
            {
                _notEquippedObjects.Remove(removableObject);

                // Order position of elements
                for (int i = 0; i < _notEquippedObjects.Count; i++)
                    if (_notEquippedObjects[i].Position != i)
                        _notEquippedObjects[i].Position = i;

                if (_selectedObject >= _notEquippedObjects.Count)
                {
                    _selectedObject = _notEquippedObjects.Count - 1;
                    if (_selectedObject < 0)
                        _selectedObject = 0;
                }

                return true;
            }
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load the necessary elements of the not equipment screen.
        /// </summary>
        public void Load()
        {

        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload the necessary elements of the not equipment screen.
        /// </summary>
        public void Unload()
        {

        }

        #endregion

        #region HandleInput Method

        /// <summary>
        /// Handle the input of the user in the inventory screen.
        /// </summary>
        /// <param name="input">Current state of the player input.</param>
        public void HandleInput(GameComponents.Input input)
        {
            if (input.Up)
            {
                if (!_inside)
                {
                    int nextValue = (_selectedObject / 3) == 0 ? _selectedObject :
                        3 * ((_selectedObject / 3) - 1) + (_selectedObject % 3);
                    if (nextValue < _notEquippedObjects.Count)
                        _selectedObject = nextValue;
                }
                else
                    _selectedEntryMenu = _selectedEntryMenu + 1 > 1 ? 0 : 1;
            }

            if (input.Down)
            {
                if (!_inside)
                {
                    int nextValue = (_selectedObject / 3) == 2 ? _selectedObject :
                        3 * ((_selectedObject / 3) + 1) + (_selectedObject % 3);
                    if (nextValue < _notEquippedObjects.Count)
                        _selectedObject = nextValue;
                }
                else
                    _selectedEntryMenu = _selectedEntryMenu - 1 < 0 ? 1 : 0;
            }

            if (input.Left)
            {
                _selectedObject = (_selectedObject - 1) < 0 ? 0 : _selectedObject - 1;
            }

            if (input.Right)
            {
                _selectedObject = (_selectedObject + 1) >= _notEquippedObjects.Count ?
                    _notEquippedObjects.Count - 1 : _selectedObject + 1;
            }

            if (input.Action)
            {
                if (!_inside)
                    _inside = true;
                else
                {
                    if (_selectedEntryMenu == 0)
                    {
                        if (_notEquippedObjects[_selectedObject] is Objects.Ammo)
                        {
                            if (_player.PlayerGun != null && _player.PlayerGun.GunType == ((Objects.Ammo)_notEquippedObjects[_selectedObject]).Type &&
                                _player.PlayerGun.CurrentAmmo < _player.PlayerGun.MaxAmmo - 1)
                                EventManager.Trigger(new EventData_ReloadGun(_player, _notEquippedObjects[_selectedObject]));
                        }
                        else
                            EventManager.Trigger(new EventData_EquipObject(_player, _notEquippedObjects[_selectedObject], _inventoryScreen));
                    }
                    else
                        EventManager.Trigger(new EventData_ThrowAway(_player, _notEquippedObjects[_selectedObject], _inventoryScreen));
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
        /// Update the elements of the inventory screen.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {

        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the not equipped objects of the equipment screen.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public void Draw(GameTime gameTime)
        {
            Vector2 viewportSize = new Vector2(
                GameSettings.DefaultInstance.ResolutionWidth,
                GameSettings.DefaultInstance.ResolutionHeight);

            // Draw the background rectangle.
            ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("GridBackground").BaseTexture as Texture2D,
                new Rectangle((int)_initialPosition.X, (int)_initialPosition.Y,
                    (int)(viewportSize.X * 3f / 8f - InventoryScreen.HPad * 2f),
                    (int)(viewportSize.Y * 3.8f / 8f - InventoryScreen.VPad)),
                new Color(255, 255, 255, 200));

            // Draw each non-equipped object
            foreach (Objects.Object playerObject in _notEquippedObjects)
                playerObject.Draw(gameTime);

            if (!_focus || _notEquippedObjects.Count == 0) return;

            Rectangle destination = new Rectangle();
            destination.X = (int)((_selectedObject % 3) * ((viewportSize.X / 8f) * 0.875f) +
                (viewportSize.X * 0.19f / 8f) +
                (viewportSize.X / 2f) + InventoryScreen.HPad);
            destination.Y = (int)((int)(_selectedObject / 3) * ((viewportSize.X / 8f) * 0.87f) +
                (viewportSize.X * 0.2f / 8f) +
                (viewportSize.Y / 8f) + InventoryScreen.VPad);
            destination.Width = (int)(viewportSize.X * 0.65f / 8f);
            destination.Height = (int)(viewportSize.X * 0.6f / 8f);
            ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DummyTexture15T").BaseTexture as Texture2D,
                destination, new Color(255, 255, 255, 200));

            int separation = (int)((viewportSize.X * 0.2f / 8f) + (viewportSize.Y / 8f) + InventoryScreen.VPad);

            if (_inside)
            {
                string[] information = new string[6];
                if (_notEquippedObjects[_selectedObject] is Objects.Armature)
                {
                    information[0] = _notEquippedObjects[_selectedObject].Name;
                    information[1] = StringHelper.DefaultInstance.Get("equipment_armature_type") + ": ";
                    if (((Objects.Armature)_notEquippedObjects[_selectedObject]).Type == Objects.Armature.ArmatureType.Helmet)
                        information[1] += StringHelper.DefaultInstance.Get("equipment_armature_type_helmet");
                    else if (((Objects.Armature)_notEquippedObjects[_selectedObject]).Type == Objects.Armature.ArmatureType.Body)
                        information[1] += StringHelper.DefaultInstance.Get("equipment_armature_type_suit");
                    else if (((Objects.Armature)_notEquippedObjects[_selectedObject]).Type == Objects.Armature.ArmatureType.Hand)
                        information[1] += StringHelper.DefaultInstance.Get("equipment_armature_type_boots");
                    else
                        information[1] += StringHelper.DefaultInstance.Get("equipment_armature_type_suit");

                    information[2] = StringHelper.DefaultInstance.Get("equipment_armature_defense") + ": " +
                        ((Objects.Armature)_notEquippedObjects[_selectedObject]).Defense;
                    information[3] = StringHelper.DefaultInstance.Get("equipment_armature_skill") + ": " +
                        ((Objects.Armature)_notEquippedObjects[_selectedObject]).Skill;
                    information[4] = StringHelper.DefaultInstance.Get("inventory_select_object_equip");
                    information[5] = StringHelper.DefaultInstance.Get("inventory_select_object_throw_away");

                    ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DialogBackground").BaseTexture as Texture2D,
                        new Rectangle(destination.X, destination.Y + destination.Height,
                            (int)(viewportSize.X * 1.6f / 8f), (int)(viewportSize.Y * 2.9f / 8f)),
                        Color.White);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                        information[0],
                        new Vector2(destination.X * 1.025f, (destination.Height * 1.375f) + destination.Y),
                        Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                        information[1],
                        new Vector2(destination.X * 1.025f, (destination.Height * 1.85f) + destination.Y),
                        Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                        information[2],
                        new Vector2(destination.X * 1.025f, (destination.Height * 2.325f) + destination.Y),
                        Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                        information[3],
                        new Vector2(destination.X * 1.025f, (destination.Height * 2.8f) + destination.Y),
                        Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("SeparationBar").BaseTexture,
                        new Rectangle(
                            (int)(destination.X * 1.025f), (int)((destination.Height * 3.275f) + destination.Y),
                            (int)(viewportSize.X * 1.25f / 8f), 3),
                        Color.White);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      information[4],
                      new Vector2(destination.X * 1.025f, (destination.Height * 3.45f) + destination.Y),
                      _selectedEntryMenu == 0 ? Color.White : Color.SlateGray,
                      0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      information[5],
                      new Vector2(destination.X * 1.025f, (destination.Height * 3.925f) + destination.Y),
                      _selectedEntryMenu == 1 ? Color.White : Color.SlateGray,
                      0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
                }
                else if (_notEquippedObjects[_selectedObject] is Objects.Weapon)
                {
                    information[0] = _notEquippedObjects[_selectedObject].Name;
                    information[1] = StringHelper.DefaultInstance.Get("equipment_weapon_type") + ": " +
                        StringHelper.DefaultInstance.Get("equipment_weapon");
                    information[2] = StringHelper.DefaultInstance.Get("equipment_weapon_ammo") + ": " +
                        ((Objects.Weapon)_notEquippedObjects[_selectedObject]).CurrentAmmo;
                    information[3] = StringHelper.DefaultInstance.Get("equipment_weapon_power") + ": " +
                        ((Objects.Weapon)_notEquippedObjects[_selectedObject]).Power;
                    information[4] = StringHelper.DefaultInstance.Get("inventory_select_object_equip");
                    information[5] = StringHelper.DefaultInstance.Get("inventory_select_object_throw_away");

                    ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DialogBackground").BaseTexture as Texture2D,
                        new Rectangle(destination.X, destination.Y + destination.Height,
                            (int)(viewportSize.X * 1.6f / 8f), (int)(viewportSize.Y * 2.9f / 8f)),
                        Color.White);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                        information[0],
                        new Vector2(destination.X * 1.025f, (destination.Height * 1.375f) + destination.Y),
                        Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                        information[1],
                        new Vector2(destination.X * 1.025f, (destination.Height * 1.85f) + destination.Y),
                        Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                        information[2],
                        new Vector2(destination.X * 1.025f, (destination.Height * 2.325f) + destination.Y),
                        Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                        information[3],
                        new Vector2(destination.X * 1.025f, (destination.Height * 2.8f) + destination.Y),
                        Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("SeparationBar").BaseTexture,
                        new Rectangle(
                            (int)(destination.X * 1.025f), (int)((destination.Height * 3.275f) + destination.Y),
                            (int)(viewportSize.X * 1.25f / 8f), 3),
                        Color.White);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      information[4],
                      new Vector2(destination.X * 1.025f, (destination.Height * 3.45f) + destination.Y),
                      _selectedEntryMenu == 0 ? Color.White : Color.SlateGray,
                      0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      information[5],
                      new Vector2(destination.X * 1.025f, (destination.Height * 3.925f) + destination.Y),
                      _selectedEntryMenu == 1 ? Color.White : Color.SlateGray,
                      0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
                }
                else if (_notEquippedObjects[_selectedObject] is Objects.Ammo)
                {
                    information[0] = _notEquippedObjects[_selectedObject].Name;
                    information[1] = StringHelper.DefaultInstance.Get("equipment_weapon_type") + ": " +
                        ((Objects.Ammo)_notEquippedObjects[_selectedObject]).Type.ToString();
                    information[2] = StringHelper.DefaultInstance.Get("equipment_weapon_ammo") + ": " +
                        ((Objects.Ammo)_notEquippedObjects[_selectedObject]).Amount;
                    information[3] = StringHelper.DefaultInstance.Get("inventory_select_object_reload");
                    information[4] = StringHelper.DefaultInstance.Get("inventory_select_object_throw_away");

                    ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DialogBackground").BaseTexture as Texture2D,
                        new Rectangle(destination.X, destination.Y + destination.Height,
                            (int)(viewportSize.X * 1.6f / 8f), (int)(viewportSize.Y * 2.6f / 8f)),
                        Color.White);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                        information[0],
                        new Vector2(destination.X * 1.025f, (destination.Height * 1.375f) + destination.Y),
                        Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                        information[1],
                        new Vector2(destination.X * 1.025f, (destination.Height * 1.85f) + destination.Y),
                        Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                        information[2],
                        new Vector2(destination.X * 1.025f, (destination.Height * 2.325f) + destination.Y),
                        Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("SeparationBar").BaseTexture,
                        new Rectangle(
                            (int)(destination.X * 1.025f), (int)((destination.Height * 2.8f) + destination.Y),
                            (int)(viewportSize.X * 1.25f / 8f), 3),
                        Color.White);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      information[3],
                      new Vector2(destination.X * 1.025f, (destination.Height * 2.975f) + destination.Y),
                      _selectedEntryMenu == 0 ? Color.White : Color.SlateGray,
                      0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      information[4],
                      new Vector2(destination.X * 1.025f, (destination.Height * 3.45f) + destination.Y),
                      _selectedEntryMenu == 1 ? Color.White : Color.SlateGray,
                      0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
                }
                else
                {
                    information[0] = _notEquippedObjects[_selectedObject].Name;
                    information[1] = StringHelper.DefaultInstance.Get("inventory_select_object_equip");
                    information[2] = StringHelper.DefaultInstance.Get("inventory_select_object_throw_away");

                    ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DialogBackground").BaseTexture as Texture2D,
                        new Rectangle(destination.X, destination.Y + destination.Height,
                            (int)(viewportSize.X * 1.6f / 8f), (int)(viewportSize.Y * 1.75f / 8f)),
                        Color.White);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                        information[0],
                        new Vector2(destination.X * 1.025f, (destination.Height * 1.375f) + destination.Y),
                        Color.White, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("SeparationBar").BaseTexture,
                        new Rectangle(
                            (int)(destination.X * 1.025f), (int)((destination.Height * 1.85f) + destination.Y),
                            (int)(viewportSize.X * 1.25f / 8f), 3),
                        Color.White);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      information[1],
                      new Vector2(destination.X * 1.025f, (destination.Height * 1.975f) + destination.Y),
                      _selectedEntryMenu == 0 ? Color.White : Color.SlateGray,
                      0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);

                    ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                      information[2],
                      new Vector2(destination.X * 1.025f, (destination.Height * 2.45f) + destination.Y),
                      _selectedEntryMenu == 1 ? Color.White : Color.SlateGray,
                      0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
                }

                
            }
        }

        #endregion
    }
}
