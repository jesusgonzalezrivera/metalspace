using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SkinnedModel
{
    /// <summary>
    /// The <c>AnimationPlayer</c> is in charge of decoding bone position
    /// matrices from an animation clip.
    /// </summary>
    public class AnimationPlayer
    {
        #region Fields

        /// <summary>
        /// Store for the CurrenKeyFrame property.
        /// </summary>
        int currentKeyframe;

        /// <summary>
        /// Store for the TimeValue property.
        /// </summary>
        TimeSpan currentTimeValue;

        /// <summary>
        /// Store for the CurrentClip property.
        /// </summary>
        AnimationClip currentClipValue;
        
        /// <summary>
        /// Store for the BoneTransforms property.
        /// </summary>
        Matrix[] boneTransforms;

        /// <summary>
        /// Store for the WorldTransforms property.
        /// </summary>
        Matrix[] worldTransforms;

        /// <summary>
        /// Store for the SkinTransforms property.
        /// </summary>
        Matrix[] skinTransforms;

        /// <summary>
        /// Backlink to the bind pose and skeleton hierarchy data.
        /// </summary>
        SkinningData skinningDataValue;

        /// <summary>
        /// Store for the Done property.
        /// </summary>
        private bool _done;
        
        /// <summary>
        /// Store for the Loop property.
        /// </summary>
        private bool _loop;

        #endregion

        #region Properties

        /// <summary>
        /// CurrentKeyframe property
        /// </summary>
        /// <value>
        /// Index of the current keyframe.
        /// </value>
        public int CurrentKeyframe
        {
            get { return currentKeyframe; }
            set { currentKeyframe = value; }
        }

        /// <summary>
        /// CurrentTime property
        /// </summary>
        /// <value>
        /// Current play position.
        /// </value>
        public TimeSpan CurrentTime
        {
            get { return currentTimeValue; }
        }

        /// <summary>
        /// CurrentClip property
        /// </summary>
        /// <value>
        /// Clip currently being decoded.
        /// </value>
        public AnimationClip CurrentClip
        {
            get { return currentClipValue; }
        }

        /// <summary>
        /// BoneTransforms property
        /// </summary>
        /// <value>
        /// Current bone transform matrices, relative to their parent bones.
        /// </value>
        public Matrix[] BoneTransforms
        {
            get { return boneTransforms; }
            set { boneTransforms = value; }
        }

        /// <summary>
        /// WorldTransforms property
        /// </summary>
        /// <value>
        /// Current bone transform matrices, in absolute format.
        /// </value>
        public Matrix[] WorldTransforms
        {
            get { return worldTransforms; }
            set { worldTransforms = value; }
        }

        /// <summary>
        /// SkinTransforms property
        /// </summary>
        /// <value>
        /// Current bone transform matrices, relative to the skinning bind pose.
        /// </value>
        public Matrix[] SkinTransforms
        {
            get { return skinTransforms; }
            set { skinTransforms = value; }
        }

        /// <summary>
        /// Done property
        /// </summary>
        /// <value>
        /// true if the current animation has finished, false otherwise.
        /// </value>
        public bool Done
        {
            get { return _done; }
            set { _done = value; }
        }

        /// <summary>
        /// Loop property
        /// </summary>
        /// <value>
        /// true if the current animation has to be repeated, false otherwise.
        /// </value>
        public bool Loop
        {
            get { return _loop; }
            set { _loop = value; }
        }

        #endregion

        #region Constructor 

        /// <summary>
        /// Constructor of the <c>AnimationPlayer</c> class.
        /// </summary>
        /// <param name="skinningData">Data that contains the information of the animations.</param>
        public AnimationPlayer(SkinningData skinningData)
        {
            if (skinningData == null)
                throw new ArgumentNullException("skinningData");

            skinningDataValue = skinningData;

            boneTransforms = new Matrix[skinningData.BindPose.Count];
            worldTransforms = new Matrix[skinningData.BindPose.Count];
            skinTransforms = new Matrix[skinningData.BindPose.Count];
        }

        #endregion

        #region StartClip Method

        /// <summary>
        /// Starts decoding the specified animation clip.
        /// </summary>
        /// <param name="clip">Clip of the current animation.</param>
        /// <param name="loop">true if the animation has to be repeated, false otherwise.</param>
        public void StartClip(AnimationClip clip, bool loop)
        {
            if (clip == null)
                throw new ArgumentNullException("clip");

            _done = false;
            _loop = loop;

            currentClipValue = clip;
            currentTimeValue = TimeSpan.Zero;
            currentKeyframe = 0;

            // Initialize bone transforms to the bind pose.
            skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
        }

        /// <summary>
        /// Starts decoding the specified animation clip.
        /// </summary>
        /// <param name="clip">Name of the clip of the current animation.</param>
        /// <param name="loop">true if the animation has to be repeated, false otherwise.</param>
        public void StartClip(string clip, bool loop)
        {
            if (skinningDataValue.AnimationClips[clip] == null)
                throw new ArgumentNullException("clip");

            _done = false;
            _loop = loop;

            currentClipValue = skinningDataValue.AnimationClips[clip];
            currentTimeValue = TimeSpan.Zero;
            currentKeyframe = 0;

            // Initialize bone transforms to the bind pose.
            skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
        }

        /// <summary>
        /// Play a specific portion of the given clip, from one time to another
        /// </summary>
        /// <param name="clip">Clip of the current animation.</param>
        /// <param name="startFrame">Index of the frame where the animation starts.</param>
        /// <param name="loop">true if the animation has to be repeated, false otherwise.</param>
        public void StartClip(AnimationClip clip, int startFrame, bool loop)
        {
            if (clip == null)
                throw new ArgumentNullException("clip");

            _done = false;
            _loop = loop;

            currentClipValue = clip;
            currentTimeValue = clip.Keyframes[startFrame].Time;
            currentKeyframe = startFrame;

            // Initialize bone transforms to the bind pose.
            skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
        }

        /// <summary>
        /// Play a specific portion of the given clip, from one time to another
        /// </summary>
        /// <param name="clip">Name of the clip of the current animation.</param>
        /// <param name="startFrame">Index of the frame where the animation starts.</param>
        /// <param name="loop">true if the animation has to be repeated, false otherwise.</param>
        public void StartClip(string clip, int startFrame, bool loop)
        {
            if (skinningDataValue.AnimationClips[clip] == null)
                throw new ArgumentNullException("clip");

            _done = false;
            _loop = loop;

            currentClipValue = skinningDataValue.AnimationClips[clip];
            currentTimeValue = skinningDataValue.AnimationClips[clip].Keyframes[startFrame].Time;
            currentKeyframe = startFrame;

            // Initialize bone transforms to the bind pose.
            skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
        }

        #endregion 

        #region StopClip Method

        /// <summary>
        /// Stop the current clip animation.
        /// </summary>
        /// <param name="clip">Name of the clip to stop.</param>
        public void StopClip(string clip)
        {
            if (currentClipValue != skinningDataValue.AnimationClips[clip])
                return;

            _done = true;
            _loop = false;
        }

        /// <summary>
        /// Stop the current clip animation.
        /// </summary>
        /// <param name="clip">Clip to stop.</param>
        public void StopClip(AnimationClip clip)
        {
            if (currentClipValue != clip)
                return;

            _done = true;
            _loop = false;
        }

        #endregion

        #region SetFrameFromClip Method

        /// <summary>
        /// Set the frame of a clip to start.
        /// </summary>
        /// <param name="clip">Name of the clip.</param>
        /// <param name="frame">Frame selected to start.</param>
        public void SetFrameFromClip(string clip, int frame)
        {
            if (skinningDataValue.AnimationClips[clip] == null)
                throw new ArgumentNullException("clip");

            _done = true;
            _loop = false;

            currentClipValue = skinningDataValue.AnimationClips[clip];
            currentTimeValue = skinningDataValue.AnimationClips[clip].Keyframes[frame].Time;
            currentKeyframe = frame;

            // Initialize bone transforms to the bind pose.
            UpdateBoneTransforms(TimeSpan.FromSeconds(0), false);
            UpdateWorldTransforms(Matrix.Identity);
            UpdateSkinTransforms();
        }

        /// <summary>
        /// Set the frame of a clip to start.
        /// </summary>
        /// <param name="clip">Clip.</param>
        /// <param name="frame">Frame selected to start.</param>
        public void SetFrameFromClip(AnimationClip clip, int frame)
        {
            if (clip == null)
                throw new ArgumentNullException("clip");

            _done = true;
            _loop = false;

            currentClipValue = clip;
            currentTimeValue = clip.Keyframes[frame].Time;
            currentKeyframe = frame;

            // Initialize bone transforms to the bind pose.
            UpdateBoneTransforms(TimeSpan.FromSeconds(0), false);
            UpdateWorldTransforms(Matrix.Identity);
            UpdateSkinTransforms();
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the current animation position.
        /// </summary>
        /// <param name="time">Global time of the game.</param>
        /// <param name="relativeToCurrentTime">true if it is necessary to update the bone transform 
        /// relative to the current time, false otherwise.</param>
        /// <param name="rootTransform">Root transform of the animation.</param>
        public void Update(TimeSpan time, bool relativeToCurrentTime,
                           Matrix rootTransform)
        {
            if (currentClipValue == null || Done)
                return;

            UpdateBoneTransforms(time, relativeToCurrentTime);
            UpdateWorldTransforms(rootTransform);
            UpdateSkinTransforms();
        }

        #endregion

        #region UpdateBoneTransforms Method

        /// <summary>
        /// Update method to refresh the BoneTransforms data.
        /// </summary>
        /// <param name="time">Global time of the game.</param>
        /// <param name="relativeToCurrentTime">true if it is necessary to update the bone transform 
        /// relative to the current time, false otherwise.</param>
        public void UpdateBoneTransforms(TimeSpan time, bool relativeToCurrentTime)
        {
            if (currentClipValue == null)
                throw new InvalidOperationException(
                            "AnimationPlayer.Update was called before StartClip");

            // Update the animation position.
            if (relativeToCurrentTime)
            {
                time += currentTimeValue;

                // If we reached the end, loop back to the start.
                while (time >= currentClipValue.Duration)
                {
                    if (_loop)
                    {
                        time -= currentClipValue.Duration;
                        currentKeyframe = 0;
                        skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
                    }
                    else
                    {
                        _done = true;
                        currentTimeValue = CurrentClip.Duration;
                        break;
                    }
                }
            }

            if (!_done && ((time < TimeSpan.Zero) || (time >= currentClipValue.Duration)))
                throw new ArgumentOutOfRangeException("time");

            // If the position moved backwards, reset the keyframe index.
            if (time < currentTimeValue)
            {
                currentKeyframe = 0;
                skinningDataValue.BindPose.CopyTo(boneTransforms, 0);
            }

            currentTimeValue = time;

            // Read keyframe matrices.
            IList<Keyframe> keyframes = currentClipValue.Keyframes;

            while (currentKeyframe < keyframes.Count)
            {
                Keyframe keyframe = keyframes[currentKeyframe];

                // Stop when we've read up to the current time position.
                if (keyframe.Time > currentTimeValue)
                    break;

                // Use this keyframe.
                boneTransforms[keyframe.Bone] = keyframe.Transform;

                currentKeyframe++;
            }
        }

        #endregion

        #region UpdateWorldTransforms Method

        /// <summary>
        /// Update method to refresh the WorldTransforms data.
        /// </summary>
        /// <param name="rootTransform">Root transform of the animation.</param>
        public void UpdateWorldTransforms(Matrix rootTransform)
        {
            // Root bone.
            worldTransforms[0] = boneTransforms[0] * rootTransform;

            // Child bones.
            for (int bone = 1; bone < worldTransforms.Length; bone++)
            {
                int parentBone = skinningDataValue.SkeletonHierarchy[bone];

                worldTransforms[bone] = boneTransforms[bone] *
                                             worldTransforms[parentBone];
            }
        }

        #endregion

        #region UpdateSkinTransforms Method

        /// <summary>
        /// Update method to refresh the SkinTransforms data.
        /// </summary>
        public void UpdateSkinTransforms()
        {
            for (int bone = 0; bone < skinTransforms.Length; bone++)
            {
                skinTransforms[bone] = skinningDataValue.InverseBindPose[bone] *
                                            worldTransforms[bone];
            }
        }

        #endregion
    }
}
