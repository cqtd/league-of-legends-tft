using System;
using UnityEngine;

namespace CQ.LeagueOfLegends.TFT.UI
{
	[Obsolete("Not Implemented", true)]
	public abstract class UIContentBase<T> : MonoBehaviour where T : class
	{
		public virtual void Init(T unit)
		{
			Refresh();
			Repaint();
		}
		
		public abstract void Repaint();
		public abstract void Refresh();

	}
}