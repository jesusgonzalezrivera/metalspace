using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Effects;
using MetalSpace.Managers;

namespace MetalSpace.Effects
{
    /// <summary>
    /// The <c>GaussianBlur</c> class represents a <c>PostProcessor</c>
    /// that apply a Gaussian Blur effect to the current scene.
    /// </summary>
    class GaussianBlur : PostProcessor
    {
        #region Fields

        /// <summary>
        /// Blur amount to be applied
        /// </summary>
        float _blurAmount;

        /// <summary>
        /// Kernel of the first dimension of the Gaussian Blur.
        /// </summary>
        float[] _weightsH;

        /// <summary>
        /// Kernel of the second dimension of the Gaussian Blur.
        /// </summary>
        float[] _weightsV;

        /// <summary>
        /// Texture offsets used for the horizontal Gaussian Blur pass.
        /// </summary>
        Vector2[] _offsetsH;

        /// <summary>
        /// Texture offsets used for the vertical Gaussian Blur pass.
        /// </summary>
        Vector2[] _offsetsV;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>GaussianBlur</c> class.
        /// </summary>
        /// <param name="blurAmount">Amount of blur to be applied.</param>
        public GaussianBlur(float blurAmount)
            : base(EngineManager.ContentManager.Load<Effect>(
            "Content/Effects/GaussianBlur"))
        {
            _blurAmount = blurAmount;

            // Calculate weights/offsets for horizontal pass
            CalcSettings(1.0f / (float)EngineManager.GameGraphicsDevice.Viewport.Width, 
                0, out _weightsH, out _offsetsH);

            // Calcutate weights/offsets for vertical pass
            CalcSettings(0, 1.0f / (float)EngineManager.GameGraphicsDevice.Viewport.Height, 
                out _weightsV, out _offsetsV);
        }

        #endregion

        #region CalcSettings Method

        /// <summary>
        /// Calculate the weights and offsets of the passes.
        /// </summary>
        /// <param name="w">Width.</param>
        /// <param name="h">Height.</param>
        /// <param name="weights">Weights.</param>
        /// <param name="offsets">Offsets.</param>
        void CalcSettings(float w, float h, out float[] weights, out Vector2[] offsets)
        {
            // 15 samples
            weights = new float[15];
            offsets = new Vector2[15];

            // Calculate values for center pixel
            weights[0] = GaussianFn(0);
            offsets[0] = new Vector2(0);

            float total = weights[0];

            // Calculate samples in pairs
            for (int i = 0; i < 7; i++)
            {
                // Weight each pair of samples according to Gaussian function
                float weight = GaussianFn(i + 1);

                weights[i * 2 + 1] = weight;
                weights[i * 2 + 2] = weight;

                total += weight * 2;

                // Samples are offset by 1.5 pixels, to make use of filtering
                // halfway between pixels
                float offset = i * 2 + 1.5f;

                Vector2 offsetVec = new Vector2(w, h) * offset;

                offsets[i * 2 + 1] = offsetVec;
                offsets[i * 2 + 2] = -offsetVec;
            }

            // Divide all weights by total so the will add up to 1
            for (int i = 0; i < weights.Length; i++)
                weights[i] /= total;
        }

        #endregion

        #region GaussianFn Method

        /// <summary>
        /// Calculate the value of the GaussianBlur with one parameter.
        /// </summary>
        /// <param name="x">Value to apply to the function.</param>
        /// <returns></returns>
        float GaussianFn(float x)
        {
            return (float)((1.0f / Math.Sqrt(2 * Math.PI * _blurAmount)) *
                            Math.Exp(-(x * x) / (2 * _blurAmount * _blurAmount)));
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Apply the GaussianBlur effect on the current scene.
        /// </summary>
        public override void Draw()
        {
            // Set values for horizontal pass
            Effect.Parameters["Offsets"].SetValue(_offsetsH);
            Effect.Parameters["Weights"].SetValue(_weightsH);

            // Render this pass into the RenderCapture
            base.Draw();
        }

        #endregion
    }
}
