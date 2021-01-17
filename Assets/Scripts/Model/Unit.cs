namespace CQ.LeagueOfLegends.TFT
{
	public abstract class Unit
	{
		/// <summary>
		/// 기본 공격력
		/// </summary>
		public float baseAttackDamage;

		/// <summary>
		/// 기본 주문력
		/// </summary>
		public float baseAbilityPower;
		
		/// <summary>
		/// 기본 방어력
		/// </summary>
		public float baseArmor;
		
		/// <summary>
		/// 기본 마법 저항력
		/// </summary>
		public float baseResist;

		/// <summary>
		/// 기본 체력
		/// </summary>
		public float baseHealth;

		/// <summary>
		/// 기본으로 채워진 마나 게이지
		/// </summary>
		public float baseMana;

		/// <summary>
		/// 기본 최대 마나 게이지
		/// </summary>
		public float baseMaxMana;

		/// <summary>
		/// 기본 마나 획득량
		/// </summary>
		public float baseManaGaining;

		/// <summary>
		/// 기본 공격 딜레이
		/// </summary>
		public float baseAttackDelay;

		/// <summary>
		/// 기본 공격 사거리
		/// </summary>
		public float baseAttackRange;

		/// <summary>
		/// 근접 유닛인가
		/// </summary>
		public bool isMelee;
	}

	/// <summary>
	/// 크립 유닛
	/// </summary>
	public class CreepUnit
	{
		
	}

	/// <summary>
	/// 크립 보스 유닛
	/// </summary>
	public class CreepBossUnit
	{
		
	}
	
	/// <summary>
	///  히어로 유닛
	/// </summary>
	public class HeroUnit
	{
		
	}
}