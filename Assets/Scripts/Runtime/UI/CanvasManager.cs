using System;
using UnityEngine;

namespace CQ.LeagueOfLegends.TFT.UI
{
	public class CanvasManager : MonoBehaviour
	{
		static CanvasManager instance;

		public NameTagsManager nametags;

		void Awake()
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		}

		public static NameTagsManager NameTagsManager {
			get
			{
				return instance.nametags;
			}
		}
	}
}