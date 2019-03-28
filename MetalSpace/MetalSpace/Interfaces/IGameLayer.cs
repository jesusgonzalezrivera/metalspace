using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Cameras;

namespace MetalSpace.Interfaces
{
    /// <summary>
    /// Interface with the common data of a layer.
    /// </summary>
    interface IGameLayer
    {
        /// <summary>
        /// Name of the layer.
        /// </summary>
        string LayerName { get; set; }

        /// <summary>
        /// Number of rows of the layer.
        /// </summary>
        int Rows { get; set; }

        /// <summary>
        /// Number of cols of the layer.
        /// </summary>
        int Cols { get; set; }

        /// <summary>
        /// Origin position of the layer.
        /// </summary>
        Vector3 Origin { get; set; }

        /// <summary>
        /// Load the content of the layer.
        /// </summary>
        void Load();

        /// <summary>
        /// Unload the content of the layer.
        /// </summary>
        void Unload();

        void Move(Vector3 distance);

        /// <summary>
        /// Update the current state of the layer.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Draw the current state of the layer.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        /// <param name="customEffect">Custom effect to be applied to the layer.</param>
        void Draw(GameTime gameTime, Effect customEffect);
    }
}
