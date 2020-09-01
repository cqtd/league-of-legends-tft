using System.Collections.Generic;
using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
	[DefaultExecutionOrder(-200)]
	public class ObjectManager : MonoBehaviour
	{
		static ObjectManager instance;

		Dictionary<int, List<AttackableUnit>> objects;

		void Awake()
		{
			instance = this;
			objects = new Dictionary<int, List<AttackableUnit>>();
			DontDestroyOnLoad(this.gameObject);
		}

		public static void Add(AttackableUnit unit)
		{
			if (!instance.objects.TryGetValue(unit.team, out var list))
			{
				list = new List<AttackableUnit>();
			}
			
			list.Add(unit);
			instance.objects[unit.team] = list;
			
			Debug.Log($"ObjectManager::Add::Unit has been added. {unit.name}");
		}

		public static void Remove(AttackableUnit unit)
		{
			var team = unit.team;
			var list = instance.objects[unit.team];
			list.Remove(unit);

			instance.objects[team] = list;
		}

		public static List<AttackableUnit> GetEnemies(int team)
		{
			List<AttackableUnit> enemies = new List<AttackableUnit>();
			foreach (int key in instance.objects.Keys)
			{
				if (team == key) continue;
				enemies.AddRange(instance.objects[key]);
			}

			return enemies;
		}
	}
}