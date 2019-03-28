using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace MetalSpace.Interfaces
{
    /// <summary>
    /// Interface to represents a game texture.
    /// </summary>
    public interface IGameTexture
    {
        /// <summary>
        /// Name of the file that contains the sound.
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Reference to the game texture.
        /// </summary>
        Texture2D BaseTexture { get; set; }

        /// <summary>
        /// true if the texture is loaded, false otherwise.
        /// </summary>
        bool ReadyToRender { get; set; }

        /// <summary>
        /// Load the content of the game texture.
        /// </summary>
        void LoadContent();

        /// <summary>
        /// Unload the content of the game texture.
        /// </summary>
        void UnloadContent();
    }
}
