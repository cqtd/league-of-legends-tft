using System;
using CQ.LeagueOfLegends.TFT.UI;
using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
	[DefaultExecutionOrder(100)]
	public class AttackableUnit : MonoBehaviour
	{
		[Header("Instance Data")]
		public int tier = 1;
		public int team = 100;

		[Header("Pawn Control")]
		public float speed = 350;
		public float velocityParameter = 0.008f;

		public float missileSpeed = 1;

		[Header("Data Binder")]
		public UnitData unitData;
		public Bullet prefabBullet;
		
		[Header("UI")]
		public float uiOffset = 0.8f;

		public bool IsTargetable {
			get
			{
				return false;
			}
		}

		AttackableUnit target;
		Vector3 destination;
		float pendingTime = 0.0f;
		Rigidbody rb;
		
		[NonSerialized] public bool roundStarted;
		[NonSerialized] public bool IsInvalid;
		[NonSerialized] public bool IsMelee;
		
		[NonSerialized] public float currentHealth;
		[NonSerialized] public float currentMana;
		[NonSerialized] public float currentAttackDamage;
		[NonSerialized] public float currentAttackRange;
		[NonSerialized] public float currentArmor;
		[NonSerialized] public float currentAbilityPower;
		[NonSerialized] public float currentMagicResist;


		void Awake()
		{
			ObjectManager.Add(this);
			rb = GetComponent<Rigidbody>();
			
			Initialize();
		}

		protected virtual void Start()
		{
			
		}

		void OnEnable()
		{
			CanvasManager.NameTagsManager.Register(this);
		}

		protected virtual void Update()
		{
			if (!roundStarted) return;
			
			if (HasTarget())
			{
				if (CanAttack())
				{
					if (!IsInAttackDelay())
					{
						DoAttack();
					}
				}
				else
				{
					Move();
				}
			}
			else
			{
				Move();
				FindTarget();
			}
		}

		void Initialize()
		{
			currentHealth = unitData.maxHealth;
			currentMana = unitData.maxMana;
			currentAttackDamage = unitData.attackDamage;
			currentAttackRange = unitData.attackRange;
			currentArmor = unitData.armor;
			currentAbilityPower = unitData.abilityPower;
			currentMagicResist = unitData.magicResist;
		}
		
		public float GetAttackDelay()
		{
			return 1 / (unitData.attackSpeed);
		}

		protected bool AttackLogic()
		{
			if (!CanAttack())
			{
				return false;
			}
			
			DoAttack();
			
			return true;
		}

		public virtual void OnAttacked(float damage)
		{
			currentHealth -= damage;

			if (currentHealth < 0)
			{
				Die();
			}
		}

		void Die()
		{
			IsInvalid = true;
			
			CanvasManager.NameTagsManager.Unregister(this);
			ObjectManager.Remove(this);
			
			gameObject.SetActive(false);
			Destroy(gameObject);
		}

		void DoAttack()
		{
			pendingTime = 0.0f;
			
			SpawnBullet();
		}

		void SpawnBullet()
		{
			Vector3 direction;
			direction = (target.transform.position - transform.position).normalized;
			
			Bullet bullet = Instantiate(this.prefabBullet);

			bullet.transform.rotation = Quaternion.LookRotation(direction);
			bullet.transform.position = transform.position;

			bullet.SetDirection(direction);
			bullet.SetSpeed(this.missileSpeed);
			bullet.SetTarget(target);
			bullet.SetContext(new DamageContext()
			{
				damage = currentAttackDamage
			});
		}

		bool IsInAttackDelay()
		{
			pendingTime += Time.deltaTime;
			
			if (pendingTime > GetAttackDelay())
			{
				return false;
			}
			
			return true;
		}

		bool CanAttack()
		{
			if (target == null)
			{
				return false;
			}

			var dist = Vector3.Distance(target.transform.position, transform.position);
			if (dist > currentAttackRange)
			{
				return false;
			}
			else
			{
				HoldPosition();
			}

			return true;
		}

		protected bool HasTarget()
		{
			if (target == null) return false;
			if (target.IsInvalid) return false;

			return true;
		}
		
		protected void Move()
		{
			if (target != null)
			{
				destination = target.transform.position;
				var gap = destination - CurrentPosition();

				var direction = gap;
				direction.Normalize();

				rb.velocity = direction * speed * velocityParameter;

			}
		}

		void HoldPosition()
		{
			destination = transform.position;
		}

		protected virtual void FindTarget()
		{
			FindAttackTarget();
		}

		void FindAttackTarget()
		{
			if (target != null)
			{
				if (!target.IsInvalid && target.IsTargetable)
				{
					return;
				}
			}

			var enemies = ObjectManager.GetEnemies(this.team);
			var min = float.MaxValue;
			AttackableUnit closest = null;

			foreach (AttackableUnit enemy in enemies)
			{
				var dist = Vector3.SqrMagnitude(enemy.transform.position - transform.position);
				if (min > dist)
				{
					min = dist;
					closest = enemy;
				}
			}

			if (closest != null)
			{
				target = closest;
				
				Debug.Log($"New Enemy Found! {this.name} - {target.name}");
			}
		}

		protected Vector3 CurrentPosition()
		{
			return new Vector3(rb.position.x, 0, rb.position.z);
		}
		
		
#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			UnityEditor.Handles.color = new Color(1, .1f, .14f);
			if (Application.isPlaying)
				UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, currentAttackRange);
			else
				UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, unitData.attackRange);

			if (target != null)
			{
				UnityEditor.Handles.color = Color.white;
				UnityEditor.Handles.DrawLines(new Vector3[] {transform.position, target.transform.position});
			}
		}
#endif
		public void TakeDamage(DamageContext context)
		{
			OnAttacked(context.damage);
		}
	}
}