using System;
using UnityEngine;

namespace CodeLibrary24.HandPoser
{
    [Serializable]
    public class JointPose
    {
        public Quaternion rotation;

        public JointPose(Quaternion rotation)
        {
            this.rotation = rotation;
        }
    }
}