using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace CQ.LeagueOfLegends.TFT.Editor
{
	public class ItemDataGenerator
	{
		
		[MenuItem("TFT/Item/Generate")]
		private static void Generate()
		{
			Debug.Log(Application.streamingAssetsPath);

			string jsonPath = Application.streamingAssetsPath + "/items.json";
			string json = System.IO.File.ReadAllText(jsonPath);

			var dto = JsonConvert.DeserializeObject<List<Item>>(json);
			
			Debug.Log(dto.Count);
		}
	}
}