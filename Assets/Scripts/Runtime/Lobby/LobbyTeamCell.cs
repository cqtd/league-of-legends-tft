using Photon.Pun;
using Photon.Realtime;

namespace CQ.LeagueOfLegends.TFT
{
	public sealed class LobbyTeamCell : LobbyCellBase
	{
		public void SetVacant()
		{
			displayName.text = string.Empty;
			avatar.gameObject.SetActive(false);
		}

		public override void Initialize(Player player)
		{
			base.Initialize(player);

			// avatar.gameObject.SetActive(true);
			// if (player.IsMasterClient)
			// {
			// 	avatar.SetMaster(true);
			// }
			// else
			// {
			// 	avatar.SetMaster(false);
			// }
		}
	}
}