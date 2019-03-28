using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MetalSpace.Textures;
using MetalSpace.Interfaces;
using MetalSpace.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetalSpace.Managers
{
    class LevelManager : GameComponent
    {
        #region Fields

        private static Dictionary<String, SceneRenderer> _levels =
            new Dictionary<string, SceneRenderer>();

        private static bool _initialized = false;

        private static int _levelsLoaded = 0;

        private static Mutex _mutex = new Mutex();

        #endregion

        #region Properties

        public static bool Initialized
        {
            get { return _initialized; }
            set { _initialized = value; }
        }

        public static int LevelsLoaded
        {
            get { return _levelsLoaded; }
            set { _levelsLoaded = value; }
        }

        #endregion

        #region Constructor

        public LevelManager(Game game) : base(game)
        {

        }

        #endregion

        #region Initialize Method

        public override void Initialize()
        {
            base.Initialize();

            foreach (SceneRenderer level in _levels.Values)
                level.Load();

            _initialized = true;
        }

        #endregion

        #region AddLevel Method

        public static void AddLevel(string levelName, SceneRenderer scene)
        {
            if (!String.IsNullOrEmpty(levelName) &&
                !_levels.ContainsKey(levelName))
            {
                if (_initialized)
                {
                    _mutex.WaitOne();
                    _levels.Add(levelName, scene);
                    scene.Load();
                    _levelsLoaded++;
                    _mutex.ReleaseMutex();
                }
            }
        }

        #endregion

        #region RemoveLevel Method

        public static void RemoveLevel(string levelName)
        {
            if (!String.IsNullOrEmpty(levelName) &&
                _levels.ContainsKey(levelName))
            {
                if (_initialized)
                {
                    _mutex.WaitOne();
                    _levels[levelName].Unload();
                    _levels.Remove(levelName);
                    _levelsLoaded--;
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
            foreach (string key in _levels.Keys)
                keys.Add(key);
            return keys;
        }

        #endregion

        #region GetLevel Method

        public static SceneRenderer GetLevel(string levelName)
        {
            if (!String.IsNullOrEmpty(levelName) &&
                _levels.ContainsKey(levelName))
                return _levels[levelName];
            else
                return null;
        }

        #endregion
    }
}
