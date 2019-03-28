using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Managers;
using MetalSpace.Settings;

namespace MetalSpace.Objects
{
    /// <summary>
    /// The <c>Armature</c> class represents an armature element in the inventory
    /// of the player. There are four types of armatures: body, hand, foot and helmet.
    /// </summary>
    public class Armature : Objects.Object
    {
        #region Fields

        /// <summary>
        /// Type of the armatures.
        /// </summary>
        public enum ArmatureType
        {
            /// <summary>
            /// The armature cover the body.
            /// </summary>
            Body,
            /// <summary>
            /// The armature cover the hands.
            /// </summary>
            Hand,
            /// <summary>
            /// The armature cover the foots.
            /// </summary>
            Foot,
            /// <summary>
            /// The armature cover the head.
            /// </summary>
            Helmet
        }

        /// <summary>
        /// Store for the Skill property.
        /// </summary>
        private int _skill;

        /// <summary>
        /// Store for the Defense property.
        /// </summary>
        private int _defense;

        /// <summary>
        /// Store for the Type property.
        /// </summary>
        private ArmatureType _type;

        #endregion

        #region Properties

        /// <summary>
        /// Skill property
        /// </summary>
        /// <value>
        /// Amount of skill that the armature gets to the user.
        /// </value>
        public int Skill
        {
            get { return _skill; }
            set { _skill = value; }
        }

        /// <summary>
        /// Defense property
        /// </summary>
        /// <value>
        /// Amount of defense that the armature gets to the user.
        /// </value>
        public int Defense
        {
            get { return _defense; }
            set { _defense = value; }
        }

        /// <summary>
        /// Type property
        /// </summary>
        /// <value>
        /// Type of the armature.
        /// </value>
        public ArmatureType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>Armature</c> class.
        /// </summary>
        /// <param name="name">Name of the object.</param>
        /// <param name="textureName">Name of the texture that represents the object.</param>
        /// <param name="position">Position in the inventory.</param>
        /// <param name="isEquipped">true if the element is equipped, false otherwise.</param>
        /// <param name="defense">Amount of defense of the armature.</param>
        /// <param name="skill">Amount of skill of the armature.</param>
        /// <param name="type">Type of the armature.</param>
        public Armature(string name, string textureName, int position, 
            bool isEquipped, int defense, int skill, ArmatureType type) :
            base(name, textureName, position, isEquipped)
        {
            _skill = skill;
            _defense = defense;
            _type = type;
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load the needed content of the <c>Armature</c>.
        /// </summary>
        public override void  Load()
        {
            base.Load();
        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload the needed content of the <c>Armature</c>.
        /// </summary>
        public override void Unload()
        {
            base.Unload();
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the current state of the <c>Armature</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public override void  Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        #endregion

        #region Draw Method
        
        /// <summary>
        /// Draw the current state of the <c>Armature</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public override void  Draw(GameTime gameTime)
        {
            if (_isEquipped)
            {
                Vector2 viewportSize = new Vector2(
                GameSettings.DefaultInstance.ResolutionWidth,
                GameSettings.DefaultInstance.ResolutionHeight);

                Rectangle destination = new Rectangle();
                switch (_type)
                {
                    case ArmatureType.Helmet:
                        destination.X = (int)(viewportSize.X * 2.025f / 8f);
                        destination.Y = (int)(viewportSize.Y * 1.38f / 8f);
                        destination.Width = (int)(viewportSize.X * 0.73f / 8f);
                        destination.Height = (int)(viewportSize.Y * 0.875f / 8f);
                        break;

                    case ArmatureType.Body:
                        destination.X = (int)(viewportSize.X * 1.825f / 8f);
                        destination.Y = (int)(viewportSize.Y * 2.35f / 8f);
                        destination.Width = (int)(viewportSize.X * 1.185f / 8f);
                        destination.Height = (int)(viewportSize.Y * 3.5f / 8f);
                        break;

                    case ArmatureType.Hand:
                        destination.X = (int)(viewportSize.X * 1.25f / 8f);
                        destination.Y = (int)(viewportSize.Y * 3.87f / 8f);
                        destination.Width = (int)(viewportSize.X * 0.5f / 8f);
                        destination.Height = (int)(viewportSize.Y * 0.7f / 8f);

                        ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture(_textureName).BaseTexture as Texture2D,
                            destination, new Color(255, 255, 255, 200));

                        destination.X = (int)(viewportSize.X * 3.095f / 8f);
                        destination.Y = (int)(viewportSize.Y * 3.87f / 8f);
                        destination.Width = (int)(viewportSize.X * 0.5f / 8f);
                        destination.Height = (int)(viewportSize.Y * 0.7f / 8f);
                        break;

                    case ArmatureType.Foot:
                        destination.X = (int)(viewportSize.X * 1.825f / 8f);
                        destination.Y = (int)(viewportSize.Y * 5.99f / 8f);
                        destination.Width = (int)(viewportSize.X * 0.555f / 8f);
                        destination.Height = (int)(viewportSize.Y * 0.75f / 8f);

                        ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture(_textureName).BaseTexture as Texture2D,
                            destination, new Color(255, 255, 255, 200));

                        destination.X = (int)(viewportSize.X * 2.45f / 8f);
                        destination.Y = (int)(viewportSize.Y * 5.99f / 8f);
                        destination.Width = (int)(viewportSize.X * 0.555f / 8f);
                        destination.Height = (int)(viewportSize.Y * 0.75f / 8f);
                        break;
                }

                ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture(_textureName).BaseTexture as Texture2D,
                    destination, new Color(255, 255, 255, 200));
            }
            else
                base.Draw(gameTime);
        }

        #endregion
    }
}
