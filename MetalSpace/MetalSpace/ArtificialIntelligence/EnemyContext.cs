using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MetalSpace.Objects;
using MetalSpace.Scene;
using MetalSpace.Interfaces;

namespace MetalSpace.ArtificialIntelligence
{
    /// <summary>
    /// The <c>EnemyContext</c> class represents the behaviour of an
    /// enemy, incluiding references to the current and last behaviour
    /// states.
    /// </summary>
    class EnemyContext
    {
        #region Fields

        /// <summary>
        /// Store for the Enemy property.
        /// </summary>
        private Enemy _enemy;

        /// <summary>
        /// Store for the Player property.
        /// </summary>
        private Player _player;

        /// <summary>
        /// Store for the MainScene property.
        /// </summary>
        private OcTree _mainScene;
        
        /// <summary>
        /// Store for the LastState property.
        /// </summary>
        private IEnemyState _lastState;

        /// <summary>
        /// Store for the CurrentState property.
        /// </summary>
        private IEnemyState _currentState;

        #endregion

        #region Properties

        /// <summary>
        /// CurrentState property
        /// </summary>
        /// <value>
        /// Return the current state of the enemy.
        /// </value>
        public IEnemyState CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }

        /// <summary>
        /// LastState property
        /// </summary>
        /// <value>
        /// Return the last state of the enemy.
        /// </value>
        public IEnemyState LastState
        {
            get { return _lastState; }
            set { _lastState = value; }
        }

        /// <summary>
        /// MainScene property
        /// </summary>
        /// <value>
        /// Reference to the <c>OcTree</c> where the enemy moves.
        /// </value>
        public OcTree MainScene
        {
            get { return _mainScene; }
            set { _mainScene = value; }
        }

        /// <summary>
        /// Player property
        /// </summary>
        /// <value>
        /// Reference to the <c>Player</c> to attack.
        /// </value>
        public Player Player
        {
            get { return _player; }
            set { _player = value; }
        }

        /// <summary>
        /// Enemy property
        /// </summary>
        /// <value>
        /// Reference to the <c>Enemy</c> of this context.
        /// </value>
        public Enemy Enemy
        {
            get { return _enemy; }
            set { _enemy = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>EnemyContext</c> class.
        /// </summary>
        /// <param name="mainScene">Reference to the OcTree where the enemy moves</param>
        /// <param name="player">Reference to the main player.</param>
        /// <param name="enemy">Reference to the current enemy.</param>
        public EnemyContext(OcTree mainScene, Player player, Enemy enemy)
        {
            _mainScene = mainScene;
            _player = player;
            _enemy = enemy;
        }

        #endregion

        #region ChangeState Method

        /// <summary>
        /// Change the current state of the enemy, saving the last state before
        /// change the current state.
        /// </summary>
        /// <param name="newState">New state of the enemy.</param>
        public void ChangeState(IEnemyState newState)
        {
            if (_currentState != newState)
            {
                _lastState = _currentState;
                _currentState = newState;
                _currentState.StartAnimation(this);
            }
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Handle the behaviour of the current state.
        /// </summary>
        /// <param name="gameTime">Elapsed time between this and the last
        /// execution of the Update method.</param>
        public void Update(GameTime gameTime)
        {
            _currentState.Handle(this);
        }

        #endregion
    }
}
