using System.Collections.Generic;

namespace Assets.Scripts.Services
{
	public class GameManager : ManagerBase
	{
		private readonly ICollection<Hand> Hands;

		public int RoundNumber { get; set; } = 1;
		public int HandSize { get; set; } = 5;

		public GameManager(int? handsize)
		{
			Hands = new List<Hand>
			{
				new() { Id = 0 }, // South - the Player
				new() { Id = 1 }, // West - NPC 1
				new() { Id = 2 }, // North - NPC 2
				new() { Id = 3 }  // East - NPC 3
			};

			if (handsize.HasValue)
			{ 
				HandSize = handsize.Value;
			}
		}

		public ICollection<Hand> GetHands() => Hands;
	}
}
