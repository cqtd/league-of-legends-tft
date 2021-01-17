using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TeamFightTactics.Gameplay
{
	public class BattleGround
	{
		private bool complete;

		private Deck a;
		private Deck b;

		public BattleGround(Deck deckA, Deck deckB)
		{
			a = deckA;
			b = deckB;

			foreach (Entity entity in deckA.entities)
			{
				entity.SetTeam(0);
			}

			foreach (Entity entity in deckB.entities)
			{
				entity.SetTeam(1);
				entity.Reverse();
			}

			BattleManager.Instance.Register(a);
			BattleManager.Instance.Register(b);
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

			const float maxSecond = 30;
			const int deltaTime = 10;
			float time = 0f;
			
			while (maxSecond > time)
			{
				foreach (Entity entity in a.entities)
				{
					entity.BattleLogic(deltaTime);
				}
				
				foreach (Entity entity in b.entities)
				{
					entity.BattleLogic(deltaTime);
				}
				
				Thread.Sleep(deltaTime);
				time += 0.01f;

				if (BattleManager.Instance.IsDone())
				{
					time = float.MaxValue;
				}
			}
			
			Console.WriteLine("Fight End");
		}

		public async void PostProcess()
		{
			Console.WriteLine("Postprocess");
		}

		public ETeam GetResult()
		{
			int restA = a.entities.Count(e => e.IsAlive());
			int restB = b.entities.Count(e => e.IsAlive());

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