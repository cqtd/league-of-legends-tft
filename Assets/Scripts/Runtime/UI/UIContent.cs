using UnityEngine;

namespace CQ.LeagueOfLegends.TFT.UI
{
	public abstract class UIContent : MonoBehaviour
	{
		public virtual void Init(AttackableUnit unit)
		{
			Refresh();
			Repaint();
		}
		
		public abstract void Repaint();
		public abstract void Refresh();

	}
}