using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
	public abstract class UnitData : ScriptableObject
	{
		public string unitName;
		
		public float attackDamage = 50.0f;
		public float attackSpeed = 0.7f;
		public float attackRange = 1.8f;
		
		public float armor = 30.0f;
		public float abilityPower = 0.0f;
		public float magicResist = 20;

		public float maxHealth = 1000f;
		public float maxMana = 100f;
	}
}