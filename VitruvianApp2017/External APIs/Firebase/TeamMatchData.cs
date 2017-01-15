using System;
namespace VitruvianApp2017
{
	public class TeamMatchData
	{
		public string matchNumber { get; set; }
		public int teamNumber { get; set; }

		// Auto
		public bool autoCross { get; set; }
		public bool gearDeposit { get; set; }
		public int autoHighHit { get; set; }
		public int autoHighMiss { get; set; }
		public int autoLowHit { get; set; }
		public int autoScore { get; set; }

		// TeleOp
		public int cycleCount { get; set; }
		public int[] cycleHighScore { get; set; }
		public double[] cycleHighAcc { get; set; }
		public int[] cycleLowScore { get; set; }
		public int droppedGears { get; set; }
		public int climbTime { get; set; }
		public bool successfulClimb { get; set; }

		// Post Match
		public int fouls { get; set; }
		public bool good { get; set; }
	}
}
