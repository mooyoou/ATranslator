using UI.InfiniteListScrollRect.Runtime;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

namespace UI.InfiniteListScrollRect.Editor
{
    [CustomEditor(typeof(Runtime.InfiniteListScrollRect), true)]
    [CanEditMultipleObjects]
    public class InfiniteListScrollRectInspector : ScrollRectEditor
    {
        SerializedProperty m_Element;
        SerializedProperty m_ListingDirection;
        SerializedProperty m_needCheckViewLength;
        protected override void OnEnable()
        {
            base.OnEnable();
            m_Element               = serializedObject.FindProperty("ElementTemplate");
            m_ListingDirection      = serializedObject.FindProperty("ListingDirection");
            m_needCheckViewLength      = serializedObject.FindProperty("needCheckViewLength");

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();


            EditorGUI.BeginChangeCheck();
            HorizontalOrVerticalLayoutGroup layoutGroup = GetLayoutGroup();
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Set InfiniteList ElementTemplate");
                ((Runtime.InfiniteListScrollRect)target).LayoutGroup =  layoutGroup;
                EditorUtility.SetDirty(target);
            }

            EditorGUILayout.PropertyField(m_Element);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_ListingDirection);
            EditorGUILayout.PropertyField(m_needCheckViewLength);
            
            if (EditorGUI.EndChangeCheck())
            {
                SerializedProperty layoutGroupProperty = serializedObject.FindProperty("LayoutGroup");
                if (layoutGroupProperty != null)
                {
                    layoutGroupProperty.objectReferenceValue = null;
                }
                serializedObject.ApplyModifiedProperties();
            }
            
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.Space();

            base.OnInspectorGUI();
        }
        
        private HorizontalOrVerticalLayoutGroup GetLayoutGroup()
        {
            if ((target as Runtime.InfiniteListScrollRect)?.ListingDirection == Direction.Vertical)
            {
                return EditorGUILayout.ObjectField("Layout Group", (target as Runtime.InfiniteListScrollRect)?.LayoutGroup, typeof(VerticalLayoutGroup), true) as HorizontalOrVerticalLayoutGroup;
            }
            else
            {
                return EditorGUILayout.ObjectField("Layout Group", (target as Runtime.InfiniteListScrollRect)?.LayoutGroup, typeof(HorizontalLayoutGroup), true) as HorizontalOrVerticalLayoutGroup;
            }
        }
    }
    
    
    
}