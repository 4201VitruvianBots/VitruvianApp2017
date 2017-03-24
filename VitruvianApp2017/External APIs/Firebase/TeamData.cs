using System;
namespace VitruvianApp2017
{
	public class TeamData
	{
		public int teamNumber { get; set; }
		public string teamName { get; set; }
		public string imageURL { get; set; }

		// Pit Data
		public double tbaOPR { get; set; }
		public double tbaDPR { get; set; }
		public string volumeConfig { get; set; }
		public int maxFuelCapacity { get; set; }
		public bool gearMechanism { get; set; }
		public bool fuelLowMechanism { get; set; }
		public bool fuelHighMechanism { get; set; }
		public bool climbingMechanism { get; set; }
		public bool gearGroundIntakeMechanism { get; set; }
		public bool fuelGroundIntakeMechanism { get; set; }
		//public bool cheesecake { get; set; }
		// auto actions
		public bool pitAutoCross { get; set; }
		public bool pitAutoGear { get; set; }
		public bool pitAutoFuelLow { get; set; }
		public bool pitAutoFuelHigh { get; set; }

		public string notes { get; set; }

		// Match Data
		public int matchCount { get; set; }

		// Auto
		public int totalAutoCrossSuccesses { get; set; }
		public double avgAutoGearScored { get; set; }
		public double avgAutoGearsDelivered { get; set; }
		public double avgAutoGearsDropped { get; set; }
		public double avgAutoHighHits { get; set; }
		public double avgAutoPressure { get; set; }

		// TeleOp
		public double avgTeleOpActions { get; set; }
		public double avgTeleOpPressure { get; set; }
		public double avgTeleOpHighHits { get; set; }
		public double avgTeleOpHighAccuracy { get; set; }
		public double avgTeleOpGearsScored { get; set; }
		public double avgTeleOpGearsTransitDropped { get; set; }
		public double avgTeleOpGearsStationDropped { get; set; }


		//High
		public int autoPressureHigh { get; set; }
		public int teleOpActionsHigh { get; set; }
		public int teleOpGearsScoredHigh { get; set; } 
		public int teleOpGearsStationDroppedHigh { get; set; }
		public int teleOpGearsTransitDroppedHigh { get; set; }
		public double teleOpPressureHigh { get; set; }


		public int successfulClimbCount { get; set; }
		public int attemptedClimbCount { get; set; }

		public int foulCount { get; set; }
		public int goodCount { get; set; }
	}
}
