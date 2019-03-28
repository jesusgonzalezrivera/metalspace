using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Managers;
using MetalSpace.Events;
using SkinnedModel;

namespace MetalSpace.Models
{
    class AnimatedModel2
    {
        #region Fields

        private DrawableModel2 _model;
        private AnimationPlayer _animation;
        private SkinningData _modelSkinningData;

        #endregion

        #region Properties

        public DrawableModel2 Model
        {
            get { return _model; }
            set { _model = value; }
        }

        public SkinningData ModelSkinningData
        {
            get { return _modelSkinningData; }
            set { _modelSkinningData = value; }
        }

        public AnimationPlayer Animation
        {
            get { return _animation; }
            set { _animation = value; }
        }

        #endregion

        #region Constructor

        public AnimatedModel2(DrawableModel2 model)
        {
            _model = model;

            _modelSkinningData = _model.Model.Model.Tag as SkinningData;
            _animation = new AnimationPlayer(_modelSkinningData);

            //SetNewEffect();
        }

        #endregion

        #region SetNewEffect Method

        private void SetNewEffect()
        {
            foreach (ModelMesh mesh in _model.Model.Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    SkinnedEffect newEffect = new SkinnedEffect(EngineManager.GameGraphicsDevice);
                    BasicEffect oldEffect = ((BasicEffect)part.Effect);

                    newEffect.EnableDefaultLighting();

                    newEffect.SpecularColor = Color.Black.ToVector3();
                    newEffect.AmbientLightColor = oldEffect.AmbientLightColor;
                    newEffect.DiffuseColor = oldEffect.DiffuseColor;
                    newEffect.Texture = oldEffect.Texture;

                    part.Effect = newEffect;
                }
            }
        }

        #endregion

        #region Update Method

        private float _timeSpeed;
        public float TimeSpeed
        {
            get { return _timeSpeed; }
            set { _timeSpeed = value; }
        }

        public void Update(GameTime gameTime)
        {
            TimeSpan elapsedTime = TimeSpan.FromSeconds(gameTime.ElapsedGameTime.TotalSeconds * _timeSpeed);
            _animation.Update(elapsedTime, true, Matrix.Identity);
        }

        #endregion

        #region Draw Method

        public void Draw(Matrix view, Matrix projection)
        {
            /*foreach (ModelMesh mesh in _model.Model.Model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    
                    //effect.DirectionalLight0.DiffuseColor = new Vector3(0.5f, 0, 0); // a red light
                    //effect.DirectionalLight0.Direction = new Vector3(1, 0, 0);  // coming along the x-axis
                    //effect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 0); // with green highlights

                    effect.World = Matrix.Identity;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }*/

            _model.Draw(view, projection, DrawingMethod.HardwareInstancing, new Dictionary<string, Matrix>(), null, _animation);
        }

        #endregion
    }
}
