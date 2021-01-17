using System.Collections.Generic;

namespace CQ.LeagueOfLegends.TFT
{
	public class Game
	{
		public List<Player> players;
	}

	public class Deck
	{
		public List<HeroUnit> entities;
	}

	public class BattleField
	{
		private const int x = 7;
		private const int y = 4;
	}

	public class LocalBattleField
	{
		
	}

	public class Player
	{
		public LocalBattleField battleField;
	}
}