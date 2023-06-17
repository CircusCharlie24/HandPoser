using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;

namespace CodeLibrary24.HandPoser
{
    [CustomEditor(typeof(HandPoser))]
    public class HandPoserEditor : Editor
    {
        private const string PackageID = "com.codelibrary24.handposer";
        private const string PackageManifestPath = "Packages/manifest.json";
        
        private HandPoser referencePose;
        private Button copyPoseButton;

        private const string ROOT_ASSET = "Assets/HandPoser";
        private const string ROOT_PACKAGE = "Packages/com.codelibrary24.handposer";

        private const string UXML_PATH =  "/Editor/HandPoserEditor.uxml";

        private HandPoser _targetPose;

        private VisualElement _copyDataContainer;

        private Toggle _copyThumbToggle;
        private Toggle _copyIndexToggle;
        private Toggle _copyMiddleToggle;
        private Toggle _copyRingToggle;
        private Toggle _copyPinkyToggle;

        public static bool IsPackageInstalled()
        {
            string jsonText = File.ReadAllText(PackageManifestPath);
            string json = EditorJsonUtility.ToJson(jsonText);
            return json.Contains(PackageID);
        }
        
        private string GetRootPath()
        {
            if (IsPackageInstalled())
            {
                return ROOT_PACKAGE;
            }
            return ROOT_ASSET;
        }

        public override VisualElement CreateInspectorGUI()
        {
            _targetPose = (HandPoser) target;
            VisualElement myInspector = new VisualElement();
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(GetRootPath()+UXML_PATH);
            visualTree.CloneTree(myInspector);
            DrawPoseCopier(myInspector);
            return myInspector;
        }

        private void DrawPoseCopier(VisualElement container)
        {
            DrawDefaultPose(container);
            DrawReferencePose(container);
            CachePoseDataContainer(container);
            CheckCopyPoseData();
            CacheCopyToggles(container);
            DrawCopyPoseButton(container);
            DrawDurationField(container);
        }

        private void DrawDefaultPose(VisualElement container)
        {
            ObjectField defaultPoseObjectField = container.Q<ObjectField>("DefaultPoseObjectField");
            defaultPoseObjectField.bindingPath = "defaultPose";
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
            _copyDataContainer = container.Q<VisualElement>("CopyDataContainer");
        }

        private void CacheCopyToggles(VisualElement container)
        {
            _copyThumbToggle = container.Q<Toggle>("CopyThumbToggle");
            _copyIndexToggle = container.Q<Toggle>("CopyIndexToggle");
            _copyMiddleToggle = container.Q<Toggle>("CopyMiddleToggle");
            _copyRingToggle = container.Q<Toggle>("CopyRingToggle");
            _copyPinkyToggle = container.Q<Toggle>("CopyPinkyToggle");
        }

        private void ShowPoseDataContainer(bool show)
        {
            if (show)
            {
                _copyDataContainer.style.display = DisplayStyle.Flex;
            }
            else
            {
                _copyDataContainer.style.display = DisplayStyle.None;
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
            if (_copyThumbToggle.value)
            {
                CopyFingerPose(referencePose.thumb, _targetPose.thumb);
            }

            if (_copyIndexToggle.value)
            {
                CopyFingerPose(referencePose.index, _targetPose.index);
            }

            if (_copyMiddleToggle.value)
            {
                CopyFingerPose(referencePose.middle, _targetPose.middle);
            }

            if (_copyRingToggle.value)
            {
                CopyFingerPose(referencePose.ring, _targetPose.ring);
            }

            if (_copyPinkyToggle.value)
            {
                CopyFingerPose(referencePose.pinky, _targetPose.pinky);
            }
        }

        private void CopyFingerPose(Finger referenceFinger, Finger targetFinger)
        {
            for (int i = 0; i < referenceFinger.joints.Length; i++)
            {
                Undo.RecordObject(targetFinger.joints[i].joint, "Record_Joint");
                targetFinger.joints[i].joint.localRotation = referenceFinger.joints[i].joint.localRotation;
            }
        }

        private void CheckCopyPoseData()
        {
            ShowPoseDataContainer(referencePose != null);
        }

        private void DrawDurationField(VisualElement container)
        {
            FloatField durationField = container.Q<FloatField>("TransitionDurationField");
            durationField.bindingPath = "poseTransitionDuration";
        }
    }
}