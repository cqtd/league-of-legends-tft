using System.Collections.Generic;

namespace TeamFightTactics.Gameplay
{
	public class Deck
	{
		public readonly List<Entity> entities;
		
		public Deck()
		{
			entities = new List<Entity>();
		}

		public Deck AddEntity(Entity entity)
		{
			entities.Add(entity);
			
			return this;
		}
	}
}