using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace CodeLibrary24.HandPoser
{
    [CustomEditor(typeof(HandPoseMaker))]
    public class HandPoserEditor : Editor
    {
        private const string PackageID = "com.codelibrary24.handposer";

        private HandPose pose;
        private Button loadPoseButton;

        private const string ROOT_ASSET = "Assets/HandPoser";
        private const string ROOT_PACKAGE = "Packages/com.codelibrary24.handposer";

        private const string EDITOR_PATH = "/Editor";
        private const string UXML_PATH = EDITOR_PATH + "/HandPoserEditor.uxml";

        private const string HAND_DATA_UXML_PATH = EDITOR_PATH + "/HandDataEditor.uxml";

        private HandPoseMaker _handPoseMaker;

        private VisualElement _loadDataContainer;

        private Toggle _ignoreThumbToggle;
        private Toggle _ignoreIndexToggle;
        private Toggle _ignoreMiddleToggle;
        private Toggle _ignoreRingToggle;
        private Toggle _ignorePinkyToggle;

        private string GetRootPath()
        {
            if (PackageFinder.IsPackageInstalled(PackageID))
            {
                return ROOT_PACKAGE;
            }

            return ROOT_ASSET;
        }

        public override VisualElement CreateInspectorGUI()
        {
            _handPoseMaker = (HandPoseMaker) target;
            VisualElement myInspector = new VisualElement();
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(GetRootPath() + UXML_PATH);
            visualTree.CloneTree(myInspector);
            DrawPoseCopier(myInspector);
            return myInspector;
        }

        private void DrawPoseCopier(VisualElement container)
        {
            DrawDefaultPose(container);
            DrawReferencePose(container);
            CachePoseDataContainer(container);
            CheckLoadPoseData();
            CacheIgnoreToggles(container);
            DrawLoadPoseButton(container);
        }

        private void DrawDefaultPose(VisualElement container)
        {
            ObjectField defaultPoseObjectField = container.Q<ObjectField>("DefaultPoseObjectField");
            defaultPoseObjectField.bindingPath = "defaultPose";
        }

        private void DrawReferencePose(VisualElement container)
        {
            ObjectField referencePoseObjectField = container.Q<ObjectField>("HandPoseObjectField");
            referencePoseObjectField.objectType = typeof(HandPose);
            pose = referencePoseObjectField.value as HandPose;
            referencePoseObjectField.RegisterValueChangedCallback((refPose =>
            {
                pose = refPose.newValue as HandPose;
                CheckLoadPoseData();
            }));
        }

        private void CachePoseDataContainer(VisualElement container)
        {
            _loadDataContainer = container.Q<VisualElement>("LoadDataContainer");
        }

        private void CacheIgnoreToggles(VisualElement container)
        {
            _ignoreThumbToggle = container.Q<Toggle>("IgnoreThumbToggle");
            _ignoreIndexToggle = container.Q<Toggle>("IgnoreIndexToggle");
            _ignoreMiddleToggle = container.Q<Toggle>("IgnoreMiddleToggle");
            _ignoreRingToggle = container.Q<Toggle>("IgnoreRingToggle");
            _ignorePinkyToggle = container.Q<Toggle>("IgnorePinkyToggle");
        }

        private void ShowLoadPoseDataContainer(bool show)
        {
            _loadDataContainer.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void DrawLoadPoseButton(VisualElement container)
        {
            loadPoseButton = container.Q<Button>("LoadPoseButton");
            loadPoseButton.clicked += () =>
            {
                LoadPose();
                EditorUtility.SetDirty(target);
            };
        }

        private void DrawSavePoseButton(VisualElement container)
        {
            Button savePoseButton = container.Q<Button>("SavePoseButton");
            savePoseButton.clicked += () =>
            {
                SavePose();
                EditorUtility.SetDirty(target);
            };
        }

        private void LoadPose()
        {
            if (!_ignoreThumbToggle.value)
            {
                LoadFingerPose(_handPoseMaker.GetHandData().thumb, pose.thumbPose);
            }

            if (!_ignoreIndexToggle.value)
            {
                LoadFingerPose(_handPoseMaker.GetHandData().index, pose.indexFingerPose);
            }

            if (!_ignoreMiddleToggle.value)
            {
                LoadFingerPose(_handPoseMaker.GetHandData().middle, pose.middleFingerPose);
            }

            if (!_ignoreRingToggle.value)
            {
                LoadFingerPose(_handPoseMaker.GetHandData().ring, pose.ringFingerPose);
            }

            if (!_ignorePinkyToggle.value)
            {
                LoadFingerPose(_handPoseMaker.GetHandData().pinky, pose.pinkyFingerPose);
            }
        }

        private void LoadFingerPose(Finger finger, FingerPose fingerPose)
        {
            for (int i = 0; i < finger.joints.Length; i++)
            {
                Undo.RecordObject(finger.joints[i].joint, "Record_Joint_" + i + "_Rotation");
                finger.joints[i].joint.localRotation = fingerPose.jointPoses[i].rotation;
            }
        }

        private void CheckLoadPoseData()
        {
            ShowLoadPoseDataContainer(pose != null);
        }

        private void SavePose()
        {
            // TODO: Create a new pose and save it or override exising pose
        }
    }
}