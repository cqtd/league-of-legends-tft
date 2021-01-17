using System.Collections.Generic;
using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
	public class BattleField
	{
		private const int x = 7;
		private const int y = 4;

		private readonly Dictionary<Vector2Int, Piece> formation = new Dictionary<Vector2Int, Piece>();

		public void Place(Vector2Int to)
		{
			
		}

		public void Move(Vector2Int from, Vector2Int to)
		{
			if (!CanMoveFrom(from))
			{
				return;
			}
			
			if (formation.ContainsKey(to))
			{
				Swap(from, to);
			}
			else
			{
				Piece fromPiece = formation[from];
				formation.Remove(from);

				formation[to] = fromPiece;
				fromPiece.initPosition = to;
			}
		}

		private bool CanMoveFrom(Vector2Int from)
		{
			return formation.ContainsKey(from);
		}

		public void Swap(Vector2Int from, Vector2Int to)
		{
			Piece prevToPiece = formation[to];
			Piece prevFromPiece = formation[from];

			formation.Remove(from);
			formation.Remove(to);

			formation[from] = prevToPiece;
			formation[to] = prevFromPiece;

			prevToPiece.initPosition = from;
			prevFromPiece.initPosition = to;
		}
	}
	
	public class LocalBattleField : BattleField
	{
		
	}

	public class OpenBattleField : BattleField
	{
		
	}

	public class Piece
	{
		public Vector2Int initPosition;
	}
}