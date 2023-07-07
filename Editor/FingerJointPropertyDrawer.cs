using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System;

namespace CodeLibrary24.HandPoser
{
    [CustomPropertyDrawer(typeof(FingerJoint))]
    public class FingerJointPropertyDrawer : PropertyDrawer
    {
        private const string BindingPathJoints = "joints";
        private const string BindingPathJoint = "joint";
        private const string AxisX = "x";
        private const string AxisY = "y";
        private const string AxisZ = "z";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement container = new VisualElement();

            VisualElement sliderContainer = new VisualElement();

            AddTransform(container, BindingPathJoint, () =>
            {
                sliderContainer.Clear();

                if (GetTransform(property) != null)
                {
                    AddSliders(sliderContainer, property);
                }
            });
            GetTransform(property);
            container.Add(sliderContainer);
            return container;
        }

        private Transform GetTransform(SerializedProperty property)
        {
            SerializedProperty objectFieldProperty = property.serializedObject.FindProperty(GetPropertyPathSubstring(property.propertyPath) + $"[{GetIndex(property)}]." + BindingPathJoint);
            UnityEngine.Object objectReference = objectFieldProperty.objectReferenceValue;
            Transform transform = objectReference as Transform;
            return transform;
        }

        private string GetPropertyPathSubstring(string propertyPath)
        {
            int endIndex = propertyPath.LastIndexOf("[");
            string subString = propertyPath.Substring(0, endIndex);
            return subString;
        }

        private int GetIndex(SerializedProperty property)
        {
            string propertyPath = property.propertyPath;
            int startIndex = propertyPath.LastIndexOf("[") + 1;
            int endIndex = propertyPath.LastIndexOf("]");
            string indexString = propertyPath.Substring(startIndex, endIndex - startIndex);
            if (int.TryParse(indexString, out int index))
            {
                return index;
            }

            Debug.LogError("Parsing Failed!! - Something went wrong!!");
            return -1;
        }

        private void AddSliders(VisualElement container, SerializedProperty property)
        {
            AddSlider(container, AxisY, property);
            AddSlider(container, AxisZ, property);
        }

        private void AddTransform(VisualElement container, string bindingPath, Action OnValueChanged)
        {
            ObjectField transformField = new ObjectField();
            transformField.objectType = typeof(Transform);
            transformField.name = bindingPath;
            transformField.label = bindingPath;
            transformField.bindingPath = bindingPath;
            transformField.style.flexGrow = 1;
            transformField.RegisterValueChangedCallback(t => { OnValueChanged?.Invoke(); });
            container.Add(transformField);
        }

        private void AddSlider(VisualElement container, string axisName, SerializedProperty property)
        {
            Slider slider = new Slider();

            slider.lowValue = 0;
            slider.highValue = 360;
            switch (axisName)
            {
                case AxisX:
                    slider.value = GetTransform(property).eulerAngles.x;
                    slider.RegisterValueChangedCallback(xValue =>
                    {
                        Undo.RecordObject(GetTransform(property), "Record_Transform_Rotation_x");
                        GetTransform(property).eulerAngles = new Vector3(xValue.newValue, GetTransform(property).eulerAngles.y, GetTransform(property).eulerAngles.z);
                    });
                    break;
                case AxisY:
                    slider.value = GetTransform(property).eulerAngles.y;
                    slider.RegisterValueChangedCallback(yValue =>
                    {
                        Undo.RecordObject(GetTransform(property), "Record_Transform_Rotation_y");
                        GetTransform(property).eulerAngles = new Vector3(GetTransform(property).eulerAngles.x, yValue.newValue, GetTransform(property).eulerAngles.z);
                    });
                    break;
                case AxisZ:
                    slider.value = GetTransform(property).eulerAngles.z;
                    slider.RegisterValueChangedCallback(zValue =>
                    {
                        Undo.RecordObject(GetTransform(property), "Record_Transform_Rotation_z");
                        GetTransform(property).eulerAngles = new Vector3(GetTransform(property).eulerAngles.x, GetTransform(property).eulerAngles.y, zValue.newValue);
                    });
                    break;
                default:
                    Debug.LogError("Wrong axis name");
                    break;
            }

            slider.direction = SliderDirection.Horizontal;
            slider.label = axisName;
            container.Add(slider);
        }
    }
}