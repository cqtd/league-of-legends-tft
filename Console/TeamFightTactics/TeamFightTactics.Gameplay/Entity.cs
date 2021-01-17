using System;
using System.Collections.Generic;
using System.Numerics;
using TeamFightTactics.StaticData;

namespace TeamFightTactics.Gameplay
{
	public class Entity
	{
		public int teamIndex;
		public Vector2 position;
		private int x;
		private int y;

		private ChampionActorData champion;
		private readonly List<Item> items = new List<Item>();

		private int level;

		public float speed = 1.0f;
		
		public float Health { get; private set; }
		public float AttackDamage { get; private set; }
		public float AttackSpeed { get; private set; }
		public float AttackRange { get; private set; }
		public float Armor { get; private set; }

		public bool IsAlive()
		{
			return Health > 0f;
		}

		public Entity(ChampionActorData data, int level)
		{
			this.champion = data;

			Health = champion.stat.healths[level - 1];
			AttackDamage = champion.stat.attackDamages[level - 1];

			AttackSpeed = champion.stat.attackSpeed;
			Armor = champion.stat.armor;
			AttackRange = champion.stat.attackRange;

			attackInterval = 1f / AttackSpeed;
			curAttackInterval = attackInterval + 0.01f;
		}

		public void Reverse()
		{
			position = new Vector2(7, 8) - position;
		}

		public void SetTeam(int index)
		{
			this.teamIndex = index;
		}

		public Entity AddItem(Item item)
		{
			items.Add(item);
			
			return this;
		}

		public Entity SetPosition(int x, int y)
		{
			this.position = new Vector2(x, y);

			this.x = x;
			this.y = y;
			
			return this;
		}

		private Entity target;
		
		public void BattleLogic(in int deltaTime)
		{
			FindTarget(deltaTime);
			Attack(deltaTime);
			Move(deltaTime);
		}

		private void FindTarget(int deltaTime)
		{
			if (target == null)
			{
				target = BattleManager.Instance.FindTarget(position, teamIndex);
				
				Console.WriteLine($"{champion.championId} :: Find Target :: {target.champion.championId}");
			}
			else if (!target.IsAlive())
			{
				target = BattleManager.Instance.FindTarget(position, teamIndex);

				if (target == null)
				{
					BattleManager.Instance.isDone = true;
				}
			}
		}

		private void Move(int deltaTime)
		{
			if (target == null)
			{
				return;
			}
			
			float dt = deltaTime * 0.001f;
			
			Vector2 velocity = (target.position - position) * dt * speed;
			position += velocity;
		}

		private float attackInterval;
		private float curAttackInterval;

		private void Attack(int deltaTime)
		{
			if (CanAttack(deltaTime))
			{
				target.OnDamage(AttackDamage);
			}
		}

		bool CanAttack(int deltaTime)
		{
			if (target == null)
			{
				return false;
			}
			
			float dist = Vector2.Distance(target.position, position);
			if (dist < AttackRange + 0.16f)
			{
				curAttackInterval += deltaTime * 0.001f;
			
				if (curAttackInterval >= attackInterval)
				{
					curAttackInterval -= attackInterval;
					return true;
				}	
			}

			return false;
		}

		public void OnDamage(float dmg)
		{
			float multiply = (100 / (100 + Armor));
			
			Health -= multiply * dmg;

			Console.ForegroundColor = teamIndex == 0 ? ConsoleColor.Cyan : ConsoleColor.Green;
			Console.WriteLine($"{champion.championId} {Health:N1}");
			Console.ForegroundColor = ConsoleColor.White;
			
			if (Health < 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"{teamIndex} :: {champion.championId} Died!");
				Console.ForegroundColor = ConsoleColor.White;
			}
		}
	}
}