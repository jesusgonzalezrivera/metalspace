using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Models;

namespace MetalSpace.Interfaces
{
    /// <summary>
    /// Interface with the basic funcionality of a drawable model.
    /// </summary>
    public interface IDrawableModel
    {
        /// <summary>
        /// Id that represents the model.
        /// </summary>
        int ModelID { get; set; }

        /// <summary>
        /// <c>GameModel</c> that contains the model.
        /// </summary>
        GameModel Model { get; set; }

        /// <summary>
        /// Group of matrix that contains the model transformations.
        /// </summary>
        Matrix[] ModelTransforms { get; set; }

        /// <summary>
        /// Position of the model.
        /// </summary>
        Vector3 Position { get; set; }

        /// <summary>
        /// Rotation of the model.
        /// </summary>
        Vector3 Rotation { get; set; }

        /// <summary>
        /// Scale of the model.
        /// </summary>
        Vector3 Scale { get; set; }

        /// <summary>
        /// Bounding box that wrap the model.
        /// </summary>
        BoundingBox BBox { get; set; }

        /// <summary>
        /// Bounding sphere that wrap the model.
        /// </summary>
        BoundingSphere BSphere { get; set; }

        /// <summary>
        /// Load the content of the model.
        /// </summary>
        void Load();

        /// <summary>
        /// Unload the content of the model.
        /// </summary>
        void Unload();

        /// <summary>
        /// Update the current state of the model.
        /// </summary>
        /// <param name="time">Global time of the game.</param>
        void Update(GameTime time);

        /// <summary>
        /// Draw the current state of the model.
        /// </summary>
        /// <param name="view">View matrix.</param>
        /// <param name="projection">Projection matrix.</param>
        /// <param name="method">Method used to draw the model (NoInstancing, HardwareInstancing).</param>
        /// <param name="individualTransformations">Individual transforms of the meshes.</param>
        /// <param name="customEffect">Custom effect to be applied to the model.</param>
        void Draw(Matrix view, Matrix projection, DrawingMethod method,
            Dictionary<string, Matrix> individualTransformations, Effect customEffect);
    }
}
