using System.Collections.Generic;
using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
	[CreateAssetMenu(menuName = "Data/Create Skill", fileName = "SkillData", order = 50)]
	public class TestSkill1 : SkillDataBase
	{
		public float range;
		public float skillDamage;
		
		public override void Use(List<AttackableUnit> targets)
		{
			var critPoss = owner.GetSkillCriticalPossibility();
			bool isCrit = owner.GetRandom.Next(100) <= critPoss * 100;
			float critMultiply = 1.0f;
			if (isCrit) critMultiply = owner.unitData.criticalMultiplier.Get(owner.Tier);
			
			foreach (AttackableUnit target in targets)
			{
				target.TakeDamage(new DamageContext()
				{
					damage = skillDamage,
					isCritical = isCrit,
					criticalMultiplier = critMultiply,
					damageType = EDamageType.AP,
				});
			}	
		}

		public override List<AttackableUnit> GetTargets()
		{
			var enemies = ObjectManager.GetEnemies(owner.Team);
			var entry = new List<AttackableUnit>();

			foreach (AttackableUnit enemy in enemies)
			{
				if (Vector3.Distance(enemy.transform.position, owner.transform.position) < range)
				{
					entry.Add(enemy);
				}
			}
			
			return entry;
		}
	}
}