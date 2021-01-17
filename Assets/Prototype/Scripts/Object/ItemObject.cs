namespace Prototype
{
	public class ItemObject : StaticData
	{
		public int id;
		public string description;

		public bool IsCombined {
			get
			{
				return id > 10;
			}
		}
	}

	public class ItemDataBridge
	{
		public int id;
		public string name;
		public string description;
	}
}