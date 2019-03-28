using System;
using System.Collections.Generic;
using System.Text;
using MetalSpace.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetalSpace.Effects
{
    /// <summary>
    /// The <c>RenderCapture</c> class represents an auxiliar render that
    /// permits to apply the post processor effects to individual objects.
    /// </summary>
    class RenderCapture
    {
        #region Fields

        /// <summary>
        /// Render target that contains the original user render.
        /// </summary>
        public RenderTarget2D _renderTarget;

        /// <summary>
        /// Render target that contains the auxiliar user render.
        /// </summary>
        public RenderTarget2D _renderTarget1;

        /// <summary>
        /// Render target that contains the final result.
        /// </summary>
        public RenderTarget2D _finalTarget;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>RenderCapture</c> class.
        /// </summary>
        public RenderCapture()
        {
            PresentationParameters pp = EngineManager.GameGraphicsDevice.PresentationParameters;
            int width = pp.BackBufferWidth;
            int height = pp.BackBufferHeight;
            SurfaceFormat format = pp.BackBufferFormat;

            _renderTarget = new RenderTarget2D(EngineManager.GameGraphicsDevice,
                width, height, false, format, pp.DepthStencilFormat, pp.MultiSampleCount,
                RenderTargetUsage.PreserveContents);

            _renderTarget1 = new RenderTarget2D(EngineManager.GameGraphicsDevice,
                width / 2, height / 2, false, format, pp.DepthStencilFormat,
                pp.MultiSampleCount, RenderTargetUsage.PreserveContents);

            _finalTarget = new RenderTarget2D(EngineManager.GameGraphicsDevice,
                width, height, false, format, pp.DepthStencilFormat, pp.MultiSampleCount,
                RenderTargetUsage.PreserveContents);
        }

        #endregion

        #region Draw Methods

        /// <summary>
        /// Set the render target to permits the draw of the individual objects.
        /// </summary>
        public void BeginRender()
        {
            EngineManager.GameGraphicsDevice.SetRenderTarget(_renderTarget);
        }

        /// <summary>
        /// Init the effect on the initial render target.
        /// </summary>
        public void InitEffect()
        {
            EngineManager.GameGraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
            EngineManager.GameGraphicsDevice.SetRenderTarget(_renderTarget1);
        }

        /// <summary>
        /// End the effect application.
        /// </summary>
        public void EndEffect()
        {

        }

        /// <summary>
        /// Set the render target to the default value.
        /// </summary>
        public void EndRender()
        {
            EngineManager.GameGraphicsDevice.SetRenderTarget(_finalTarget);
            EngineManager.GameGraphicsDevice.Textures[1] = _renderTarget;

            ScreenManager.SpriteBatch.Begin(0, BlendState.Opaque);

            ScreenManager.SpriteBatch.Draw(_renderTarget1, new Rectangle(0, 0,
                EngineManager.GameGraphicsDevice.Viewport.Width,
                EngineManager.GameGraphicsDevice.Viewport.Height),
                Color.White);

            ScreenManager.SpriteBatch.End();

            EngineManager.GameGraphicsDevice.SetRenderTarget(null);
        }

        #endregion

        /// <summary>
        /// Get the current value of the render target.
        /// </summary>
        /// <returns></returns>
        public Texture2D GetTexture()
        {
            return _renderTarget;
        }
    }
}
