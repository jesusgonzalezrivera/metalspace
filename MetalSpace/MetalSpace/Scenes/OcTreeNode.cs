using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Models;
using MetalSpace.Managers;
using MetalSpace.Events;
using MetalSpace.Events;

namespace MetalSpace.Scene
{
    /// <summary>
    /// Type of the model contained in the OcTreeNode.
    /// </summary>
    public enum NodeType
    {
        /// <summary>
        /// Normal type.
        /// </summary>
        None,
        /// <summary>
        /// Staircase down.
        /// </summary>
        Staircase1Down,
        /// <summary>
        /// Staircase down.
        /// </summary>
        Staircase2Down,
        /// <summary>
        /// Staircase up.
        /// </summary>
        Staircase1Up,
        /// <summary>
        /// Staircase up.
        /// </summary>
        Staircase2Up,
        /// <summary>
        /// Ladder.
        /// </summary>
        Ladder,
        /// <summary>
        /// Door.
        /// </summary>
        Door
    }

    /// <summary>
    /// The <c>OcTreeNode</c> class represent an individual node that conform
    /// the <c>OcTree</c> structure.
    /// </summary>
    public class OcTreeNode
    {
        #region Fields

        /// <summary>
        /// Store for the Parent property.
        /// </summary>
        private OcTreeNode _parent;

        /// <summary>
        /// Store for the Center property.
        /// </summary>
        private Vector3 _center;

        /// <summary>
        /// Store for the Size property.
        /// </summary>
        private float _size;

        /// <summary>
        /// Store for the BoundingPlane property.
        /// </summary>
        private Plane _nodeBoundingPlane;

        /// <summary>
        /// Store for the BoundingBox property.
        /// </summary>
        private BoundingBox _nodeBoundingBox;

        /// <summary>
        /// Store for the ModelList property.
        /// </summary>
        private List<KeyValuePair<NodeType, DrawableModel>> _modelList;

        /// <summary>
        /// Store for the ChildList property.
        /// </summary>
        private List<OcTreeNode> _childList;

        /// <summary>
        /// Store for the CubeLineVertices property.
        /// </summary>
        private VertexPositionColor[] _cubeLineVertices;

        /// <summary>
        /// Store for the CubeLineIndices property.
        /// </summary>
        private short[] _cubeLineIndices = { 
            0, 1, 1, 2, 2, 3, 3, 0, 4, 5, 5, 6, 
            6, 7, 7, 4, 0, 4, 1, 5, 2, 6, 3, 7 };

        #endregion

        #region Properties

        /// <summary>
        /// Parent property
        /// </summary>
        /// <value>
        /// Reference to the node that contains the current node (null if it is the root node).
        /// </value>
        public OcTreeNode Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// Center property
        /// </summary>
        /// <value>
        /// Center position of the node.
        /// </value>
        public Vector3 Center
        {
            get { return _center; }
            set { _center = value; }
        }

        /// <summary>
        /// Size property
        /// </summary>
        /// <value>
        /// Size of the node.
        /// </value>
        public float Size
        {
            get { return _size; }
            set { _size = value; }
        }

        /// <summary>
        /// BoundingBox property
        /// </summary>
        /// <value>
        /// Bounding box that wrap the node.
        /// </value>
        public BoundingBox BoundingBox
        {
            get { return _nodeBoundingBox; }
            set { _nodeBoundingBox = value; }
        }

        /// <summary>
        /// BoundingPlane property
        /// </summary>
        /// <value>
        /// Bounding plane that represent a staircase.
        /// </value>
        public Plane BoundingPlane
        {
            get { return _nodeBoundingPlane; }
            set { _nodeBoundingPlane = value; }
        }

        /// <summary>
        /// List of model contained in the node (empty if the node contains other nodes).
        /// </summary>
        public List<KeyValuePair<NodeType, DrawableModel>> ModelList
        {
            get { return _modelList; }
            set { _modelList = value; }
        }

        /// <summary>
        /// List of nodes contained in the node (empty if the node contains models).
        /// </summary>
        public List<OcTreeNode> ChildList
        {
            get { return _childList; }
            set { _childList = value; }
        }

        /// <summary>
        /// Vertices that represents the bounding box.
        /// </summary>
        public VertexPositionColor[] CubeLineVertices
        {
            get { return _cubeLineVertices; }
            set { _cubeLineVertices = value; }
        }

