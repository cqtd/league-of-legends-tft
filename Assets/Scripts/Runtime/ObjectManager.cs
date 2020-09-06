using System.Collections.Generic;
using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
	[DefaultExecutionOrder(-200)]
	public class ObjectManager : MonoBehaviour
	{
		static ObjectManager instance;

		Dictionary<int, List<AttackableUnit>> objects;
		List<AttackableUnit> all;

		void Awake()
		{
			instance = this;
			objects = new Dictionary<int, List<AttackableUnit>>();
			all = new List<AttackableUnit>();
			
			DontDestroyOnLoad(this.gameObject);
			
			AttackableUnit.onUnitCreated.AddListener(Add);
			AttackableUnit.onDestroy.AddListener(Remove);
		}
		
		public static void Add_Static(AttackableUnit unit)
		{
			if (!instance.objects.TryGetValue(unit.Team, out var list))
			{
				list = new List<AttackableUnit>();
			}
			
			list.Add(unit);
			instance.objects[unit.Team] = list;

			instance.all.Add(unit);
			
			// Debug.Log($"ObjectManager::Add::Unit has been added. {unit.name}");
		}

		public static void Remove_Static(AttackableUnit unit)
		{
			var team = unit.Team;
			var list = instance.objects[unit.Team];
			list.Remove(unit);

			instance.objects[team] = list;
			instance.all.Remove(unit);
		}

		void Add(AttackableUnit unit)
		{
			if (!objects.TryGetValue(unit.Team, out var list))
			{
				list = new List<AttackableUnit>();
			}
			
			list.Add(unit);
			objects[unit.Team] = list;

			all.Add(unit);
			
			// Debug.Log($"ObjectManager::Add::Unit has been added. {unit.name}");
		}

		void Remove(AttackableUnit unit)
		{
			var team = unit.Team;
			var list = objects[unit.Team];
			list.Remove(unit);

			objects[team] = list;
			all.Remove(unit);
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

		public static List<AttackableUnit> GetAllUnits()
		{
			return instance.all;
		}
	}
}