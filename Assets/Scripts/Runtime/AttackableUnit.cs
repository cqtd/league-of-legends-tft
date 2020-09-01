using System;
using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
	[DefaultExecutionOrder(100)]
	public class AttackableUnit : MonoBehaviour
	{
		
		
		public int team = 100;
		public float attackRange = 1.5f;
		public float attackDelay = 0.7f;

		public float attackDamage = 50.0f;
		public float armor = 30.0f;
		public float abilityPower = 0.0f;
		public float magicResist = 20;

		public float maxHealth;
		public float maxMana;

		[Header("Pawn Control")]
		public float speed = 350;
		public float velocityParameter = 0.008f;
		

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
		
		[NonSerialized] public float currentHealth;
		[NonSerialized] public float currentMana;
		[NonSerialized] public float currentAttackDamage;
		[NonSerialized] public float currentArmor;

		void Awake()
		{
			ObjectManager.Add(this);
			rb = GetComponent<Rigidbody>();
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
			
		}

		void DoAttack()
		{
			pendingTime = 0.0f;
			
			// Debug.Log($"Attack! {gameObject.name} - {target.gameObject.name}");
			target.OnAttacked(attackDamage);
		}

		bool IsInAttackDelay()
		{
			pendingTime += Time.deltaTime;
			
			if (pendingTime > attackDelay)
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
			if (dist > attackRange)
			{
				return false;
			}
			else
			{
				Stop();
			}

			return true;
		}

		protected bool HasTarget()
		{
			return target != null;
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

		void Stop()
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

		void OnDestroy()
		{
			IsInvalid = true;
			ObjectManager.Remove(this);
		}
		
		protected Vector3 CurrentPosition()
		{
			return new Vector3(rb.position.x, 0, rb.position.z);
		}
		
		
#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			UnityEditor.Handles.color = new Color(1, .1f, .14f);
			UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, attackRange);

			if (target != null)
			{
				UnityEditor.Handles.color = Color.white;
				UnityEditor.Handles.DrawLines(new Vector3[] {transform.position, target.transform.position});
			}
		}
#endif
		
		[ContextMenu("Clear Target")]
		void ClearTarget()
		{
			target = null;
		}
	}
}