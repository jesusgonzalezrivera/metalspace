using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MetalSpace.Sound;
using MetalSpace.Interfaces;

namespace MetalSpace.Managers
{
    /// <summary>
    /// The <c>SoundManager</c> class represents a sound container, which
    /// prevents the user having to create and destroy many times the same sounds.
    /// </summary>
    class SoundManager : GameComponent
    {
        #region Fields

        /// <summary>
        /// List of sounds indexed by name.
        /// </summary>
        private static Dictionary<String, IGameSound> _sounds =
            new Dictionary<string, IGameSound>();

        /// <summary>
        /// Store for the Initialized property.
        /// </summary>
        private static bool _initialized = false;

        /// <summary>
        /// Store for the SoundsLoaded property.
        /// </summary>
        private static int _soundsLoaded = 0;

        /// <summary>
        /// Mutex thread to avoid simultaneous loading and destroying of the sounds.
        /// </summary>
        private static Mutex _mutex = new Mutex();

        #endregion

        #region Properties

        /// <summary>
        /// Initialized property
        /// </summary>
        /// <value>
        /// true if the <c>SoundManager</c> was initialized, false otherwise.
        /// </value>
        public static bool Initialized
        {
            get { return _initialized; }
            set { _initialized = value; }
        }

        /// <summary>
        /// SoundsLoaded property
        /// </summary>
        /// <value>
        /// Number of sounds loaded.
        /// </value>
        public static int SoundsLoaded
        {
            get { return _soundsLoaded; }
            set { _soundsLoaded = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>SoundManager</c> class.
        /// </summary>
        /// <param name="game">Reference to the main class.</param>
        public SoundManager(Game game) : base(game)
        {

        }

        #endregion

        #region Initialize Method

        /// <summary>
        /// Initialize the content of the <c>SoundManager</c>.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            foreach (IGameSound sound in _sounds.Values)
            {
                if (!sound.ReadyToRender)
                    sound.LoadContent();
            }

            _initialized = true;
        }

        #endregion

        #region AddSound Method

        /// <summary>
        /// Add a new model to the <c>SoundManager</c>.
        /// </summary>
        /// <param name="soundName">Name of the sound.</param>
        /// <param name="newSound">Reference to the sound to be added.</param>
        public static void AddSound(string soundName, IGameSound newSound)
        {
            if (!String.IsNullOrEmpty(soundName) &&
                !_sounds.ContainsKey(soundName))
            {   
                if (_initialized)
                {
                    _mutex.WaitOne();
                    _sounds.Add(soundName, newSound);
                    newSound.LoadContent();
                    _soundsLoaded++;
                    _mutex.ReleaseMutex();
                }
            }
        }

        #endregion

        #region RemoveSound Method

        /// <summary>
        /// Remove an existing sound of the <c>SoundManager</c>.
        /// </summary>
        /// <param name="textureName">Name of the sound.</param>
        public static void RemoveSound(string soundName)
        {
            if (!String.IsNullOrEmpty(soundName) &&
                _sounds.ContainsKey(soundName))
            {
                if (_initialized)
                {
                    _mutex.WaitOne();
                    _sounds[soundName].UnloadContent();
                    _sounds.Remove(soundName);
                    _soundsLoaded--;
                    _mutex.ReleaseMutex();
                }
            }
        }

        #endregion

        #region GetKeys Method

        /// <summary>
        /// Get the list of keys (names) of the loaded sounds.
        /// </summary>
        /// <returns>List of keys (names) of the loaded sounds.</returns>
        public static List<string> GetKeys()
        {
            List<string> keys = new List<string>();
            foreach (string key in _sounds.Keys)
                keys.Add(key);
            return keys;
        }

        #endregion

        #region GetSound Method

        /// <summary>
        /// Get the reference of the sound specified.
        /// </summary>
        /// <param name="textureName">Name of the sound.</param>
        /// <returns>Reference to the sound with the specified name.</returns>
        public static IGameSound GetSound(string soundName)
        {
            if (!String.IsNullOrEmpty(soundName) &&
                _sounds.ContainsKey(soundName))
                return _sounds[soundName];
            else
                return null;
        }

        #endregion

        #region StopAllSounds Method

        public static void StopAllSounds()
        {
            foreach (IGameSound sound in _sounds.Values)
                sound.Pause();
        }

        #endregion
    }
}
