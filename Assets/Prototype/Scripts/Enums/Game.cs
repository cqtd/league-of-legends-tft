using System;
using UnityEngine;

namespace Prototype
{
	public class Game : MonoBehaviour
	{
		public static Game Instance;
		
		

		private void Awake()
		{
			Instance = this;
		}
	}
}