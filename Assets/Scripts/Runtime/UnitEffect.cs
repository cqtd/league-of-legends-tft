using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
	[RequireComponent(typeof(AttackableUnit))]
	public class UnitEffect : MonoBehaviour
	{
		[SerializeField] Bullet bulletPrefab = default;
		[SerializeField] AttackableUnit owner = default;
		
		void Reset()
		{
			owner = GetComponent<AttackableUnit>();
		}

		void Awake()
		{
			owner.onAttack += SpawnBullet;
		}

		void SpawnBullet(BulletContext context)
		{
			Bullet bullet = Instantiate(this.bulletPrefab);

			bullet.transform.rotation = Quaternion.LookRotation(context.direction);
			bullet.transform.position = transform.position;

			bullet.SetDirection(context.direction);
			bullet.SetSpeed(context.missileSpeed);
			bullet.SetTarget(context.target);
			bullet.SetContext(context.damage);
		}
	}
}