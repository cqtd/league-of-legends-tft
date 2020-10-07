using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
	public class PickManager : MonoBehaviour
	{
		[SerializeField] ChampionSpawnPoint[] championSpawnPoints = new ChampionSpawnPoint[0];
		[SerializeField] PlayerSpawnPoint[] playerSpawnPoints = new PlayerSpawnPoint[0];

		[SerializeField] GameObject blocker;
	}
}