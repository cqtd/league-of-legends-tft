using System.Collections.Generic;
using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
	public abstract class SkillDataBase : ScriptableObject
	{
		public string skillName;
		protected Champion owner;

		public virtual void Initialize(Champion owner)
		{
			this.owner = owner;
		}
		
		public abstract void Use(List<AttackableUnit> targets);

		public abstract List<AttackableUnit> GetTargets();
	}
}