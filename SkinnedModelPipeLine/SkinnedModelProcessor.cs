using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using SkinnedModel;

namespace SkinnedModelPipeLine
{
    /// <summary>
    /// The <c>SkinnedModelProcessor</c> class represents a custom processor that 
    /// extends the built in framework ModelProcessor class, adding animation support.
    /// </summary>
    [ContentProcessor]
    public class SkinnedModelProcessor : ModelProcessor
    {
        #region Fields
        
        /// <summary>
        /// Store for the DegreesX property.
        /// </summary>
        private float degreesX;

        /// <summary>
        /// Store for the DegreesY property.
        /// </summary>
        private float degreesY;

        /// <summary>
        /// Store for the DegreesZ property.
        /// </summary>
        private float degreesZ;

        /// <summary>
        /// Store for the ScaleMultiple property.
        /// </summary>
        private float scaleMultiple = 1.0f;

        #endregion

        #region Properties

        /// <summary>
        /// DegreesX property
        /// </summary>
        /// <value>
        /// X rotation of the bones and animations.
        /// </value>
        public float DegreesX
        {
            get { return degreesX; }
            set { degreesX = value; }
        }

        /// <summary>
        /// DegreesY property
        /// </summary>
        /// <value>
        /// Y rotation of the bones and animations.
        /// </value>
        public float DegreesY
        {
            get { return degreesY; }
            set { degreesY = value; }
        }

        /// <summary>
        /// DegreesZ property
        /// </summary>
        /// <value>
        /// Z rotation of the bones and animations.
        /// </value>
        public float DegreesZ
        {
            get { return degreesZ; }
            set { degreesZ = value; }
        }

        /// <summary>
        /// ScaleMultiple property
        /// </summary>
        /// <value>
        /// Scale of the bones and animations
        /// </value>
        public float ScaleMultiple
        {
            get { return scaleMultiple; }
            set { scaleMultiple = value; }
        }

        /// <summary>
        /// MergeAnimations property
        /// </summary>
        /// <value>
        /// Names of the differents animations.
        /// </value>
        public string MergeAnimations { get; set; }

        #endregion

        #region Process Method

        /// <summary>
        /// Converts an intermediate format content pipeline NodeContent tree to a 
        /// ModelContent object with embedded animation data.
        /// </summary>
        /// <param name="input">Input information fo the model.</param>
        /// <param name="context">Context of the content processor.</param>
        /// <returns></returns>
        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            if (!string.IsNullOrEmpty(MergeAnimations))
            {
                foreach (string mergeFile in MergeAnimations.Split(';')
                                                            .Select(s => s.Trim())
                                                            .Where(s => !string.IsNullOrEmpty(s)))
                {
                    MergeAnimation(input, context, mergeFile);
                }
            }

            ValidateMesh(input, context, null);

            RotateAll(input, DegreesX, DegreesY, DegreesZ, ScaleMultiple);

            // Find the skeleton.
            BoneContent skeleton = MeshHelper.FindSkeleton(input);

            if (skeleton == null)
                throw new InvalidContentException("Input skeleton not found.");

            // We don't want to have to worry about different parts of the model being
            // in different local coordinate systems, so let's just bake everything.
            FlattenTransforms(input, skeleton);

            // Read the bind pose and skeleton hierarchy data.
            IList<BoneContent> bones = MeshHelper.FlattenSkeleton(skeleton);

            if (bones.Count > SkinnedEffect.MaxBones)
            {
                throw new InvalidContentException(string.Format(
                    "Skeleton has {0} bones, but the maximum supported is {1}.",
                    bones.Count, SkinnedEffect.MaxBones));
            }

            List<Matrix> bindPose = new List<Matrix>();
            List<Matrix> inverseBindPose = new List<Matrix>();
            List<int> skeletonHierarchy = new List<int>();

            foreach (BoneContent bone in bones)
            {
                bindPose.Add(bone.Transform);
                inverseBindPose.Add(Matrix.Invert(bone.AbsoluteTransform));
                skeletonHierarchy.Add(bones.IndexOf(bone.Parent as BoneContent));
            }

            // Convert animation data to our runtime format.
            Dictionary<string, AnimationClip> animationClips;
            animationClips = ProcessAnimations(skeleton.Animations, bones);

            // Chain to the base ModelProcessor class so it can convert the model data.
            ModelContent model = base.Process(input, context);

