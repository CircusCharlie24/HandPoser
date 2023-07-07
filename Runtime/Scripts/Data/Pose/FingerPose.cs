using System;
using UnityEngine.Serialization;

namespace CodeLibrary24.HandPoser
{
    [Serializable]
    public class FingerPose
    {
        public JointPose[] jointPoses = new JointPose[4];
    }
}