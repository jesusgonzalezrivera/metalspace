using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Managers;
using MetalSpace.Interfaces;
using MetalSpace.Events;

namespace MetalSpace.Models
{
    /// <summary>
    /// The <c>GameModel</c> represents an individual model that can not be
    /// drawed and it is stored in the <c>ModelManager</c>.
    /// </summary>
    public class GameModel : IGameModel
    {
        #region Fields

        /// <summary>
        /// Store for the FileName property.
        /// </summary>
        private string _fileName;
        
        /// <summary>
        /// Store for the Model property.
        /// </summary>
        private Model _model;

        /// <summary>
        /// Store for the Texture property.
        /// </summary>
        private Texture2D _texture;

        /// <summary>
        /// Store for the ReadyToRender property.
        /// </summary>
        private bool _readyToRender;

        /// <summary>
        /// Default loader used to load the model.
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
        /// Model property
        /// </summary>
        /// <value>
        /// Reference to the model loaded.
        /// </value>
        public Model Model
        {
            get { return _model; }
            set { _model = value; }
        }

        /// <summary>
        /// Texture property
        /// </summary>
        /// <value>
        /// Reference to the texture applied in the model.
        /// </value>
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
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
        /// Constructor of the <c>GameModel</c> class.
        /// </summary>
        public GameModel() { }

        /// <summary>
        /// Constructor of the <c>GameModel</c> class.
        /// </summary>
        /// <param name="fileName">Name of the file that contains the model.</param>
        /// <param name="defaultLoader">true if it is necessary to use the default loader, false otherwise.</param>
        public GameModel(string fileName, bool defaultLoader = false)
        {
            _fileName = fileName;

            _defaultLoader = defaultLoader;
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load the needed content of the <c>GameModel</c>.
        /// </summary>
        public void Load()
        {
            if (!String.IsNullOrEmpty(_fileName) &&
                !String.IsNullOrWhiteSpace(_fileName))
            {
                if(_defaultLoader)
                    _model = EngineManager.ContentManager.Load<Model>(_fileName);
                else
                    _model = EngineManager.Loader.Load<Model>(_fileName);

                if(_model.Meshes[0].Effects[0] is BasicEffect)
                    _texture = ((BasicEffect)_model.Meshes[0].Effects[0]).Texture;
                else
                    _texture = ((SkinnedEffect)_model.Meshes[0].Effects[0]).Texture;

                _readyToRender = true;
            }
        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload the content needed in the <c>GameModel</c>.
        /// </summary>
        public void Unload()
        {
            if(_texture != null)
                _texture.Dispose();

            _model = null;
        }

        #endregion
    }
}
