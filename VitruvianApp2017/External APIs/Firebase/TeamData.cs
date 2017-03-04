using System;
namespace VitruvianApp2017
{
	public class TeamData
	{
		public int teamNumber { get; set; }
		public string teamName { get; set; }

		// Pit Data
		public string volumeConfig { get; set; }
		public int maxFuelCapacity { get; set; }
		public bool groundIntake { get; set; }
		//public bool cheesecake { get; set; }

		// Match Data
		public TeamMatchData[] Matches { get; set; }
		public double tbaOPR { get; set; }
		public int matchCount { get; set; }
		public double avgEstScore { get; set; }

		public double avgAutoScore { get; set; }
		public int autoGearDeposits { get; set; }
		public double avgAutoFuelScore { get; set; }

		public double avgCycles { get; set; }
		public double avgGearDeposits { get; set; }
		public double avgHighScore { get; set; }
		public double avgHighAcc { get; set; }
		public double avgLowScore { get; set; }

		public int successfulClimbCount { get; set; }
		public double avgClimbTime { get; set; }

		public int foulCount { get; set; }
		public int goodCount { get; set; }
	}
}
