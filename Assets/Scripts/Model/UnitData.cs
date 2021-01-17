using System;
using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
	/// <summary>
	/// 스태틱 데이터
	/// </summary>
	[Serializable]
	public class UnitBattleData
	{
		/// <summary>
		/// 기본 방어력
		/// </summary>
		public float armor;
		
		/// <summary>
		/// 기본 마법 저항력
		/// </summary>
		public float resist;

		/// <summary>
		/// 기본 체력
		/// </summary>
		public float[] health;

		/// <summary>
		/// 기본으로 채워진 마나 게이지
		/// </summary>
		public float mana;

		/// <summary>
		/// 기본 최대 마나 게이지
		/// </summary>
		public float maxMana;

		/// <summary>
		/// 기본 마나 획득량
		/// </summary>
		public float manaGaining;
		
		/// <summary>
		/// 기본 공격력
		/// </summary>
		public float[] attackDamage;
		
		/// <summary>
		/// 기본 공격 딜레이
		/// </summary>
		public float attackDelay;

		/// <summary>
		/// 기본 공격 사거리
		/// </summary>
		public float attackRange;

		/// <summary>
		/// 근접 유닛인가
		/// </summary>
		public bool isMelee;
	}

	public class CreepBattleData : UnitBattleData
	{
		
	}
}