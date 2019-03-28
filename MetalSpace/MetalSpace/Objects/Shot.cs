using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Scene;
using MetalSpace.Managers;

namespace MetalSpace.Objects
{
    /// <summary>
    /// The <c>Shot</c> class represents an individual shot, shooted by the player.
    /// </summary>
    class Shot
    {
        #region Fields

        /// <summary>
        /// Type of the shot.
        /// </summary>
        private Gun.ShotType _shotType;

        /// <summary>
        /// Number of tiles that the texture contains.
        /// </summary>
        private int _numberOfTiles;

        /// <summary>
        /// Name of the texture that contains the tiles.
        /// </summary>
        private string _textureName;

        /// <summary>
        /// Store for the Origin property.
        /// </summary>
        private Vector3 _origin;

        /// <summary>
        /// Speed of the shots.
        /// </summary>
        private Vector3 _speed;

        /// <summary>
        /// Store for the Up property.
        /// </summary>
        private Vector3 _up;

        /// <summary>
        /// Store for the Normal property.
        /// </summary>
        private Vector3 _normal;

        /// <summary>
        /// Left direction of the shot.
        /// </summary>
        private Vector3 _left;
        
        /// <summary>
        /// Position of the lower left vertex.
        /// </summary>
        private Vector3 _lowerLeft;

        /// <summary>
        /// Position of the upper left vertex.
        /// </summary>
        private Vector3 _upperLeft;

        /// <summary>
        /// Position of the lower right vertex.
        /// </summary>
        private Vector3 _lowerRight;

        /// <summary>
        /// Position of the upper right vertex.
        /// </summary>
        private Vector3 _upperRight;

        /// <summary>
        /// Bounding box that wrap the shot.
        /// </summary>
        private BoundingBox _shotBBox;

        /// <summary>
        /// Bounding sphere that wrap the shot.
        /// </summary>
        private BoundingSphere _shotBSphere;

        /// <summary>
        /// Effect to be aplied to the shot.
        /// </summary>
        private static AlphaTestEffect _effect;

        /// <summary>
        /// List of vertices that represents the shot.
        /// </summary>
        private List<VertexPositionNormalTexture> _vertices;

        #endregion

        #region Properties

        /// <summary>
        /// Origin property
        /// </summary>
        /// <value>
        /// Origin position of the shot.
        /// </value>
        public Vector3 Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        /// <summary>
        /// Up property
        /// </summary>
        /// <value>
        /// Up direction of the shot.
        /// </value>
        public Vector3 Up
        {
            get { return _up; }
            set { _up = value; }
        }

        /// <summary>
        /// Normal property
        /// </summary>
        /// <value>
        /// Normal direction of the shot.
        /// </value>
        public Vector3 Normal
        {
            get { return _normal; }
            set { _normal = value; }
        }

        /// <summary>
        /// ShotBBox property
        /// </summary>
        /// <value>
        /// Bounding box that wrap the shot.
        /// </value>
        public BoundingBox ShotBBox
        {
            get { return _shotBBox; }
            set { _shotBBox = value; }
        }

