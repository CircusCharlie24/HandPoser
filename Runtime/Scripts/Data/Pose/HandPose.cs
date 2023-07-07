using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeLibrary24.HandPoser
{
    [CreateAssetMenu(menuName = "CodeLibrary24/HandPoser/HandPose", fileName = "NewHandPose", order = 0)]
    public class HandPose : ScriptableObject
    {
        public FingerPose thumbPose;
        public FingerPose indexFingerPose;
        public FingerPose middleFingerPose;
        public FingerPose ringFingerPose;
        public FingerPose pinkyFingerPose;
        public float poseTransitionDuration = 0.35f;

        public HandPose()
        {
            thumbPose = new FingerPose();
            indexFingerPose = new FingerPose();
            middleFingerPose = new FingerPose();
            ringFingerPose = new FingerPose();
            pinkyFingerPose = new FingerPose();
        }
    }
}