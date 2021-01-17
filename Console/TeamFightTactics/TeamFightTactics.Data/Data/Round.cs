namespace TeamFightTactics.StaticData
{
	public class Round : Data
	{
		public class Stage
		{
			public EStageType type;
				
			public Piece[] creeps;
		}

		public Stage[] stages;
	}
}