        /// <summary>
        /// ShotBBSphere property
        /// </summary>
        /// <value>
        /// Bounding sphere that wrap the shot.
        /// </value>
        public BoundingSphere ShotBBSphere
        {
            get { return _shotBSphere; }
            set { _shotBSphere = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>Shot</c> class.
        /// </summary>
        /// <param name="shotType">Type of the shot.</param>
        /// <param name="origin">Origin of the shot.</param>
        /// <param name="up">Up direction of the shot.</param>
        /// <param name="speed">Speed of the shot.</param>
        public Shot(Gun.ShotType shotType, Vector3 origin, Vector3 up, Vector3 speed)
        {
            _origin = origin;
            if (shotType == Gun.ShotType.Normal)
            {
                _up = up / 6f;
                _normal = Vector3.Backward;
            }
            else
            {
                _up = up / 16f;
                _normal = Vector3.Backward * 6;
            }
            
            _speed = speed;

            _shotType = shotType;
            _numberOfTiles = 2;
            _textureName = "ShotTiles";

            if (_effect == null)
            {
                _effect = new AlphaTestEffect(EngineManager.GameGraphicsDevice);
                _effect.World = Matrix.Identity;
            }

            _left = Vector3.Cross(_normal, _up);
            
            _lowerLeft = _origin;
            _lowerLeft += -_left;
            _upperLeft = _lowerLeft + _up;
            _lowerRight = _lowerLeft + _left;
            _upperRight = _lowerRight + _up;

            _shotBBox = new BoundingBox(
                new Vector3(_lowerLeft.X, _lowerLeft.Y, _lowerLeft.Z - 0.1f),
                new Vector3(_upperRight.X, _upperRight.Y, _upperRight.Z + 0.1f));

            _shotBSphere = new BoundingSphere(new Vector3(
                ((_upperRight.X - _lowerLeft.X) / 2f) + _lowerLeft.X,
                ((_upperRight.Y - _lowerLeft.Y) / 2f) + _lowerLeft.Y, 
                ((_upperRight.Z - _lowerLeft.Z) / 2f) + _lowerLeft.Z), 0.2f);

            _vertices = new List<VertexPositionNormalTexture>();
            _vertices.Add(new VertexPositionNormalTexture(
                _lowerRight, _normal,
                new Vector2((1.0f / _numberOfTiles) * ((_shotType == Gun.ShotType.Normal ? 1 : 2) - 1), 1)));

            _vertices.Add(new VertexPositionNormalTexture(
                _upperRight, _normal,
                new Vector2((1.0f / _numberOfTiles) * ((_shotType == Gun.ShotType.Normal ? 1 : 2) - 1), 0)));

            _vertices.Add(new VertexPositionNormalTexture(
                _lowerLeft, _normal,
                new Vector2((1.0f / _numberOfTiles) * (_shotType == Gun.ShotType.Normal ? 1 : 2), 1)));

            _vertices.Add(new VertexPositionNormalTexture(
                _upperRight, _normal,
                new Vector2((1.0f / _numberOfTiles) * ((_shotType == Gun.ShotType.Normal ? 1 : 2) - 1), 0)));

            _vertices.Add(new VertexPositionNormalTexture(
                _upperLeft, _normal,
                new Vector2((1.0f / _numberOfTiles) * (_shotType == Gun.ShotType.Normal ? 1 : 2), 0)));

            _vertices.Add(new VertexPositionNormalTexture(
                _lowerLeft, _normal,
                new Vector2((1.0f / _numberOfTiles) * (_shotType == Gun.ShotType.Normal ? 1 : 2), 1)));
        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload all the content used by the <c>Shot</c>.
        /// </summary>
        public void Unload()
        {
            _vertices.Clear();
            _vertices = null;
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the current state of the <c>Shot</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public void Update(GameTime gameTime)
        {
            _vertices.Clear();

            float time = (float)(gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
            _origin += new Vector3(_speed.X * time, _speed.Y * time, _speed.Z);
            _lowerLeft = _origin;
            _lowerLeft += -_left;
            _upperLeft = _lowerLeft + _up;
            _lowerRight = _lowerLeft + _left;
            _upperRight = _lowerRight + _up;

            _shotBBox.Min = new Vector3(_lowerLeft.X, _lowerLeft.Y, _lowerLeft.Z - 0.1f);
            _shotBBox.Max = new Vector3(_upperRight.X, _upperRight.Y, _upperRight.Z + 0.1f);
            _shotBSphere.Center = new Vector3(
                ((_upperRight.X - _lowerLeft.X) / 2f) + _lowerLeft.X,
                ((_upperRight.Y - _lowerLeft.Y) / 2f) + _lowerLeft.Y, 
                ((_upperRight.Z - _lowerLeft.Z) / 2f) + _lowerLeft.Z);

            _vertices.Add(new VertexPositionNormalTexture(
                _lowerRight, _normal,
                new Vector2((1.0f / _numberOfTiles) * ((_shotType == Gun.ShotType.Normal ? 1 : 2) - 1), 1)));

            _vertices.Add(new VertexPositionNormalTexture(
                _upperRight, _normal,
                new Vector2((1.0f / _numberOfTiles) * ((_shotType == Gun.ShotType.Normal ? 1 : 2) - 1), 0)));

            _vertices.Add(new VertexPositionNormalTexture(
                _lowerLeft, _normal,
                new Vector2((1.0f / _numberOfTiles) * (_shotType == Gun.ShotType.Normal ? 1 : 2), 1)));

            _vertices.Add(new VertexPositionNormalTexture(
                _upperRight, _normal,
                new Vector2((1.0f / _numberOfTiles) * ((_shotType == Gun.ShotType.Normal ? 1 : 2) - 1), 0)));

            _vertices.Add(new VertexPositionNormalTexture(
                _upperLeft, _normal,
                new Vector2((1.0f / _numberOfTiles) * (_shotType == Gun.ShotType.Normal ? 1 : 2), 0)));

            _vertices.Add(new VertexPositionNormalTexture(
                _lowerLeft, _normal,
                new Vector2((1.0f / _numberOfTiles) * (_shotType == Gun.ShotType.Normal ? 1 : 2), 1)));            
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the <c>Shot</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public void Draw(GameTime gameTime)
        {
            ShotManager.TileVertexBuffer.SetData<VertexPositionNormalTexture>(_vertices.ToArray());
            
            _effect.View = CameraManager.ActiveCamera.View;
            _effect.Projection = CameraManager.ActiveCamera.Projection;
            _effect.Texture = TextureManager.GetTexture(_textureName).BaseTexture;

            EngineManager.GameGraphicsDevice.SetVertexBuffer(ShotManager.TileVertexBuffer);
            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                EngineManager.GameGraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0,
                    ShotManager.TileVertexBuffer.VertexCount / 3);
            }
            EngineManager.GameGraphicsDevice.SetVertexBuffer(null);
        }

        #endregion
    }
}
