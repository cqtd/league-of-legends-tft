using System;

namespace Prototype
{
	public class TraitsObject : StaticData
	{
		public string key;
		public string displayName;
		public string innate;
		public string description;
		public ETraitType type;
		public set[] sets;

		[Serializable]
		public class set
		{
			public ETraitStyle style;
			public int min;
			public int max;
		}
	}
	
	[Serializable]
	public class TraitsDataBridge
	{
		public string key;
		public string name;
		public string innate;
		public string description;
		public string type;
		public set[] sets;

		[Serializable]
		public class set
		{
			public string style;
			public int min;
			public int max;
		}
	}
}