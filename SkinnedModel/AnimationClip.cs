using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SkinnedModel
{
    /// <summary>
    /// The <c>AnimationClip</c> class  holds all the keyframes needed to describe a single animation.
    /// </summary>
    public class AnimationClip
    {
        #region Properties

        /// <summary>
        /// Duration property
        /// </summary>
        /// <value>
        /// Duration of the animation.
        /// </value>
        [ContentSerializer]
        public TimeSpan Duration { get; private set; }

        /// <summary>
        /// Keyframes property
        /// </summary>
        /// <value>
        /// List of keyframes.
        /// </value>
        [ContentSerializer]
        public List<Keyframe> Keyframes { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>AnimationClip</c> class.
        /// </summary>
        private AnimationClip() { }

        /// <summary>
        /// Constructor of the <c>AnimationClip</c> class.
        /// </summary>
        /// <param name="duration">Duration of the animation.</param>
        /// <param name="keyframes">List of keyframes.</param>
        public AnimationClip(TimeSpan duration, List<Keyframe> keyframes)
        {
            Duration = duration;
            Keyframes = keyframes;
        }

        #endregion
    }
}
