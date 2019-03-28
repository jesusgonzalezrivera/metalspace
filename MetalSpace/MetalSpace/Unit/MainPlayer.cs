using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Events;
using MetalSpace.Managers;
using MetalSpace.Models;
using MetalSpace.Scene;

namespace MetalSpace.Unit
{
    class MainPlayer : GameUnit
    {
        #region Properties

        #endregion

        #region Constructor

        public MainPlayer(string modelName, Vector3 position, Vector3 rotation,
                          Vector3 scale, int life, int maxLife, Vector2 velocity)
            : base(modelName, position, rotation, scale, life, maxLife, velocity)
        {
            Speed = Vector2.Zero;
            MaxSpeed = new Vector2(5f, 5f);

            _jump = false;
            _endJump = true;

            _lastPlayerState = State.Waiting;
            _playerState = State.Waiting;
            _lastPlayerXDirection = XDirection.Right;
            _playerXDirection = XDirection.Right;
            _lastPlayerYDirection = YDirection.None;
            _playerYDirection = YDirection.None;
            _lastPlayerAction = Action.None;
            _playerAction = Action.None;

            Model.Animation.StartClip("Waiting", true);
        }

        #endregion

        #region Load Method

        public void Load()
        {
            base.Load();
        }

        #endregion

        #region Unload Method

        public void Unload()
        {
            base.Unload();
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Model.Update(gameTime);
        }

        #endregion

        #region Draw

        public void Draw(GameTime gameTime, Effect customEffect = null)
        {
            //Model.Draw(CameraManager.ActiveCamera.View, CameraManager.ActiveCamera.Projection);
            //Model.Draw(CameraManager.ActiveCamera.View, CameraManager.ActiveCamera.Projection, DrawingMethod.HardwareInstancing,
            //    new Dictionary<string, Matrix>(), customEffect);
        }

        #endregion
    }
}
