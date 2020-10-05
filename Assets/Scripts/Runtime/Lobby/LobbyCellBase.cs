using TMPro;
using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
	public abstract class LobbyCellBase : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI displayName;
		[SerializeField] LobbyAvatar avatar;
	}
}