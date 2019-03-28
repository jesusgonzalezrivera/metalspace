using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Models;
using MetalSpace.Managers;
using MetalSpace.Events;
using MetalSpace.Interfaces;

namespace MetalSpace.Scene
{
    /// <summary>
    /// The <c>GameLayer3D</c> represents a 3D layer that the user can not reach.
    /// </summary>
    public class GameLayer3D : IGameLayer
    {
        #region Fields

        /// <summary>
        /// Store for the LayerName property.
        /// </summary>
        private string _layerName;

        /// <summary>
        /// Store for the Depth property.
        /// </summary>
        private int _depth;

        /// <summary>
        /// Store for the Rows property.
        /// </summary>
        private int _rows;

        /// <summary>
        /// Store for the Cols property.
        /// </summary>
        private int _cols;

        /// <summary>
        /// Store for the Origin property.
        /// </summary>
        private Vector3 _origin;

        /// <summary>
        /// List of models used in the <c>GameLayer3D</c>.
        /// </summary>
        private List<DrawableModel> _modelList;

        #endregion

        #region Properties

        /// <summary>
        /// LayerName property
        /// </summary>
        /// <value>
        /// Name of the layer.
        /// </value>
        public string LayerName
        {
            get { return _layerName; }
            set { _layerName = value; }
        }

        /// <summary>
        /// Rows property
        /// </summary>
        /// <value>
        /// Number of rows of the <c>GameLayer3D</c>.
        /// </value>
        public int Rows
        {
            get { return _rows; }
            set { _rows = value; }
        }

        /// <summary>
        /// Cols property
        /// </summary>
        /// <value>
        /// Number of cols of the <c>GameLayer3D</c>.
        /// </value>
        public int Cols
        {
            get { return _cols; }
            set { _cols = value; }
        }

        /// <summary>
        /// Depth property
        /// </summary>
        /// <value>
        /// Number of layer that compose the <c>GameLayer3D</c>.
        /// </value>
        public int Depth
        {
            get { return _depth; }
            set { _depth = value; }
        }

        /// <summary>
        /// Origin property
        /// </summary>
        /// <value>
        /// Origin position of the <c>GameLayer3D</c>.
        /// </value>
        public Vector3 Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>GameLayer3D</c>.
        /// </summary>
        /// <param name="layerName">Name of the layer.</param>
        public GameLayer3D(string layerName)
        {
            _layerName = layerName;

            _modelList = new List<DrawableModel>();
        }

        #endregion

        #region ToInt Method

        /// <summary>
        /// Conver an individual character into the corresponding number.
        /// </summary>
        /// <param name="character">Character to convert.</param>
        /// <returns>Number that correspond to the character.</returns>
        public int ToInt(char character)
        {
            return (int)(character - '0');
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load the needed content of the <c>GameLayer3D</c>.
        /// </summary>
        public void Load()
        {
            string[,] information = FileHelper.ReadGameLayer3D(_layerName);

            _depth = Convert.ToInt32(information[0, 0]);
            _rows = Convert.ToInt32(information[0, 1]);
            _cols = Convert.ToInt32(information[0, 2]);

            _origin = new Vector3(
                (float) Convert.ToDouble(information[1, 0]),
                (float)Convert.ToDouble(information[1, 1]),
                (float)Convert.ToDouble(information[1, 2]));

            string[] modelInformation;
            for (int i = 0; i < _depth; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    for (int k = 0; k < _cols; k++)
                    {
                        if (information[j + (_rows * i) + 2, k] != "Null")
                        {
                            modelInformation = information[j + (_rows * i) + 2, k].Split('_');

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

                            _modelList.Add(new DrawableModel((GameModel)ModelManager.GetModel(modelInformation[0]),
                                new Vector3(_origin.X + j, _origin.Y + k, _origin.Z - i - 0.5f), rotation,
                                new Vector3(ToInt(modelInformation[1][1]), ToInt(modelInformation[1][2]), 
                                            ToInt(modelInformation[1][0])), 0));
                        }
                    }
                }
            }
        }

        #endregion

        #region Move Method

        public void Move(Vector3 distance)
        {
            for (int i = 0; i < _modelList.Count; i++)
                _modelList[i].Position = new Vector3(_modelList[i].Position.X + distance.X,
                    _modelList[i].Position.Y + distance.Y, _modelList[i].Position.Z + distance.Z);
        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload the content used in the <c>GameLayer3D</c>.
        /// </summary>
        public void Unload()
        {
            _modelList.Clear();
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the current state of the <c>GameLayer3D</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public void Update(GameTime gameTime)
        {

        }

        #endregion

        #region Draw Method

        private bool _checkFrustum = true;
        public void Draw(GameTime gameTime, Effect customEffect, bool checkFrustum = true)
        {
            _checkFrustum = checkFrustum;
            Draw(gameTime, customEffect);
        }

        /// <summary>
        /// Draw the current state of the <c>GameLayer3D</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        /// <param name="customEffect">Custom effect to be applied in the <c>GameLayer3D</c>.</param>
        public void Draw(GameTime gameTime, Effect customEffect)
        {
            Dictionary<string, Matrix> dictionary = new Dictionary<string, Matrix>();
            BoundingFrustum boundingFrustum = new BoundingFrustum(
                CameraManager.ActiveCamera.View * CameraManager.ActiveCamera.Projection);
            foreach(DrawableModel model in _modelList)
            {
                ContainmentType cameraNodeContainment = boundingFrustum.Contains(model.BSphere);//boundingFrustum.Contains(model.GetBoundingBox());
                if (!_checkFrustum || (_checkFrustum && cameraNodeContainment != ContainmentType.Disjoint))
                    model.Draw(CameraManager.ActiveCamera.View, CameraManager.ActiveCamera.Projection, 
                        DrawingMethod.HardwareInstancing, dictionary, customEffect);
            }
        }

        #endregion
    }
}
