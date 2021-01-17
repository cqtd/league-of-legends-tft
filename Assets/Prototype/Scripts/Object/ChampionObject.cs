using System;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Prototype
{
	public class ChampionObject : StaticData
	{
		public string championId;
		public int cost;
		public TraitsObject[] traits;

		public BattleStat battleStat;
		


		#region UNITY_EDITOR

		#if UNITY_EDITOR

		[ContextMenu("Paste from Clipboard")]
		private void Paste()
		{
			string clipboard = GUIUtility.systemCopyBuffer;
			string[] a = clipboard.Split('\n');

			for (int i = 0; i < a.Length; i++)
			{
				string line = a[i];

				if (i == 0)
				{
					try
					{
						string[] block = line.Split('/');
						battleStat.healths = block.Select(e => float.Parse(e.TrimStart(' ').TrimEnd(' '))).ToArray();
					}
					catch (Exception e)
					{
						Debug.LogException(e);
					}
				}

				else if (i == 2)
				{
					try
					{
						string[] block = line.Split('/');

						if (block.Length < 2)
						{
							continue;
						}

						battleStat.initialMana = int.Parse(block[0].TrimStart(' ').TrimEnd(' '));
						battleStat.maxMana = int.Parse(block[1].TrimStart(' ').TrimEnd(' '));
					}
					catch (Exception e)
					{
						Debug.LogException(e);
					}
				}

				else if (i == 4)
				{
					try
					{
						string[] block = line.Split('/');
						battleStat.attackDamages =
							block.Select(e => float.Parse(e.TrimStart(' ').TrimEnd(' '))).ToArray();
					}
					catch (Exception e)
					{
						Debug.LogException(e);
					}
				}

				else if (i == 6)
				{
					try
					{
						string[] block = line.Split('/');

						if (block.Length < 2)
						{
							continue;
						}

						battleStat.armor = int.Parse(block[0].Replace(" ", ""));
						battleStat.resist = int.Parse(block[1].Replace(" ", ""));
					}
					catch (Exception e)
					{
						Debug.LogException(e);
					}
				}

				else if (i == 8)
				{
					try
					{
						battleStat.attackSpeed = float.Parse(line.Replace(" ", ""));
					}
					catch (Exception e)
					{
						Debug.LogException(e);
					}
				}

				else if (i == 10)
				{
					try
					{
						battleStat.attackRange = float.Parse(line.Replace(" ", ""));
					}
					catch (Exception e)
					{
						Debug.LogException(e);
					}
				}
			}
			
			EditorUtility.SetDirty(this);
			AssetDatabase.SaveAssets();
		}
		
#endif

		#endregion
	}

	[Serializable]
	public class BattleStat
	{
		public float[] healths = new float[3];
		public float armor;
		public float resist;
		public float[] attackDamages = new float[3];
		public float attackSpeed;
		public float attackRange;
		public float initialMana;
		public float maxMana;

		public bool isMelee;
	}

	[Serializable]
	public class ChampionDataBridge
	{
		public string name;
		public string championId;
		public int cost;
		public string[] traits;
	}
}