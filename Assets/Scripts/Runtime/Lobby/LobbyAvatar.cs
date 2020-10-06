using UnityEngine;
using UnityEngine.UI;

namespace CQ.LeagueOfLegends.TFT
{
	public class LobbyAvatar : MonoBehaviour
	{
		[SerializeField] Image master;

		public void SetMaster(bool isMaster)
		{
			master.gameObject.SetActive(isMaster);
		}
	}
}