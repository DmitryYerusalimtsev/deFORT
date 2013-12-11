using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Defort
{
	public class Game 
	{
		static Player player;
		public static Player Player
		{
			set { player = value;}
			get { return player;}
		}
		public static List<GameObject> Units;
	}
}
