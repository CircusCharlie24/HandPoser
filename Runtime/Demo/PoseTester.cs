using UnityEngine;

namespace CodeLibrary24.HandPoser
{
    public class PoseTester : MonoBehaviour
    {
        public HandPoser targetPose;
        public HandPoser referencePose;

        [Range(0, 1)]
        public float lerpValue = 0;

        [ContextMenu("Test Pose")]
        private void TestPose()
        {
            referencePose.MakePose(targetPose);
        }
        
        [ContextMenu("Lerp Pose")]
        private void LerpPose()
        {
            referencePose.LerpPose(targetPose, lerpValue);
        }
    }
}