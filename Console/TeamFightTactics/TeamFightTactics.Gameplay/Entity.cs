using System.Numerics;
using TeamFightTactics.StaticData;

namespace TeamFightTactics.Gameplay
{
	public class Entity
	{
		private int teamIndex;
		private Vector2 position;

		private ChampionActorData champion;
		private Item[] items;

		public bool IsAlive()
		{
			return true;
		}

		public Entity(ChampionActorData data, Item[] items, int x, int y)
		{
			this.champion = data;
			this.items = items;
			
			position = new Vector2(x, y);
		}

		public void Reverse()
		{
			position = new Vector2(7, 8) - position;
		}

		public void SetTeam(int index)
		{
			this.teamIndex = index;
		}
	}
}