namespace CQ.LeagueOfLegends.TFT
{
	public class Item
	{
		public int id;
		public string name;
		public string description;
	}

	public class BaseItem : Item
	{
		
	}

	public class CombinedItem : Item
	{

	}

	public class ItemManager
	{
		public static CombinedItem Combine(BaseItem item1, BaseItem item2)
		{
			int newId;
			if (item1.id == item2.id)
			{
				newId = item1.id * 10 + item1.id;
			}
			
			else if (item1.id > item2.id)
			{
				newId = item2.id * 10 + item1.id;
			}

			else
			{
				newId = item1.id * 10 + item2.id;
			}
			
			return GetItemByIndex(newId) as CombinedItem;
		}

		public static Item GetItemByIndex(int index)
		{
			return null;
		}
	}
}