using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Managers;
using MetalSpace.Models;
using MetalSpace.Events;

namespace MetalSpace.Scene
{
    public struct Door
    {
        public string nextLevel;
        public Vector3 nextPosition;
        public BoundingBox boundingBox;
        public string neededObject;
    }

    /// <summary>
    /// The <c>OcTree</c> class represents an octree structure that permits
    /// to check collisions and not draw the models that not appears in the
    /// bounding frustrum of the camera.
    /// </summary>
    public class OcTree
    {
        #region Constants

        /// <summary>
        /// Minimun size of an node.
        /// </summary>
        public const float MinSize = 1f;

        /// <summary>
        /// Maximum number of objects in each node.
        /// </summary>
        public const int   MaxObjectsInNode = 1;

        /// <summary>
        /// Minimum X size of a node.
        /// </summary>
        private const float xSize = 1.0f;

        /// <summary>
        /// Minimum Y size of a node.
        /// </summary>
        private const float ySize = 1.0f;

        /// <summary>
        /// Minimum Z size of a node.
        /// </summary>
        private const float zSize = 1.0f;

        #endregion

        #region Fields

        /// <summary>
        /// Name of the OcTree.
        /// </summary>
        private string _name;

        /// <summary>
        /// Store for the RootNode property.
        /// </summary>
        private OcTreeNode _rootNode;

        /// <summary>
        /// Store for the ModelsDrawn property.
        /// </summary>
        private static int _modelsDrawn;

        /// <summary>
        /// Store for the ModelsStoredInQuadTree property.
        /// </summary>
        private static int _modelsStoredInQuadTree;

        /// <summary>
        /// Store for the CubeVertexBuffer property.
        /// </summary>
        private static DynamicVertexBuffer _cubeVertexBuffer;

        /// <summary>
        /// Store for the CubeIndexBuffer property.
        /// </summary>
        private static DynamicIndexBuffer  _cubeIndexBuffer;

        /// <summary>
        /// Store for the Doors property.
        /// </summary>
        //private Dictionary<string, KeyValuePair<Vector3, BoundingBox>> _doors;
        private List<Door> _doors;
        //private Dictionary<string, KeyValuePair<Vector3, BoundingBox>> _doors;

        #endregion

        #region Properties

        /// <summary>
        /// RootNode property
        /// </summary>
        /// <value>
        /// Root node of the OcTree structure.
        /// </value>
        public OcTreeNode RootNode
        {
            get { return _rootNode; }
            set { _rootNode = value; }
        }

        /// <summary>
        /// ModelsDrawn property
        /// </summary>
        /// <value>
        /// Numberof models drawed by the OcTree.
        /// </value>
        public static int ModelsDrawn
        {
            get { return _modelsDrawn; }
            set { _modelsDrawn = value; }
        }

        /// <summary>
        /// ModelsStoredInQuadTree
        /// </summary>
        /// <value>
        /// Number of models stored in each node.
        /// </value>
        public static int ModelsStoredInQuadTree
        {
            get { return _modelsStoredInQuadTree; }
            set { _modelsStoredInQuadTree = value; }
        }

        /// <summary>
        /// CubeVertexBuffer property
        /// </summary>
        /// <value>
        /// Vertex buffer that permits to load the vertex in the hardware.
        /// </value>
        public static DynamicVertexBuffer CubeVertexBuffer
        {
            get { return _cubeVertexBuffer; }
            set { _cubeVertexBuffer = value; }
        }

        /// <summary>
        /// CubeIndexBuffer property
        /// </summary>
        /// <value>
        /// Index buffer that control the order of the vertex drawing.
        /// </value>
        public static DynamicIndexBuffer CubeIndexBuffer
        {
            get { return _cubeIndexBuffer; }
            set { _cubeIndexBuffer = value; }
        }

