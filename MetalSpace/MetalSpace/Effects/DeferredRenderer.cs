using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Scene;
using MetalSpace.Events;
using MetalSpace.Cameras;
using MetalSpace.Managers;

namespace MetalSpace.Effects
{
    /// <summary>
    /// <c>PointLight</c> struct that represent a point light
    /// </summary>
    public struct PointLight
    {
        /// <summary>
        /// Position of the point light.
        /// </summary>
        public Vector3 lightPosition;

        /// <summary>
        /// Color of the point light.
        /// </summary>
        public Color color;

        /// <summary>
        /// Radius of the point light.
        /// </summary>
        public float lightRadius;

        /// <summary>
        /// Intensity of the point light.
        /// </summary>
        public float LightIntensity;
    }

    /// <summary>
    /// <c>SpotLight</c> struct that represent a spot light
    /// </summary>
    public struct SpotLight
    {
        /// <summary>
        /// Position of the spot light.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Strength of the spot light.
        /// </summary>
        public float Strength;

        /// <summary>
        /// Direction of the spot light.
        /// </summary>
        public Vector3 Direction;

        /// <summary>
        /// Cone angle of the spot light.
        /// </summary>
        public float ConeAngle;

        /// <summary>
        /// Cone delay of the spot light.
        /// </summary>
        public float ConeDelay;
    }

    /// <summary>
    /// The <c>DeferredRenderer</c> class represents a renderer that applies
    /// different types of lights to the game scene.
    /// </summary>
    class DeferredRenderer
    {
        #region Fields

        /// <summary>
        /// QuadRenderer that render a full screen quad.
        /// </summary>
        private QuadRenderer _quadRenderer;

        /// <summary>
        /// RenderTarget2D that render the color channel.
        /// </summary>
        private RenderTarget2D _colorTarget;

        /// <summary>
        /// RenderTarget2D that render the normal channel.
        /// </summary>
        private RenderTarget2D _normalTarget;

        /// <summary>
        /// RenderTarget2D that render the depth channel.
        /// </summary>
        private RenderTarget2D _depthTarget;

        /// <summary>
        /// RenderTarget2D that render the lights.
        /// </summary>
        private RenderTarget2D _lightsTarget;

        /// <summary>
        /// Half pixel for the render.
        /// </summary>
        private Vector2 _halfPixel;

        /// <summary>
        /// Effect that render in the G buffer.
        /// </summary>
        public Effect _renderGBuffer;

        /// <summary>
        /// Effect that render the behaviour of a directional light.
        /// </summary>
        private Effect _directionalLightEffect;

        /// <summary>
        /// Effect that render the behaviour of a point light.
        /// </summary>
        private Effect _pointLightEffect;

        /// <summary>
        /// Effect that render the behaviour of a spot light.
        /// </summary>
        private Effect _spotLightEffect;

        /// <summary>
        /// Effect that combine all the effects.
        /// </summary>
        private Effect _finalCombineEffect;
        
        /// <summary>
        /// List of point lights to be applied.
        /// </summary>
        private PointLight[] _pointLights;

        /// <summary>
        /// List of spot lights to be applied.
        /// </summary>
        private SpotLight[] _spotLights;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>DeferredRenderer</c> class.
        /// </summary>
        public DeferredRenderer()
        {
            _quadRenderer = new QuadRenderer();
        }

        #endregion

        #region Load Content

