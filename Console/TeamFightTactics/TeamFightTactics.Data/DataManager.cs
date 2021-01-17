using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TeamFightTactics.Common;

namespace TeamFightTactics.StaticData
{
	public class DataManager : Singleton<DataManager>
	{
		private const string DATA_DIR =
			@"C:\Github\Unity Projects\league-of-legends-tft\Console\TeamFightTactics\Season4.5";

		private Trait[] traits;
		private Item[] items;
		private ChampionActorData[] champions;
		private Round[] rounds;

		private Dictionary<int, List<ChampionActorData>> championMap;
		
		private Dictionary<int, Item> baseItemMap;
		private Dictionary<int, Item> combinedItemMap;

		private Random championRandom;
		private Random baseItemRandom;
		private Random combinedItemRandom;

		public Item GetBaseItem()
		{
			int key = baseItemMap.Keys.ElementAt(baseItemRandom.Next(0, baseItemMap.Keys.Count - 1));
			return baseItemMap[key];
		}

		public Item GetCombinedItem()
		{
			int key = combinedItemMap.Keys.ElementAt(combinedItemRandom.Next(0, combinedItemMap.Keys.Count - 1));
			return combinedItemMap[key];
		}
		
		public ChampionActorData GetChampionByCost(int cost)
		{
			var champs = championMap[cost];
			return champs[championRandom.Next(0, champs.Count - 1)];
		}

		public Round GetRoundOrNull(int index)
		{
			if (rounds.Length > index)
			{
				return rounds[index];
			}

			return null;
		}

		public void Initialize()
		{
			championRandom = new Random(1);
			baseItemRandom = new Random(2);
			combinedItemRandom = new Random(3);
			
			ReadJson();

			championMap = new Dictionary<int, List<ChampionActorData>>();
			foreach (ChampionActorData champion in champions)
			{
				if (!championMap.TryGetValue(champion.cost, out var list))
				{
					list = new List<ChampionActorData>();
				}
				
				list.Add(champion);
				championMap[champion.cost] = list;
			}

			baseItemMap = new Dictionary<int, Item>();
			combinedItemMap = new Dictionary<int, Item>();
			foreach (Item item in items)
			{
				if (item.id > 10)
				{
					combinedItemMap[item.id] = item;
				}
				else
				{
					baseItemMap[item.id] = item;
				}
			}

			CreateTestRound();
		}

		private void ReadJson()
		{
			string traitsJson = DATA_DIR + @"\traits.json";
			string itemsJson = DATA_DIR + @"\items.json";
			string championsJson = DATA_DIR + @"\champions.json";
			string statsJson = DATA_DIR + @"\stats.json";

			traits = JsonConvert.DeserializeObject<Trait[]>(File.ReadAllText(traitsJson));
			items = JsonConvert.DeserializeObject<Item[]>(File.ReadAllText(itemsJson));
			
			var statMap =JsonConvert.DeserializeObject<Dictionary<string, ActorData.BattleStat>>(File.ReadAllText(statsJson)); 
			var bridges = JsonConvert.DeserializeObject<ChampionActorData.Bridge[]>(File.ReadAllText(championsJson));

			champions = bridges.Select(e =>
			{
				return new ChampionActorData()
				{
					name = e.name,
					championId = e.championId,
					cost = e.cost,
					traits = e.traits.Select(trait => traits.FirstOrDefault(t => t.key == trait)).ToArray(),
					stat = statMap[e.championId]
				};
			}).ToArray();
		}

		private void CreateTestRound()
		{
			CreepActorData turtleCreep = new CreepActorData()
			{
				name = "Turtle",
				stat = new ActorData.BattleStat()
				{
					healths = new [] { 700f, 800f, 900f },
					attackDamages = new [] {10f, 20f, 30f},
					attackSpeed = 0.5f,
					attackRange = 1f,
					armor = 0,
					resist = 0,
					initialMana = 0,
					maxMana = 0,
				}
			};

			// 1 round hard coding
			rounds = new[]
			{
				new Round()
				{
					stages = new[]
					{
						new Round.Stage()
						{
							type = EStageType.CAROUSEL
						},

						new Round.Stage()
						{
							type = EStageType.CREEP,
							creeps = new[]
							{
								new Piece()
								{
									actorData = turtleCreep,
									posX = 1,
									posY = 1,
								},

								new Piece()
								{
									actorData = turtleCreep,
									posX = 6,
									posY = 0,
								},
							}
						},

						new Round.Stage()
						{
							type = EStageType.CREEP,
							creeps = new[]
							{
								new Piece()
								{
									actorData = turtleCreep,
									posX = 1,
									posY = 1,
								},

								new Piece()
								{
									actorData = turtleCreep,
									posX = 6,
									posY = 0,
								},

								new Piece()
								{
									actorData = turtleCreep,
									posX = 3,
									posY = 0,
								},
							}
						},

						new Round.Stage()
						{
							type = EStageType.CREEP,
							creeps = new[]
							{
								new Piece()
								{
									actorData = turtleCreep,
									posX = 1,
									posY = 1,
								},

								new Piece()
								{
									actorData = turtleCreep,
									posX = 6,
									posY = 0,
								},

								new Piece()
								{
									actorData = turtleCreep,
									posX = 3,
									posY = 0,
								},

								new Piece()
								{
									actorData = turtleCreep,
									posX = 2,
									posY = 1,
								},
							}
						},
					}
				}
			};
		}
	}
}