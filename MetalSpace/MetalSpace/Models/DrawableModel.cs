using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Managers;
using MetalSpace.Interfaces;
using MetalSpace.Events;
using SkinnedModel;

namespace MetalSpace.Models
{
    /// <summary>
    /// Type of drawing to use with the models.
    /// </summary>
    public enum DrawingMethod
    {
        /// <summary>
        /// No instancing (software instancing).
        /// </summary>
        NoInstancing,
        /// <summary>
        /// Hardware instancing.
        /// </summary>
        HardwareInstancing
    }

    /// <summary>
    /// The <c>DrawableModel</c> class represents a model that have all the
    /// needed elements for the drawing process.
    /// </summary>
    public class DrawableModel : IDrawableModel
    {
        #region Fields

        /// <summary>
        /// Store for the ModelID property.
        /// </summary>
        private int _modelID;

        /// <summary>
        /// Store for the Model property.
        /// </summary>
        private GameModel _model;

        /// <summary>
        /// Store for the ModelTransforms property.
        /// </summary>
        private Matrix[] _modelTransforms;

        /// <summary>
        /// Store for the Position property.
        /// </summary>
        private Vector3 _position = Vector3.Zero;

        /// <summary>
        /// Store for the Rotation property.
        /// </summary>
        private Vector3 _rotation;

        /// <summary>
        /// Store for the Scale property.
        /// </summary>
        private Vector3 _scale;

        /// <summary>
        /// Index buffer to identify the order of the vertices in the drawing process.
        /// </summary>
        private DynamicIndexBuffer _indexBuffer;

        /// <summary>
        /// Buffer that contains the vertices of the model in the drawing process.
        /// </summary>
        private DynamicVertexBuffer _vertexBuffer;

        /// <summary>
        /// Store for the BBox property.
        /// </summary>
        private BoundingBox _boundingBox;

        /// <summary>
        /// Store for the BSphere property.
        /// </summary>
        private BoundingSphere _boundingSphere;

        #endregion

        #region Properties

        /// <summary>
        /// ModelID property
        /// </summary>
        /// <value>
        /// Model identifier.
        /// </value>
        public int ModelID
        {
            get { return _modelID; }
            set { _modelID = value; }
        }

        /// <summary>
        /// Model property
        /// </summary>
        /// <value>
        /// Reference to the model in the <c>ModelManager</c>.
        /// </value>
        public GameModel Model
        {
            get { return _model; }
            set { _model = value; }
        }

        /// <summary>
        /// ModelTransform property
        /// </summary>
        /// <value>
        /// Transforms applied to the model (determined by the position, rotation and scale).
        /// </value>
        public Matrix[] ModelTransforms
        {
            get { return _modelTransforms; }
            set { _modelTransforms = value; }
        }

        /// <summary>
        /// Position property
        /// </summary>
        /// <value>
        /// Position of the model.
        /// </value>
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Rotation property
        /// </summary>
        /// <value>
        /// Rotation of the model.
        /// </value>
        public Vector3 Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        /// <summary>
        /// Scale property
        /// </summary>
        /// <value>
        /// Scale of the model.
        /// </value>
        public Vector3 Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        /// <summary>
        /// BBox property
        /// </summary>
        /// <value>
        /// Bounding box that wrap the model.
        /// </value>
        public BoundingBox BBox
        {
            get { return _boundingBox; }
            set { _boundingBox = value; }
        }

