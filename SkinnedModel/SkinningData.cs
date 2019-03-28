using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SkinnedModel
{
    /// <summary>
    /// Combines all the data needed to render and animate a skinned object.
    /// This is typically stored in the Tag property of the Model being animated.
    /// </summary>
    public class SkinningData
    {
        #region Properties

        /// <summary>
        /// AnimationClips property
        /// </summary>
        /// <value>
        /// Collection of animation clips stored by name in a dictionary.
        /// </value>
        [ContentSerializer]
        public Dictionary<string, AnimationClip> AnimationClips { get; private set; }


        /// <summary>
        /// BindPose property
        /// </summary>
        /// <value>
        /// Bindpose matrices for each bone in the skeleton, relative to the parent bone.
        /// </value>
        [ContentSerializer]
        public List<Matrix> BindPose { get; private set; }


        /// <summary>
        /// InverseBindPose property
        /// </summary>
        /// <value>
        /// Vertex to bonespace transforms for each bone in the skeleton.
        /// </value>
        [ContentSerializer]
        public List<Matrix> InverseBindPose { get; private set; }


        /// <summary>
        /// SkeletonHierarchy property
        /// </summary>
        /// <value>
        /// For each bone in the skeleton, stores the index of the parent bone.
        /// </value>
        [ContentSerializer]
        public List<int> SkeletonHierarchy { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>SkinningData</c> property.
        /// </summary>
        private SkinningData()
        {
        }

        /// <summary>
        /// Constructor of the <c>SkinningData</c> property.
        /// </summary>
        /// <param name="animationClips">List of animations clips stored by name.</param>
        /// <param name="bindPose">Bindpose matrices for each bone in the skeleton, relative to the parent bone.</param>
        /// <param name="inverseBindPose">Vertex to bonespace transforms for each bone in the skeleton.</param>
        /// <param name="skeletonHierarchy">For each bone in the skeleton, the index of the parent bone.</param>
        public SkinningData(Dictionary<string, AnimationClip> animationClips,
                            List<Matrix> bindPose, List<Matrix> inverseBindPose,
                            List<int> skeletonHierarchy)
        {
            AnimationClips = animationClips;
            BindPose = bindPose;
            InverseBindPose = inverseBindPose;
            SkeletonHierarchy = skeletonHierarchy;
        }

        #endregion
    }
}
