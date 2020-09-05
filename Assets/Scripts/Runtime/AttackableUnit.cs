using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CQ.LeagueOfLegends.TFT
{
	public class UnitEvent : UnityEvent<AttackableUnit> { }
	public class UnitBoolEvent : UnityEvent<AttackableUnit, bool> { }
	
	[DefaultExecutionOrder(100)]
	public class AttackableUnit : MonoBehaviour
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void Reload()
		{
			activeUnits = new List<AttackableUnit>();
			
			onActivationChanged = new UnitBoolEvent();
			onCreate = new UnitEvent();
			onDestroy = new UnitEvent();
		}

		/// <summary>
		/// 모든 활성화 유닛 리스트
		/// </summary>
		[NonSerialized] public static List<AttackableUnit> activeUnits = new List<AttackableUnit>();
		
		/// <summary>
		/// 활성화 변경 콜백
		/// </summary>
		[NonSerialized] public static UnitBoolEvent onActivationChanged;
		
		/// <summary>
		/// 유닛 생성 콜백
		/// </summary>
		[NonSerialized] public static UnitEvent onCreate;
		
		/// <summary>
		/// 유닛 삭제 콜백
		/// </summary>
		[NonSerialized] public static UnitEvent onDestroy;
		
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

		[NonSerialized] AttackableUnit target;
		[NonSerialized] Vector3 destination;
		[NonSerialized] float pendingTime = 0.0f;
		[NonSerialized] Rigidbody rb;
		
		[NonSerialized] public bool roundStarted;
		[NonSerialized] public bool IsInvalid;
		[NonSerialized] public bool IsMelee;
		
		[NonSerialized] float health;
		[NonSerialized] float mana;

		BuffManager buffManager;
		List<IComponent> components;

		#region Character Data

		public float GetAttackDamage() 
		{
			float vanila = unitData.attackDamage.Get(tier);

			// @todo
			// process item value
			// process buff value

			return vanila;
		}

		public float GetAttackRange()
		{
			float vanila = unitData.attackRange.Get(tier);

			// @todo
			// process item value
			// process buff value
			
			return vanila;
		}
		
		public float GetAttackSpeed()
		{
			float vanila = unitData.attackSpeed.Get(tier);

			// @todo
			// process item value
			// process buff value
			
			return vanila;
		}
				
		public float GetAttackDelay()
		{
			return 1 / (GetAttackSpeed());
		}

		public float GetHealth()
		{
			return health;
		}

		public void SetHealth(float value)
		{
			this.health = value;
		}
		
		public float GetMaxHealth()
		{
			float vanila = unitData.maxHealth.Get(tier);

			// @todo
			// process item value
			// process buff value
			
			return vanila;
		}
		
		public float GetMana()
		{
			return mana;
		}

		public void SetMana(float value)
		{	
			this.mana = value;
		}
		
		public float GetMaxMana()
		{
			float vanila = unitData.maxMana.Get(tier);

			// @todo
			// process item value
			// process buff value
			
			return vanila;
		}

		#endregion

		#region Unity Events

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
			activeUnits.Add(this);
			onActivationChanged.Invoke(this, true);
			
			onCreate.Invoke(this);
			
			// CanvasManager.NameTagsManager.Register(this);
		}

		void OnDisable()
		{
			activeUnits.Remove(this);
			onActivationChanged.Invoke(this, false);
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

			foreach (IComponent component in components)
			{
				component.OnUpdate();
			}
		}

		void FixedUpdate()
		{
			foreach (IComponent component in components)
			{
				component.OnFixedUpdate();
			}
		}

		#region Editor
		
#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			UnityEditor.Handles.color = new Color(1, .1f, .14f);
			if (Application.isPlaying)
				UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, GetAttackRange());
			else
				UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, unitData.attackRange.defaultValue);

			if (target != null)
			{
				UnityEditor.Handles.color = Color.white;
				UnityEditor.Handles.DrawLines(new Vector3[] {transform.position, target.transform.position});
			}
		}
#endif
		#endregion		

		#endregion

		void Initialize()
		{
			health = unitData.initHealth.Get(tier);
			mana = unitData.initMana.Get(tier);
			
			components = new List<IComponent>();
			
			buffManager = new BuffManager();
			buffManager.Initialize();
			
			components.Add(buffManager);
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
			SetHealth(GetHealth() - damage);

			if (GetHealth() < 0)
			{
				Die();
			}
		}

		void Die()
		{
			IsInvalid = true;
			
			onDestroy.Invoke(this);
			// CanvasManager.NameTagsManager.Unregister(this);
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
				damage = GetAttackDamage(),
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
			if (dist > GetAttackRange())
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

		public void TakeDamage(DamageContext context)
		{
			OnAttacked(context.damage);
		}
	}
}