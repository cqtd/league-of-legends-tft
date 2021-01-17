using TeamFightTactics.Common;
using TeamFightTactics.StaticData;

namespace TeamFightTactics.Gameplay
{
	public class Game : Singleton<Game>
	{
		public EGameState currentState = EGameState.NONE;
		public EGameState previousState = EGameState.NONE;

		public Player[] players;
		
		public void Initialize()
		{
			players = new Player[8];
			
			for (int i = 0; i < 8; i++)
			{
				players[i] = new Player();
				players[i].Initialize($"Player {i}");
			}

			int currentRound = 0;
			while (true)
			{
				Round round = DataManager.Instance.GetRoundOrNull(currentRound);
				if (round == null) break;

				foreach (Round.Stage stage in round.stages)
				{
					
					switch (stage.type)
					{
						case EStageType.CAROUSEL:
						{
							previousState = currentState;
							currentState = EGameState.CAROUSEL;
							
							Carousel carousel = new Carousel();

							for (int i = 0; i < players.Length; i++)
							{
								carousel.RandomSelect(ref players[i]);
							}

							break;
						}
						case EStageType.CREEP:
						{
							previousState = currentState;
							currentState = EGameState.DECK_BUILDING;

							for (int i = 0; i < players.Length; i++)
							{
								
							}
							
							break;
						}
						case EStageType.BOSS:
							break;
						case EStageType.BATTLE:
						{
							break;
						}
					}
				}

				currentRound++;
			} 
		}
	}
}