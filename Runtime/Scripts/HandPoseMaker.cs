using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

namespace CodeLibrary24.HandPoser
{
    public class HandPoseMaker : MonoBehaviour
    {
        [SerializeField] private HandData handData;
        [SerializeField] private HandPose defaultPose;

        public void MakeDefaultPose()
        {
            MakePose(defaultPose);
        }

        public void MakePose(HandPose targetPose)
        {
            MakeFingerPose(handData.thumb, targetPose.thumbPose, targetPose.poseTransitionDuration);
            MakeFingerPose(handData.index, targetPose.indexFingerPose, targetPose.poseTransitionDuration);
            MakeFingerPose(handData.middle, targetPose.middleFingerPose, targetPose.poseTransitionDuration);
            MakeFingerPose(handData.ring, targetPose.ringFingerPose, targetPose.poseTransitionDuration);
            MakeFingerPose(handData.pinky, targetPose.pinkyFingerPose, targetPose.poseTransitionDuration);
        }

        private void MakeFingerPose(Finger referenceFinger, FingerPose targetFinger, float duration)
        {
            for (int i = 0; i < referenceFinger.joints.Length; i++)
            {
                Quaternion newRotation = targetFinger.jointPoses[i].rotation;
                referenceFinger.joints[i].joint.DOLocalRotateQuaternion(newRotation, duration);
            }
        }

        public void LerpToPose(HandPose targetPose, float lerpValue)
        {
            LerpToFingerPose(handData.thumb, defaultPose.thumbPose, targetPose.thumbPose, lerpValue);
            LerpToFingerPose(handData.index, defaultPose.indexFingerPose, targetPose.indexFingerPose, lerpValue);
            LerpToFingerPose(handData.middle, defaultPose.middleFingerPose, targetPose.middleFingerPose, lerpValue);
            LerpToFingerPose(handData.ring, defaultPose.ringFingerPose, targetPose.ringFingerPose, lerpValue);
            LerpToFingerPose(handData.pinky, defaultPose.pinkyFingerPose, targetPose.pinkyFingerPose, lerpValue);
        }

        private void LerpToFingerPose(Finger referenceFinger, FingerPose defaultFingerPose, FingerPose targetFingerPose, float lerpValue)
        {
            for (int i = 0; i < referenceFinger.joints.Length; i++)
            {
                Quaternion newRotation = Quaternion.Lerp(defaultFingerPose.jointPoses[i].rotation, targetFingerPose.jointPoses[i].rotation, lerpValue);
                referenceFinger.joints[i].joint.localRotation = newRotation;
            }
        }

        public HandData GetHandData()
        {
            return handData;
        }
    }
}