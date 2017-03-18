﻿using System;
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
		public bool autoGearScored { get; set; }
		public bool autoGearDelivered { get; set; }
		public bool autoGearDropped { get; set; }
		public int autoHighHits { get; set; }
		public int autoLowHits { get; set; }
		public int autoPressure { get; set; }

		// TeleOp
		public ActionData[] matchActions { get; set; }
		public int actionCount { get; set; }
		public int teleOpPressure { get; set; }
		public double teleOpHighCycleAcc { get; set; }
		public int teleOpGearsDeposit { get; set; }
		public int teleOpGearsTransitDropped { get; set; }
		public int teleOpGearsStationDropped { get; set; }
		public bool successfulClimb { get; set; }
		public bool attemptedClimb { get; set; }

		// Post Match
		public int fouls { get; set; }
		public bool good { get; set; }
	}
}
