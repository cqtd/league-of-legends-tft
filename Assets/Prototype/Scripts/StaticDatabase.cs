using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Prototype
{
	[CreateAssetMenu(menuName = "Create HeroDatabase", fileName = "HeroDatabase", order = 0)]
	public class StaticDatabase : ScriptableObject
	{
		[Header("Storage")]
		public TraitsObject[] traits;
		public ChampionObject[] champions;
		public ItemObject[] items;

		#region UNITY_EDITOR

#if UNITY_EDITOR

		[ContextMenu("Export Json")]
		private void Export()
		{
			var map = new Dictionary<string, BattleStat>();
			
			foreach (ChampionObject champion in champions)
			{
				map[champion.championId] = champion.battleStat;
			}

			File.WriteAllText("Console/TeamFightTactics/Season4.5/stats.json", JsonConvert.SerializeObject(map, Formatting.Indented));
		}
		
		[Header("Editor")]
		public Object championsJson;
		public Object itemsJson;
		public Object traitsJson;
		
		[ContextMenu("Find Champion Thumbnails", false, 1)]
		private void FindChampionThumbnail()
		{
			AssetDatabase.StartAssetEditing();
			
			foreach (var obj in champions)
			{
				obj.icon = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Prototype/champions/{obj.championId}.png");
				EditorUtility.SetDirty(obj);	
			}
			
			AssetDatabase.StopAssetEditing();
			AssetDatabase.SaveAssets();
		}
		
		[ContextMenu("Find Item Thumbnails", false, 2)]
		private void FindItemThumbnail()
		{
			AssetDatabase.StartAssetEditing();
			
			foreach (var obj in items)
			{
				obj.icon = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Prototype/items/{obj.id:00}.png");
				EditorUtility.SetDirty(obj);	
			}
			
			AssetDatabase.StopAssetEditing();
			AssetDatabase.SaveAssets();
		}
		
		[ContextMenu("Find Traits Thumbnails", false, 3)]
		private void FindTraitsThumbnail()
		{
			AssetDatabase.StartAssetEditing();
			
			foreach (var obj in traits)
			{
				obj.icon = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Prototype/traits/{obj.displayName}.png");
				EditorUtility.SetDirty(obj);	
			}
			
			AssetDatabase.StopAssetEditing();
			AssetDatabase.SaveAssets();
		}
		
		[ContextMenu("Generate Items", false ,21)]
		private void CreateItems()
		{
			string itemPath = AssetDatabase.GetAssetPath(itemsJson);
			string json = File.ReadAllText(itemPath);

			var championData = JsonConvert.DeserializeObject<ItemDataBridge[]>(json);

			items = new ItemObject[championData.Length];
			
			AssetDatabase.StartAssetEditing();

			for (int i = 0; i < championData.Length; i++)
			{
				try
				{
					ItemDataBridge bridge = championData[i];
					ItemObject instance = CreateInstance<ItemObject>();

					instance.displayName = bridge.name;
					instance.id = bridge.id;
					instance.description = bridge.description;

					items[i] = instance;
					AssetDatabase.CreateAsset(instance,
						$"Assets/Prototype/Objects/Item/{instance.displayName}.asset");
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
			}
			
			AssetDatabase.StopAssetEditing();
			
			EditorUtility.SetDirty(this);
			
			AssetDatabase.SaveAssets();
		}

		[ContextMenu("Create Champions")]
		private void CreateChampions()
		{
			string championPath = AssetDatabase.GetAssetPath(championsJson);
			string json = File.ReadAllText(championPath);

			var championData = JsonConvert.DeserializeObject<ChampionDataBridge[]>(json);

			champions = new ChampionObject[championData.Length];
			
			AssetDatabase.StartAssetEditing();

			for (int i = 0; i < championData.Length; i++)
			{
				try
				{
					ChampionDataBridge bridge = championData[i];
					ChampionObject instance = CreateInstance<ChampionObject>();

					instance.displayName = bridge.name;
					instance.championId = bridge.championId;
					instance.cost = bridge.cost;
					instance.traits = bridge.traits.Select(t =>
					{
						return traits.FirstOrDefault(e => e.key == t);
					}).ToArray();

					champions[i] = instance;
					AssetDatabase.CreateAsset(instance,
						$"Assets/Prototype/Objects/Champion/{instance.championId}.asset");
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
			}
			
			AssetDatabase.StopAssetEditing();
			
			EditorUtility.SetDirty(this);
			
			AssetDatabase.SaveAssets();
		}

		[ContextMenu("Create Traits")]
		private void CreateTraits()
		{
			string traitPath = AssetDatabase.GetAssetPath(traitsJson);
			string json = File.ReadAllText(traitPath);

			var traitData = JsonConvert.DeserializeObject<TraitsDataBridge[]>(json);
			
			traits = new TraitsObject[traitData.Length];
			AssetDatabase.StartAssetEditing();
			
			for (int i = 0; i < traitData.Length; i++)
			{
				try
				{
					TraitsDataBridge bridge = traitData[i];

					TraitsObject instance = CreateInstance<TraitsObject>();

					instance.key = bridge.key;
					instance.displayName = bridge.name;
					instance.innate = bridge.innate;
					instance.description = bridge.description;
					instance.type = (ETraitType) Enum.Parse(typeof(ETraitType), bridge.type);

					instance.sets = bridge.sets.Select(s => new TraitsObject.set()
					{
						min = s.min,
						max = s.max,
						style = (ETraitStyle) (Enum.Parse(typeof(ETraitStyle), s.style))
					}).ToArray();

					traits[i] = instance;

					AssetDatabase.CreateAsset(instance, $"Assets/Prototype/Objects/Traits/{instance.key}.asset");

					// Debug.Log(bridge.key);
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
			}
			
			AssetDatabase.StopAssetEditing();
			
			EditorUtility.SetDirty(this);
			
			AssetDatabase.SaveAssets();
		}
#endif
		
		#endregion

		public ItemObject[] GetBaseItems(ItemObject combined)
		{
			return new[]
			{
				items.FirstOrDefault(e => e.id == combined.id / 10),
				items.FirstOrDefault(e => e.id == combined.id % 10)
			};
		}

		public ItemObject[] GetCombinableItems(ItemObject baseItem)
		{
			return new HashSet<ItemObject>(items.Where(e =>
				!ReferenceEquals(e, baseItem) && (e.id / 10 == baseItem.id || e.id % 10 == baseItem.id))).ToArray();
		}
	}
}