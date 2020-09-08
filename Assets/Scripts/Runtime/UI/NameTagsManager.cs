using System.Collections.Generic;
using UnityEngine;

namespace CQ.LeagueOfLegends.TFT.UI
{
	public class NameTagsManager : MonoBehaviour
	{
		Dictionary<AttackableUnit, NameTags> map;
		Dictionary<AttackableUnit, Indicator> indicators;

		void Awake()
		{
			map = new Dictionary<AttackableUnit, NameTags>();
			indicators = new Dictionary<AttackableUnit, Indicator>();
			
			AttackableUnit.onUnitCreated.AddListener(RegisterIndicator);
			AttackableUnit.onUnitDead.AddListener(UnregisterIndicator);
		}

		void Register(AttackableUnit unit)
		{
			var nameTags = Resources.Load<NameTags>("UI/NameTag");
			var inst = Instantiate(nameTags, transform);

			map[unit] = inst;
			inst.unitName.text = unit.unitData.unitName;
			inst.tier.text = unit.Tier.ToString();
			
			inst.healthBar.value = unit.GetHealth() / unit.GetMaxHealth();
			inst.manaBar.value = unit.GetMana() / unit.GetMaxMana();
		}

		void Unregister(AttackableUnit unit)
		{
			if (!map.TryGetValue(unit, out var inst))
			{
				return;
			}

			if (ReferenceEquals(inst, null))
			{
				return;
			}


			{
				// Debug.Log(inst.name);
				Destroy(inst.gameObject);
				map.Remove(unit);	
			}
		}
		
		void RegisterIndicator(AttackableUnit unit)
		{
			var nameTags = Resources.Load<Indicator>("UI/Indicator");
			var inst = Instantiate(nameTags, transform);

			indicators[unit] = inst;
			// inst.unitName.text = unit.unitData.unitName;
			// inst.tier.text = unit.Tier.ToString();
			
			inst.SetHealth(unit.GetHealth(), unit.GetMaxHealth());
			// inst.healthBar.value = unit.GetHealth() / unit.GetMaxHealth();
			inst.manaBar.fillAmount = unit.GetMana() / unit.GetMaxMana();
			inst.tier.text = unit.Tier.ToString();
		}

		void UnregisterIndicator(AttackableUnit unit)
		{
			if (!indicators.TryGetValue(unit, out var inst))
			{
				return;
			}

			if (ReferenceEquals(inst, null))
			{
				return;
			}

			{
				// Debug.Log(inst.name);
				inst.beforeDestroy = true;
				Destroy(inst.gameObject);
				indicators.Remove(unit);	
			}
		}


		void LateUpdate()
		{
			foreach (KeyValuePair<AttackableUnit,NameTags> valuePair in map)
			{
				var inst = valuePair.Value;
				var unit = valuePair.Key;
				
				inst.healthBar.value = unit.GetHealth() / unit.GetMaxHealth();
				inst.manaBar.value = unit.GetMana() / unit.GetMaxMana();

				var scrPos = Camera.main.WorldToScreenPoint(unit.IndicatorPos);
				inst.transform.position = scrPos;
			}
			
			foreach (KeyValuePair<AttackableUnit,Indicator> valuePair in indicators)
			{
				var inst = valuePair.Value;
				var unit = valuePair.Key;
				
				inst.SetHealth(unit.GetHealth(), unit.GetMaxHealth());
				inst.manaBar.fillAmount = unit.GetMana() / unit.GetMaxMana();
				inst.tier.text = unit.Tier.ToString();

				var scrPos = Camera.main.WorldToScreenPoint(unit.IndicatorPos);
				inst.transform.position = scrPos;
			}
		}
	}
}