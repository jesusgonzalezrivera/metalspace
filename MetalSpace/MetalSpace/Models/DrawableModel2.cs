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
    class DrawableModel2
    {
        #region Properties

        private int _modelID;
        public int ModelID
        {
            get { return _modelID; }
            set { _modelID = value; }
        }

        private GameModel _model;
        public GameModel Model
        {
            get { return _model; }
            set { _model = value; }
        }

        private Matrix[] _modelTransforms;
        public Matrix[] ModelTransform
        {
            get { return _modelTransforms; }
            set { _modelTransforms = value; }
        }

        private Vector3 _position = Vector3.Zero;
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private Vector3 _rotation;
        public Vector3 Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        private Vector3 _scale;
        public Vector3 Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        private DynamicIndexBuffer _indexBuffer;
        private DynamicVertexBuffer _vertexBuffer;

        private BoundingBox _boundingBox;
        public BoundingBox BBox
        {
            get { return _boundingBox; }
            set { _boundingBox = value; }
        }

        private BoundingSphere _boundingSphere;
        public BoundingSphere BSphere
        {
            get { return _boundingSphere; }
            set { _boundingSphere = value; }
        }

        private Plane _boundingPlane;
        public Plane BPlane
        {
            get { return _boundingPlane; }
            set { _boundingPlane = value; }
        }

        #endregion

        #region Constructor

        public DrawableModel2() { }

        public DrawableModel2(GameModel model, Vector3 position,
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
            _boundingPlane = GetBoundingPlane();
        }

        #endregion

        #region Destructor

        ~DrawableModel2()
        {
            _model = null;
            _scale = Vector3.Zero;
            _position = Vector3.Zero;
            _rotation = Vector3.Zero;

            _vertexBuffer.Dispose();
            _indexBuffer.Dispose();
        }

        #endregion

        #region GetCenterModel Method

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

        public BoundingBox GetBoundingBox()
        {/*
            // Create variables to keep min and max xyz values for the model
            Vector3 modelMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            Vector3 modelMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

            foreach (ModelMesh mesh in _model.Model.Meshes)
            {
                //Create variables to hold min and max xyz values for the mesh
                Vector3 meshMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
                Vector3 meshMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

                // There may be multiple parts in a mesh (different materials etc.) so loop through each
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    // The stride is how big, in bytes, one vertex is in the vertex buffer
                    int stride = part.VertexBuffer.VertexDeclaration.VertexStride;

                    byte[] vertexData = new byte[stride * part.NumVertices];
                    part.VertexBuffer.GetData(part.VertexOffset * stride, vertexData, 0, part.NumVertices, 1); // fixed 13/4/11

                    // Find minimum and maximum xyz values for this mesh part
                    // We know the position will always be the first 3 float values of the vertex data
                    Vector3 vertPosition = new Vector3();
                    for (int ndx = 0; ndx < vertexData.Length; ndx += stride)
                    {
                        vertPosition.X = BitConverter.ToSingle(vertexData, ndx);
                        vertPosition.Y = BitConverter.ToSingle(vertexData, ndx + sizeof(float));
                        vertPosition.Z = BitConverter.ToSingle(vertexData, ndx + sizeof(float) * 2);

                        // update our running values from this vertex
                        meshMin = Vector3.Min(meshMin, vertPosition);
                        meshMax = Vector3.Max(meshMax, vertPosition);
                    }
                }

                // transform by mesh bone transforms
                meshMin = Vector3.Transform(meshMin, GetWorldMatrix());
                meshMax = Vector3.Transform(meshMax, GetWorldMatrix());

                // Expand model extents by the ones from this mesh
                modelMin = Vector3.Min(modelMin, meshMin);
                modelMax = Vector3.Max(modelMax, meshMax);
            }

            // Create and return the model bounding box
            return new BoundingBox(modelMin, modelMax);*/

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

        #region GetBoundingPlane Method

        public Plane GetBoundingPlane()
        {
            return new Plane(
                new Vector3(_boundingBox.Min.X, _boundingBox.Max.Y, _boundingBox.Min.Z),
                new Vector3(_boundingBox.Min.X, _boundingBox.Max.Y, _boundingBox.Max.Z),
                new Vector3(_boundingBox.Max.X, _boundingBox.Max.Y, _boundingBox.Max.Z));
        }

        #endregion

        #region GetWorldMatrix Method

        public Matrix GetWorldMatrix()
        {
            /*Matrix punto;
            Matrix.Invert(punto).Translation*/

            return Matrix.CreateScale(Scale) *
                Matrix.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z) *
                Matrix.CreateTranslation(Position);
        }

        #endregion

        #region Draw Methods

        public void Draw(Matrix view, Matrix projection, DrawingMethod method,
            Dictionary<string, Matrix> individualTransformations, Effect effect, AnimationPlayer player = null)
        {
            if (!_model.ReadyToRender)
                return;
            switch (method)
            {
                case DrawingMethod.NoInstancing:
                    DrawModelNoInstancing(view, projection, individualTransformations, effect, player);
                    break;

                case DrawingMethod.HardwareInstancing:
                    DrawModelHardwareInstancing(view, projection, individualTransformations, effect, player);
                    break;
            }
        }

        private void DrawModelNoInstancing(Matrix view, Matrix projection,
            Dictionary<string, Matrix> individualTransformations, Effect customEffect, AnimationPlayer player)
        {
            Matrix baseWorld = GetWorldMatrix();

            foreach (ModelMesh mesh in _model.Model.Meshes)
            {
                Matrix localWorld = ModelTransform[mesh.ParentBone.Index] * baseWorld;

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

                        effect.EnableDefaultLighting();
                        effect.CurrentTechnique.Passes[0].Apply();
                    }
                }

                mesh.Draw();
            }
        }

        private void DrawModelHardwareInstancing(Matrix view, Matrix projection,
            Dictionary<string, Matrix> individualTransformations, Effect customEffect, AnimationPlayer player)
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
                    else if (meshPart.Effect is BasicEffect)
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

                        effect.EnableDefaultLighting();
                        effect.CurrentTechnique.Passes[0].Apply();
                    }
                    else if (meshPart.Effect is SkinnedEffect)
                    {
                        SkinnedEffect effect = (SkinnedEffect)meshPart.Effect;

                        if (player != null)
                            effect.SetBoneTransforms(player.GetSkinTransforms());

                        if (individualTransformations.ContainsKey(mesh.Name))
                            effect.World = individualTransformations[mesh.Name] *
                                GetParentTransform(_model.Model, mesh.ParentBone) *
                                baseWorld;
                        else
                            effect.World = GetParentTransform(_model.Model, mesh.ParentBone) *
                                baseWorld;

                        effect.View = view;
                        effect.Projection = projection;

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

        private Matrix GetParentTransform(Model m, ModelBone mb)
        {
            return (mb == m.Root) ? mb.Transform :
                mb.Transform * GetParentTransform(m, mb.Parent);
        }

        #endregion
    }
}
