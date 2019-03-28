using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Scene;
using MetalSpace.Events;
using MetalSpace.Objects;

namespace MetalSpace.Managers
{
    /// <summary>
    /// The <c>ShotManager</c> represents the class that permits to control
    /// state of the diferent shots shooted by the user.
    /// </summary>
    class ShotManager
    {
        #region Fields

        /// <summary>
        /// Reference to the scene where the shots move.
        /// </summary>
        private SceneRenderer _scene;

        /// <summary>
        /// Reference to the player.
        /// </summary>
        private Player _player;

        /// <summary>
        /// Reference to the enemies that can receive the shots.
        /// </summary>
        private Enemy[] _enemies;

        /// <summary>
        /// Store for the Shots property.
        /// </summary>
        private List<Shot> _shots;

        /// <summary>
        /// Store for the TileVertexBuffer property.
        /// </summary>
        private static DynamicVertexBuffer _tileVertexBuffer;

        #endregion

        #region Properties

        /// <summary>
        /// Shots property
        /// </summary>
        /// <value>
        /// List of shots in the <c>ShotManager</c>.
        /// </value>
        public List<Shot> Shots
        {
            get { return _shots; }
            set { _shots = value; }
        }

        /// <summary>
        /// TileVertexBuffer property
        /// </summary>
        /// <value>
        /// Reference to the vertex buffer where the vertex are stored.
        /// </value>
        public static DynamicVertexBuffer TileVertexBuffer
        {
            get { return _tileVertexBuffer; }
            set { _tileVertexBuffer = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>ShotManager</c> class.
        /// </summary>
        /// <param name="scene">Reference to the scene where the shots move.</param>
        /// <param name="player">Reference to the player.</param>
        /// <param name="enemies">Reference to the enemies</param>
        public ShotManager(SceneRenderer scene, Player player, Enemy[] enemies)
        {
            _shots = new List<Shot>();

            _scene = scene;
            _player = player;
            _enemies = enemies;
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load all the needed elements of the <c>ShotManager</c>.
        /// </summary>
        public void Load()
        {
            _tileVertexBuffer = new DynamicVertexBuffer(
                EngineManager.GameGraphicsDevice,
                VertexPositionNormalTexture.VertexDeclaration,
                6,
                BufferUsage.WriteOnly);
        }

        #endregion

        #region AddShot Method

        /// <summary>
        /// Add a new shot to the <c>ShotManager</c>.
        /// </summary>
        /// <param name="shot">Reference to the shot.</param>
        public void AddShot(Shot shot)
        {
            _shots.Add(shot);
        }

        #endregion

        #region RemoveShot Method

        /// <summary>
        /// Remove an existing shot in the <c>ShotManager</c>.
        /// </summary>
        /// <param name="shot">Reference to the shot.</param>
        public void RemoveShot(Shot shot)
        {
            if (_shots.Contains(shot))
                _shots.Remove(shot);
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the state of the shots in the <c>ShotManager</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public void Update(GameTime gameTime)
        {
            foreach (Shot shot in _shots)
                shot.Update(gameTime);

            // Check that shots are in the global scene
            List<Shot> aux = new List<Shot>();
            foreach (Shot shot in _shots)
                if (_scene.MainLayer.RootNode.BoundingBox.Contains(shot.ShotBBox) == ContainmentType.Contains)
                    aux.Add(shot);

            _shots = aux;
            
            // Check collisions with objects in the scene
            aux = new List<Shot>();
            foreach (Shot shot in _shots)
            {
                OcTreeNode node = _scene.MainLayer.SearchNode(shot.ShotBBox);
                if (node == null || node.ModelList[0].Key == NodeType.Ladder)
                    aux.Add(shot);
            }

            _shots = aux;

            // Check collisions with enemies
            aux = new List<Shot>();
            
            List<Shot> aux2 = new List<Shot>();
            foreach (Shot shot in _shots)
                aux2.Add(shot);

            foreach (Enemy enemy in _enemies)
            {
                if (enemy.Visible)
                {
                    foreach (Shot shot in aux2)
                    {
                        if (shot.ShotBBSphere.Intersects(enemy.DModel.BSphere))
                        {
                            if (!aux.Contains(shot))
                            {
                                aux.Add(shot);
                                EventManager.Trigger(new EventData_CharactersAttack(_player, enemy));
                            }
                        }
                    }
                }
            }

            _shots.Clear();
            foreach(Shot shot in aux2)
                if(!aux.Contains(shot))
                    _shots.Add(shot);
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current shots in the <c>ShotManager</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public void Draw(GameTime gameTime)
        {
            foreach (Shot shot in _shots)
                shot.Draw(gameTime);
        }

        #endregion
    }
}
