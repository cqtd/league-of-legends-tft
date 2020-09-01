using System;
using System.Linq;
using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
	[DefaultExecutionOrder(100)]
	public class AttackableUnit : MonoBehaviour
	{
		public int team;
		public float attackRange;

		[NonSerialized]
		public bool IsInvalid;
		public bool IsTargetable {
			get
			{
				return false;
			}
		}

		AttackableUnit target;

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

		void Awake()
		{
			ObjectManager.Add(this);
			
		}
		

		protected virtual void Update()
		{
			if (HasTarget())
			{
				AttackLogic();
				return;
			}

			Move();
			FindTarget();
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

		void DoAttack()
		{
			
		}

		bool CanAttack()
		{
			return true;
		}

		protected bool HasTarget()
		{
			return target != null;
		}

		protected void Move()
		{
			
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

		[ContextMenu("Clear Target")]
		void ClearTarget()
		{
			target = null;
		}

		void OnDestroy()
		{
			IsInvalid = true;
			ObjectManager.Remove(this);
		}
	}
}