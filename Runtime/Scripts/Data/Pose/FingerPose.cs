using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace CodeLibrary24.HandPoser
{
    [Serializable]
    public class FingerPose
    {
        public List<JointPose> jointPoses;
    }
}