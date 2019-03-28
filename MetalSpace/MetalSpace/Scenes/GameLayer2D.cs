using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Events;
using MetalSpace.Managers;
using MetalSpace.Interfaces;

namespace MetalSpace.Scene
{
    /// <summary>
    /// The <c>GameLayer2D</c> represents a 2D layer positioned in the 3D world
    /// to represent a wall of each room of the game.
    /// </summary>
    public class GameLayer2D : IGameLayer
    {
        #region Fields

        /// <summary>
        /// Store for the LayerName property.
        /// </summary>
        private string _layerName;

        /// <summary>
        /// Number of tiles in the texture of tiles.
        /// </summary>
        private int _numberOfTiles;

        /// <summary>
        /// Name of the tiles texture.
        /// </summary>
        private string _textureName;
        
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
        /// Store for the Up property.
        /// </summary>
        private Vector3 _up;

        /// <summary>
        /// Store for the Normal property.
        /// </summary>
        private Vector3 _normal;

        /// <summary>
        /// Store for the BoundingPlane property.
        /// </summary>
        private Plane _boundingPlane;

        /// <summary>
        /// Effect applied to the <c>GameLayer2D</c>.
        /// </summary>
        private static AlphaTestEffect _effect;

        /// <summary>
        /// List of vertices that represents the <c>GameLayer2D</c>.
        /// </summary>
        private List<VertexPositionNormalTexture> _vertices;

        /// <summary>
        /// Vertex buffer used to load the vertices in the graphic card.
        /// </summary>
        private DynamicVertexBuffer _tileVertexBuffer;

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
        /// Number of rows of the <c>GameLayer2D</c>.
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
        /// Number of cols of the <c>GameLayer2D</c>.
        /// </value>
        public int Cols
        {
            get { return _cols; }
            set { _cols = value; }
        }

        /// <summary>
        /// Origin property
        /// </summary>
        /// <value>
        /// Origin position of the <c>GameLayer2D</c>.
        /// </value>
        public Vector3 Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        /// <summary>
        /// Up direction of the <c>GameLayer2D</c>.
        /// </summary>
        public Vector3 Up
        {
            get { return _up; }
            set { _up = value; }
        }

        /// <summary>
        /// Normal direction of the <c>GameLayer2D</c>.
        /// </summary>
        public Vector3 Normal
        {
            get { return _normal; }
            set { _normal = value; }
        }

