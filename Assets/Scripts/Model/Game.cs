using System.Collections.Generic;

namespace CQ.LeagueOfLegends.TFT
{
	public class Game
	{
		public static Game Instance;
		
		public readonly List<Player> players;
		public readonly Dictionary<int, OpenBattleField> battelField;
		public readonly GameConfiguration configuration;
		public readonly Market market;

		public Game()
		{
			configuration = new GameConfiguration();
			market = new Market();
			players = new List<Player>();
			battelField = new Dictionary<int, OpenBattleField>();
			
			for (int i = 0; i < Constant.PLAYER_COUNT; i++)
			{
				Player player = new Player(i);
				battelField[i] = new OpenBattleField();
				
				players.Add(player);
			}

			Instance = this;
		}
	}

	public class GameConfiguration
	{
		
	}

	public class Deck
	{
		public List<HeroUnit> entities;
	}

	public class Player
	{
		public LocalBattleField battleField;
		public Deck deck;
		public Product[] product;
		
		public int playerIndex;
		public int level;

		public int gold;
		public int exp;

		public List<Item> items;

		public Player(int index)
		{
			this.playerIndex = index;
			level = 1;
			gold = 0;
			exp = 0;
		}

		/// <summary>
		/// 레벨 업 구매 가능 여부
		/// </summary>
		/// <returns></returns>
		public bool CanBuyExp()
		{
			if (gold < Constant.EXP_PRICE) return false;
			if (level >= Constant.MAX_LEVEL) return false;

			return true;
		}

		/// <summary>
		/// 레벨 업 구매
		/// </summary>
		public void BuyExp()
		{
			ConsumeGold(Constant.EXP_PRICE);

			exp += Constant.EXP_REWARD;
			
			ValidateExperience();
		}

		/// <summary>
		/// 경험치 변경 후 레벨업 확인
		/// </summary>
		private void ValidateExperience()
		{
			int maxExp = Constant.NEED_EXP[level];
			
			if (maxExp <= exp)
			{
				exp -= maxExp;
				level++;
				
				// onPlayerLevelUp.Invoke(level);
			}			
		}

		/// <summary>
		/// 리롤 구매 가능 여부
		/// </summary>
		/// <returns></returns>
		public bool CanBuyReroll()
		{
			if (gold < Constant.REROLL_PRICE) return false;

			return true;
		}

		/// <summary>
		/// 리롤 구매
		/// </summary>
		public void BuyReroll()
		{
			ConsumeGold(Constant.REROLL_PRICE);

			product = Game.Instance.market.GetNewProduct(level);

			// onReroll.Invoke();
		}

		/// <summary>
		/// 골드 사용
		/// </summary>
		/// <param name="amount"></param>
		public void ConsumeGold(int amount)
		{
			gold -= amount;
			
			// onConsumeGold.Invoke(amount);
		}
	}

	public class Market
	{
		public Product[] GetNewProduct(int level)
		{
			return new Product[5];
		}
	}

	public class Product
	{
		public int heroId;
		
	}
}