            // Store our custom animation data in the Tag property of the model.
            model.Tag = new SkinningData(animationClips, bindPose,
                                         inverseBindPose, skeletonHierarchy);

            return model;
        }

        #endregion

        #region RotateAll Method

        /// <summary>
        /// Rotate the bones with the rotation specified by the user.
        /// </summary>
        /// <param name="node">Current node to be rotated.</param>
        /// <param name="degX">X rotation of the bones and animations.</param>
        /// <param name="degY">Y rotation of the bones and animations.</param>
        /// <param name="degZ">Z rotation of the bones and animations.</param>
        /// <param name="scaleFactor">Scale of the bones and animations.</param>
        public void RotateAll(NodeContent node, float degX, float degY, float degZ, float scaleFactor)
        {
            Matrix rotate = Matrix.Identity *
                Matrix.CreateRotationX(MathHelper.ToRadians(degX)) *
                Matrix.CreateRotationY(MathHelper.ToRadians(degY)) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(degZ));
            
            Matrix transform = Matrix.Identity * Matrix.CreateScale(scaleFactor) * rotate;
            MeshHelper.TransformScene(node, transform);
        }

        #endregion

        #region MergeAnimation Method

        /// <summary>
        /// Merge the different animations of the model.
        /// </summary>
        /// <param name="input">Input information fo the model.</param>
        /// <param name="context">Context of the content processor.</param>
        /// <param name="mergeFile">Name of the file that contains the merge animations.</param>
        void MergeAnimation(NodeContent input, ContentProcessorContext context, string mergeFile)
        {
            NodeContent mergeModel = context.BuildAndLoadAsset<NodeContent, NodeContent>(
                                                new ExternalReference<NodeContent>("Models/" + mergeFile), null);

            BoneContent rootBone = MeshHelper.FindSkeleton(input);

            if (rootBone == null)
            {
                context.Logger.LogWarning(null, input.Identity, "Source model has no root bone.");
                return;
            }

            BoneContent mergeRoot = MeshHelper.FindSkeleton(mergeModel);

            if (mergeRoot == null)
            {
                context.Logger.LogWarning(null, input.Identity, "Merge model '{0}' has no root bone.", mergeFile);
                return;
            }

            foreach (string animationName in mergeRoot.Animations.Keys)
            {
                if (rootBone.Animations.ContainsKey(animationName))
                {
                    context.Logger.LogWarning(null, input.Identity,
                        "Cannot merge animation '{0}' from '{1}', because this animation already exists.",
                        animationName, mergeFile);

                    continue;
                }

                context.Logger.LogImportantMessage("Merging animation '{0}' from '{1}'.", animationName, mergeFile);

                rootBone.Animations.Add(animationName, mergeRoot.Animations[animationName]);
            }
        }

        #endregion

        #region ProcessAnimations Method

        /// <summary>
        /// Converts an intermediate format content pipeline AnimationContentDictionary
        /// object to our runtime AnimationClip format.
        /// </summary>
        /// <param name="animations">Animation content dictionary.</param>
        /// <param name="bones">List of bones.</param>
        /// <returns>Dictionary of animation clips ordered by name.</returns>
        static Dictionary<string, AnimationClip> ProcessAnimations(
            AnimationContentDictionary animations, IList<BoneContent> bones)
        {
            // Build up a table mapping bone names to indices.
            Dictionary<string, int> boneMap = new Dictionary<string, int>();

            for (int i = 0; i < bones.Count; i++)
            {
                string boneName = bones[i].Name;

                if (!string.IsNullOrEmpty(boneName))
                    boneMap.Add(boneName, i);
            }

            // Convert each animation in turn.
            Dictionary<string, AnimationClip> animationClips = new Dictionary<string, AnimationClip>();

            foreach (KeyValuePair<string, AnimationContent> animation in animations)
            {
                AnimationClip processed = ProcessAnimation(animation.Value, boneMap);

                animationClips.Add(animation.Key, processed);
            }

            if (animationClips.Count == 0)
            {
                throw new InvalidContentException(
                            "Input file does not contain any animations.");
            }

            return animationClips;
        }


        /// <summary>
        /// Converts an intermediate format content pipeline AnimationContent
        /// object to our runtime AnimationClip format.
        /// </summary>
        /// <param name="animation">Content of the animations.</param>
        /// <param name="boneMap">Dictionary of the bones ordered by name.</param>
        /// <returns>Processed animation clip.</returns>
        static AnimationClip ProcessAnimation(AnimationContent animation,
                                              Dictionary<string, int> boneMap)
        {
            List<Keyframe> keyframes = new List<Keyframe>();

            // For each input animation channel.
            foreach (KeyValuePair<string, AnimationChannel> channel in animation.Channels)
            {
                // Look up what bone this channel is controlling.
                int boneIndex;

                if (!boneMap.TryGetValue(channel.Key, out boneIndex))
                {
                    throw new InvalidContentException(string.Format(
                        "Found animation for bone '{0}', " +
                        "which is not part of the skeleton.", channel.Key));
                }

                // Convert the keyframe data.
                foreach (AnimationKeyframe keyframe in channel.Value)
                {
                    keyframes.Add(new Keyframe(boneIndex, keyframe.Time, keyframe.Transform));
                }
            }

            // Sort the merged keyframes by time.
            keyframes.Sort(CompareKeyframeTimes);

            if (keyframes.Count == 0)
                throw new InvalidContentException("Animation has no keyframes.");

            if (animation.Duration <= TimeSpan.Zero)
                throw new InvalidContentException("Animation has a zero duration.");

            return new AnimationClip(animation.Duration, keyframes);
        }

        #endregion

        #region CompareKeyframeTimes Method

        /// <summary>
        /// Comparison function for sorting keyframes into ascending time order.
        /// </summary>
        /// <param name="a">First keyframe to be compared.</param>
        /// <param name="b">Second keyframe to be compared.</param>
        /// <returns>Result of the comparison.</returns>
        static int CompareKeyframeTimes(Keyframe a, Keyframe b)
        {
            return a.Time.CompareTo(b.Time);
        }

        #endregion

        #region ValidateMesh Method

        /// <summary>
        /// Makes sure this mesh contains the kind of data we know how to animate.
        /// </summary>
        /// <param name="node">Content node.</param>
        /// <param name="context">Context of the content processor.</param>
        /// <param name="parentBoneName">Name of the parent bone.</param>
        static void ValidateMesh(NodeContent node, ContentProcessorContext context,
            string parentBoneName)
        {
            MeshContent mesh = node as MeshContent;

            if (mesh != null)
            {
                // Validate the mesh.
                if (parentBoneName != null)
                {
                    context.Logger.LogWarning(null, null,
                        "Mesh {0} is a child of bone {1}. SkinnedModelProcessor " +
                        "does not correctly handle meshes that are children of bones.",
                        mesh.Name, parentBoneName);
                }

                if (!MeshHasSkinning(mesh))
                {
                    context.Logger.LogWarning(null, null,
                        "Mesh {0} has no skinning information, so it has been deleted.",
                        mesh.Name);

                    mesh.Parent.Children.Remove(mesh);
                    return;
                }
            }
            else if (node is BoneContent)
            {
                // If this is a bone, remember that we are now looking inside it.
                parentBoneName = node.Name;
            }

            // Recurse (iterating over a copy of the child collection,
            // because validating children may delete some of them).
            foreach (NodeContent child in new List<NodeContent>(node.Children))
                ValidateMesh(child, context, parentBoneName);
        }

        #endregion

        #region MeshHasSkinning Method

        /// <summary>
        /// Checks whether a mesh contains skininng information.
        /// </summary>
        /// <param name="mesh">Mesh to check.</param>
        /// <returns>true if the mesh contains skining information, false otherwise.</returns>
        static bool MeshHasSkinning(MeshContent mesh)
        {
            foreach (GeometryContent geometry in mesh.Geometry)
            {
                if (!geometry.Vertices.Channels.Contains(VertexChannelNames.Weights()))
                    return false;
            }

            return true;
        }

        #endregion

        #region FlattenTransforms Method

        /// <summary>
        /// Bakes unwanted transforms into the model geometry, so everything 
        /// ends up in the same coordinate system.
        /// </summary>
        /// <param name="node">Content node.</param>
        /// <param name="skeleton">Skeleton.</param>
        static void FlattenTransforms(NodeContent node, BoneContent skeleton)
        {
            foreach (NodeContent child in node.Children)
            {
                // Don't process the skeleton, because that is special.
                if (child == skeleton)
                    continue;

                // Bake the local transform into the actual geometry.
                MeshHelper.TransformScene(child, child.Transform);

                // Having baked it, we can now set the local
                // coordinate system back to identity.
                child.Transform = Matrix.Identity;

                // Recurse.
                FlattenTransforms(child, skeleton);
            }
        }

        #endregion
    }
}