using System;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
	public class BattleField : MonoBehaviour
	{
		public Pad padPrefab;

		private Dictionary<Vector2Int, Pad> map;

		private void Awake()
		{
			map = new Dictionary<Vector2Int, Pad>();
			
			for (int x = 0; x < 7; x++)
			{
				for (int z = 0; z < 8; z++)
				{
					Pad instance = Instantiate(padPrefab, transform);

					bool offset = z % 2 == 1;

					instance.transform.position = new Vector3(x, 0, z);
					if (offset)
					{
						instance.transform.position += Vector3.right * 0.5f;
					}
					
					instance.gameObject.name = $"pad_{x}.{z}";

					map[new Vector2Int(x, z)] = instance;
				}
			}
		}
	}
}