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

			Entity[] teamA = new[]
			{
				new Entity(
					DataManager.Instance.GetChampionByCost(1),
					new Item[0] , 0, 0)
			};
			
			Entity[] teamB = new[]
			{
				new Entity(
					DataManager.Instance.GetChampionByCost(1),
					new Item[0] , 0, 0)
			};
			
			new BattleGround(teamA, teamB)
				.Wait()
				.OnComplete(Console.WriteLine);
			
			Console.ReadLine();
		}
	}
}