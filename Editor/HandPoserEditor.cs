using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;

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
            DrawGUI(myInspector);
            return myInspector;
        }

        private void DrawGUI(VisualElement container)
        {
            DrawDefaultPose(container);
            DrawReferencePose(container);
            CachePoseDataContainer(container);
            CheckLoadPoseData();
            CacheIgnoreToggles(container);
            DrawLoadPoseButton(container);
            DrawCreatePoseButton(container);
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

        private void DrawCreatePoseButton(VisualElement container)
        {
            Button createPoseButton = container.Q<Button>("CreatePoseButton");
            createPoseButton.clicked += () =>
            {
                CreatePose();
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

        private void CreatePose() // TODO: Make this into an extension method
        {
            HandPose newHandPose = ScriptableObject.CreateInstance<HandPose>();

            string filePath = EditorUtility.SaveFilePanelInProject(
                "Save new hand pose",
                newHandPose.name,
                "asset",
                "Where do you want to save the new hand pose?");

            if (filePath.Length == 0)
            {
                return;
            }

            SetValuesToNewPose(newHandPose);

            AssetDatabase.CreateAsset(newHandPose, filePath);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newHandPose;
        }

        private void SetValuesToNewPose(HandPose handPose)
        {
            SetFingerPose(_handPoseMaker.GetHandData().thumb, handPose.thumbPose);
            SetFingerPose(_handPoseMaker.GetHandData().index, handPose.indexFingerPose);
            SetFingerPose(_handPoseMaker.GetHandData().middle, handPose.middleFingerPose);
            SetFingerPose(_handPoseMaker.GetHandData().ring, handPose.ringFingerPose);
            SetFingerPose(_handPoseMaker.GetHandData().pinky, handPose.pinkyFingerPose);
        }

        private void SetFingerPose(Finger finger, FingerPose fingerPose)
        {
            fingerPose.jointPoses.Clear();
            foreach (FingerJoint fingerJoint in finger.joints)
            {
                fingerPose.jointPoses.Add(new JointPose(fingerJoint.joint.localRotation));
            }
        }
    }
}