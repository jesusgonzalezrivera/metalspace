using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Managers;
using MetalSpace.Interfaces;

namespace MetalSpace.Textures
{
    /// <summary>
    /// The <c>GameTexture</c> class represents an individual texture that can be used
    /// in the game.
    /// </summary>
    class GameTexture : IGameTexture
    {
        #region Fields

        /// <summary>
        /// Store for the FileName property.
        /// </summary>
        private string _fileName;

        /// <summary>
        /// Store for the BaseTexture property.
        /// </summary>
        private Texture2D _baseTexture;

        /// <summary>
        /// Store for the ReadyToRender property.
        /// </summary>
        private bool _readyToRender;

        /// <summary>
        /// true if it is necessary to use the default loader, false otherwise.
        /// </summary>
        private bool _defaultLoader;

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
        /// BaseTexture property
        /// </summary>
        /// <value>
        /// Reference to the texture.
        /// </value>
        public Texture2D BaseTexture
        {
            get { return _baseTexture; }
            set { _baseTexture = value; }
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

        #endregion
        
        #region Constructor

        /// <summary>
        /// Constructor of the <c>GameTexture</c> class.
        /// </summary>
        public GameTexture() { }

        /// <summary>
        /// Constructor of the <c>GameTexture</c> class.
        /// </summary>
        /// <param name="fileName">Name of the file that contains the texture.</param>
        /// <param name="defaultLoader">true if it is necessary to use the default loader, false otherwise.</param>
        public GameTexture(string fileName, bool defaultLoader = false)
        {
            _fileName = fileName;
            _defaultLoader = defaultLoader;
        }

        #endregion

        #region LoadContent Method

        /// <summary>
        /// Load the needed content of the <c>GameTexture</c>.
        /// </summary>
        public void LoadContent()
        {
            if (!String.IsNullOrEmpty(_fileName) && 
                !String.IsNullOrWhiteSpace(_fileName))
            {
                if(_defaultLoader)
                    _baseTexture = EngineManager.ContentManager.Load<Texture2D>(_fileName);
                else
                    _baseTexture = EngineManager.Loader.Load<Texture2D>(_fileName);
                _readyToRender = true;
            }
        }

        #endregion

        #region UnloadContent Method

        /// <summary>
        /// Unload the needed content of the <c>GameTexture</c>.
        /// </summary>
        public void UnloadContent()
        {
            _baseTexture.Dispose();
        }

        #endregion
    }
}
