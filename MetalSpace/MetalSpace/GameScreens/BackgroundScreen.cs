using System;
using System.Collections.Generic;
using System.Text;
using MetalSpace.Interfaces;
using MetalSpace.GameComponents;
using MetalSpace.Managers;
using MetalSpace.Sound;
using MetalSpace.Textures;
using MetalSpace.Models;
using MetalSpace.GameScreens;
using MetalSpace.Events;
using MetalSpace.Cameras;
using MetalSpace.Effects;
using MetalSpace.Animations;
using MetalSpace.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetalSpace.GameScreens
{
    /// <summary>
    /// The <c>BackgroundScreen</c> class represents the background image that
    /// it is used in all the menus. It permits the draw of a background texture,
    /// the title of the game and the spacecraft model (with or without post
    /// processing effect).
    /// </summary>
    public class BackgroundScreen : GameScreen
    {
        #region Fields
        
        /// <summary>
        /// Concs string that contains the name of the model.
        /// </summary>
        const string spacecraftModel = "Spacecraft";

        /// <summary>
        /// Const string that contains the name of the background texture.
        /// </summary>
        const string backgroundTexture = "MenuBackground";

        /// <summary>
        /// Model be drawn in front of the background.
        /// </summary>
        private DrawableModel _model;

        /// <summary>
        /// Animation for the first mesh in the model.
        /// </summary>
        private ObjectAnimation _animationRing1;

        /// <summary>
        /// Animation for the second mesh in the model.
        /// </summary>
        private ObjectAnimation _animationRing2;

        /// <summary>
        /// Individual mesh transform of the models.
        /// </summary>
        private Dictionary<string, Matrix> _individualTransformations;

        /// <summary>
        /// <c>RenderCapture</c> to use the post processor only in the model.
        /// </summary>
        RenderCapture renderCapture;

        /// <summary>
        /// <c>PostProcessor</c> to apply a gaussian blur effect in the model.
        /// </summary>
        PostProcessor postProcessor;

        /// <summary>
        /// The title size.
        /// </summary>
        private Vector2 _titleSize;

        /// <summary>
        /// The title position.
        /// </summary>
        private Vector2 _titlePosition;

        /// <summary>
        /// The menu title.
        /// </summary>
        private string _menuTitle;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>BackgroundScreen</c> class.
        /// </summary>
        /// <param name="name">Name of the <c>GameScreen</c>.</param>
        public BackgroundScreen(string name)
        {
            GameGraphicsDevice = EngineManager.GameGraphicsDevice;

            Name = name;

            TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            TransitionOffTime = TimeSpan.FromSeconds(0.5f);

            _menuTitle = "MetalSpace";
            _titleSize = ScreenManager.FontTitle.MeasureString(_menuTitle);
            Vector2 titleCenter = new Vector2(GameSettings.DefaultInstance.ResolutionWidth / 2.5f, 75f);
            _titlePosition = new Vector2(50f, titleCenter.Y - (_titleSize.Y / 2));

            _individualTransformations = new Dictionary<string, Matrix>();
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load and calculate all the necessary content of the game.
        /// </summary>
        public override void Load()
        {
            base.Load();
            
            // Set the camera
            CameraManager.AddCamera(CameraManager.DefaultCamera,
                new TargetCamera(new Vector3(0, 0, 70), new Vector3(0, 0, 0)));
            CameraManager.SetActiveCamera(CameraManager.DefaultCamera);

            // Create the post processing elements
            renderCapture = new RenderCapture();
            postProcessor = new GaussianBlur(10);//2f);

            // Load all needed textures
            TextureManager.AddTexture(backgroundTexture, new GameTexture("Content/Textures/MenuBackground", true));
            TextureManager.AddTexture("DeadBackground", new GameTexture("Content/Textures/DeadBackground", true));
            TextureManager.AddTexture("SeparationBar", new GameTexture("Content/Textures/SeparationBar", true));
            TextureManager.AddTexture("SelectedMenuEntry", new GameTexture("Content/Textures/SelectedMenuEntry", true));
            TextureManager.AddTexture("DummyTexture15T", new GameTexture("Content/Textures/DummyTexture15T", true));
            TextureManager.AddTexture("DummyTexture", new GameTexture("Content/Textures/DummyTexture", true));
            TextureManager.AddTexture("BarTexture", new GameTexture("Content/Textures/BarTexture", true));
            TextureManager.AddTexture("DialogBackground", new GameTexture("Content/Textures/DialogBackground", true));
            TextureManager.AddTexture("EasyDifficult", new GameTexture("Content/Textures/EasyDifficult", true));
            TextureManager.AddTexture("NormalDifficult", new GameTexture("Content/Textures/NormalDifficult", true));
            TextureManager.AddTexture("HardDifficult", new GameTexture("Content/Textures/HardDifficult", true));
            TextureManager.AddTexture("LoadingBackground", new GameTexture("Content/Textures/LoadingBackground", true));

            // Load all needed sounds
            SoundManager.AddSound("Menu", new GameSound("Content/Sounds/Menu", true));
            SoundManager.GetSound("Menu").Volume = GameSettings.DefaultInstance.MusicVolume;
            SoundManager.GetSound("Menu").Play(true, true);

            SoundManager.AddSound("MenuSelect", new GameSound("Content/Sounds/MenuSelect", true));
            SoundManager.GetSound("MenuSelect").Volume = GameSettings.DefaultInstance.SoundVolume;
            SoundManager.AddSound("MenuAccept", new GameSound("Content/Sounds/MenuAccept", true));
            SoundManager.GetSound("MenuAccept").Volume = GameSettings.DefaultInstance.SoundVolume;

            // Load all needed models
            ModelManager.AddModel(spacecraftModel, new GameModel("Content/Models/Spacecraft", true));
            ModelManager.AddModel("AnimatedMonster", new GameModel("Content/Models/AnimatedMonster", true));
            _model = new DrawableModel((GameModel)ModelManager.GetModel(spacecraftModel),
                new Vector3(-10, 20, 0), new Vector3(-45, 120, 0), new Vector3(1.25f, 1.25f, 1.25f), 0);
            
            // Fix the animation elements
            _animationRing1 = new ObjectAnimation(Vector3.Zero, Vector3.Zero,
                _model.Rotation, _model.Rotation + new Vector3(0, MathHelper.TwoPi, 0),
                TimeSpan.FromSeconds(1f), true);

            _animationRing2 = new ObjectAnimation(Vector3.Zero, Vector3.Zero,
                _model.Rotation + new Vector3(0, MathHelper.Pi, 0),
                _model.Rotation + new Vector3(0, MathHelper.TwoPi + MathHelper.Pi, 0),
                TimeSpan.FromSeconds(1f), true);

            _individualTransformations.Add("polySurface26",
                Matrix.CreateScale(_animationRing1.Scale) *
                Matrix.CreateRotationX(_animationRing1.Rotation.X) *
                Matrix.CreateRotationY(_animationRing1.Rotation.Y) *
                Matrix.CreateRotationZ(_animationRing1.Rotation.Z) *
                Matrix.CreateTranslation(_animationRing1.Position));

            _individualTransformations.Add("polySurface27",
                Matrix.CreateScale(_animationRing2.Scale) *
                Matrix.CreateRotationX(_animationRing2.Rotation.X) *
                Matrix.CreateRotationY(_animationRing2.Rotation.Y) *
                Matrix.CreateRotationZ(_animationRing2.Rotation.Z) *
                Matrix.CreateTranslation(_animationRing2.Position));
        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload the necessary elements of the background.
        /// </summary>
        public override void Unload()
        {
            base.Unload();

            SoundManager.GetSound("Menu").Stop();
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the state of the background screen.
        /// </summary>
        /// <param name="gameTime">Current time of the game.</param>
        /// <param name="otherScreenHasFocus">true if other screen has the focus, false otherwise.</param>
        /// <param name="coveredByOtherScreen">true if other screen cover this screen, false otherwise.</param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            _animationRing1.Update(gameTime.ElapsedGameTime);
            _animationRing2.Update(gameTime.ElapsedGameTime);

            _individualTransformations["polySurface26"] = Matrix.CreateRotationY(_animationRing1.Rotation.Y);
            _individualTransformations["polySurface27"] = Matrix.CreateRotationY(_animationRing2.Rotation.Y);
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the background screen.
        /// </summary>
        /// <param name="gameTime">Current time of the game.</param>
        public override void Draw(GameTime gameTime)
        {
            Viewport viewport = EngineManager.GameGraphicsDevice.Viewport;

            // Change the RenderTarget to the render capture to draw the model
            renderCapture.BeginRender();
            GameGraphicsDevice.Clear(Color.Transparent);

            GameGraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GameGraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap; 

            GameGraphicsDevice.BlendState = BlendState.AlphaBlend;
            GameGraphicsDevice.DepthStencilState = DepthStencilState.Default;

            _model.Draw(CameraManager.ActiveCamera.View, CameraManager.ActiveCamera.Projection, 
                DrawingMethod.NoInstancing, _individualTransformations, null);

            postProcessor.Input = renderCapture.GetTexture();

            // Aply the post processing effect
            renderCapture.InitEffect();
            postProcessor.Draw();
            renderCapture.EndEffect();

            renderCapture.EndRender();
            
            // Draw the background image
            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullCounterClockwise, null, CanvasManager.ScaleMatrix);
            ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture(backgroundTexture).BaseTexture as Texture2D,
                new Rectangle(0, 0, GameSettings.DefaultInstance.ResolutionWidth, GameSettings.DefaultInstance.ResolutionHeight),
                new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            // Draw the title
            ScreenManager.SpriteBatch.DrawString(ScreenManager.FontTitle, _menuTitle, _titlePosition, Color.White);
            ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("SeparationBar").BaseTexture,
                new Rectangle((int)_titlePosition.X, (int)(_titlePosition.Y + _titleSize.Y),
                    (int)_titleSize.X, 5),
                Color.White);

            ScreenManager.SpriteBatch.End();

            // Join the background and the model
            ScreenManager.SpriteBatch.Begin();

            if(GameSettings.DefaultInstance.HighDetail)
                ScreenManager.SpriteBatch.Draw(renderCapture._finalTarget, 
                    new Rectangle(0, 0, viewport.Width, viewport.Height), Color.White);
            else
                ScreenManager.SpriteBatch.Draw(renderCapture._renderTarget, 
                    new Rectangle(0, 0, viewport.Width, viewport.Height), Color.White);
            
            ScreenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }

        public void StartAnimation()
        {

        }

        #endregion
    }
}