        /// <summary>
        /// Load all the neccessary elements of the renderer, including render targets,
        /// point and spot lights.
        /// </summary>
        public void LoadContent()
        {
            // Calculate the halfPixel value
            _halfPixel = new Vector2()
            {
                X = 0.5f / (float)EngineManager.GameGraphicsDevice.PresentationParameters.BackBufferWidth,
                Y = 0.5f / (float)EngineManager.GameGraphicsDevice.PresentationParameters.BackBufferHeight
            };

            _quadRenderer.LoadContent();

            // Create the Render Targets in which will render the objects
            PresentationParameters pp = EngineManager.GameGraphicsDevice.PresentationParameters;
            int width = pp.BackBufferWidth;
            int height = pp.BackBufferHeight;
            SurfaceFormat format = pp.BackBufferFormat;

            _colorTarget = new RenderTarget2D(EngineManager.GameGraphicsDevice, width, height, false,
                SurfaceFormat.Color, pp.DepthStencilFormat, pp.MultiSampleCount,
                RenderTargetUsage.PreserveContents);
            _normalTarget = new RenderTarget2D(EngineManager.GameGraphicsDevice, width, height, false,
                SurfaceFormat.Color, pp.DepthStencilFormat, pp.MultiSampleCount,
                RenderTargetUsage.PreserveContents);
            _depthTarget = new RenderTarget2D(EngineManager.GameGraphicsDevice, width, height, false,
                SurfaceFormat.Single, pp.DepthStencilFormat, pp.MultiSampleCount,
                RenderTargetUsage.PreserveContents);
            _lightsTarget = new RenderTarget2D(EngineManager.GameGraphicsDevice, width, height, false,
                SurfaceFormat.Color, pp.DepthStencilFormat, pp.MultiSampleCount,
                RenderTargetUsage.PreserveContents);

            // Load the effects to be used in the deferred rendering
            _renderGBuffer = EngineManager.ContentManager.Load<Effect>("Content/Effects/RenderGBuffer");
            _directionalLightEffect = EngineManager.ContentManager.Load<Effect>("Content/Effects/DirectionalLight");
            _finalCombineEffect = EngineManager.ContentManager.Load<Effect>("Content/Effects/CombineFinal");
            _pointLightEffect = EngineManager.ContentManager.Load<Effect>("Content/Effects/PointLight");

            // Create the point lights in the scene
            _pointLights = new PointLight[4];

            _pointLights[0] = new PointLight();
            _pointLights[0].lightPosition = new Vector3(0, 10, 5);
            _pointLights[0].color = Color.White;
            _pointLights[0].lightRadius = 15;
            _pointLights[0].LightIntensity = 5;

            _pointLights[1] = new PointLight();
            _pointLights[1].lightPosition = new Vector3(10, 10, 5);
            _pointLights[1].color = Color.White;
            _pointLights[1].lightRadius = 15;
            _pointLights[1].LightIntensity = 5;

            _pointLights[2] = new PointLight();
            _pointLights[2].lightPosition = new Vector3(20, 10, 5);
            _pointLights[2].color = Color.White;
            _pointLights[2].lightRadius = 15;
            _pointLights[2].LightIntensity = 5;

            _pointLights[3] = new PointLight();
            _pointLights[3].lightPosition = new Vector3(25, 25, -25);
            _pointLights[3].color = Color.White;
            _pointLights[3].lightRadius = 50;
            _pointLights[3].LightIntensity = 50;

            // Create the spot lights in the scene
            _spotLights = new SpotLight[3];

            _spotLights[0] = new SpotLight();
            _spotLights[0].Position = new Vector3(0, 10, 5);
            _spotLights[0].Direction = new Vector3(0, -1, -1);
            _spotLights[0].Strength = 0.7f;
            _spotLights[0].ConeAngle = 0.5f;
            _spotLights[0].ConeDelay = 2.0f;

            _spotLights[1] = new SpotLight();
            _spotLights[1].Position = new Vector3(10, 10, 0);
            _spotLights[1].Direction = new Vector3(10, 0, 0);
            _spotLights[1].Strength = 0.7f;
            _spotLights[1].ConeAngle = 0.5f;
            _spotLights[1].ConeDelay = 2.0f;

            _spotLights[2] = new SpotLight();
            _spotLights[2].Position = new Vector3(20, 10, 0);
            _spotLights[2].Direction = new Vector3(20, 0, 0);
            _spotLights[2].Strength = 0.7f;
            _spotLights[2].ConeAngle = 0.5f;
            _spotLights[2].ConeDelay = 2.0f;
        }

        #endregion

        #region Unload Content

