namespace CQ.LeagueOfLegends.TFT
{
	public class Constant
	{
		public const int PLAYER_COUNT = 8;
		public const int MAX_LEVEL = 9;

		public const int EXP_PRICE = 4;
		public const int REROLL_PRICE = 2;
		
		public const int EXP_REWARD = 2;

		public static readonly int[] NEED_EXP =
		{
			00, // 0 - 1 
			00, // 1 - 2
			02, // 2 - 3
			06, // 3 - 4 
			10, // 4 - 5
			20, // 5 - 6
			36, // 6 - 7
			56, // 7 - 8
			80, // 8 - 9
		};
	}
}