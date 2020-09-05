using System.Collections;
using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
	public class RoundManager : MonoBehaviour
	{
		public int bakeTime = 2;
		public int countdown = 3;

		void Start()
		{
			StartCoroutine(RoundCountdown());
		}

		IEnumerator RoundCountdown()
		{
			Debug.Log($"Ready for countdown... {bakeTime} seconds left.");
			yield return new WaitForSeconds(bakeTime);
			while (countdown > 0)
			{
				Debug.Log($"Countdown... {countdown}");
				countdown--;
				
				yield return new WaitForSeconds(1);
			}

			foreach (AttackableUnit unit in ObjectManager.GetAllUnits())
			{
				unit.roundStarted = true;
			}
		}
	}
}