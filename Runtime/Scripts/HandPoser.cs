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

        private const float poseTransitionDuration = 0.5f; // TODO: Make this public and add to the editor

        public void MakePose(HandPoser targetPose)
        {
            MakeFingerPose(thumb, targetPose.thumb);
            MakeFingerPose(index, targetPose.index);
            MakeFingerPose(middle, targetPose.middle);
            MakeFingerPose(ring, targetPose.ring);
            MakeFingerPose(pinky, targetPose.pinky);
        }

        private void MakeFingerPose(Finger referenceFinger, Finger targetFinger)
        {
            for (int i = 0; i < referenceFinger.joints.Length; i++)
            {
                Quaternion newRotation = targetFinger.joints[i].joint.localRotation;
                referenceFinger.joints[i].joint.DOLocalRotateQuaternion(newRotation, poseTransitionDuration);
            }
        }
    }
}

