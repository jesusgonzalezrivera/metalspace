using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MetalSpace.Unit;
using MetalSpace.Scene;
using MetalSpace.Models;
using MetalSpace.Interfaces;

namespace MetalSpace.Unit
{
    public class Character : MoveableObject
    {
        #region Fields

        private int _life;
        private int _maxLife;

        private int _attack;

        #endregion

        #region Properties

        public int Life
        {
            get { return _life; }
            set { _life = value; }
        }

        public int MaxLife
        {
            get { return _maxLife; }
            set { _maxLife = value; }
        }

        public int Attack
        {
            get { return _attack; }
            set { _attack = value; }
        }

        #endregion

        #region Constructor

        public Character(OcTree octree, IDrawableModel model, 
            Vector2 maxSpeed, int maxLife, int attack)
            : base(octree, model, maxSpeed, true, true)
        {
            _life = maxLife;
            _maxLife = maxLife;

            _attack = attack;
        }

        #endregion

        #region Load Method

        public override void Load()
        {
            base.Load();
        }

        #endregion

        #region Unload Method

        public override void Unload()
        {
            base.Unload();
        }

        #endregion

        #region Update Method

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        #endregion

        #region Draw Method

        public override void Draw(GameTime time)
        {
            base.Draw(time);
        }

        #endregion
    }
}
