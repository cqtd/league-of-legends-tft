using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace CQ.LeagueOfLegends.TFT
{
	using Events;
	
	[DefaultExecutionOrder(100)]
	public class AttackableUnit : MonoBehaviour
	{
		
		#region Static Members
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void BeforeSceneLoad()
		{
			// activeUnits = new List<AttackableUnit>();
			//
			// onActivationChanged = new UnitBoolEvent();
			// onUnitCreated = new UnitEvent();
			// onUnitDead = new UnitEvent();
			// onDestroy = new UnitEvent();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		static void AfterSceneLoad()
		{
			
		}

		/// <summary>
		/// 모든 활성화 유닛 리스트
		/// </summary>
		[NonSerialized] public static List<AttackableUnit> activeUnits = new List<AttackableUnit>();
		
		/// <summary>
		/// 활성화 변경 콜백
		/// </summary>
		[NonSerialized] public static readonly UnitBoolEvent onActivationChanged = new UnitBoolEvent();

		/// <summary>
		/// 유닛 생성 콜백 - Start에서 호출
		/// </summary>
		[NonSerialized] public static readonly UnitEvent onUnitCreated = new UnitEvent();
		
		/// <summary>
		/// 유닛 사망 콜백 - Die에서 호출
		/// </summary>
		[NonSerialized] public static readonly UnitEvent onUnitDead = new UnitEvent();

		/// <summary>
		/// 유닛 삭제 콜백 - 삭제되기 1 프레임 전에 호출
		/// </summary>
		[NonSerialized] public static readonly UnitEvent onDestroy = new UnitEvent();
		
		#endregion

		[Header("데이터")]
		public UnitData unitData;
		public int initialTier = 1;
		public int initialTeam = 100;

		public Vector3 IndicatorPos {
			get { return topHead.position; }
		}

		public bool IsTargetable {
			get
			{
				return false;
			}
		}

		#region Instance Data

		[NonSerialized] AttackableUnit target;
		[NonSerialized] Vector3 destination;
		[NonSerialized] float pendingTime;
		[NonSerialized] Rigidbody rb;
		[NonSerialized] Collider col;
		[NonSerialized] Renderer ren;
		[NonSerialized] Transform topHead;

		[NonSerialized] public bool roundStarted;
		[NonSerialized] public bool IsInvalid;
		[NonSerialized] public bool IsMelee;
		
		[NonSerialized] float health;
		[NonSerialized] float mana;
		
		[NonSerialized] float speedFactor = 1.0f;
		[NonSerialized] protected System.Random random;
		
		public System.Random GetRandom {
			get { return this.random; }
		}
		
		public int Tier { get; protected set; }
		public int Team { get; protected set; }

		BuffManager buffManager;
		List<IComponent> components;
		
		#endregion

		#region Callback Action

		public UnityAction<BulletContext> onAttack;
		
		#endregion

		#region Character Data
		
		public float GetAttackMissileSpeed()
		{
			float result = unitData.attackMissileSpeed.Get(Tier);

			// @todo
			// process item value
			// process buff value

			return result;
		}

		public float GetAttackDamage() 
		{
			float result = unitData.attackDamage.Get(Tier);

			// @todo
			// process item value
			// process buff value

			return result;
		}

		public float GetAttackRange()
		{
			float result = unitData.attackRange.Get(Tier);

			// @todo
			// process item value
			// process buff value
			
			return result;
		}
		
		public float GetAttackSpeed()
		{
			float result = unitData.attackSpeed.Get(Tier);

			// @todo
			// process item value
			// process buff value
			
			return result;
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
			float result = unitData.maxHealth.Get(Tier);

			// @todo
			// process item value
			// process buff value
			
			return result;
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
			float result = unitData.maxMana.Get(Tier);

			// @todo
			// process item value
			// process buff value
			
			return result;
		}

		public float GetMovementSpeed()
		{
			float result = unitData.movementSpeed.Get(Tier);

			return result * speedFactor;
		}

		public void SetMovementFactor(float value)
		{
			this.speedFactor = value;
		}

		#endregion

		#region Unity Events

		void Awake()
		{
			rb = GetComponent<Rigidbody>();
			col = GetComponent<Collider>();
			ren = GetComponent<Renderer>();
			
			Initialize();
		}

		protected virtual void Start()
		{
			onUnitCreated.Invoke(this);
		}

		void OnEnable()
		{
			activeUnits.Add(this);
			onActivationChanged.Invoke(this, true);
		}

		void OnDisable()
		{
			activeUnits.Remove(this);
			onActivationChanged.Invoke(this, false);
		}

		void OnDestroy()
		{
			onDestroy?.Invoke(this);
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
					ProcessMovement();
				}
			}
			else
			{
				ProcessMovement();
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
			Team = initialTeam;
			Tier = initialTier;
			
			health = unitData.initHealth.Get(Tier);
			mana = unitData.initMana.Get(Tier);
			
			components = new List<IComponent>();
			
			buffManager = new BuffManager();
			buffManager.Initialize();
			
			components.Add(buffManager);
			topHead = GetTopHeadBone();
			
			random = new System.Random(GetHashCode());
		}

		Transform GetTopHeadBone()
		{
			foreach (Transform o in transform)
			{
				if (string.CompareOrdinal(o.name, "TopHead") == 0)
				{
					return o;
				}
			}

			return null;
		}

		#region Attack Logic

		protected bool AttackLogic()
		{
			if (!CanAttack())
			{
				return false;
			}
			
			DoAttack();
			
			return true;
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
		
		void DoAttack()
		{
			pendingTime = 0.0f;
			
			Vector3 direction = (target.transform.position - transform.position).normalized;

			var critPoss = unitData.criticalPossibility.Get(Tier);
			bool isCrit = random.Next(100) <= critPoss * 100;
			float critMultiply = 1.0f;
			if (isCrit) critMultiply = unitData.criticalMultiplier.Get(Tier);

			BulletContext ctx = new BulletContext()
			{
				damage = new DamageContext()
				{
					damage = GetAttackDamage(),
					isCritical = isCrit,
					criticalMultiplier = critMultiply,
					damageType = EDamageType.AD,
				},
				
				owner = this,
				startPos = transform.position,
				target = target,
				missileSpeed = GetAttackMissileSpeed(),
				direction = direction,
			};

			onAttack?.Invoke(ctx);
			SetMana(GetMana() + unitData.manaPerAttack.Get(Tier));
		}

		public virtual void OnAttacked(float damage)
		{
			SetMana(GetMana() + unitData.manaPerHit.Get(Tier));
			SetHealth(GetHealth() - damage);

			if (GetHealth() < 0)
			{
				SetHealth(0);
				DoDie();
			}
		}
		
		#endregion

		#region Target

		protected bool HasTarget()
		{
			if (target == null) return false;
			if (target.IsInvalid) return false;

			return true;
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

			List<AttackableUnit> enemies = ObjectManager.GetEnemies(this.Team);
			
			float min = float.MaxValue;
			AttackableUnit closest = null;

			foreach (AttackableUnit enemy in enemies)
			{
				float dist = Vector3.SqrMagnitude(enemy.transform.position - transform.position);
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

		#endregion

		#region Move

		protected void ProcessMovement()
		{
			if (target != null)
			{
				destination = target.transform.position;
				var gap = destination - CurrentPosition();

				var direction = gap;
				direction.Normalize();

				rb.velocity = direction * GetMovementSpeed();
			}
		}

		public void MoveTo(Vector3 pos)
		{
			destination = target.transform.position;
		}

		void HoldPosition()
		{
			destination = transform.position;
		}

		protected Vector3 CurrentPosition()
		{
			return new Vector3(rb.position.x, 0, rb.position.z);
		}

		#endregion

		void DoDie()
		{
			IsInvalid = true;
			
			ren.enabled = false;
			col.enabled = false;
			
			onUnitDead.Invoke(this);
			
			StartCoroutine(BookDestroy());
		}

		IEnumerator BookDestroy()
		{
			yield return new WaitForSeconds(1.0f);
			onDestroy.Invoke(this);
			yield return null;
			
			Destroy(gameObject, 1f);
		}

		public void TakeDamage(DamageContext context)
		{
			OnAttacked(context.damage);
		}
	}
}