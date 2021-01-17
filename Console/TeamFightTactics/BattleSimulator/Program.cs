using System;
using TeamFightTactics.Gameplay;
using TeamFightTactics.StaticData;

namespace BattleSimulator
{
	public class Program
	{
		public static void Main(string[] args)
		{
			DataManager.Instance.Initialize();

			Deck deckA = new Deck()
				.AddEntity(
					new Entity(DataManager.Instance.GetChampionByCost(1), 1)
						.SetPosition(0,0)
				)
				.AddEntity(
					new Entity(DataManager.Instance.GetChampionByCost(1), 1)
						.SetPosition(1,0)
				)
				.AddEntity(
					new Entity(DataManager.Instance.GetChampionByCost(1), 1)
						.SetPosition(2,0)
				)
				;
			
			Deck deckB = new Deck()
				.AddEntity(
					new Entity(DataManager.Instance.GetChampionByCost(1), 2)
						.SetPosition(0,0)
				)				
				.AddEntity(
					new Entity(DataManager.Instance.GetChampionByCost(1), 2)
						.SetPosition(2,0)
				);
			
			new BattleGround(deckA, deckB)
				.Wait()
				.OnComplete(Console.WriteLine);
			
			Console.ReadLine();
		}
	}
}