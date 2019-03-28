using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Events;

namespace MetalSpace.Managers
{
    /// <summary>
    /// The <c>CanvasManager</c> class represents the tool that permits to
    /// change the size of the elements (texture, models...) depending of the
    /// relation between the virtual resolution and the window resolution.
    /// </summary>
    static class CanvasManager
    {
        #region Fields

        /// <summary>
        /// Reference to the graphics device manager.
        /// </summary>
        private static GraphicsDeviceManager _deviceManager = null;

        /// <summary>
        /// true if the user uses full screen mode, false otherwise.
        /// </summary>
        private static bool _fullScreen;

        /// <summary>
        /// Window width size.
        /// </summary>
        private static int _width;

        /// <summary>
        /// Window height size.
        /// </summary>
        private static int _height;

        /// <summary>
        /// Internal width used to render the game.
        /// </summary>
        private static int _virtualWidth;

        /// <summary>
        /// Internal height used to render the game.
        /// </summary>
        private static int _virtualHeight;

        /// <summary>
        /// true if it is necessary to change the scale matrix, false otherwise.
        /// </summary>
        private static bool _dirtyMatrix;
        
        /// <summary>
        /// Store for the ScaleMatrix property.
        /// </summary>
        private static Matrix _scaleMatrix;

        #endregion

        #region Properties

        /// <summary>
        /// ScaleMatrix property
        /// </summary>
        /// <value>
        /// Scale matrix to use in the rendering options to scale all the elemets in
        /// the game to the real resolution (between the internal and the windows 
        /// resolution).
        /// </value>
        public static Matrix ScaleMatrix
        {
            get 
            {
                if (_dirtyMatrix)
                    RecreateScaleMatrix();

                return _scaleMatrix;
            }
        }

        #endregion

        #region Init Method

        /// <summary>
        /// Initilize the elements used int he <c>CanvasManager</c>.
        /// </summary>
        /// <param name="deviceManager">Reference to the device manager used in the game.</param>
        public static void Init(ref GraphicsDeviceManager deviceManager)
        {
            _deviceManager = deviceManager;
            _width = _deviceManager.PreferredBackBufferWidth;
            _height = _deviceManager.PreferredBackBufferHeight;
            _dirtyMatrix = true;
            ApplyResolutionSettings();
        }

        #endregion

        #region SetResolution Method

        /// <summary>
        /// Set the window resolution used by the user.
        /// </summary>
        /// <param name="width">Window width resolution.</param>
        /// <param name="height">Window Height resolution.</param>
        /// <param name="fullScreen">true if the user uses full screen mode, false otherwise.</param>
        public static void SetResolution(int width, int height, bool fullScreen)
        {
            _width = width;
            _height = height;
            _fullScreen = fullScreen;

            ApplyResolutionSettings();
        }

        #endregion

        #region SetVirtualResolution Method

        /// <summary>
        /// Set the internal resolution used in the game.
        /// </summary>
        /// <param name="width">Internal width resolution.</param>
        /// <param name="height">Internal height resolution.</param>
        public static void SetVirtualResolution(int width, int height)
        {
            _virtualWidth = width;
            _virtualHeight = height;

            _dirtyMatrix = true;
        }

        #endregion

        #region ApplyResolutionSettings Method

        /// <summary>
        /// Apply the current windows and internal resolutions to the device manager.
        /// </summary>
        private static void ApplyResolutionSettings()
        {
            if (_fullScreen == false)
            {
                if ((_width <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width) &&
                    (_height <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
                {
                    _deviceManager.IsFullScreen = _fullScreen;
                    _deviceManager.PreferredBackBufferWidth = _width;
                    _deviceManager.PreferredBackBufferHeight = _height;
                    _deviceManager.ApplyChanges();
                }
            }
            else
            {
                foreach(DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                {
                    if ((dm.Width == _width) && (dm.Height == _height))
                    {
                        _deviceManager.IsFullScreen = _fullScreen;
                        _deviceManager.PreferredBackBufferWidth = _width;
                        _deviceManager.PreferredBackBufferHeight = _height;
                        _deviceManager.ApplyChanges();
                    }
                }
            }

            _dirtyMatrix = true;

            _width = _deviceManager.PreferredBackBufferWidth;
            _height = _deviceManager.PreferredBackBufferHeight;
        }

        #endregion

        #region BeginDraw Method

        /// <summary>
        /// Set the device manager to the correct resolutions parameters.
        /// </summary>
        public static void BeginDraw()
        {
            FullViewport();

            _deviceManager.GraphicsDevice.Clear(Color.Black);

            ResetViewport();

            _deviceManager.GraphicsDevice.Clear(Color.Black);
        }

        #endregion

        #region RecreateScaleMatrix Method

        /// <summary>
        /// Calculate the <c>ScaleMatrix</c> to permits the scale depending of the
        /// difference between the windows and internal resolutions.
        /// </summary>
        private static void RecreateScaleMatrix()
        {
            _dirtyMatrix = false;
            _scaleMatrix = Matrix.CreateScale(
                (float)_deviceManager.GraphicsDevice.Viewport.Width / _virtualWidth,
                (float)_deviceManager.GraphicsDevice.Viewport.Height / _virtualHeight,
                1f);
        }

        #endregion

        #region FullViewport Method

        /// <summary>
        /// Set the device manager with the windows width and height.
        /// </summary>
        private static void FullViewport()
        {
            Viewport viewport = new Viewport();
            viewport.X = viewport.Y = 0;
            viewport.Width = _width;
            viewport.Height = _height;
            _deviceManager.GraphicsDevice.Viewport = viewport;
        }

        #endregion

        #region GetVirtualAspectRatio Method

        /// <summary>
        /// Get the aspect ratio between the internal width and height.
        /// </summary>
        /// <returns>Aspect ratio between the internal width and height.</returns>
        public static float GetVirtualAspectRatio()
        {
            return (float)_virtualWidth / (float)_virtualHeight;
        }

        #endregion

        #region ResetViewport Method

        /// <summary>
        /// Reset the device manager with the current windows width and height and
        /// the internal width and height.
        /// </summary>
        public static void ResetViewport()
        {
            float targetAspectRatio = GetVirtualAspectRatio();

            bool changed = false;
            int width = _deviceManager.PreferredBackBufferWidth;
            int height = (int)(width / targetAspectRatio + 0.5f);

            if (height > _deviceManager.PreferredBackBufferHeight)
            {
                height = _deviceManager.PreferredBackBufferHeight;
                width = (int)(height * targetAspectRatio + 0.5f);
                changed = true;
            }

            Viewport viewport = new Viewport();

            viewport.X = (_deviceManager.PreferredBackBufferWidth / 2) - (width / 2);
            viewport.Y = (_deviceManager.PreferredBackBufferHeight / 2) - (height / 2);
            viewport.Width = width;
            viewport.Height = height;
            viewport.MinDepth = 0;
            viewport.MaxDepth = 1;

            if (changed)
                _dirtyMatrix = true;

            _deviceManager.GraphicsDevice.Viewport = viewport;
        }

        #endregion
    }
}