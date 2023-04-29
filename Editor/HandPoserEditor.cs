using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;

namespace CodeLibrary24.HandPoser
{
    [CustomEditor(typeof(HandPoser))]
    public class HandPoserEditor : Editor
    {
        private HandPoser referencePose;
        private Button copyPoseButton;


        // private const string ROOT = "Assets";
        private const string ROOT = "Packages/com.codelibrary24.handposer";

        private const string UXML_PATH = ROOT + "/HandPoser/Editor/HandPoserEditor.uxml";

        private HandPoser targetPose;

        private VisualElement copyDataContainer;

        private Toggle copyThumbToggle;
        private Toggle copyIndexToggle;
        private Toggle copyMiddleToggle;
        private Toggle copyRingToggle;
        private Toggle copyPinkyToggle;


        public override VisualElement CreateInspectorGUI()
        {
            targetPose = (HandPoser)target;
            VisualElement myInspector = new VisualElement();
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UXML_PATH);
            visualTree.CloneTree(myInspector);
            DrawPoseCopier(myInspector);
            return myInspector;
        }

        private void DrawPoseCopier(VisualElement container)
        {
            DrawReferencePose(container);
            CachePoseDataContainer(container);
            CheckCopyPoseData();
            CacheCopyToggles(container);
            DrawCopyPoseButton(container);
        }

        private void DrawReferencePose(VisualElement container)
        {
            ObjectField referencePoseObjectField = container.Q<ObjectField>("HandPoserObjectField");
            referencePose = referencePoseObjectField.value as HandPoser;
            referencePoseObjectField.RegisterValueChangedCallback((refPose =>
            {
                referencePose = refPose.newValue as HandPoser;
                CheckCopyPoseData();
            }));
        }

        private void CachePoseDataContainer(VisualElement container)
        {
            copyDataContainer = container.Q<VisualElement>("CopyDataContainer");
        }

        private void CacheCopyToggles(VisualElement container)
        {
            copyThumbToggle = container.Q<Toggle>("CopyThumbToggle");
            copyIndexToggle = container.Q<Toggle>("CopyIndexToggle");
            copyMiddleToggle = container.Q<Toggle>("CopyMiddleToggle");
            copyRingToggle = container.Q<Toggle>("CopyRingToggle");
            copyPinkyToggle = container.Q<Toggle>("CopyPinkyToggle");
        }

        private void ShowPoseDataContainer(bool show)
        {
            if (show)
            {
                copyDataContainer.style.display = DisplayStyle.Flex;
            }
            else
            {
                copyDataContainer.style.display = DisplayStyle.None;
            }
        }

        private void DrawCopyPoseButton(VisualElement container)
        {
            copyPoseButton = container.Q<Button>("CopyPoseButton");
            copyPoseButton.clicked += () =>
            {
                CopyPose(false);
                EditorUtility.SetDirty(target);
            };
        }

        private void CopyPose(bool mirrorPose)
        {
            if (copyThumbToggle.value)
            {
                CopyFingerPose(referencePose.thumb, targetPose.thumb);
            }
            if (copyIndexToggle.value)
            {
                CopyFingerPose(referencePose.index, targetPose.index);
            }
            if (copyMiddleToggle.value)
            {
                CopyFingerPose(referencePose.middle, targetPose.middle);
            }
            if (copyRingToggle.value)
            {
                CopyFingerPose(referencePose.ring, targetPose.ring);
            }
            if (copyPinkyToggle.value)
            {
                CopyFingerPose(referencePose.pinky, targetPose.pinky);
            }
        }

        private void CopyFingerPose(Finger referenceFinger, Finger targetFinger)
        {
            for (int i = 0; i < referenceFinger.joints.Length; i++)
            {
                Undo.RecordObject(targetFinger.joints[i].joint, "Record_Joint");

                Quaternion referenceRotation = referenceFinger.joints[i].joint.localRotation;
                Quaternion newRotation = referenceRotation;
                targetFinger.joints[i].joint.localRotation = newRotation;
            }
        }

        private void CheckCopyPoseData()
        {
            ShowPoseDataContainer(referencePose != null);
        }
    }
}