        /// <summary>
        /// BSphere property
        /// </summary>
        /// <value>
        /// Bounding sphere that wrap the model.
        /// </value>
        public BoundingSphere BSphere
        {
            get { return _boundingSphere; }
            set { _boundingSphere = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>DrawableModel</c> class.
        /// </summary>
        public DrawableModel() { }

        /// <summary>
        /// Constructor of the <c>DrawableModel</c> class.
        /// </summary>
        /// <param name="model">Reference to the model in the <c>ModelManager</c>.</param>
        /// <param name="position">Initial position of the model.</param>
        /// <param name="rotation">Initial rotation of the model.</param>
        /// <param name="scale">Initial scale of the model.</param>
        /// <param name="modelID">Identifier of the model.</param>
        public DrawableModel(GameModel model, Vector3 position, 
            Vector3 rotation, Vector3 scale, int modelID)
        {
            _modelID = modelID;

            _model = model;
            _modelTransforms = new Matrix[_model.Model.Bones.Count];
            _model.Model.CopyAbsoluteBoneTransformsTo(_modelTransforms);

            _scale = scale;
            _position = position;
            _rotation = rotation;

            _vertexBuffer = new DynamicVertexBuffer(EngineManager.GameGraphicsDevice,
                typeof(VertexPositionColor), 8, BufferUsage.WriteOnly);
            _indexBuffer = new DynamicIndexBuffer(EngineManager.GameGraphicsDevice,
                typeof(short), 24, BufferUsage.WriteOnly);
            
            _boundingBox = GetBoundingBox();
            _boundingSphere = GetBoundingSphere();
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load all the necessary content of the <c>DrawableModel</c>.
        /// </summary>
        public virtual void Load()
        {

        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload all the needed elements of the <c>DrawableModel</c>.
        /// </summary>
        public virtual void Unload()
        {
            _scale = Vector3.Zero;
            _rotation = Vector3.Zero;
            _position = Vector3.Zero;

            _vertexBuffer.Dispose();
            _vertexBuffer = null;

            _indexBuffer.Dispose();
            _indexBuffer = null;

            _modelTransforms = null;

            _model = null;
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destroy the current instance of the model.
        /// </summary>
        ~DrawableModel()
        {
            _model = null;
            _scale = Vector3.Zero;
            _position = Vector3.Zero;
            _rotation = Vector3.Zero;

            if(_vertexBuffer != null)
                _vertexBuffer.Dispose();

            if(_indexBuffer != null)
                _indexBuffer.Dispose();
        }

        #endregion

        #region GetCenterModel Method

        /// <summary>
        /// Get the center position of the model.
        /// </summary>
        /// <returns>Center position of the model.</returns>
        public Vector3 GetCenterModel()
        {
            Matrix[] boneTransforms = new Matrix[_model.Model.Bones.Count];
            _model.Model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            Vector3 modelCenter = Vector3.Zero;
            foreach (ModelMesh mesh in _model.Model.Meshes)
            {
                BoundingSphere meshBounds = mesh.BoundingSphere;
                Matrix transform = boneTransforms[mesh.ParentBone.Index];
                Vector3 meshCenter = Vector3.Transform(meshBounds.Center, transform);

                modelCenter += meshCenter;
            }

            modelCenter /= _model.Model.Meshes.Count;

            return modelCenter;
        }

        #endregion

        #region GetBoundingBox Method

        /// <summary>
        /// Calculate the bounding box that wrap the model.
        /// </summary>
        /// <returns>Bounding box that wrap the model.</returns>
        public BoundingBox GetBoundingBox()
        {
            // Initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            
            foreach (ModelMesh mesh in _model.Model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex data as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(
                            new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), 
                            GetWorldMatrix());

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }

            if (min.Y == max.Y)
                min.Y -= 0.1f;

            return new BoundingBox(min, max);
        }

        #endregion

        #region GetBoundingSphere Method

        /// <summary>
        /// Get the bounding sphere that wrap the model.
        /// </summary>
        /// <returns>Bounding sphere that wrap the model.</returns>
        public BoundingSphere GetBoundingSphere()
        {
            BoundingSphere boundingSphere = new BoundingSphere();

            foreach (ModelMesh mesh in _model.Model.Meshes)
            {
                if (boundingSphere.Radius == 0)
                    boundingSphere = mesh.BoundingSphere;
                else
                    boundingSphere = BoundingSphere.CreateMerged(
                        boundingSphere, mesh.BoundingSphere);
            }

            boundingSphere.Center += _position;

            return boundingSphere;
        }

        #endregion

        #region GetWorldMatrix Method

        /// <summary>
        /// Get the world matrix used by the model.
        /// </summary>
        /// <returns>World matrix used by the model.</returns>
        public Matrix GetWorldMatrix()
        {
            return Matrix.CreateScale(Scale) *
                Matrix.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z) *
                Matrix.CreateTranslation(Position);
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the current state of the model.
        /// </summary>
        /// <param name="time">Global time of the game.</param>
        public virtual void Update(GameTime time)
        {

        }

        #endregion

        #region Draw Methods

        /// <summary>
        /// Draw the current state of the model.
        /// </summary>
        /// <param name="view">View matrix of the active camera.</param>
        /// <param name="projection">Projection matrix of the active camera.</param>
        /// <param name="method">Method to draw the model (no instancing, hardware instancing).</param>
        /// <param name="individualTransformations">Individual transforms of the meshes.</param>
        /// <param name="customEffect">Custom effect to be applied to the model.</param>
        public virtual void Draw(Matrix view, Matrix projection, DrawingMethod method,
            Dictionary<string, Matrix> individualTransformations, Effect customEffect)
        {
            if (!_model.ReadyToRender)
                return;
            switch (method)
            {
                case DrawingMethod.NoInstancing:
                    DrawModelNoInstancing(view, projection, individualTransformations, customEffect);
                    break;

                case DrawingMethod.HardwareInstancing:
                    DrawModelHardwareInstancing(view, projection, individualTransformations, customEffect);
                    break;
            }
        }

        /// <summary>
        /// Draw the model using no instancing method
        /// </summary>
        /// <param name="view">View matrix of the active camera.</param>
        /// <param name="projection">Projection matrix of the active camera.</param>
        /// <param name="individualTransformations">Individual transforms of the meshes.</param>
        /// <param name="customEffect">Custom effect to be applied to the model.</param>
        private void DrawModelNoInstancing(Matrix view, Matrix projection,
            Dictionary<string, Matrix> individualTransformations, Effect customEffect)
        {
            Matrix baseWorld = GetWorldMatrix();
            
            foreach (ModelMesh mesh in _model.Model.Meshes)
            {
                Matrix localWorld = ModelTransforms[mesh.ParentBone.Index] * baseWorld;

                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    if (!(meshPart.Effect is BasicEffect))
                    {
                        IEffectMatrices iem = meshPart.Effect as IEffectMatrices;
                        if (customEffect != null)
                        {
                            customEffect.Parameters["xTexture"].SetValue(_model.Texture);
                            if (individualTransformations.ContainsKey(mesh.Name))
                                customEffect.Parameters["xWorld"].SetValue(
                                    individualTransformations[mesh.Name] *
                                    localWorld);
                            else
                                customEffect.Parameters["xWorld"].SetValue(
                                    localWorld);

                            customEffect.CurrentTechnique.Passes[0].Apply();
                        }
                        else
                        {
                            if (individualTransformations.ContainsKey(mesh.Name))
                                iem.World = individualTransformations[mesh.Name] *
                                            GetParentTransform(_model.Model, mesh.ParentBone) *
                                            baseWorld;
                            else
                                iem.World = GetParentTransform(_model.Model, mesh.ParentBone) *
                                            baseWorld;

                            iem.Projection = projection;
                            iem.View = view;
                        }
                    }
                    else
                    {
                        BasicEffect effect = (BasicEffect)meshPart.Effect;

                        if (individualTransformations.ContainsKey(mesh.Name))
                            effect.World = individualTransformations[mesh.Name] * localWorld;
                        else
                            effect.World = localWorld;

                        effect.View = view;
                        effect.Projection = projection;

                        effect.SpecularColor = Color.CadetBlue.ToVector3();
                        effect.SpecularPower = 64;

                        effect.EnableDefaultLighting();
                        effect.CurrentTechnique.Passes[0].Apply();
                    }
                }

                mesh.Draw();
            }
        }

        /// <summary>
        /// Draw the model using hardware instancing method.
        /// </summary>
        /// <param name="view">View matrix of the active camera.</param>
        /// <param name="projection">Projection matrix of the active camera.</param>
        /// <param name="individualTransformations">Individual transforms of the meshes.</param>
        /// <param name="customEffect">Custom effect to be applied to the model.</param>
        private void DrawModelHardwareInstancing(Matrix view, Matrix projection,
            Dictionary<string, Matrix> individualTransformations, Effect customEffect)
        {
            Matrix baseWorld = GetWorldMatrix();

            if (customEffect != null)
            {
                customEffect.CurrentTechnique = customEffect.Techniques["MultipleTargets"];
                customEffect.Parameters["xView"].SetValue(view);
                customEffect.Parameters["xProjection"].SetValue(projection);
            }

            foreach (ModelMesh mesh in _model.Model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    if (customEffect != null)
                    {
                        customEffect.Parameters["xTexture"].SetValue(_model.Texture);
                        if (individualTransformations.ContainsKey(mesh.Name))
                            customEffect.Parameters["xWorld"].SetValue(
                                individualTransformations[mesh.Name] *
                                GetParentTransform(_model.Model, mesh.ParentBone) *
                                baseWorld);
                        else
                            customEffect.Parameters["xWorld"].SetValue(
                                GetParentTransform(_model.Model, mesh.ParentBone) * baseWorld);

                        customEffect.CurrentTechnique.Passes[0].Apply();
                    }
                    else if(meshPart.Effect is BasicEffect)
                    {
                        BasicEffect effect = (BasicEffect)meshPart.Effect;

                        if (individualTransformations.ContainsKey(mesh.Name))
                            effect.World = individualTransformations[mesh.Name] *
                                GetParentTransform(_model.Model, mesh.ParentBone) *
                                baseWorld;
                        else
                            effect.World = GetParentTransform(_model.Model, mesh.ParentBone) *
                                baseWorld;

                        effect.View = view;
                        effect.Projection = projection;

                        effect.SpecularColor = Color.CadetBlue.ToVector3();
                        effect.SpecularPower = 64;

                        effect.EnableDefaultLighting();
                        effect.CurrentTechnique.Passes[0].Apply();
                    }
                    
                    EngineManager.GameGraphicsDevice.SetVertexBuffer(meshPart.VertexBuffer,
                                                                         meshPart.VertexOffset);
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

        /// <summary>
        /// Returns the transform of the parent bone.
        /// </summary>
        /// <param name="m">Reference to the model.</param>
        /// <param name="mb">Reference to the model bone.</param>
        /// <returns>Transform of the parent bone.</returns>
        protected Matrix GetParentTransform(Model m, ModelBone mb)
        {
            return (mb == m.Root) ? mb.Transform :
                mb.Transform * GetParentTransform(m, mb.Parent);
        }

        #endregion
    }
}
