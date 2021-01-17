using TeamFightTactics.Gameplay;
using TeamFightTactics.StaticData;

namespace TeamFightTactics
{
	public class Program
	{
		public static void Main(string[] args)
		{
			DataManager.Instance.Initialize();
			Game.Instance.Initialize();
		}
	}

	namespace Gameplay
	{


		public enum EGameState
		{
			NONE,
			
			/// <summary>
			/// 회전 초밥
			/// </summary>
			CAROUSEL, 
			
			/// <summary>
			/// 배틀 대기
			/// </summary>
			DECK_BUILDING,
			
			/// <summary>
			/// 배틀 중
			/// </summary>
			BATTLE,
			
			/// <summary>
			/// 배틀 결과 계산
			/// </summary>
			RESULT,
			
			GAME_END,
		}


	}
}