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
    /// <summary>
    /// The <c>AnimatedModel</c> class represents a model that contain a
    /// skeleton and different animations, so the user can draw the needed
    /// animation in each time with the necessary speed.
    /// </summary>
    public class AnimatedModel : DrawableModel
    {
        #region Fields

        /// <summary>
        /// Store for the ModelSkinningData property.
        /// </summary>
        private SkinningData _modelSkinningData;

        /// <summary>
        /// Store for the MeshesToDraw property.
        /// </summary>
        private List<string> _meshesToDraw;

        /// <summary>
        /// Store for the Animation property.
        /// </summary>
        private AnimationPlayer _animation;

        /// <summary>
        /// Store for the TimeSpeed property.
        /// </summary>
        private float _timeSpeed;

        /// <summary>
        /// Store for the Damaged propert.
        /// </summary>
        private bool _damaged = false;

        /// <summary>
        /// Color to use when the unit that contains this model is damaged.
        /// </summary>
        private Vector3 _damageColor;

        /// <summary>
        /// Default natural diffuse color to use when the unit that contains this model is not damaged.
        /// </summary>
        private Vector3 _naturalDiffuseColor;

        /// <summary>
        /// Default natural ambient color to use when the unit that contains this model is not damaged.
        /// </summary>
        private Vector3 _naturalAmbientColor;
        
        #endregion

        #region Properties

        /// <summary>
        /// ModelSkinningData property
        /// </summary>
        /// <value>
        /// Structure that contains the information for the animations.
        /// </value>
        public SkinningData ModelSkinningData
        {
            get { return _modelSkinningData; }
            set { _modelSkinningData = value; }
        }

        /// <summary>
        /// Animation property
        /// </summary>
        /// <value>
        /// Player for the animations.
        /// </value>
        public AnimationPlayer Animation
        {
            get { return _animation; }
            set { _animation = value; }
        }
        
        /// <summary>
        /// TimeSpeed property
        /// </summary>
        /// <value>
        /// Speed for the current animation.
        /// </value>
        public float TimeSpeed
        {
            get { return _timeSpeed; }
            set { _timeSpeed = value; }
        }

        /// <summary>
        /// Damaged property
        /// </summary>
        /// <value>
        /// true if the unit that use this model is damaged, false otherwise.
        /// </value>
        public bool Damaged
        {
            get { return _damaged; }
            set { _damaged = value; }
        }

        /// <summary>
        /// MeshesToDraw property
        /// </summary>
        /// <value>
        /// List of meshes to draw (it is not necessary to draw all the armatures...).
        /// </value>
        public List<string> MeshesToDraw
        {
            get { return _meshesToDraw; }
            set { _meshesToDraw = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>AnimatedModel</c> class.
        /// </summary>
        /// <param name="model">Reference to the model.</param>
        /// <param name="position">Initial position of the model.</param>
        /// <param name="rotation">Initial rotation of the model.</param>
        /// <param name="scale">Initial scale of the model.</param>
        /// <param name="modelID">Id to identify the model.</param>
        /// <param name="meshesToDraw">List of meshes to draw.</param>
        public AnimatedModel(GameModel model, Vector3 position, 
            Vector3 rotation, Vector3 scale, int modelID, bool meshesToDraw = false)
            : base(model, position, rotation, scale, modelID)
        {
            _modelSkinningData = Model.Model.Tag as SkinningData;
            _animation = new AnimationPlayer(_modelSkinningData);

            SkinnedEffect effect = (SkinnedEffect)base.Model.Model.Meshes[0].MeshParts[0].Effect;

            _damageColor = Color.Red.ToVector3();
            _naturalDiffuseColor = effect.DiffuseColor;
            _naturalAmbientColor = effect.AmbientLightColor;

            if (meshesToDraw)
            {
                _meshesToDraw = new List<string>();
                _meshesToDraw.Add("Model");
            }
            else
                _meshesToDraw = null;
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load the necessary content of the <c>AnimatedModel</c>.
        /// </summary>
        public override void Load()
        {
            base.Load();
        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload the necessary content of the <c>AnimatedModel</c>.
        /// </summary>
        public override void Unload()
        {
            base.Unload();
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the current state of the <c>AnimatedModel</c>.
        /// </summary>
        /// <param name="time">Global time of the game.</param>
        public override void  Update(GameTime time)
        {
 	        base.Update(time);
            
            TimeSpan elapsedTime = TimeSpan.FromSeconds(
                time.ElapsedGameTime.TotalSeconds * _timeSpeed);
            _animation.Update(elapsedTime, true, Matrix.Identity);
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the <c>AnimatedModel</c>.
        /// </summary>
        /// <param name="view">View matrix of the active camera.</param>
        /// <param name="projection">Projection matrix of the active camera.</param>
        /// <param name="method">Method to draw the model (no instancing, hardware instancing).</param>
        /// <param name="individualTransformations">Individual transforms of the meshes.</param>
        /// <param name="customEffect">Custom effect to be applied to the model.</param>
        public override void Draw(Matrix view, Matrix projection, DrawingMethod method,
            Dictionary<string, Matrix> individualTransformations, Effect customEffect)
        {
            Matrix baseWorld = GetWorldMatrix();

            if (customEffect != null)
            {
                customEffect.CurrentTechnique = customEffect.Techniques["MultipleTargets"];
                customEffect.Parameters["xView"].SetValue(view);
                customEffect.Parameters["xProjection"].SetValue(projection);
            }
            
            foreach (ModelMesh mesh in base.Model.Model.Meshes)
            {
                if (_meshesToDraw != null && !_meshesToDraw.Contains(mesh.Name))
                    continue;

                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    if (customEffect != null)
                    {
                        customEffect.Parameters["xTexture"].SetValue(Model.Texture);
                        if (individualTransformations.ContainsKey(mesh.Name))
                            customEffect.Parameters["xWorld"].SetValue(
                                individualTransformations[mesh.Name] *
                                GetParentTransform(Model.Model, mesh.ParentBone) *
                                baseWorld);
                        else
                            customEffect.Parameters["xWorld"].SetValue(
                                GetParentTransform(Model.Model, mesh.ParentBone) * baseWorld);

                        customEffect.CurrentTechnique.Passes[0].Apply();
                    }
                    else if (meshPart.Effect is SkinnedEffect)
                    {
                        SkinnedEffect effect = (SkinnedEffect)meshPart.Effect;
                        
                        if (_animation != null)
                            effect.SetBoneTransforms(_animation.SkinTransforms);

                        if (individualTransformations.ContainsKey(mesh.Name))
                            effect.World = individualTransformations[mesh.Name] *
                                GetParentTransform(Model.Model, mesh.ParentBone) *
                                baseWorld;
                        else
                            effect.World = GetParentTransform(Model.Model, mesh.ParentBone) *
                                baseWorld;

                        effect.View = view;
                        effect.Projection = projection;

                        if (_damaged == true)
                        {
                            effect.DiffuseColor = _damageColor;
                            effect.AmbientLightColor = _damageColor;
                        }
                        else
                        {
                            effect.DiffuseColor = _naturalDiffuseColor;
                            effect.AmbientLightColor = _naturalAmbientColor;
                        }

                        effect.SpecularColor = Color.CadetBlue.ToVector3();
                        effect.SpecularPower = 256;

                        effect.EnableDefaultLighting();
                        effect.CurrentTechnique.Passes[0].Apply();
                    }

                    EngineManager.GameGraphicsDevice.SetVertexBuffer(meshPart.VertexBuffer, meshPart.VertexOffset);
                    EngineManager.GameGraphicsDevice.Indices = meshPart.IndexBuffer;

                    EngineManager.GameGraphicsDevice.DrawIndexedPrimitives(
                        PrimitiveType.TriangleList,
                        0, 0,
                        meshPart.NumVertices,
                        meshPart.StartIndex,
                        meshPart.PrimitiveCount);
                }
            }
        }

        #endregion
    }
}
