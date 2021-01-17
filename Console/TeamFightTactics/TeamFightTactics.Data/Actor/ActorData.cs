namespace TeamFightTactics.StaticData
{
	public class ActorData : Data
	{
		public string name { get; set; }

		public BattleStat stat { get; set; }

		public class BattleStat
		{
			public float[] healths { get; set; }
			public float[] attackDamages { get; set; }

			public float attackSpeed { get; set; }
			public float attackRange { get; set; }

			public float armor { get; set; }
			public float resist { get; set; }

			public float initialMana { get; set; }
			public float maxMana { get; set; }
		}
	}
}