        /// <summary>
        /// Unload all the elements used in the Renderer.
        /// </summary>
        public void UnloadContent()
        {
            _colorTarget.Dispose();
            _colorTarget = null;

            _normalTarget.Dispose();
            _normalTarget = null;

            _depthTarget.Dispose();
            _depthTarget = null;

            _lightsTarget.Dispose();
            _lightsTarget = null;

            _renderGBuffer.Dispose();
            _renderGBuffer = null;

            _directionalLightEffect.Dispose();
            _directionalLightEffect = null;

            _finalCombineEffect.Dispose();
            _finalCombineEffect = null;

            _pointLightEffect.Dispose();
            _pointLightEffect = null;

            _pointLights = null;
            _spotLights = null;
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the lights (not neccessary).
        /// </summary>
        /// <param name="gameTime">Current time of the game.</param>
        public void Update(GameTime gameTime)
        {

        }

        #endregion

        #region Draw Methods

        /// <summary>
        /// Change the current RenderTarget for start the drawing.
        /// </summary>
        public void BeginDraw()
        {
            EngineManager.GameGraphicsDevice.SetRenderTargets(_colorTarget, _normalTarget, _depthTarget);
            EngineManager.GameGraphicsDevice.Clear(Color.Black);
        }

        /// <summary>
        /// Finish the draw effect proccess.
        /// 
        /// Apply the lights to the scene, combine the effects and restore
        /// the default RenderTarget.
        /// </summary>
        public void EndDraw()
        {
            // Restore the default RenderTarget
            EngineManager.GameGraphicsDevice.SetRenderTargets(null);

            // Draw the lights
            DrawLights();

            // Combine the colors and lights
            CombineColorAndLights();

            int halfWidth = EngineManager.GameGraphicsDevice.Viewport.Width / 3;
            int halfHeight = EngineManager.GameGraphicsDevice.Viewport.Height / 3;

            for (int i = 0; i < 8; i++)
                EngineManager.GameGraphicsDevice.SamplerStates[i] = SamplerState.PointClamp;

            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointWrap,
                DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, CanvasManager.ScaleMatrix);

            ScreenManager.SpriteBatch.Draw(_colorTarget, new Rectangle(0, 0, halfWidth, halfHeight), Color.White);
            ScreenManager.SpriteBatch.Draw(_normalTarget, new Rectangle(0, halfHeight, halfWidth, halfHeight), Color.White);
            ScreenManager.SpriteBatch.Draw(_depthTarget, new Rectangle(halfWidth, 0, halfWidth, halfHeight), Color.White);
            ScreenManager.SpriteBatch.Draw(_lightsTarget, new Rectangle(halfWidth, halfHeight, halfWidth, halfHeight), Color.White);

            ScreenManager.SpriteBatch.End();
        }

        /// <summary>
        /// Draw the different types of lights in the scene.
        /// </summary>
        public void DrawLights()
        {
            EngineManager.GameGraphicsDevice.SetRenderTarget(_lightsTarget);

            EngineManager.GameGraphicsDevice.Clear(Color.Transparent);
            EngineManager.GameGraphicsDevice.BlendState = BlendState.AlphaBlend;
            EngineManager.GameGraphicsDevice.DepthStencilState = DepthStencilState.None;

            //foreach (SpotLight light in _spotLights)
            //    AddSpotLight(light);

            foreach (PointLight light in _pointLights)
                AddPointLight(light);

            EngineManager.GameGraphicsDevice.BlendState = BlendState.Opaque;
            EngineManager.GameGraphicsDevice.DepthStencilState = DepthStencilState.None;
            EngineManager.GameGraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

            EngineManager.GameGraphicsDevice.SetRenderTarget(null);
        }

