namespace CQ.LeagueOfLegends.TFT
{
	public enum EGameState
	{
		NONE,
		
		BEFORE_BATTLE,
		IN_BATTLE,
		AFTER_BATTLE,
		
		BATTLEFILD_TRANSITION,
		
		BEFORE_PICK,
		IN_PICK,
		AFTER_PICK,
		
		FINISHED,
	}
}