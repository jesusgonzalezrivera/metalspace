using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Sound;
using MetalSpace.Models;
using MetalSpace.Objects;
using MetalSpace.Textures;
using MetalSpace.Managers;
using MetalSpace.Interfaces;
using MetalSpace.Events;
using MetalSpace.Settings;
using MetalSpace.Scene;
using MetalSpace.Cameras;

namespace MetalSpace.GameScreens
{
    class ChangingGame2 : GameScreen
    {
        #region Fields

        private Player _player = null;
        private Vector3 _newPosition = Vector3.Zero;

        private string _originMapName;
        private string _nextMapName;
        private Dictionary<string, string> _mapInformation;

        private Vector3 _distanceDoors;

        private Vector3 _centralPointSourceDoor;
        private Vector3 _centralPointDestinationDoor;
        private Vector3 _distanceSourceDoor;
        private Vector3 _distanceDestinationDoor;

        private bool _moving;
        private Vector3 _end;
        private Vector3 _start;
        private Vector2 _speed;

        #endregion

        #region Constructor

        public ChangingGame2(string originalMapName, SceneRenderer originalScene, Player player, Vector3 newPosition, string mapName)
        {
            GameGraphicsDevice = EngineManager.GameGraphicsDevice;

            _originMapName = originalMapName;
            _nextMapName = mapName;
            _mapInformation = FileHelper.readMapInformation(mapName);

            _newPosition = newPosition;
            _player = player;
        }

        #endregion

        #region Load Method

        public override void Load()
        {
            base.Load();

            //LevelManager.GetLevel(_nextMapName).Load();

            // Get distance between doors
            foreach (Door door in LevelManager.GetLevel(_originMapName).MainLayer.Doors)
                if (door.nextLevel == _nextMapName)
                    _distanceDoors = door.boundingBox.Min;

            foreach (Door door in LevelManager.GetLevel(_nextMapName).MainLayer.Doors)
                if (door.nextLevel == _originMapName)
                    _distanceDoors -= door.boundingBox.Min;

            if (_distanceDoors.X > 0)
                _distanceDoors = new Vector3(_distanceDoors.X + 3, _distanceDoors.Y, 0);
            else
                _distanceDoors = new Vector3(_distanceDoors.X - 3, _distanceDoors.Y, 0);

            // Move the new scene
            LevelManager.GetLevel(_nextMapName).Move(_distanceDoors);

            // Calculate the central point of the doors
            foreach (Door door in LevelManager.GetLevel(_originMapName).MainLayer.Doors)
                if (door.nextLevel == _nextMapName)
                    _centralPointSourceDoor = ((door.boundingBox.Max - door.boundingBox.Min) / 2f) + door.boundingBox.Min;

            foreach (Door door in LevelManager.GetLevel(_nextMapName).MainLayer.Doors)
                if (door.nextLevel == _originMapName)
                    _centralPointDestinationDoor = ((door.boundingBox.Max - door.boundingBox.Min) / 2f) + door.boundingBox.Min;

            // Calculate the distance between the player and the doors
            _distanceSourceDoor = _centralPointSourceDoor - _player.DModel.BSphere.Center;
            foreach (Door door in LevelManager.GetLevel(_originMapName).MainLayer.Doors)
                if (door.nextLevel == _nextMapName)
                    _distanceDestinationDoor = door.nextPosition - _centralPointDestinationDoor;

            // Calculate start and end points
            CameraManager.ActiveCamera = new FreeCamera(
                new Vector3(_player.Position.X, _player.Position.Y + 1f, 17f), 0, 0);

            _start = CameraManager.ActiveCamera.Position;
            //_end = CameraManager.ActiveCamera.Position + _distanceSourceDoor + _distanceDestinationDoor;
            //_end = new Vector3(_end.X, _end.Y, 17f);
            foreach (Door door in LevelManager.GetLevel(_originMapName).MainLayer.Doors)
                if (door.nextLevel == _nextMapName)
                {
                    if (_distanceDoors.X > 0)
                        _end = new Vector3(door.nextPosition.X + _distanceDoors.X, door.nextPosition.Y + _distanceDoors.Y + 1f, 17f);
                    else
                        _end = new Vector3(door.nextPosition.X + _distanceDoors.X, door.nextPosition.Y + _distanceDoors.Y + 1f, 17f);
                }

            // Calculate the speed of the movement
            _moving = true;
            _speed = Vector2.Normalize(new Vector2(_end.X - _start.X, _end.Y - _start.Y));

            SoundManager.StopAllSounds();
            //EngineManager.Game.IsFixedTimeStep = true;

            base.Load();
        }

        #endregion

        #region Unload Method

        public override void Unload()
        {
            base.Unload();
        }

        #endregion

        #region HandleInput Method

        public override void HandleInput(GameComponents.Input input, GameTime gameTime)
        {
            base.HandleInput(input, gameTime);

            /*((FreeCamera)CameraManager.ActiveCamera).Rotate(input.MouseMoved.X * 0.01f, input.MouseMoved.Y * 0.01f);

            Vector3 moveVector = Vector3.Zero;
            if (input.Up) moveVector += Vector3.Forward;
            if (input.Down) moveVector += Vector3.Backward;
            if (input.Right) moveVector += Vector3.Left;
            if (input.Left) moveVector += Vector3.Right;
            moveVector *= ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f) * 30.0f;

            ((FreeCamera)CameraManager.ActiveCamera).Move(moveVector);*/
        }

        #endregion

        #region Update Method

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            ((FreeCamera) CameraManager.ActiveCamera).Update();

            if (_moving)
            {
                float time = (float)(gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
                CameraManager.ActiveCamera.Position += Vector3.Multiply(
                    new Vector3(_speed.X, _speed.Y, 0), 5f * time);
            }

            Vector3 _currentPosition = CameraManager.ActiveCamera.Position;

            if (Vector3.Distance(_start, _currentPosition) >= Vector3.Distance(_start, _end))
            {
                //LevelManager.GetLevel(_originMapName).Unload();
                //LevelManager.GetLevel(_originMapName) = null;

                _moving = false;
                LevelManager.GetLevel(_nextMapName).Move(-_distanceDoors);
                EngineManager.Game.IsFixedTimeStep = false;
                ScreenManager.RemoveScreen("LoadingScreen");
                ScreenManager.AddScreen("ContinueGame", new MainGameScreen(
                    "ContinueGame", _nextMapName, _mapInformation, _player, _newPosition, LevelManager.GetLevel(_nextMapName)));
            }
        }

        #endregion

        #region Draw Method

        public override void Draw(GameTime gameTime)
        {
            GameGraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GameGraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GameGraphicsDevice.BlendState = BlendState.Opaque;

            GameGraphicsDevice.BlendState = BlendState.AlphaBlend;
            GameGraphicsDevice.DepthStencilState = DepthStencilState.Default;

            ScreenManager.SpriteBatch.Begin();

            LevelManager.GetLevel(_originMapName).Draw(gameTime, null, false);
            LevelManager.GetLevel(_nextMapName).Draw(gameTime, null, false);

            ScreenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}
