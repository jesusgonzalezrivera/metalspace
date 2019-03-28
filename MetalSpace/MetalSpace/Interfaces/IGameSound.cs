using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace MetalSpace.Interfaces
{
    /// <summary>
    /// Interface to represents a game sound.
    /// </summary>
    public interface IGameSound
    {
        /// <summary>
        /// Name of the file that contains the sound.
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Effect that represents the game sound.
        /// </summary>
        SoundEffect GameSoundEffect { get; set; }

        /// <summary>
        /// Instance to the effect that represents the game sound.
        /// </summary>
        SoundEffectInstance GameSoundPlayer { get; set; }

        /// <summary>
        /// true if the sound is loaded, false otherwise.
        /// </summary>
        bool ReadyToRender { get; set; }

        /// <summary>
        /// Level of volume of the game effect.
        /// </summary>
        float Volume { get; set; }

        /// <summary>
        /// Load the content of the game sound.
        /// </summary>
        void LoadContent();

        /// <summary>
        /// Unload the content of the game sound.
        /// </summary>
        void UnloadContent();

        /// <summary>
        /// Play the current sound effect.
        /// </summary>
        /// <param name="newSound">true if we want to create a new instance of the game sound, false otherwise.</param>
        /// <param name="loop">true if we need to repeat constantly the sound, false otherwise.</param>
        void Play(bool newSound = false, bool loop = false);

        /// <summary>
        /// Stop the current sound effect.
        /// </summary>
        void Stop();

        /// <summary>
        /// Pause the current sound effect.
        /// </summary>
        void Pause();
    }
}
