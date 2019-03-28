using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MetalSpace.Managers;
using MetalSpace.Interfaces;

namespace MetalSpace.Sound
{
    /// <summary>
    /// The <c>GameSound</c> class represent an individual sound of the game
    /// (sound effect or music).
    /// </summary>
    class GameSound : IGameSound
    {
        #region Fields

        /// <summary>
        /// Store for the FileName property.
        /// </summary>
        private string _fileName;

        /// <summary>
        /// Store for the GameSoundEffect property.
        /// </summary>
        private SoundEffect _gameSoundEffect;

        /// <summary>
        /// Store for the GameSoundPlayer property.
        /// </summary>
        private SoundEffectInstance _gameSoundPlayer;

        /// <summary>
        /// Store for the ReadyToRender property.
        /// </summary>
        private bool _readyToRender;

        /// <summary>
        /// true if it is necessary to use the default loader, false otherwise.
        /// </summary>
        private bool _defaultLoader;

        /// <summary>
        /// Store for the Volume property.
        /// </summary>
        private float _volume;

        #endregion

        #region Properties

        /// <summary>
        /// FileName property
        /// </summary>
        /// <value>
        /// Name of the file that store the model.
        /// </value>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        /// <summary>
        /// GameSoundEffect property
        /// </summary>
        /// <value>
        /// Reference to the game sound effect.
        /// </value>
        public SoundEffect GameSoundEffect
        {
            get { return _gameSoundEffect; }
            set { _gameSoundEffect = value; }
        }

        /// <summary>
        /// GameSoundPlayer property
        /// </summary>
        /// <value>
        /// Reference to the game sound player.
        /// </value>
        public SoundEffectInstance GameSoundPlayer
        {
            get { return _gameSoundPlayer; }
            set { _gameSoundPlayer = value; }
        }

        /// <summary>
        /// ReadyToRender property
        /// </summary>
        /// <value>
        /// true if the model is ready to render, false otherwise.
        /// </value>
        public bool ReadyToRender
        {
            get { return _readyToRender; }
            set { _readyToRender = value; }
        }

        /// <summary>
        /// Volume property
        /// </summary>
        /// <value>
        /// Amount of volume used to play the sound.
        /// </value>
        public float Volume
        {
            get { return _volume; }
            set 
            { 
                _volume = value;
                _gameSoundPlayer.Volume = _volume;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>GameSound</c> class.
        /// </summary>
        public GameSound() { }

        /// <summary>
        /// Constructor of the <c>GameSound</c> class.
        /// </summary>
        /// <param name="fileName">Name of the file that contains the sound.</param>
        /// <param name="defaultLoader">true if it is necessary to use the default loader, false otherwise.</param>
        public GameSound(string fileName, bool defaultLoader = false)
        {
            _fileName = fileName;
            _defaultLoader = defaultLoader;
            _volume = 1.0f;
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load the needed content of the <c>GameSound</c>.
        /// </summary>
        public void LoadContent()
        {
            if (!String.IsNullOrEmpty(_fileName) &&
                !String.IsNullOrWhiteSpace(_fileName))
            {
                if (_defaultLoader)
                    _gameSoundEffect = EngineManager.ContentManager.Load<SoundEffect>(_fileName);
                else
                    _gameSoundEffect = EngineManager.Loader.Load<SoundEffect>(_fileName);
                _gameSoundPlayer = _gameSoundEffect.CreateInstance();
                
                _readyToRender = true;
            }
        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload the needed content of the <c>GameSound</c>.
        /// </summary>
        public void UnloadContent()
        {
            _gameSoundPlayer.Dispose();
            _gameSoundEffect.Dispose();
            _readyToRender = false;
        }

        #endregion

        #region Play Method

        /// <summary>
        /// Play the current sound.
        /// </summary>
        /// <param name="newSound">true if we want a new instance of the GameSoundEffect, false otherwise.</param>
        /// <param name="loop">true if it is necessary to play the sound constantly, false otherwise.</param>
        public void Play(bool newSound = false, bool loop = false)
        {
            if (newSound)
            {
                _gameSoundPlayer = _gameSoundEffect.CreateInstance();
                _gameSoundPlayer.Volume = _volume;
            }

            if (_gameSoundPlayer.State == SoundState.Stopped)
            {
                _gameSoundPlayer.IsLooped = loop;
                _gameSoundEffect.CreateInstance().Resume();
                _gameSoundPlayer.Play();
            }
            else if (_gameSoundPlayer.State == SoundState.Paused)
                _gameSoundPlayer.Resume();
        }

        #endregion

        #region Pause Method

        /// <summary>
        /// Pause the current playback of the sound.
        /// </summary>
        public void Pause()
        {
            if (_gameSoundPlayer.State == SoundState.Playing)
                _gameSoundPlayer.Pause();
        }

        #endregion

        #region Stop Method

        /// <summary>
        /// Stop the current playback of the sound.
        /// </summary>
        public void Stop()
        {
            if (_gameSoundPlayer.State == SoundState.Playing || 
                _gameSoundPlayer.State == SoundState.Paused)
                _gameSoundPlayer.Stop();
        }

        #endregion
    }
}
