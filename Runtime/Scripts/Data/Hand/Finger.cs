using System;

namespace CodeLibrary24.HandPoser
{
    [Serializable]
    public class Finger
    {
        public FingerJoint[] joints = new FingerJoint[4];
    }
}

