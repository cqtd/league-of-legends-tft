using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Prototype.Editor
{
	[CustomEditor(typeof(ItemObject))]
	public class ItembjInspector : StaticObjectInspector
	{
		private ItemObject[] entry;

		private bool isCombinedItem;
		
		private void OnEnable()
		{
			ItemObject actor = (ItemObject) target;

			StaticDatabase database =
				AssetDatabase.LoadAssetAtPath<StaticDatabase>("Assets/Prototype/Objects/static_db.asset");
			
			if (actor.IsCombined)
			{
				entry = database.GetBaseItems(actor);
				
				isCombinedItem = true;
			}
			else
			{
				entry = database.GetCombinableItems(actor);
			}
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			EditorGUILayout.Space(10);

			if (isCombinedItem)
			{
				GUILayout.Label("재료 아이템", EditorStyles.boldLabel);
			}
			else
			{
				GUILayout.Label("조합 가능 아이템", EditorStyles.boldLabel);
			}
			
			EditorGUILayout.Space(6);

			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.Space(6);

			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUILayout.Space(6);
				foreach (ItemObject item in entry)
				{
					GUIContent gc = new GUIContent(AssetPreview.GetAssetPreview(item.icon), $"{item.displayName}\n\n{item.description}");
					if (GUILayout.Button(gc,
						new[] {GUILayout.Height(64), GUILayout.Width(64)}))
					{
						Selection.objects = new Object[] {item};
					}
				}
				
				GUILayout.FlexibleSpace();
			}
			
			EditorGUILayout.Space(10);
			EditorGUILayout.EndVertical();
		}
	}
}