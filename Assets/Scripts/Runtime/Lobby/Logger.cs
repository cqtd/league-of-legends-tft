using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
	public class Logger
	{
		public static void Verbose(string msg, Object obj = null)
		{
			Debug.Log(msg, obj);
		}
	}
}