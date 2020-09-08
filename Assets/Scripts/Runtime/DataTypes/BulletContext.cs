using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
	public class BulletContext
	{
		public DamageContext damage;
		public AttackableUnit owner;
		public AttackableUnit target;
		public Vector3 startPos;
		public Vector3 direction;
		public float missileSpeed;
	}
}