using System;
namespace VitruvianApp2017
{
	public class MatchData
	{
		// Pre-Match
		public string matchID { get; set; }
		public string scouterName { get; set; }
		public string matchNumber { get; set; }
		public int teamNumber { get; set; }
		public string alliance { get; set; }
		public string startPos { get; set; }

		// Auto
		public bool autoCross { get; set; }
		public bool autoGearScored { get; set; }
		public int autoPressure { get; set; }

		// TeleOp
		public int actionCount { get; set; }
		public int teleOpPressure { get; set; }
		public int teleOpGearsScored { get; set; }

		public bool successfulClimb { get; set; }

		// Post Match
		public int fouls { get; set; }
		public bool good { get; set; }
		public long sendTime { get; set; }
		public bool dataIsReady { get; set; }
	}

	public class ActionData
	{
		public int pressureScored { get; set; }
		public bool gearScored { get; set; }
	}
}
