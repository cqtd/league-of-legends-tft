namespace CQ.LeagueOfLegends.TFT
{
	// https://developer.riotgames.com/docs/lol#data-dragon
	// https://tftactics.gg/db/items
	
	/// <summary>
	/// 유닛 인스턴스
	/// </summary>
	public abstract class Unit
	{
		public UnitBattleData battleData;
		
		public virtual float GetArmor()
		{
			var value = battleData.armor;

			return value;
		}

		public virtual float GetResist()
		{
			var value = battleData.resist;

			return value;
		}
	}

	public class AttackableUnit : Unit
	{

	}

	public class SkillUnit : Unit
	{
		/// <summary>
		/// 기본 주문력
		/// </summary>
		public float baseAbilityPower;
	}

	/// <summary>
	/// 크립 유닛
	/// </summary>
	public class CreepUnit : Unit
	{
		
	}

	/// <summary>
	/// 크립 보스 유닛
	/// </summary>
	public class CreepBossUnit : SkillUnit
	{
		
	}
	
	/// <summary>
	///  히어로 유닛
	/// </summary>
	public class HeroUnit : Unit
	{
		
	}
}