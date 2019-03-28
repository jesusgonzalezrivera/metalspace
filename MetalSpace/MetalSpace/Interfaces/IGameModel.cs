using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Models;

namespace MetalSpace.Interfaces
{
    /// <summary>
    /// Interface for a static model.
    /// </summary>
    public interface IGameModel
    {
        /// <summary>
        /// Name of the file that contains the model.
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Reference to the model to be drawed.
        /// </summary>
        Model Model { get; set; }

        /// <summary>
        /// true if the model is loaded, false otherwise.
        /// </summary>
        bool ReadyToRender { get; set; }

        /// <summary>
        /// Load the content of the game model.
        /// </summary>
        void Load();

        /// <summary>
        /// Unload the content of the game model.
        /// </summary>
        void Unload();
    }
}
