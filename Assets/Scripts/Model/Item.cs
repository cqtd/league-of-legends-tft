using System.Collections.Generic;
using System.Linq;
using Prototype;

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
		private Dictionary<int, BaseItem> baseItemMap;
		private Dictionary<int, CombinedItem> combinedItemMap;
		
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

		public void Initialize(StaticDatabase database)
		{
			baseItemMap = new Dictionary<int, BaseItem>();
			combinedItemMap = new Dictionary<int, CombinedItem>();

			foreach (ItemObject item in database.items)
			{
				if (item.IsCombined)
				{
					combinedItemMap[item.id] = new CombinedItem()
					{
						id = item.id,
						name = item.displayName,
						description = item.description
					};
				}
				else
				{
					baseItemMap[item.id] = new BaseItem()
					{
						id = item.id,
						name = item.displayName,
						description = item.description
					};
				}
			}
		}
		
		public BaseItem[] GetBaseItems(CombinedItem combined)
		{
			return new[]
			{
				baseItemMap.Values.ToArray().FirstOrDefault(e => e.id == combined.id / 10),
				baseItemMap.Values.ToArray().FirstOrDefault(e => e.id == combined.id % 10)
			};
		}

		public CombinedItem[] GetCombinableItems(BaseItem baseItem)
		{
			return new HashSet<CombinedItem>(combinedItemMap.Values.ToArray().Where(e =>
				e.id / 10 == baseItem.id || e.id % 10 == baseItem.id)).ToArray();
		}
	}
}