        /// <summary>
        /// Bounding plane that represents the <c>GameLayer2D</c>.
        /// </summary>
        public Plane BoundingPlane
        {
            get { return _boundingPlane; }
            set { _boundingPlane = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>GameLayer2D</c> class.
        /// </summary>
        /// <param name="layerName">Name of the layer.</param>
        public GameLayer2D(string layerName)
        {
            _layerName = layerName;

            _vertices = new List<VertexPositionNormalTexture>();

            if (_effect == null)
            {
                _effect = new AlphaTestEffect(EngineManager.GameGraphicsDevice);
                _effect.World = Matrix.Identity;
            }
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load the needed content of the <c>GameLayer2D</c>.
        /// </summary>
        public void Load()
        {
            string[,] information = FileHelper.ReadGameLayer2D(_layerName);

            _rows = Convert.ToInt32(information[0, 0]);
            _cols = Convert.ToInt32(information[0, 1]);
            
            _origin = new Vector3(
                (float) Convert.ToDouble(information[1, 0]),
                (float) Convert.ToDouble(information[1, 1]),
                (float) Convert.ToDouble(information[1, 2]));

            _up = new Vector3(
                Convert.ToInt32(information[2, 0]),
                Convert.ToInt32(information[2, 1]),
                Convert.ToInt32(information[2, 2]));

            _normal = new Vector3(
                Convert.ToInt32(information[3, 0]),
                Convert.ToInt32(information[3, 1]),
                Convert.ToInt32(information[3, 2]));

            _textureName = information[4, 0];
            _numberOfTiles = Convert.ToInt32(information[4, 1]);

            Vector3 left = Vector3.Cross(_normal, _up);
            Vector3 lowerLeft, upperLeft, lowerRight, upperRight;
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    //if (information[i + 5, j] != "Null")
                    //{
                    string[] aux;
                    if (information[i + 5, j] != "Null")
                        aux = information[i + 5, j].Split('_');
                    else
                        aux = "3_11".Split('_');

                        lowerLeft = _origin + (_up * i);
                        lowerLeft += -left * j;
                        lowerLeft += -left * ToInt(aux[1][0]);
                        upperLeft = lowerLeft + (_up * ToInt(aux[1][1]));
                        lowerRight = lowerLeft + (left * ToInt(aux[1][0]));
                        upperRight = lowerRight + (_up * ToInt(aux[1][1]));

                        _vertices.Add(new VertexPositionNormalTexture(
                            lowerRight, _normal, new Vector2((1.0f / _numberOfTiles) * (Convert.ToInt32(aux[0]) - 1), 1)));
                        _vertices.Add(new VertexPositionNormalTexture(
                            upperRight, _normal, new Vector2((1.0f / _numberOfTiles) * (Convert.ToInt32(aux[0]) - 1), 0)));
                        _vertices.Add(new VertexPositionNormalTexture(
                            lowerLeft, _normal, new Vector2((1.0f / _numberOfTiles) * Convert.ToInt32(aux[0]), 1)));

                        _vertices.Add(new VertexPositionNormalTexture(
                            upperRight, _normal, new Vector2((1.0f / _numberOfTiles) * (Convert.ToInt32(aux[0]) - 1), 0)));
                        _vertices.Add(new VertexPositionNormalTexture(
                            upperLeft, _normal, new Vector2((1.0f / _numberOfTiles) * Convert.ToInt32(aux[0]), 0)));
                        _vertices.Add(new VertexPositionNormalTexture(
                            lowerLeft, _normal, new Vector2((1.0f / _numberOfTiles) * Convert.ToInt32(aux[0]), 1)));
                    //}
                }
            }

            _boundingPlane = new Plane(
                _vertices.ToArray()[0].Position,
                _vertices.ToArray()[1].Position,
                _vertices.ToArray()[2].Position);

            _tileVertexBuffer = new DynamicVertexBuffer(EngineManager.GameGraphicsDevice,
                VertexPositionNormalTexture.VertexDeclaration,
                _rows * _cols * 6,
                BufferUsage.WriteOnly);
        }

        #endregion

        #region Move Method

        public void Move(Vector3 distance)
        {
            for(int i=0; i< _vertices.Count; i++)
                _vertices[i] = new VertexPositionNormalTexture(new Vector3(_vertices[i].Position.X + distance.X,
                    _vertices[i].Position.Y + distance.Y, _vertices[i].Position.Z + distance.Z), _vertices[i].Normal,
                    _vertices[i].TextureCoordinate);
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

        #region Unload Method

        /// <summary>
        /// Unload the needed content of the <c>GameLayer2D</c>.
        /// </summary>
        public void Unload()
        {
            //_effect.Dispose();

            _vertices.Clear();
            _tileVertexBuffer.Dispose();
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the current state of the <c>GameLayer2D</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public void Update(GameTime gameTime)
        {

        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the game.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        /// <param name="customEffect">Custom effect to be applied in the <c>GameLayer2D</c>.</param>
        public void Draw(GameTime gameTime, Effect customEffect)
        {
            _tileVertexBuffer.SetData<VertexPositionNormalTexture>(_vertices.ToArray());

            if (customEffect != null)
            {
                customEffect.Parameters["xTexture"].SetValue(TextureManager.GetTexture(_textureName).BaseTexture);
                customEffect.Parameters["xWorld"].SetValue(Matrix.Identity);
                customEffect.Parameters["xView"].SetValue(CameraManager.ActiveCamera.View);
                customEffect.Parameters["xProjection"].SetValue(CameraManager.ActiveCamera.Projection);

                //EngineManager.GameGraphicsDevice.SetVertexBuffer(SceneRenderer.TileVertexBuffer);
                foreach (EffectPass pass in customEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    EngineManager.GameGraphicsDevice.SetVertexBuffer(_tileVertexBuffer);

                    //EngineManager.GameGraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, SceneRenderer.TileVertexBuffer.VertexCount / 3);
                    EngineManager.GameGraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, _tileVertexBuffer.VertexCount / 3);
                }
                EngineManager.GameGraphicsDevice.SetVertexBuffer(null);
            }
            else
            {
                _effect.View = CameraManager.ActiveCamera.View;
                _effect.Projection = CameraManager.ActiveCamera.Projection;
                _effect.Texture = TextureManager.GetTexture(_textureName).BaseTexture;
                _effect.DiffuseColor = Color.LightBlue.ToVector3();
                //EngineManager.GameGraphicsDevice.SetVertexBuffer(SceneRenderer.TileVertexBuffer);
                
                foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    EngineManager.GameGraphicsDevice.SetVertexBuffer(_tileVertexBuffer);

                    EngineManager.GameGraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, _tileVertexBuffer.VertexCount / 3);
                }
                EngineManager.GameGraphicsDevice.SetVertexBuffer(null);
            }
        }

        #endregion
    }
}
