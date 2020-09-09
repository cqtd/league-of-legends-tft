using System.Collections.Generic;
using UnityEngine;

namespace CQ.LeagueOfLegends.TFT.UI
{
	public sealed class UnitIndicatorCanvas : UICanvas<UnitIndicator>
	{
		protected override string PrefabPath {
			get { return "UI/Indicator"; }
		}


		protected override void OnBeforeUnbind(AttackableUnit unit, UnitIndicator instance)
		{
			base.OnBeforeUnbind(unit, instance);
			
			instance.beforeDestroy = true;
		}

		void LateUpdate()
		{
			foreach (KeyValuePair<AttackableUnit,UnitIndicator> valuePair in entities)
			{
				var inst = valuePair.Value;
				var unit = valuePair.Key;
				
				inst.Repaint();

				var scrPos = Camera.main.WorldToScreenPoint(unit.IndicatorPos);
				inst.transform.position = scrPos;
			}
		}

	}
}