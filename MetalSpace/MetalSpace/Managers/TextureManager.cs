using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MetalSpace.Textures;
using MetalSpace.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetalSpace.Managers
{
    /// <summary>
    /// The <c>TextureManager</c> class represents a texture container, which
    /// prevents the user having to create and destroy many times the same textures.
    /// </summary>
    class TextureManager : GameComponent
    {
        #region Fields

        /// <summary>
        /// List of textures indexed by name.
        /// </summary>
        private static Dictionary<String, IGameTexture> _textures =
            new Dictionary<string, IGameTexture>();

        /// <summary>
        /// Store for the Initialized property.
        /// </summary>
        private static bool _initialized = false;

        /// <summary>
        /// Store for the TexturesLoaded property.
        /// </summary>
        private static int _texturesLoaded = 0;

        /// <summary>
        /// Mutex thread to avoid simultaneous loading and destroying of the textures.
        /// </summary>
        private static Mutex _mutex = new Mutex();

        #endregion

        #region Properties

        /// <summary>
        /// Initialized property
        /// </summary>
        /// <value>
        /// true if the <c>TextureManager</c> was initialized, false otherwise.
        /// </value>
        public static bool Initialized
        {
            get { return _initialized; }
            set { _initialized = value; }
        }

        /// <summary>
        /// TexturesLoaded property
        /// </summary>
        /// <value>
        /// Number of textures loaded.
        /// </value>
        public static int TexturesLoaded
        {
            get { return _texturesLoaded; }
            set { _texturesLoaded = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>TextureManager</c> class.
        /// </summary>
        /// <param name="game">Instance of the main game.</param>
        public TextureManager(Game game) : base(game)
        {

        }

        #endregion

        #region Initialize Method

        /// <summary>
        /// Initialize the content of the <c>TextureManager</c>.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            foreach (IGameTexture texture in _textures.Values)
            {
                if (!texture.ReadyToRender)
                    texture.LoadContent();
            }

            _initialized = true;
        }

        #endregion

        #region AddTexture Method

        /// <summary>
        /// Add a new texture to the <c>TextureManager</c>.
        /// </summary>
        /// <param name="textureName">Name of the texture.</param>
        /// <param name="newTexture">Reference to the texture to be added.</param>
        public static void AddTexture(string textureName, IGameTexture newTexture)
        {
            if (!String.IsNullOrEmpty(textureName) &&
                !_textures.ContainsKey(textureName))
            {
                if (_initialized)
                {
                    _mutex.WaitOne();
                    _textures.Add(textureName, newTexture);
                    newTexture.LoadContent();
                    _texturesLoaded++;
                    _mutex.ReleaseMutex();
                }
            }
        }

        #endregion

        #region RemoveTexture Method

        /// <summary>
        /// Remove an existing texture of the <c>TextureManager</c>.
        /// </summary>
        /// <param name="textureName">Name of the texture.</param>
        public static void RemoveTexture(string textureName)
        {
            if (!String.IsNullOrEmpty(textureName) &&
                _textures.ContainsKey(textureName))
            {
                if (_initialized)
                {
                    _mutex.WaitOne();
                    _textures[textureName].UnloadContent();
                    _textures.Remove(textureName);
                    _texturesLoaded--;
                    _mutex.ReleaseMutex();
                }
            }
        }

        #endregion

        #region GetKeys Method
        
        /// <summary>
        /// Get the list of keys (names) of the loaded textures.
        /// </summary>
        /// <returns>List of keys (names) of the loaded textures.</returns>
        public static List<string> GetKeys()
        {
            List<string> keys = new List<string>();
            foreach (string key in _textures.Keys)
                keys.Add(key);
            return keys;
        }

        #endregion

        #region GetTexture Method

        /// <summary>
        /// Get the reference of the texture specified.
        /// </summary>
        /// <param name="textureName">Name of the texture.</param>
        /// <returns>Reference to the texture with the specified name.</returns>
        public static IGameTexture GetTexture(string textureName)
        {
            if (!String.IsNullOrEmpty(textureName) &&
                _textures.ContainsKey(textureName))
                return _textures[textureName];
            else
                return null;
        }

        #endregion
    }
}
