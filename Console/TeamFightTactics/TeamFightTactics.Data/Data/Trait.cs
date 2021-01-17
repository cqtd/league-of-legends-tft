namespace TeamFightTactics.StaticData
{
	public class Trait : Data
	{
		public string key { get; set; }
		public string name { get; set; }
		public string innate { get; set; }
		public string description { get; set; }
		public ETraitType type { get; set; }
		public set[] sets { get; set; }

		public class set
		{
			public ETraitStyle style { get; set; }
			public int min { get; set; }
			public int max { get; set; }
		}
	}
}