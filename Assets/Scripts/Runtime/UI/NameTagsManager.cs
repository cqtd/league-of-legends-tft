using System.Collections.Generic;
using UnityEngine;

namespace CQ.LeagueOfLegends.TFT.UI
{
	public class NameTagsManager : MonoBehaviour
	{
		Dictionary<AttackableUnit, NameTags> map;

		void Awake()
		{
			map = new Dictionary<AttackableUnit, NameTags>();
		}

		public void Register(AttackableUnit unit)
		{
			var nameTags = Resources.Load<NameTags>("UI/NameTag");
			var inst = Instantiate(nameTags, transform);

			map[unit] = inst;
			inst.unitName.text = unit.unitData.unitName;
			inst.tier.text = unit.tier.ToString();
			
			inst.healthBar.value = unit.currentHealth / unit.unitData.maxHealth;
			inst.manaBar.value = unit.currentMana / unit.unitData.maxMana;
		}

		public void Unregister(AttackableUnit unit)
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

		void LateUpdate()
		{
			foreach (KeyValuePair<AttackableUnit,NameTags> valuePair in map)
			{
				var inst = valuePair.Value;
				var unit = valuePair.Key;
				
				inst.healthBar.value = unit.currentHealth / unit.unitData.maxHealth;
				inst.manaBar.value = unit.currentMana / unit.unitData.maxMana;

				var scrPos = Camera.main.WorldToScreenPoint(unit.transform.position + unit.uiOffset * Vector3.up);
				inst.transform.position = scrPos;
			}
		}
	}
}