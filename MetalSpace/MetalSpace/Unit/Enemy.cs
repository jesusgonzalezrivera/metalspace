using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Models;
using MetalSpace.Managers;

namespace MetalSpace.Unit
{
    class Enemy
    {
        #region Fields

        private int _life;
        private int _maxLife;

        public Vector2 Speed;
        public Vector2 MaxSpeed;

        public BoundingSphere _boundingSphere;
        private AnimatedModel _model;

        #endregion

        #region Constructor

        public Enemy()
        {
            _life = 100;
            _maxLife = 100;
        }

        public Enemy(string modelName, Vector3 position, Vector3 rotation, Vector3 scale,
            int life, int maxLife, Vector2 speed)
        {
            _life = life;
            _maxLife = maxLife;

            Speed = speed;

            //_model = new AnimatedModel(new DrawableModel((GameModel)ModelManager.GetModel(modelName), position, rotation, scale, 0));
            _model = new AnimatedModel((GameModel)ModelManager.GetModel(modelName), position, rotation, scale, 0);

            _model.TimeSpeed = 0.25f;
            _model.Animation.StartClip("Waiting", true);
        }

        #endregion

        #region Load Method

        public void Load()
        {

        }

        #endregion

        #region Unload Method

        public void Unload()
        {
            Speed = Vector2.Zero;
            MaxSpeed = Vector2.Zero;

            _model = null;
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            _model.Update(gameTime);
        }

        #endregion

        #region Draw

        public void Draw(GameTime gametime)
        {
            //_model.Draw(CameraManager.ActiveCamera.View, CameraManager.ActiveCamera.Projection);
        }

        #endregion
    }
}
