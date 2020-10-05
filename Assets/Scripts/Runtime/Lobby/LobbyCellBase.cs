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
		}
	}
}