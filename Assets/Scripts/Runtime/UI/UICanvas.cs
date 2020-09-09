using System.Collections.Generic;
using UnityEngine;

namespace CQ.LeagueOfLegends.TFT.UI
{
	public abstract class UICanvas<T> : MonoBehaviour where T : UIContent
	{
		protected Dictionary<AttackableUnit, T> entities;
		protected T loadedPrefab;
		protected abstract string PrefabPath { get; }

		protected virtual void Awake()
		{
			entities = new Dictionary<AttackableUnit, T>();
			
			AttackableUnit.onUnitCreated.AddListener(Bind);
			AttackableUnit.onUnitDead.AddListener(UnBind);
		}

		protected virtual void OnAfterBind(AttackableUnit unit, T instance)
		{
			
		}

		protected virtual void OnBeforeUnbind(AttackableUnit unit, T instance)
		{
			
		}

		protected virtual void Bind(AttackableUnit unit)
		{
			if (loadedPrefab == null)
				loadedPrefab = Resources.Load<T>(PrefabPath);
			
			T instance = Instantiate(loadedPrefab, transform);
			instance.Init(unit);

			entities[unit] = instance;

			OnAfterBind(unit, instance);
		}

		protected virtual void UnBind(AttackableUnit unit)
		{
			if (!entities.TryGetValue(unit, out var inst))
			{
				return;
			}

			if (ReferenceEquals(inst, null))
			{
				return;
			}

			OnBeforeUnbind(unit, inst);
			
			Destroy(inst.gameObject);
			entities.Remove(unit);
		}
	}
}