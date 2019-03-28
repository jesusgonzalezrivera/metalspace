using System;
using System.Collections.Generic;
using System.Text;
using MetalSpace.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetalSpace.Effects
{
    /// <summary>
    /// The <c>PostProcessor</c> class represent a base class that permits
    /// to apply a post processor effect to the current scene.
    /// </summary>
    class PostProcessor
    {
        #region Fields

        /// <summary>
        /// Store for the Input property.
        /// </summary>
        private Texture2D _input;

        /// <summary>
        /// Store for the Effect property.
        /// </summary>
        private Effect _effect;

        #endregion

        #region Properties

        /// <summary>
        /// Input property
        /// </summary>
        /// <value>
        /// /// Texture to be used to apply the effect.
        /// </value>
        public Texture2D Input
        {
            get { return _input; }
            set { _input = value; }
        }

        /// <summary>
        /// Effect property
        /// </summary>
        /// <value>
        /// Effect to be applied to the input image.
        /// </value>
        public Effect Effect
        {
            get { return _effect; }
            set { _effect = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>PostProcessor</c> class.
        /// </summary>
        /// <param name="effect">Effect to be applied for the post processor.</param>
        public PostProcessor(Effect effect)
        {
            _effect = effect;
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the effect on the input texture.
        /// </summary>
        public virtual void Draw()
        {
            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, 
                BlendState.Opaque, null, null, null, _effect);

            ScreenManager.SpriteBatch.Draw(_input,
                new Rectangle(0, 0, EngineManager.GameGraphicsDevice.Viewport.Width, 
                    EngineManager.GameGraphicsDevice.Viewport.Height), 
                Color.White);
            
            ScreenManager.SpriteBatch.End();

            EngineManager.GameGraphicsDevice.DepthStencilState = DepthStencilState.Default;
            EngineManager.GameGraphicsDevice.BlendState = BlendState.Opaque;
        }

        #endregion
    }
}
