using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SkinnedModel
{
    /// <summary>
    /// The <c>Keyframe</c> class describes the position of a single bone at 
    /// a single point in time.
    /// </summary>
    public class Keyframe
    {
        #region Properties

        /// <summary>
        /// Bone property
        /// </summary>
        /// <value>
        /// Index of the target bone that is animated by this keyframe.
        /// </value>
        [ContentSerializer]
        public int Bone { get; private set; }

        /// <summary>
        /// Time property
        /// </summary>
        /// <value>
        /// Time offset from the start of the animation to this keyframe.
        /// </value>
        [ContentSerializer]
        public TimeSpan Time { get; private set; }

        /// <summary>
        /// Transform property
        /// </summary>
        /// <value>
        /// Bone transform for this keyframe.
        /// </value>
        [ContentSerializer]
        public Matrix Transform { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Private constructor for use by the XNB deserializer.
        /// </summary>
        private Keyframe() { }

        /// <summary>
        /// Constructs a new keyframe object.
        /// </summary>
        public Keyframe(int bone, TimeSpan time, Matrix transform)
        {
            Bone = bone;
            Time = time;
            Transform = transform;
        }

        #endregion
    }
}
