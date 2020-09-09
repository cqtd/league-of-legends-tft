using System;
using UnityEngine;

namespace CQ.LeagueOfLegends.TFT.UI
{
	public class CanvasManager : MonoBehaviour
	{
		static CanvasManager instance;

		public UnitIndicatorCanvas nametags;

		void Awake()
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		}

		public static UnitIndicatorCanvas UnitIndicatorCanvas {
			get
			{
				return instance.nametags;
			}
		}
	}
}