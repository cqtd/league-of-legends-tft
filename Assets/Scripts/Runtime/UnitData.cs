using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
	public abstract class UnitData : ScriptableObject
	{
		public string unitName;

		public bool isMelee = true;
		
		public FloatProperty attackDamage = 50.0f;
		public FloatProperty attackSpeed = 0.7f;
		public FloatProperty attackRange = 1.8f;
		public FloatProperty attackMissileSpeed = (1.0f, 0.05f);

		public FloatProperty criticalPossibility = (20.0f, 0.0f);
		public FloatProperty criticalMultiplier = (1.0f, 0.0f);
		public FloatProperty criticalSkillPossibility = (0.0f, 0.0f);
		
		public FloatProperty armor = (30.0f, 8f);
		public FloatProperty abilityPower = (0.0f, 0f);
		public FloatProperty magicResist = (20, 4);

		public FloatProperty maxHealth = (720f, 340f);
		public FloatProperty initHealth = (720f, 340f);
		public FloatProperty maxMana = (100f, 0f);
		public FloatProperty initMana = (35f, 5f);

		public FloatProperty manaPerAttack = (10f, 2);
		public FloatProperty manaPerHit = (5f, 3);
	}
}