        //public Dictionary<string, KeyValuePair<Vector3, BoundingBox>> Doors
        public List<Door> Doors
        {
            get { return _doors; }
            set { _doors = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>OcTree</c> class.
        /// </summary>
        /// <param name="layerName">Name of the layer.</param>
        public OcTree(string layerName)
        {
            _name = layerName;

            _cubeVertexBuffer = new DynamicVertexBuffer(EngineManager.GameGraphicsDevice,
                typeof(VertexPositionColor), 8, BufferUsage.WriteOnly);
            _cubeIndexBuffer = new DynamicIndexBuffer(EngineManager.GameGraphicsDevice,
                typeof(short), 24, BufferUsage.WriteOnly);
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load the needed elements of the <c>OcTree</c>.
        /// </summary>
        public void Load()
        {
            string[,] information = FileHelper.readMainLayer(_name);
            AddLayer(information);
        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload the elements used in the <c>OcTree</c>.
        /// </summary>
        public void Unload()
        {
            Stack<OcTreeNode> auxStack = new Stack<OcTreeNode>();
            auxStack.Push(RootNode);

            while (auxStack.Count != 0)
            {
                OcTreeNode node = auxStack.Pop();

                if (node.ChildList.Count == 0)
                {
                    node.Unload();
                }
                else
                {
                    for (int i = node.ChildList.Count - 1; i >= 0; i--)
                        auxStack.Push(node.ChildList[i]);
                }
            }
            
            _cubeVertexBuffer.Dispose();
            _cubeIndexBuffer.Dispose();
        }

        #endregion

        #region SearchNode Method

        /// <summary>
        /// Search a node of the <c>OcTree</c> structure.
        /// </summary>
        /// <param name="box">Bounding box covered by the node.</param>
        /// <returns>Reference to the node searched.</returns>
        public OcTreeNode SearchNode(BoundingBox box)
        {
            bool found = false;
            OcTreeNode currentNode = _rootNode;
            while (!found)
            {
                if (currentNode.ChildList.Count == 0)
                    found = true;
                else
                {
                    bool aux = false;
                    foreach (OcTreeNode node in currentNode.ChildList)
                        if (node.BoundingBox.Contains(box) != ContainmentType.Disjoint)
                        {
                            currentNode = node;
                            aux = true;
                        }

                    if (aux == false)
                    {
                        found = true;
                        currentNode = null;
                    }
                }
            }

            if (currentNode == _rootNode && currentNode.BoundingBox.Contains(box) != ContainmentType.Disjoint)
                return null;
            else
                return currentNode;
        }

        /// <summary>
        /// Search a node of the <c>OcTree</c> structure.
        /// </summary>
        /// <param name="box">Position covered by the node.</param>
        /// <returns>Reference to the node searched.</returns>
        public OcTreeNode SearchNode(Vector3 position)
        {
            bool found = false;
            OcTreeNode currentNode = _rootNode;
            while (!found)
            {
                if (currentNode.ChildList.Count == 0)
                    found = true;
                else
                {
                    bool aux = false;
                    foreach (OcTreeNode node in currentNode.ChildList)
                        if (node.BoundingBox.Contains(position) == ContainmentType.Contains)
                        {
                            currentNode = node;
                            aux = true;
                        }

                    if (aux == false)
                    {
                        found = true;
                        currentNode = null;
                    }
                }
            }

            if (currentNode == _rootNode && currentNode.BoundingBox.Contains(position) != ContainmentType.Contains)
                return null;
            else
                return currentNode;
        }

        /// <summary>
        /// Search a node of the <c>OcTree</c> structure.
        /// </summary>
        /// <param name="box">Id of the model content in the node.</param>
        /// <returns>Reference to the node searched.</returns>
        public OcTreeNode SearchNode(int modelID)
        {
            Stack<OcTreeNode> auxStack = new Stack<OcTreeNode>();
            auxStack.Push(RootNode);

            while (auxStack.Count != 0)
            {
                OcTreeNode node = auxStack.Pop();

                if (node.ChildList.Count == 0)
                {
                    foreach (KeyValuePair<NodeType, DrawableModel> model in node.ModelList)
                        if (model.Value.ModelID == modelID)
                            return node;
                }
                else
                {
                    for (int i = node.ChildList.Count - 1; i >= 0; i--)
                        auxStack.Push(node.ChildList[i]);
                }
            }

            return null;
        }

        #endregion

        #region Move Method

        /// <summary>
        /// Search a node of the <c>OcTree</c> structure.
        /// </summary>
        /// <param name="box">Bounding box covered by the node.</param>
        /// <returns>Reference to the node searched.</returns>
        public void Move(Vector3 distance)
        {
            List<OcTreeNode> nodes = new List<OcTreeNode>();
            nodes.Add(_rootNode);

            while (nodes.Count != 0)
            {
                OcTreeNode node = nodes[0];
                nodes.Remove(node);

                if (node.ChildList.Count != 0)
                {
                    foreach (OcTreeNode childNode in node.ChildList)
                        nodes.Add(childNode);
                }
                else
                {
                    foreach (KeyValuePair<NodeType, DrawableModel> model in node.ModelList)
                        model.Value.Position += distance;

                    node.Center += distance;

                    node.CalcBoundingBox();
                    node.CalcVertex();
                    //node.BoundingBox = new BoundingBox(node.BoundingBox.Min + distance, 
                    //    node.BoundingBox.Max + distance);
                }
            }
        }

        #endregion

        #region AddModel Method

        /// <summary>
        /// Add a new model to the OcTree structure, creating a new node if it
        /// is necessary.
        /// </summary>
        /// <param name="model">Reference to the model to be added.</param>
        /// <returns>true if the model was added, false otherwise.</returns>
        public int AddModel(DrawableModel model)
        {
            OcTreeNode node = SearchNode(model.Position);
            
            NodeType type = NodeType.None;
            if (model.Model.FileName == "Content/Models/CubeBaseFrontStaircase2-1")
                type = NodeType.Staircase1Down;
            else if (model.Model.FileName == "Content/Models/CubeBaseFrontStaircase2-2")
                type = NodeType.Staircase2Down;
            else if (model.Model.FileName == "Content/Models/CubeBaseFrontLadder")
                type = NodeType.Ladder;
            else if (model.Model.FileName.Contains("Door"))
                type = NodeType.Door;

            return node.AddModel(type, model);
        }

        /// <summary>
        /// Add a new model to the OcTree structure, creating a new node if it
        /// is necessary.
        /// </summary>
        /// <param name="model">Reference to the model to be added.</param>
        /// <param name="position">Position of the model.</param>
        /// <param name="rotation">Rotation of the mode.</param>
        /// <param name="scale">Scale of the model.</param>
        /// <returns>true if the model was added, false otherwise.</returns>
        public int AddModel(GameModel model, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            OcTreeNode node = SearchNode(position);

            DrawableModel newModel = new DrawableModel(model, position, rotation, scale, _modelsStoredInQuadTree++);
            NodeType type = NodeType.None;
            if (newModel.Model.FileName == "Content/Models/CubeBaseFrontStaircase2-1")
            {
                if (rotation.X == MathHelper.ToRadians(270))
                    type = NodeType.Staircase1Up;
                else
                    type = NodeType.Staircase1Down;
            }
            else if (newModel.Model.FileName == "Content/Models/CubeBaseFrontStaircase2-2")
            {
                if (rotation.X == MathHelper.ToRadians(270))
                    type = NodeType.Staircase2Up;
                else
                    type = NodeType.Staircase2Down;
            }
            else if (newModel.Model.FileName == "Content/Models/CubeBaseFrontLadder")
            {
                type = NodeType.Ladder;
            }
            else if (newModel.Model.FileName.Contains("Door"))
            {
                
                type = NodeType.Door;
            }
            
            return node.AddModel(type, newModel);
        }

        #endregion

        #region ToInt Method

        /// <summary>
        /// Convert a character in its number representation.
        /// </summary>
        /// <param name="character">Character to be converted.</param>
        /// <returns>Integer with the number of the character.</returns>
        public int ToInt(char character)
        {
            return (int)(character - '0');
        }

        #endregion

        #region AddLayer Method

        /// <summary>
        /// Create the OcTree structure from the information of the layer.
        /// </summary>
        /// <param name="information">Information of the layer.</param>
        public void AddLayer(string[,] information)
        {
            int depth = Convert.ToInt32(information[0, 0]);
            int rows = Convert.ToInt32(information[0, 1]);
            int cols = Convert.ToInt32(information[0, 2]);

            Vector3 origin = new Vector3(
                Convert.ToInt32(information[1, 0]),
                Convert.ToInt32(information[1, 1]),
                Convert.ToInt32(information[1, 2]));

            bool found = false;
            int size = rows > cols ? rows : cols;
            for (int i = 1; !found; i++)
            {
                if (Math.Pow(4, i) < size && Math.Pow(4, i + 1) > size)
                {
                    size = (int)Math.Pow(4, i + 1);
                    found = true;
                }
            }

            int numberOfDoors = Convert.ToInt32(information[2, 0]);
            //_doors = new Dictionary<string, KeyValuePair<Vector3, BoundingBox>>();
            _doors = new List<Door>();
            for (int i = 0; i < numberOfDoors; i++)
            {
                Door newDoor = new Door();
                newDoor.nextLevel = information[3 + i, 9];
                newDoor.neededObject = information[3 + i, 10] != "Null" ? information[3 + i, 10] : null;
                newDoor.boundingBox = new BoundingBox(
                    new Vector3((float)Convert.ToDouble(information[3 + i, 0]), 
                        (float)Convert.ToDouble(information[3 + i, 1]), 
                        (float)Convert.ToDouble(information[3 + i, 2])),
                    new Vector3((float)Convert.ToDouble(information[3 + i, 3]), 
                        (float)Convert.ToDouble(information[3 + i, 4]), 
                        (float)Convert.ToDouble(information[3 + i, 5])));
                newDoor.nextPosition = new Vector3((float)Convert.ToDouble(information[3 + i, 6]), 
                        (float)Convert.ToDouble(information[3 + i, 7]), 
                        (float)Convert.ToDouble(information[3 + i, 8]));
                _doors.Add(newDoor);
            }

            _rootNode = new OcTreeNode(null, new Vector3(
                origin.X + (rows / 2f) - 0.5f, 
                origin.Y + (cols / 2f) - 0.5f, 
                origin.Z + (depth/ 2f) - 0.5f), size);

            string[] modelInformation;
            for (int i = 0; i < depth; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    for (int k = 0; k < cols; k++)
                    {
                        if (information[j + (rows * i) + 3 + numberOfDoors, k] != "Null")
                        {
                            modelInformation = information[j + (rows * i) + 3 + numberOfDoors, k].Split('_');

                            Vector3 rotation = Vector3.Zero;
                            for (int l = 2; l < modelInformation.Length; l++)
                            {
                                switch (modelInformation[l][0])
                                {
                                    case 'X':
                                        rotation.X = MathHelper.ToRadians((float)Convert.ToDouble(modelInformation[l].Split('X')[1]));
                                        break;

                                    case 'Y':
                                        rotation.Y = MathHelper.ToRadians((float)Convert.ToDouble(modelInformation[l].Split('Y')[1]));
                                        break;

                                    case 'Z':
                                        rotation.Z = MathHelper.ToRadians((float)Convert.ToDouble(modelInformation[l].Split('Z')[1]));
                                        break;
                                }
                            }

                            AddModel((GameModel)ModelManager.GetModel(modelInformation[0]),
                                new Vector3(j * xSize, k * ySize, i * zSize), rotation,
                                new Vector3(ToInt(modelInformation[1][1]) * xSize,
                                            ToInt(modelInformation[1][2]) * ySize, 
                                            ToInt(modelInformation[1][0]) * zSize));
                        }
                    }
                }
            }

            RemoveEmpty();
        }

        #endregion

        #region RemoveModel Method

        /// <summary>
        /// Remove a model that exists in the <c>OcTree</c> structure.
        /// </summary>
        /// <param name="modelID">Identifier of the model.</param>
        /// <returns>true if the model was removed, false otherwise.</returns>
        public DrawableModel RemoveModel(int modelID)
        {
            OcTreeNode node = SearchNode(modelID);
            if (node != null)
            {
                DrawableModel model = null;
                for (int i = 0; i < node.ModelList.Count; i++)
                {
                    if (node.ModelList[i].Value.ModelID == modelID)
                    {
                        model = node.ModelList[i].Value;
                        node.ModelList.Remove(node.ModelList[i]);
                    }
                }

                return model;
            }
            else
                return null;
        }

        #endregion

        #region RemoveEmpty Method

        /// <summary>
        /// Remove the empty nodes created in the generation of the structure.
        /// </summary>
        public void RemoveEmpty()
        {
            Stack<OcTreeNode> auxStack = new Stack<OcTreeNode>();
            auxStack.Push(RootNode);

            while (auxStack.Count != 0)
            {
                OcTreeNode node = auxStack.Pop();
             
                if (node.ChildList.Count == 0)
                {
                    if (node.ModelList.Count == 0)
                        node.Parent.ChildList.Remove(node);
                }
                else
                {
                    for (int i = node.ChildList.Count - 1; i >= 0; i--)
                        auxStack.Push(node.ChildList[i]);
                }
            }
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the current state of the <c>OcTree</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public void Update(GameTime gameTime)
        {

        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Type of content to be drawed.
        /// </summary>
        public enum DrawOptions
        {
            /// <summary>
            /// Draw only the models.
            /// </summary>
            Models,
            /// <summary>
            /// Draw only the bounding boxes.
            /// </summary>
            Boxes,
            /// <summary>
            /// Draw the models and the bounding boxes.
            /// </summary>
            Both
        }

        /// <summary>
        /// Draw the current state of the <c>OcTree</c> structure.
        /// </summary>
        /// <param name="option">Options used to draw.</param>
        /// <param name="modelEffect">Effect to apply to the models.</param>
        /// <param name="boxEffect">Effect to apply to the bounding boxes.</param>
        public void Draw(DrawOptions option, Effect modelEffect, BasicEffect boxEffect, bool checkFrustum = true)
        {
            switch (option)
            {
                case DrawOptions.Models:
                    DrawModels(CameraManager.ActiveCamera.View, CameraManager.ActiveCamera.Projection, 
                        new BoundingFrustum(CameraManager.ActiveCamera.View * CameraManager.ActiveCamera.Projection), modelEffect, checkFrustum);
                    break;

                case DrawOptions.Boxes:
                    DrawBoxes(CameraManager.ActiveCamera.View, CameraManager.ActiveCamera.Projection,
                        new BoundingFrustum(CameraManager.ActiveCamera.View * CameraManager.ActiveCamera.Projection), boxEffect, checkFrustum);
                    break;

                case DrawOptions.Both:
                    DrawModels(CameraManager.ActiveCamera.View, CameraManager.ActiveCamera.Projection,
                        new BoundingFrustum(CameraManager.ActiveCamera.View * CameraManager.ActiveCamera.Projection), modelEffect, checkFrustum);
                    DrawBoxes(CameraManager.ActiveCamera.View, CameraManager.ActiveCamera.Projection,
                        new BoundingFrustum(CameraManager.ActiveCamera.View * CameraManager.ActiveCamera.Projection), boxEffect, checkFrustum);
                    break;
            }
        }

        #endregion

        #region DrawModels Method

        /// <summary>
        /// Draw the models that exists in the <c>OcTree</c> structure.
        /// </summary>
        /// <param name="viewMatrix">View matrix of the active camera.</param>
        /// <param name="projectionMatrix">Projection matrix of the active camera.</param>
        /// <param name="cameraFrustum">Bounding frustum of the camera.</param>
        /// <param name="effect">Effect to apply to the models.</param>
        public void DrawModels(Matrix viewMatrix, Matrix projectionMatrix,
            BoundingFrustum cameraFrustum, Effect effect, bool checkFrustum = true)
        {
            Stack<OcTreeNode> auxStack = new Stack<OcTreeNode>();
            auxStack.Push(RootNode);

            while (auxStack.Count != 0)
            {
                OcTreeNode node = auxStack.Pop();

                ContainmentType cameraNodeContainment = cameraFrustum.Contains(node.BoundingBox);
                if (!checkFrustum || (checkFrustum && cameraNodeContainment != ContainmentType.Disjoint))
                {
                    if (node.ChildList.Count == 0)
                    {
                        //foreach(KeyValuePair<NodeType, DrawableModel> model in node.ModelList)
                        //    if(model.Value.Model.FileName.Contains("Door"))
                        //        EventManager.Trigger(new EventData_LogMessage("TROZO PUERTA: " + model.Value.Model.FileName + " - " + node.BoundingBox.ToString()));
                        node.Draw(viewMatrix, projectionMatrix, effect);
                    }
                    else
                    {
                        for (int i = node.ChildList.Count - 1; i >= 0; i--)
                            auxStack.Push(node.ChildList[i]);
                    }
                }
            }
        }

        #endregion
        
        #region DrawBoxes Method

        /// <summary>
        /// Draw the bounding boxes that exists in the <c>OcTree</c> structure.
        /// </summary>
        /// <param name="viewMatrix">View matrix of the active camera.</param>
        /// <param name="projectionMatrix">Projection matrix of the active camera.</param>
        /// <param name="cameraFrustum">Bounding frustum of the camera.</param>
        /// <param name="effect">Effect to apply to the models.</param>
        public void DrawBoxes(Matrix viewMatrix, Matrix projectionMatrix,
            BoundingFrustum cameraFrustum, BasicEffect effect, bool checkFrustum = true)
        {
            Dictionary<string, Matrix> dictionary = new Dictionary<string, Matrix>();

            Stack<OcTreeNode> auxStack = new Stack<OcTreeNode>();
            auxStack.Push(RootNode);

            while (auxStack.Count != 0)
            {
                OcTreeNode node = auxStack.Pop();

                ContainmentType cameraNodeContainment = cameraFrustum.Contains(node.BoundingBox);
                if (!checkFrustum || (checkFrustum && cameraNodeContainment != ContainmentType.Disjoint))
                {
                    if (node.ChildList.Count == 0)
                    {
                        node.DrawBoundingBox(Matrix.Identity, viewMatrix, projectionMatrix, effect);
                    }
                    else
                    {
                        for (int i = node.ChildList.Count - 1; i >= 0; i--)
                            auxStack.Push(node.ChildList[i]);

                        node.DrawBoundingBox(Matrix.Identity, viewMatrix, projectionMatrix, effect);
                    }
                }
            }
        }

        #endregion
    }
}
