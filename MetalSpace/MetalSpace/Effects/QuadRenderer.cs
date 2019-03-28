using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Managers;

namespace MetalSpace.Effects
{
    /// <summary>
    /// The <c>QuadRenderer</c> class permits to render full screen
    /// quads VertexPositionTexture
    /// </summary>
    class QuadRenderer
    {
        #region Private Members    
        
        /// <summary>
        /// Indexes of the quad vertices.
        /// </summary>
        private short[] ib;

        /// <summary>
        /// Vertices that represent the quad.
        /// </summary>
        private VertexPositionTexture[] verts;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>QuadRenderer</c> class.
        /// </summary>
        public QuadRenderer() { }

        #endregion

        #region LoadContent Method

        /// <summary>
        /// Load the quad.
        /// </summary>
        public void LoadContent()
        {
            verts = new VertexPositionTexture[]
            {
                new VertexPositionTexture(new Vector3(0,0,0), new Vector2(1,1)),
                new VertexPositionTexture(new Vector3(0,0,0), new Vector2(0,1)),
                new VertexPositionTexture(new Vector3(0,0,0), new Vector2(0,0)),
                new VertexPositionTexture(new Vector3(0,0,0), new Vector2(1,0))
            };

            ib = new short[] { 0, 1, 2, 2, 3, 0 };

        } 
        #endregion

        #region Render Method

        /// <summary>
        /// Render the quad.
        /// </summary>
        /// <param name="v1">Vector.One * -1</param>
        /// <param name="v2">Vector.One</param>
        public void Render(Vector2 v1, Vector2 v2)
        {
            verts[0].Position.X = v2.X;
            verts[0].Position.Y = v1.Y;

            verts[1].Position.X = v1.X;
            verts[1].Position.Y = v1.Y;

            verts[2].Position.X = v1.X;
            verts[2].Position.Y = v2.Y;

            verts[3].Position.X = v2.X;
            verts[3].Position.Y = v2.Y;

            EngineManager.GameGraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>
                (PrimitiveType.TriangleList, verts, 0, 4, ib, 0, 2);
        }

        #endregion
    }
}