        /// <summary>
        /// Add a new point lights to the render.
        /// </summary>
        /// <param name="pointLight">Point light to be added.</param>
        private void AddPointLight(PointLight pointLight)
        {
            // Set the G-Buffer parameters
            _pointLightEffect.Parameters["colorMap"].SetValue(_colorTarget);
            _pointLightEffect.Parameters["normalMap"].SetValue(_normalTarget);
            _pointLightEffect.Parameters["depthMap"].SetValue(_depthTarget);

            // Compute the light world, view and projection matrix
            _pointLightEffect.Parameters["World"].SetValue(
                Matrix.CreateScale(pointLight.lightRadius) * 
                Matrix.CreateTranslation(pointLight.lightPosition));
            _pointLightEffect.Parameters["View"].SetValue(CameraManager.ActiveCamera.View);
            _pointLightEffect.Parameters["Projection"].SetValue(CameraManager.ActiveCamera.Projection);
            _pointLightEffect.Parameters["cameraPosition"].SetValue(CameraManager.ActiveCamera.Position);
            _pointLightEffect.Parameters["InvertViewProjection"].SetValue(
                Matrix.Invert(CameraManager.ActiveCamera.View *
                CameraManager.ActiveCamera.Projection));

            // Set the position, color, radius, intensity and half pixel
            _pointLightEffect.Parameters["lightPosition"].SetValue(pointLight.lightPosition);
            _pointLightEffect.Parameters["Color"].SetValue(pointLight.color.ToVector3());
            _pointLightEffect.Parameters["lightRadius"].SetValue(pointLight.lightRadius);
            _pointLightEffect.Parameters["lightIntensity"].SetValue(pointLight.LightIntensity);
            _pointLightEffect.Parameters["halfPixel"].SetValue(_halfPixel);
            
            // Calculate the distance between the camera and light center
            float cameraToCenter = Vector3.Distance(CameraManager.ActiveCamera.Position, 
                pointLight.lightPosition);
            
            // If we are inside the light volume, draw the sphere's inside face
            if (cameraToCenter < pointLight.lightRadius)
                EngineManager.GameGraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            else
                EngineManager.GameGraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

            EngineManager.GameGraphicsDevice.DepthStencilState = DepthStencilState.None;

            // Draw the point light effect
            _pointLightEffect.Techniques[0].Passes[0].Apply();
            foreach (ModelMesh mesh in ModelManager.GetModel("Sphere").Model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    EngineManager.GameGraphicsDevice.Indices = meshPart.IndexBuffer;
                    EngineManager.GameGraphicsDevice.SetVertexBuffer(meshPart.VertexBuffer);

                    EngineManager.GameGraphicsDevice.DrawIndexedPrimitives(
                        PrimitiveType.TriangleList, 
                        0, 0, 
                        meshPart.NumVertices, 
                        meshPart.StartIndex, 
                        meshPart.PrimitiveCount);
                }
            }

            EngineManager.GameGraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            EngineManager.GameGraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        /// <summary>
        /// Add a new spot light to the render.
        /// </summary>
        /// <param name="spotLight">New spot light to be added.</param>
        public void AddSpotLight(SpotLight spotLight)
        {
            _directionalLightEffect.Parameters["colorMap"].SetValue(_colorTarget);
            _directionalLightEffect.Parameters["normalMap"].SetValue(_normalTarget);
            _directionalLightEffect.Parameters["depthMap"].SetValue(_depthTarget);

            _directionalLightEffect.Parameters["lightDirection"].SetValue(spotLight.Direction);
            _directionalLightEffect.Parameters["Color"].SetValue(Color.White.ToVector3());

            _directionalLightEffect.Parameters["cameraPosition"].SetValue(CameraManager.ActiveCamera.Position);
            _directionalLightEffect.Parameters["InvertViewProjection"].SetValue(
                Matrix.Invert(CameraManager.ActiveCamera.View * CameraManager.ActiveCamera.Projection));

            _directionalLightEffect.Parameters["halfPixel"].SetValue(_halfPixel);

            _directionalLightEffect.Techniques[0].Passes[0].Apply();
            _quadRenderer.Render(Vector2.One * -1, Vector2.One);
        }

        /// <summary>
        /// Combine the color and lights renders.
        /// </summary>
        public void CombineColorAndLights()
        {
            _finalCombineEffect.Parameters["colorMap"].SetValue(_colorTarget);
            _finalCombineEffect.Parameters["lightMap"].SetValue(_lightsTarget);
            _finalCombineEffect.Parameters["halfPixel"].SetValue(_halfPixel);

            _finalCombineEffect.Techniques[0].Passes[0].Apply();  
            _quadRenderer.Render(Vector2.One * -1, Vector2.One);
        }

        #endregion
    }
}
