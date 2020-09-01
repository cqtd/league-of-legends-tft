using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CQ.LeagueOfLegends.TFT
{
	public class BlockManager : MonoBehaviour
	{
		public int iterator = 0;
		
		[ContextMenu("Sort")]
		void Sort()
		{
			var dict = new Dictionary<int, GameObject>();

			for (int i = 0; i < 14; i++)
			{
				var target = transform.GetChild(i + iterator * 14);
				dict[int.Parse(target.gameObject.name.Replace("Hex", ""))] = target.gameObject;
			}

			foreach (GameObject go in dict.Values.ToList())
			{
				go.transform.parent = null;
			}

			var list = dict.Keys.ToList().OrderBy(e => e).ToList();
			for (int i = 0; i < list.Count; i++)
			{
				int index = list[i];
				
				var target = dict[index];
				target.transform.SetParent(this.transform);
				target.transform.SetSiblingIndex(i + iterator * 14);
			}

			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
		}
	}
}