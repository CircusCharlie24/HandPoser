
using UnityEngine;

namespace CodeLibrary24.HandPoser
{
    public class PoseTester : MonoBehaviour
    {
        public HandPoser targetPose;
        public HandPoser referencePose;

        [ContextMenu("Test Pose")]
        private void TestPose()
        {
            referencePose.MakePose(targetPose);
        }

    }
}