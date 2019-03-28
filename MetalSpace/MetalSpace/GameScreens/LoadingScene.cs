using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Sound;
using MetalSpace.Models;
using MetalSpace.Textures;
using MetalSpace.Managers;
using MetalSpace.Events;
using MetalSpace.Settings;

namespace MetalSpace.GameScreens
{
    class LoadingScene : GameScreen
    {
        #region Fields

        public const int totalSafeTime = 1500;
        public int _safeTime;

        private Dictionary<string, string> _mapInformation;
        private List<string> _loadableResources;
        private List<string> _removableResources;

        private bool _state;

        private int _totalReleasablesResources;
        private int _releasedResources;
        private int _totalLoadableResources;
        private int _loadedResources;

        private Vector2 _position;
        
        #endregion

        #region Properties

        #endregion

        #region Constructor

        public LoadingScene(GraphicsDevice device, string mapName)
        {
            GameGraphicsDevice = device;

            _state = false;

            _loadableResources = new List<string>();
            _removableResources = new List<string>();

            _mapInformation = FileHelper.readMapInformation(mapName);
            List<string> resources = new List<string>(_mapInformation["Elements"].Split(' '));

            List<string> loadedModels = ModelManager.GetKeys();
            List<string> loadedTextures = TextureManager.GetKeys();
            List<string> loadedSounds = SoundManager.GetKeys();
            foreach(string resource in resources)
            {
                if (resource.Split('_')[1] == "Texture")
                {
                    if (loadedTextures.Contains(resource.Split('_')[0]))
                        //resources.Remove(resource);
                        _loadableResources.Add(resource);
                    else
                        _removableResources.Add(resource + "_Texture");
                }
                else if (resource.Split('_')[1] == "Model")
                {
                    if (loadedModels.Contains(resource.Split('_')[0]))
                        //resources.Remove(resource);
                        _loadableResources.Add(resource);
                    else
                        _removableResources.Add(resource + "_Model");
                }
                else if (resource.Split('_')[1] == "Sound")
                {
                    if (loadedSounds.Contains(resource.Split('_')[0]))
                        //resources.Remove(resource);
                        _loadableResources.Add(resource);
                    else
                        _removableResources.Add(resource + "_Model");
                }
            }

            //_loadableResources = new List<string>(resources);

            _removableResources.Remove("SeparationBar_Texture");
            _removableResources.Remove("SelectedMenuEntry_Texture");
            _removableResources.Remove("DummyTexture15T_Texture");
            _removableResources.Remove("DummyTexture_Texture");
            _removableResources.Remove("BarTexture_Texture");
            _removableResources.Remove("DialogBackground_Texture");
            
            _loadedResources = 0;
            _totalLoadableResources = _loadableResources.Count;
            _releasedResources = 0;
            _totalReleasablesResources = _removableResources.Count;
        }

        #endregion

        #region Load Method

        public override void LoadContent()
        {
            _position = new Vector2(
                GameSettings.DefaultInstance.ResolutionWidth/2f - TextureManager.GetTexture("BarTexture").BaseTexture.Width / 2f,
                GameSettings.DefaultInstance.ResolutionHeight/2f - TextureManager.GetTexture("BarTexture").BaseTexture.Height / 2f);

            base.LoadContent();
        }

        #endregion

        #region Unload Method

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        #endregion

        #region HandleInput Method

        public override void HandleInput(GameComponents.Input input, GameTime gameTime)
        {
            base.HandleInput(input, gameTime);
        }

        #endregion

        #region Update Method

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            _safeTime += gameTime.ElapsedGameTime.Milliseconds;

            if (_state == false)
            {
                if (_removableResources.Count != 0)
                {
                    string resource = _removableResources[0];
                    string[] information = resource.Split('_');
                    if (information[1] == "Texture")
                    {
                        ThreadStart threadStarter = delegate
                        {
                            TextureManager.RemoveTexture(information[0]);
                            _releasedResources++;
                        };

                        Thread addResourceThread = new Thread(threadStarter);
                        addResourceThread.Start();
                    }
                    else if (information[1] == "Model")
                    {
                        ThreadStart threadStarter = delegate
                        {
                            ModelManager.RemoveModel(information[0]);
                            _releasedResources++;
                        };
                        Thread addResourceThread = new Thread(threadStarter);
                        addResourceThread.Start();
                    }
                    else if (information[1] == "Sound")
                    {
                        ThreadStart threadStarter = delegate
                        {
                            SoundManager.RemoveSound(information[0]);
                            _releasedResources++;
                        };
                        Thread addResourceThread = new Thread(threadStarter);
                        addResourceThread.Start();
                    }

                    _removableResources.Remove(resource);
                }
                else
                    _state = true;
            }
            else
            {
                if (_loadableResources.Count != 0)
                {
                    string resource = _loadableResources[0];
                    string[] information = resource.Split('_');
                    if (information[1] == "Texture")
                    {
                        ThreadStart threadStarter = delegate
                        {
                            TextureManager.AddTexture(information[0], new GameTexture("Content/Textures/" + information[0]));
                            _loadedResources++;
                        };

                        Thread addResourceThread = new Thread(threadStarter);
                        addResourceThread.Start();
                    }
                    else if (information[1] == "Model")
                    {
                        ThreadStart threadStarter = delegate
                        {
                            ModelManager.AddModel(information[0], new GameModel("Content/Models/" + information[0]));
                            _loadedResources++;
                        };
                        Thread addResourceThread = new Thread(threadStarter);
                        addResourceThread.Start();
                    }
                    else if (information[1] == "Sound")
                    {
                        ThreadStart threadStarter = delegate
                        {
                            SoundManager.AddSound(information[0], new GameSound("Content/Sounds/" + information[0]));
                            _loadedResources++;
                        };
                        Thread addResourceThread = new Thread(threadStarter);
                        addResourceThread.Start();
                    }

                    _loadableResources.Remove(resource);
                }
            }

            if (_releasedResources >= _totalReleasablesResources - 1 && 
                _loadedResources >= _totalLoadableResources - 1 &&
                _safeTime > totalSafeTime)
            {
                ScreenManager.RemoveScreen("LoadScene");
                ScreenManager.AddScreen("ContinueGame", new MainGameScreen(this.GameGraphicsDevice, "ContinueGame", _mapInformation));
            }
        }

        #endregion

        #region Draw Method

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            
            Texture2D borderTexture = TextureManager.GetTexture("BarTexture").BaseTexture as Texture2D;

            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullCounterClockwise, null, CanvasManager.ScaleMatrix);

            double eachPercentWidth = (double)borderTexture.Width / (double)(_totalReleasablesResources + _totalLoadableResources);
            int xPosition = GameSettings.DefaultInstance.ResolutionWidth / 3;
            ScreenManager.SpriteBatch.Draw(
                TextureManager.GetTexture("DummyTexture").BaseTexture as Texture2D,
                new Rectangle((int)_position.X, (int)_position.Y,
                    (int)((_loadedResources + _releasedResources) * eachPercentWidth), borderTexture.Height),
                Color.Red);

            ScreenManager.SpriteBatch.Draw(
                borderTexture,
                new Rectangle((int)_position.X, (int)_position.Y, borderTexture.Width, borderTexture.Height),
                Color.White);

            ScreenManager.SpriteBatch.End();
        }

        #endregion
    }
}
