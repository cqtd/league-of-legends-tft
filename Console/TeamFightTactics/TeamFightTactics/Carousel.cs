using System;
using TeamFightTactics.Instance;
using TeamFightTactics.StaticData;

namespace TeamFightTactics.Gameplay
{
	public class Carousel
	{
		public CarouselInstance[] entities;

		public Carousel()
		{
			entities = new CarouselInstance[9];
			for (int i = 0; i < 9; i++)
			{
				entities[i] = new CarouselInstance()
				{
					data = DataManager.Instance.GetChampionByCost(1),
					item = new[] {DataManager.Instance.GetBaseItem(),},
				};
			}
		}

		public bool CanSelect(int index)
		{
			if (entities.Length <= index) return false;
			if (entities[index] == null) return false;

			return true;
		}

		public void Select(ref Player player, int index)
		{
			CarouselInstance instance = entities[index];
			entities[index] = null;

			if (player.CanAddPieceToReserve())
			{
				player.AddPieceToReserve(instance);
			}
			else
			{
				player.AddInstanceToField(instance);
			}
			
			Console.WriteLine($"{player.name} :: Selected {instance.data.championId}");
		}

		public void RandomSelect(ref Player player)
		{
			for (int i = 0; i < 9; i++)
			{
				if (CanSelect(i))
				{
					Select(ref player, i);
					break;
				}
			}
		}
	}
}