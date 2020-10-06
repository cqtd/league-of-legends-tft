#if UNITY_EDITOR
using UnityEditor;

namespace CQ.LeagueOfLegends.TFT.UI
{
	[UnityEditor.CustomEditor(typeof(UIButton))]
	public class UIButtonEditor : UnityEditor.UI.ButtonEditor
	{
		SerializedProperty m_textProperty;
		SerializedProperty m_textColorsProperty;
		SerializedProperty m_transProperty;

		protected override void OnEnable()
		{
			base.OnEnable();
			m_textProperty = serializedObject.FindProperty("text");
			m_textColorsProperty = serializedObject.FindProperty("textColors");
			m_transProperty = serializedObject.FindProperty("trans");
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			EditorGUILayout.Space();

			serializedObject.Update();
			EditorGUILayout.PropertyField(m_textProperty);
			
			EditorGUILayout.PropertyField(m_transProperty);
			++EditorGUI.indentLevel;
			{
				EditorGUILayout.PropertyField(m_textColorsProperty);
				
			}
			--EditorGUI.indentLevel;
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}
#endif