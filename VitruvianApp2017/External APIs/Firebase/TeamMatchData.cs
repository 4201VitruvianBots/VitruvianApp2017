using System;
namespace VitruvianApp2017
{
	public class TeamMatchData
	{
		// Pre-Match
		public string scouterName { get; set; }
		public string matchNumber { get; set; }
		public int teamNumber { get; set; }
		public string alliance { get; set; }
		public string startPos { get; set; }

		// Auto
		public bool autoCross { get; set; }
		public bool autoGearDeposit { get; set; }
		public bool autoGearDropped { get; set; } // Needs to be added in AutoMatch Scouting
		public int autoHighHits { get; set; }
		public int autoLowHits { get; set; }
		public int autoScore { get; set; }

		// TeleOp
		public int cycleCount { get; set; }
		public int[] teleOpLowCycleScore { get; set; }
		public int[] teleOpHighCycleScore { get; set; }
		public double[] teleOpHighCycleAcc { get; set; }
		public int teleOpLowScore { get; set; }
		public int teleOpHighScore { get; set; }
		public double teleOpHighAcc { get; set; }
		public int teleopGearsDeposit { get; set; }
		public int teleOpGearsDropped { get; set; }
		public int climbTime { get; set; }
		public bool successfulClimb { get; set; }
		public int teleOptotalScore { get; set; }

		// Post Match
		public int fouls { get; set; }
		public bool good { get; set; }
	}
}
