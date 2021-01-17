namespace TeamFightTactics.StaticData
{
	public class ChampionActorData : ActorData
	{
		public string championId { get; set; }
		public int cost { get; set; }

		public Trait[] traits { get; set; }

		public class Bridge
		{
			public string name { get; set; }
			public string championId { get; set; }
			public int cost { get; set; }
			public string[] traits { get; set; }
		}
	}
}