using CQ.LeagueOfLegends.TFT;
using UnityEditor;
using UnityEngine;

namespace Editor
{
	[CustomPropertyDrawer(typeof(FloatProperty))]
	public class FloatPropertyDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{

			if (cache && !useValuePerLevel.boolValue)
			{
				return 40 + definedValues.arraySize * 20f;
			}

			return 40;
		}

		SerializedProperty defaultValue;
		SerializedProperty valuePerLevel;
		SerializedProperty useValuePerLevel;
		SerializedProperty definedValues;
		
		bool cache = false;
		string displayName;
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (!cache)
			{
				this.displayName = property.name;

				property.Next(true);
				defaultValue = property.Copy();

				property.Next(true);
				valuePerLevel = property.Copy();

				property.Next(true);
				useValuePerLevel = property.Copy();

				property.Next(true);
				definedValues = property.Copy();

				cache = true;
			}

			Rect contentPosition = EditorGUI.PrefixLabel(position, new GUIContent(displayName));

			if (position.height > 16f)
			{
				position.height = 16f;
				EditorGUI.indentLevel += 1;
				contentPosition = EditorGUI.IndentedRect(position);
				contentPosition.y += 18f;
			}

			float full = contentPosition.width;
			
			GUI.skin.label.padding = new RectOffset(3, 3, 6, 6);

			EditorGUIUtility.labelWidth = 100;
			EditorGUI.indentLevel = 0;
			
			contentPosition.width = full * 0.25f;
			EditorGUI.BeginProperty(contentPosition, label, useValuePerLevel);
			{
				EditorGUI.BeginChangeCheck();
				bool newDefaultValue = EditorGUI.Toggle(contentPosition, new GUIContent("Linear"),
					useValuePerLevel.boolValue);

				if (EditorGUI.EndChangeCheck())
				{
					useValuePerLevel.boolValue = newDefaultValue;
				}
			}
			EditorGUI.EndProperty();

			if (useValuePerLevel.boolValue)
			{
				contentPosition.x += contentPosition.width + 10;
				contentPosition.width = full * 0.75f * 0.5f;

				EditorGUI.BeginProperty(contentPosition, label, defaultValue);
				{
					EditorGUI.BeginChangeCheck();
					float newDefaultValue = EditorGUI.FloatField(contentPosition, new GUIContent("Default Value"),
						defaultValue.floatValue);

					if (EditorGUI.EndChangeCheck())
					{
						defaultValue.floatValue = newDefaultValue;
					}
				}
				EditorGUI.EndProperty();

				contentPosition.x += contentPosition.width + 10;
			
				EditorGUI.BeginProperty(contentPosition, label, valuePerLevel);
				{
					EditorGUI.BeginChangeCheck();
					float newDefaultValue = EditorGUI.FloatField(contentPosition, new GUIContent("valuePerLevel"),
						valuePerLevel.floatValue);

					if (EditorGUI.EndChangeCheck())
					{
						valuePerLevel.floatValue = newDefaultValue;
					}
				}
				EditorGUI.EndProperty();				
			}
			else
			{
				// @TODO
				// https://answers.unity.com/questions/1585678/how-to-edit-arraylist-property-with-custom-propert.html
				// 참고할 것
				
				// contentPosition.x += contentPosition.width + 10;
				// contentPosition.width = full * 0.75f * 0.5f;
				// // contentPosition.height = definedValues.arraySize * 16f;
				//
				// EditorGUI.BeginProperty(contentPosition, label, definedValues);
				// {
				// 	EditorGUI.BeginChangeCheck();
				//
				// 	EditorGUI.PropertyField(contentPosition, definedValues, new GUIContent("Values"));
				// 	
				// 	// float newDefaultValue = EditorGUI.FloatField(contentPosition, new GUIContent("Default Value"),
				// 	// 	definedValues.floatValue);
				//
				// 	if (EditorGUI.EndChangeCheck())
				// 	{
				// 		// definedValues.floatValue = newDefaultValue;
				// 	}
				// }
				// EditorGUI.EndProperty();
			}

			// base.OnGUI(position, property, label);
		}
	}
}