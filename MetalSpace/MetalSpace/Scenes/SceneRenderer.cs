using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Objects;
using MetalSpace.Events;
using MetalSpace.Managers;
using MetalSpace.Interfaces;

namespace MetalSpace.Scene
{
    /// <summary>
    /// The <c>SceneRenderer</c> class represents manager that permits to control
    /// all the layers (2D, 3D and main layer) of the scene.
    /// </summary>
    public class SceneRenderer
    {
        #region Fields

        /// <summary>
        /// Information of the current map.
        /// </summary>
        private Dictionary<string, string> _mapInformation;

        /// <summary>
        /// Store for the MainLayer property.
        /// </summary>
        private OcTree _mainLayer;
        
        /// <summary>
        /// List of all layers in the game (except the main layer).
        /// </summary>
        private List<IGameLayer> _layers;

        /// <summary>
        /// Effect to be applied to the models.
        /// </summary>
        private BasicEffect _boxEffect;

        #endregion

        #region Properties

        /// <summary>
        /// MainLayer property
        /// </summary>
        /// <value>
        /// Main layer where the player moves.
        /// </value>
        public OcTree MainLayer
        {
            get { return _mainLayer; }
            set { _mainLayer = value; }
        }

        /// <summary>
        /// Layers2D property
        /// </summary>
        /// <value>
        /// List of layers that represent the walls (GameLayer2D).
        /// </value>
        public List<GameLayer2D> Layers2D
        {
            get
            {
                List<GameLayer2D> list = new List<GameLayer2D>();
                foreach (IGameLayer layer in _layers)
                    if (layer is GameLayer2D)
                        list.Add((GameLayer2D) layer);

                return list;
            }
        }

        /// <summary>
        /// Layers3D property
        /// </summary>
        /// <value>
        /// List of layers that represent the world (GameLayer3D).
        /// </value>
        public List<GameLayer3D> Layers3D
        {
            get
            {
                List<GameLayer3D> list = new List<GameLayer3D>();
                foreach (IGameLayer layer in _layers)
                    if (layer is GameLayer3D)
                        list.Add((GameLayer3D)layer);

                return list;
            }
        }

        public Dictionary<string, string> MapInformation
        {
            get { return _mapInformation; }
            set { _mapInformation = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>SceneRenderer</c> class.
        /// </summary>
        /// <param name="mapInformation">Information of the map.</param>
        public SceneRenderer(Dictionary<string, string> mapInformation)
        {
            _mapInformation = mapInformation;

            _layers = new List<IGameLayer>();

            _boxEffect = new BasicEffect(EngineManager.GameGraphicsDevice);
            _boxEffect.World = Matrix.Identity;
            _boxEffect.LightingEnabled = false;
            _boxEffect.TextureEnabled = false;
            _boxEffect.VertexColorEnabled = true;
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load the needed elements of the <c>SceneRenderer</c>-
        /// </summary>
        public void Load()
        {
            string[] aux;
            for (int i = 0; i < Convert.ToInt32(_mapInformation["NumberOfLayers"]); i++)
            {
                aux = _mapInformation["Layer" + i.ToString()].Split(' ');
                if (aux[1] == "Layer2D")
                    _layers.Add(new GameLayer2D(aux[0]));
                else if (aux[1] == "Layer3D")
                    _layers.Add(new GameLayer3D(aux[0]));
                else
                    _mainLayer = new OcTree(aux[0]);
            }

            foreach (IGameLayer layer in _layers)
                layer.Load();

            _mainLayer.Load();
        }

        #endregion

        #region Move Method

        public void Move(Vector3 distance)
        {
            MainLayer.Move(distance);
            foreach (IGameLayer layer in Layers2D)
                layer.Move(distance);
            foreach (IGameLayer layer in Layers3D)
                layer.Move(distance);
        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload the needed elements of the <c>SceneRenderer</c>.
        /// </summary>
        public void Unload()
        {
            _mapInformation.Clear();

            foreach (IGameLayer layer in _layers)
                layer.Unload();

            _mainLayer.Unload();

            _boxEffect.Dispose();

            //_tileVertexBuffer.Dispose();
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the current state of the <c>SceneRenderer</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public void Update(GameTime gameTime)
        {
            foreach (IGameLayer layer in _layers)
                layer.Update(gameTime);

            _mainLayer.Update(gameTime);
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the <c>SceneRenderer</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        /// <param name="customEffect">Effect to be applied in the models.</param>
        public void Draw(GameTime gameTime, Effect customEffect, bool checkFrustum = true)
        {
            foreach (IGameLayer layer in _layers)
            {
                if (layer is GameLayer3D)
                    ((GameLayer3D) layer).Draw(gameTime, customEffect, checkFrustum);
                else
                    layer.Draw(gameTime, customEffect);
            }

            _mainLayer.Draw(OcTree.DrawOptions.Models, null, _boxEffect, checkFrustum);
        }

        #endregion
    }
}