        /// <summary>
        /// Indices that indicates the order for drawing the vertices.
        /// </summary>
        public short[] CubeLineIndices
        {
            get { return _cubeLineIndices; }
            set { _cubeLineIndices = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>OcTreeNode</c> class.
        /// </summary>
        /// <param name="parent">Node that contains the current node.</param>
        /// <param name="center">Center of the node.</param>
        /// <param name="size">Size of the node.</param>
        public OcTreeNode(OcTreeNode parent, Vector3 center, float size)
        {
            _parent = parent;

            _center = center;
            _size = size;

            CalcBoundingBox();
            CalcVertex();

            _modelList = new List<KeyValuePair<NodeType, DrawableModel>>();
            _childList = new List<OcTreeNode>(8);   
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load the needed content of the <c>OcTreeNode</c>.
        /// </summary>
        public void Load()
        {

        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload the content used in the <c>OcTreeNode</c>.
        /// </summary>
        public void Unload()
        {
            if(_modelList != null)
                _modelList.Clear();
        }

        #endregion

        #region CalcBoundingBox Method

        /// <summary>
        /// Calculate the bounding box that wrap the <c>OcTreeNode</c>.
        /// </summary>
        public void CalcBoundingBox()
        {
            Vector3 diagonalVector = new Vector3(_size / 2.0f, _size / 2.0f, _size / 2.0f);
            _nodeBoundingBox = new BoundingBox(_center - diagonalVector, _center + diagonalVector);
        }

        #endregion

        #region CalcBoundingPlane Method

        /// <summary>
        /// Calculate the bounding plane that represents the surface of the <c>OcTreeNode</c>.
        /// </summary>
        public void CalcBoundingPlane()
        {
            Vector3 diagonalVector = new Vector3(_size / 2.0f, _size / 2.0f, _size / 2.0f);
            Vector3 Min = _center - diagonalVector;
            Vector3 Max = _center + diagonalVector;

            switch(_modelList[0].Key)
            {
                case NodeType.None:
                    _nodeBoundingPlane = new Plane(
                        new Vector3(Min.X, Max.Y, Min.Z),
                        new Vector3(Min.X, Max.Y, Max.Z),
                        new Vector3(Max.X, Max.Y, Max.Z));
                    break;
                case NodeType.Staircase1Down:
                    _nodeBoundingPlane = new Plane(
                        new Vector3(Min.X, Max.Y, Min.Z),
                        new Vector3(Min.X, Max.Y, Max.Z),
                        new Vector3(Max.X, Max.Y - (1f / 2f), Max.Z));
                    break;

                case NodeType.Staircase1Up:
                    _nodeBoundingPlane = new Plane(
                        new Vector3(Min.X, Max.Y - (1f / 2f), Min.Z),
                        new Vector3(Min.X, Max.Y - (1f / 2f), Max.Z),
                        new Vector3(Max.X, Max.Y, Max.Z));
                    break;

                case NodeType.Staircase2Down:
                    Max.Y = Max.Y - (1f / 2f);
                    _nodeBoundingPlane = new Plane(
                        new Vector3(Min.X, Max.Y, Min.Z),
                        new Vector3(Min.X, Max.Y, Max.Z),
                        new Vector3(Max.X, Max.Y - (1f / 2f), Max.Z));
                    break;

                case NodeType.Staircase2Up:
                    Max.Y = Max.Y - (1f / 2f);
                    _nodeBoundingPlane = new Plane(
                        new Vector3(Min.X, Max.Y - (1f / 2f), Min.Z),
                        new Vector3(Min.X, Max.Y - (1f / 2f), Max.Z),
                        new Vector3(Max.X, Max.Y, Max.Z));
                    break;
            }

            if (_modelList[0].Key == NodeType.Staircase1Down || _modelList[0].Key == NodeType.Staircase2Down ||
                _modelList[0].Key == NodeType.Staircase1Up || _modelList[0].Key == NodeType.Staircase2Up)
            {
                diagonalVector += new Vector3(0.1f, 0.1f, 0.1f);
                _nodeBoundingBox = new BoundingBox(_center - diagonalVector, _center + diagonalVector);
            }
        }

        #endregion

        #region CalcVertex Method

        /// <summary>
        /// Calculate the vertex that represent the bounding box of the <c>OcTreeNode</c>.
        /// </summary>
        public void CalcVertex()
        {
            Vector3[] corners = _nodeBoundingBox.GetCorners();

            _cubeLineVertices = new VertexPositionColor[8];
            for (int i = 0; i < corners.Length; i++)
                _cubeLineVertices[i] = new VertexPositionColor(corners[i], Color.White);
        }

        #endregion

        #region CreateChildNodes Method

        /// <summary>
        /// Create the list of childs (8 in the OcTree structure).
        /// </summary>
        private void CreateChildNodes()
        {
            float sizeOver2 = _size / 2.0f;
            float sizeOver4 = _size / 4.0f;

            _childList.Add(new OcTreeNode(this, _center + new Vector3(sizeOver4, sizeOver4, -sizeOver4), sizeOver2));
            _childList.Add(new OcTreeNode(this, _center + new Vector3(-sizeOver4, sizeOver4, -sizeOver4), sizeOver2));
            _childList.Add(new OcTreeNode(this, _center + new Vector3(sizeOver4, sizeOver4, sizeOver4), sizeOver2));
            _childList.Add(new OcTreeNode(this, _center + new Vector3(-sizeOver4, sizeOver4, sizeOver4), sizeOver2));
            _childList.Add(new OcTreeNode(this, _center + new Vector3(sizeOver4, -sizeOver4, -sizeOver4), sizeOver2));
            _childList.Add(new OcTreeNode(this, _center + new Vector3(-sizeOver4, -sizeOver4, -sizeOver4), sizeOver2));
            _childList.Add(new OcTreeNode(this, _center + new Vector3(sizeOver4, -sizeOver4, sizeOver4), sizeOver2));
            _childList.Add(new OcTreeNode(this, _center + new Vector3(-sizeOver4, -sizeOver4, sizeOver4), sizeOver2));
        }

        #endregion

        #region Distribute Method

        /// <summary>
        /// Distribute the models between the child nodes.
        /// </summary>
        /// <param name="type">Type of the node.</param>
        /// <param name="model">Model to be distributed.</param>
        private void Distribute(NodeType type, DrawableModel model)
        {
            Vector3 position = model.Position;
            if (position.Y > _center.Y)         // UP
            {
                if (position.Z < _center.Z)     // FORWARD
                {
                    if (position.X < _center.X) // LEFT
                        _childList[1].AddModel(type, model);
                    else                        // RIGHT
                        _childList[0].AddModel(type, model);
                }
                else                            // BACKWARD
                {
                    if (position.X < _center.X) // LEFT
                        _childList[3].AddModel(type, model);
                    else                        // RIGHT
                        _childList[2].AddModel(type, model);
                }
            }
            else                                // DOWN
            {
                if (position.Z < _center.Z)     // FORWARD
                {
                    if (position.X < _center.X) // LEFT
                        _childList[5].AddModel(type, model);
                    else                        // RIGHT
                        _childList[4].AddModel(type, model);
                }
                else                            // BACKWARD
                {
                    if (position.X < _center.X) // LEFT
                        _childList[7].AddModel(type, model);
                    else                        // RIGHT
                        _childList[6].AddModel(type, model);
                }
            }
        }

        #endregion

        #region AddModel Methods

        /// <summary>
        /// Add a new model to the list of models contained in the <c>OcTreeNode</c>.
        /// </summary>
        /// <param name="type">Type of the node.</param>
        /// <param name="model">Reference to the model.</param>
        /// <returns></returns>
        public int AddModel(NodeType type, DrawableModel model)
        {
            if (_childList.Count == 0)
            {
                _modelList.Add(new KeyValuePair<NodeType, DrawableModel>(type, model));
                if (_size == 1f && _modelList.Count != 0)// && _modelList[0].Key != NodeType.None)
                    CalcBoundingPlane();
                
                bool maxObjectsReached = (_modelList.Count > OcTree.MaxObjectsInNode);
                bool minSizeNotReached = (_size > OcTree.MinSize);
                if (maxObjectsReached && minSizeNotReached)
                {
                    CreateChildNodes();
                    foreach (KeyValuePair<NodeType, DrawableModel> currentModel in _modelList)
                        Distribute(currentModel.Key, currentModel.Value);
                    _modelList.Clear();
                }
            }
            else
            {
                Distribute(type, model);
            }

            return model.ModelID;
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the <c>OcTreeNode</c>.
        /// </summary>
        /// <param name="viewMatrix">View matrix of the camera.</param>
        /// <param name="projectionMatrix">Projection matrix of the camera.</param>
        /// <param name="effect">Effect to be applied to the models.</param>
        public void Draw(Matrix viewMatrix, Matrix projectionMatrix, Effect effect)
        {
            if (_modelList.Count != 0)
            {
                Dictionary<string, Matrix> dictionary = new Dictionary<string, Matrix>();
                foreach (KeyValuePair<NodeType, DrawableModel> model in _modelList)
                {
                    model.Value.Draw(viewMatrix, projectionMatrix, DrawingMethod.HardwareInstancing, dictionary, effect);
                    OcTree.ModelsDrawn++;
                }
            }
        }

        #endregion

        #region DrawBoundingBox Method

        /// <summary>
        /// Draw the bounding box that represent the <c>OcTreeNode</c>.
        /// </summary>
        /// <param name="worldMatrix">World matrix.</param>
        /// <param name="view">View matrix of the camera.</param>
        /// <param name="projection">Projection matrix of the camera.</param>
        /// <param name="effect">Effect to be applied to the bounding box.</param>
        public void DrawBoundingBox(Matrix worldMatrix, Matrix view, Matrix projection, BasicEffect effect)
        {
            OcTree.CubeVertexBuffer.SetData(_cubeLineVertices, 0, 8, SetDataOptions.Discard);
            OcTree.CubeIndexBuffer.SetData(_cubeLineIndices, 0, 24, SetDataOptions.Discard);

            EngineManager.GameGraphicsDevice.SetVertexBuffer(OcTree.CubeVertexBuffer);
            EngineManager.GameGraphicsDevice.Indices = OcTree.CubeIndexBuffer;

            effect.World = worldMatrix;
            effect.View = view;
            effect.Projection = projection;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                EngineManager.GameGraphicsDevice.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, 8, 0, 12);
            }

            EngineManager.GameGraphicsDevice.SetVertexBuffer(null);
            EngineManager.GameGraphicsDevice.Indices = null;
        }

        #endregion
    }
}
