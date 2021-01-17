using System;
using System.Collections.Generic;
using System.Linq;
using TeamFightTactics.Instance;

namespace TeamFightTactics.Gameplay
{
	public class Player
	{
		public readonly Reserve reserve = new Reserve();
		public readonly Field field = new Field();

		public string name;
		public void Initialize(string str)
		{
			name = str;
		}
		
		public bool CanAddPieceToReserve()
		{
			return reserve.Any(e => e == null);
		}

		public void AddPieceToReserve(ChampionInstanceBase instanceBase)
		{
			int index = reserve.FirstVacancy;

			if (instanceBase is ReserveInstance rInstance)
			{
				reserve[index] = rInstance;
			}
			else
			{
				reserve[index] =
					new ReserveInstance()
					{
						data = instanceBase.data,
						item = instanceBase.item,
					};
			}
		}
		
		public void AddInstanceToField(ChampionInstanceBase instanceBase)
		{
			
		}
		
		public class Field
		{
			private readonly Dictionary<int, InstanceBase[]> map;
			
			public Field()
			{
				map = new Dictionary<int, InstanceBase[]>();
				for (int i = 0; i < 4; i++)
				{
					map[i] = new InstanceBase[9];
				}
			}

			public InstanceBase this[int x, int y] {
				get
				{
					if (x >= 7 || y >= 4) return null;
					
					return map[x][y];
				}
			}
		}

		public class Reserve
		{
			private readonly ReserveInstance[] entities = new ReserveInstance[9];

			public ReserveInstance this[int index] {
				get { return entities[index]; }
				set { entities[index] = value; }
			}

			public bool Any(Func<ReserveInstance, bool> predicate)
			{
				return entities.Any(predicate);
			}

			public int Length {
				get
				{
					return entities.Length;
				}
			}

			public int FirstVacancy {
				get
				{
					for (int i = 0; i < entities.Length; i++)
					{
						if (entities[i] == null)
							return i;
					}

					return -1;
				}
			}
		}
	}
}