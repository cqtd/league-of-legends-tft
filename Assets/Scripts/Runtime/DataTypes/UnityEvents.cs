using UnityEngine.Events;

namespace CQ.LeagueOfLegends.TFT.Events
{
	public class UnitEvent : UnityEvent<AttackableUnit>
	{
		
	}

	public class UnitBoolEvent : UnityEvent<AttackableUnit, bool>
	{
		
	}

}