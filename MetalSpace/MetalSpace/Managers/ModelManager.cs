using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using MetalSpace.Interfaces;

namespace MetalSpace.Managers
{
    /// <summary>
    /// The <c>ModelManager</c> class represents a model container, which
    /// prevents the user having to create and destroy many times the same models.
    /// </summary>
    class ModelManager : GameComponent
    {
        #region Fields

        /// <summary>
        /// List of models indexed by name.
        /// </summary>
        private static Dictionary<string, IGameModel> _models =
            new Dictionary<string, IGameModel>();

        /// <summary>
        /// Store for the Initialized property.
        /// </summary>
        private static bool _initialized = false;

        /// <summary>
        /// Store for the ModelsLoaded property.
        /// </summary>
        private static int _modelsLoaded = 0;

        /// <summary>
        /// Mutex thread to avoid simultaneous loading and destroying of the models.
        /// </summary>
        private static Mutex _mutex = new Mutex();

        #endregion

        #region Properties

        /// <summary>
        /// Initialized property
        /// </summary>
        /// <value>
        /// true if the <c>ModelManager</c> was initialized, false otherwise.
        /// </value>
        public static bool Initialized
        {
            get { return _initialized; }
            set { _initialized = value; }
        }

        /// <summary>
        /// ModelsLoaded property
        /// </summary>
        /// <value>
        /// Number of models loaded.
        /// </value>
        public static int ModelsLoaded
        {
            get { return _modelsLoaded; }
            set { _modelsLoaded = value; }
        }

        #endregion

        #region Construtor

        /// <summary>
        /// Constructor of the <c>ModelManager</c> class.
        /// </summary>
        /// <param name="game">Instance of the main game.</param>
        public ModelManager(Game game) : base(game)
        {

        }

        #endregion

        #region Initialize Method

        /// <summary>
        /// Initialize the content of the <c>ModelManager</c>.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            foreach (IGameModel model in _models.Values)
            {
                if (!model.ReadyToRender)
                    model.Load();
            }

            _initialized = true;
        }

        #endregion

        #region AddModel Method

        /// <summary>
        /// Add a new model to the <c>ModelManager</c>.
        /// </summary>
        /// <param name="modelName">Name of the model.</param>
        /// <param name="newModel">Reference to the model to be added.</param>
        public static void AddModel(string modelName, IGameModel newModel)
        {
            if (!String.IsNullOrEmpty(modelName) &&
                !_models.ContainsKey(modelName))
            {
                if (_initialized)
                {
                    _mutex.WaitOne();
                    _models.Add(modelName, newModel);
                    newModel.Load();
                    _modelsLoaded++;
                    _mutex.ReleaseMutex();
                }
            }
        }

        #endregion

        #region RemoveModel Method

        /// <summary>
        /// Remove an existing model of the <c>ModelManager</c>.
        /// </summary>
        /// <param name="textureName">Name of the model.</param>
        public static void RemoveModel(string modelName)
        {
            if (!String.IsNullOrEmpty(modelName) &&
                _models.ContainsKey(modelName))
            {
                if (_initialized)
                {
                    _mutex.WaitOne();
                    _models[modelName].Unload();
                    _models.Remove(modelName);
                    _modelsLoaded--;
                    _mutex.ReleaseMutex();
                }
            }
        }

        #endregion

        #region GetKeys Method

        /// <summary>
        /// Get the list of keys (names) of the loaded models.
        /// </summary>
        /// <returns>List of keys (names) of the loaded models.</returns>
        public static List<string> GetKeys()
        {
            List<string> keys = new List<string>();
            foreach (string key in _models.Keys)
                keys.Add(key);
            return keys;
        }

        #endregion

        #region GetModel Method

        /// <summary>
        /// Get the reference of the model specified.
        /// </summary>
        /// <param name="textureName">Name of the model.</param>
        /// <returns>Reference to the model with the specified name.</returns>
        public static IGameModel GetModel(string modelName)
        {
            if (!String.IsNullOrEmpty(modelName) &&
                _models.ContainsKey(modelName))
                return _models[modelName];
            else
                return null;
        }

        #endregion
    }
}