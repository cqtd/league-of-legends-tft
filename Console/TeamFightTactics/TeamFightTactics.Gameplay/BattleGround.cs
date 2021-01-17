using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TeamFightTactics.Gameplay
{
	public class BattleGround
	{
		private readonly Dictionary<int, Entity[]> map;

		private readonly List<Entity> teamA;
		private readonly List<Entity> teamB;
		
		private bool complete;

		public BattleGround(Entity[] teamA, Entity[] teamB)
		{
			map = new Dictionary<int, Entity[]>();
			
			for (int i = 0; i < 4; i++)
			{
				map[i] = new Entity[9];
			}

			this.teamA = teamA.ToList();
			this.teamB = teamB.ToList();

			foreach (Entity entity in teamA)
			{
				entity.SetTeam(0);
			}

			foreach (Entity entity in teamB)
			{
				entity.SetTeam(1);
				entity.Reverse();
			}
		}

		public BattleGround Wait()
		{
			Work();
			
			while (!complete)
			{
				Thread.Sleep(10);
			}
			
			return this;
		}

		private async void Work()
		{
			await Task.Run(Preprocess);
			await Task.Run(Fight);
			await Task.Run(PostProcess);

			complete = true;
		}

		public void OnComplete(Action<string> result)
		{
			result?.Invoke($"Result :: {GetResult()}");
		}

		public async void Preprocess()
		{
			Console.WriteLine("Preprocess");
		}

		public async void Fight()
		{
			Console.WriteLine("Fight Begin");
			
			Thread.Sleep(1000);
			
			Console.WriteLine("Fight End");
		}

		private async void FightThread()
		{
			Thread.Sleep(1000);
		}

		public async void PostProcess()
		{
			Console.WriteLine("Postprocess");
		}

		public ETeam GetResult()
		{
			int restA = teamA.Count(e => e.IsAlive());
			int restB = teamB.Count(e => e.IsAlive());

			if (restA > 0 && restB > 0)
			{
				return ETeam.DRAW;
			}
			
			if (restA > 0)
			{
				return ETeam.A;
			}

			return ETeam.B;
		}

		public enum ETeam
		{
			DRAW,
			A,
			B,
		}
	}
}