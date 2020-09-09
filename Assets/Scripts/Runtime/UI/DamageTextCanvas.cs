using UnityEngine;

namespace CQ.LeagueOfLegends.TFT.UI
{
	public sealed class DamageTextCanvas : UICanvas<DamageText>
	{
		const string prefabPath = "UI/DamageText";

		public Color attackDamage;
		public Color magicDamage;
		public Color fixedDamage;

		static DamageTextCanvas instance;
		
		// Critical
		// 150% -100% scale
		// 0 - 1 alpha
		
		// non critical
		// spawn drop

		protected override string PrefabPath {
			get => prefabPath;
		}

		protected override void Bind(AttackableUnit unit)
		{
			unit.onDamaged += OnDamagedCallback; 
		}

		protected override void UnBind(AttackableUnit unit)
		{
			unit.onDamaged -= OnDamagedCallback;
		}

		void OnDamagedCallback(DamageContext context)
		{
			if (loadedPrefab == null)
				loadedPrefab = Resources.Load<DamageText>(PrefabPath);
			
			DamageText instance = Instantiate(loadedPrefab, transform);
			instance.Init(context);
		}

		protected override void Awake()
		{
			base.Awake();

			instance = this;
		}

		public static Color AttackDamage {
			get
			{
				return instance.attackDamage;
			}
		}
		
		public static Color MagicDamage {
			get
			{
				return instance.magicDamage;
			}
		}
		
		public static Color FixedDamage {
			get
			{
				return instance.fixedDamage;
			}
		}
	}
}