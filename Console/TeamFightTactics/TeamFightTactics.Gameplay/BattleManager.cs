using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TeamFightTactics.Common;

namespace TeamFightTactics.Gameplay
{
	public class BattleManager : Singleton<BattleManager>
	{
		private List<Entity> allEntities;

		public BattleManager()
		{
			allEntities = new List<Entity>();
		}

		public bool isDone;
		
		public bool IsDone()
		{
			return isDone;
		}
		
		public void Register(Deck deck)
		{
			foreach (Entity deckEntity in deck.entities)
			{
				allEntities.Add(deckEntity);
			}
		}

		public Entity FindTarget(Vector2 pos, int team)
		{
			Entity cached = null;
			float distance = float.MaxValue;
			
			foreach (Entity entity in allEntities)
			{
				if (entity.teamIndex == team) continue;
				if (!entity.IsAlive()) continue;

				var dist = Vector2.Distance(pos, entity.position);
				if (dist < distance)
				{
					cached = entity;
					distance = dist;
				}
			}

			return cached;
		}
	}
}