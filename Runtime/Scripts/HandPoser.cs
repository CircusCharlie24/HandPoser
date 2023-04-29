using UnityEngine;
using DG.Tweening;

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
    }
}

