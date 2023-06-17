using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

namespace CodeLibrary24.HandPoser
{
    public class HandPoser : MonoBehaviour
    {
        public Finger thumb;
        public Finger index;
        public Finger middle;
        public Finger ring;
        public Finger pinky;

        public float poseTransitionDuration = 0.5f;

        private bool _isRealtimePosingEnabled;

        public HandPoser defaultPose; // TODO: Could be better by making a separate data class.

        public void MakePose(HandPoser targetPose)
        {
            MakeFingerPose(thumb, targetPose.thumb, targetPose.poseTransitionDuration);
            MakeFingerPose(index, targetPose.index, targetPose.poseTransitionDuration);
            MakeFingerPose(middle, targetPose.middle, targetPose.poseTransitionDuration);
            MakeFingerPose(ring, targetPose.ring, targetPose.poseTransitionDuration);
            MakeFingerPose(pinky, targetPose.pinky, targetPose.poseTransitionDuration);
        }

        private void MakeFingerPose(Finger referenceFinger, Finger targetFinger, float duration)
        {
            for (int i = 0; i < referenceFinger.joints.Length; i++)
            {
                Quaternion newRotation = targetFinger.joints[i].joint.localRotation;
                referenceFinger.joints[i].joint.DOLocalRotateQuaternion(newRotation, duration);
            }
        }

        public void LerpPose(HandPoser targetPose, float lerpValue)
        {
            LerpFingerPose(thumb, defaultPose.thumb, targetPose.thumb, lerpValue);
            LerpFingerPose(index, defaultPose.index, targetPose.index, lerpValue);
            LerpFingerPose(middle, defaultPose.middle, targetPose.middle, lerpValue);
            LerpFingerPose(ring, defaultPose.ring, targetPose.ring, lerpValue);
            LerpFingerPose(pinky, defaultPose.pinky, targetPose.pinky, lerpValue);
        }

        private void LerpFingerPose(Finger referenceFinger, Finger defaultFinger, Finger targetFinger, float lerpValue)
        {
            for (int i = 0; i < referenceFinger.joints.Length; i++)
            {
                Quaternion newRotation = Quaternion.Lerp(defaultFinger.joints[i].joint.localRotation, targetFinger.joints[i].joint.localRotation, lerpValue);
                referenceFinger.joints[i].joint.localRotation = newRotation;
            }
        }
    }
}