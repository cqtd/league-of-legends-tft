using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
	public abstract class LobbyCellBase : MonoBehaviour
	{
		[SerializeField] protected TextMeshProUGUI displayName = default;
		[SerializeField] protected LobbyAvatar avatar = default;
		
		public virtual void Initialize(Player player)
		{
			displayName.text = player.NickName;
			
			avatar.gameObject.SetActive(true);
			if (player.IsMasterClient)
			{
				avatar.SetMaster(true);
			}
			else
			{
				avatar.SetMaster(false);
			}
		}
	}
}