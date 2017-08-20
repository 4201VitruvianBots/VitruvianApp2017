using System;

namespace TheBlueAlliance.Models
{
	public class EventMatches
	{
		public class Alliances
		{
			public Alliance_Members blue { get; set; }
			public Alliance_Members red { get; set; }
		}

		public class Alliance_Members
		{
			public int score { get; set; }
			public string[] team_keys { get; set; }
			public string[] surrogate_team_keys { get; set; }
		}

		public class Match
		{
			public string key { get; set; }
			public string comp_level { get; set; }
			public int set_number { get; set; }
			public int match_number { get; set; }
			public Alliances alliances { get; set; }
			public string winning_alliance { get; set; }
			public string event_key { get; set; }
			public int time { get; set; }
			//public int predicted_time { get; set; } // null error will break this
			public int actual_time { get; set; }

			public Videoes[] videos { get; set; }
			public int post_result_time { get; set; }
			public Score_Breakdown score_breakdown { get; set; }
		}

		public class Score_Breakdown
		{
			public Steamworks_Breakdown blue { get; set; }
			public Steamworks_Breakdown red { get; set; }
		}

		public class Videoes{
			public string key { get; set; }
			public string type { get; set; }
		}

		public class Generic_Match_Breakdown
		{
			public int autoPoints { get; set; }
			public int foulCount { get; set; }
			public int foulPoints { get; set; }
			public int techFoulCount { get; set; }
			public int teleopPoints { get; set; }
			public int totalPoints { get; set; }
		}


		public class Steamworks_Breakdown : Generic_Match_Breakdown
		{
			public int autoFuelHigh { get; set; }
			public int autoFuelLow { get; set; }
			public int autoFuelPoints { get; set; }
			public int autoMobilityPoints { get; set; }
			public int autoRotorPoints { get; set; }
			public int kPaBonusPoints { get; set; }
			public bool kPaRankingPointAchieved { get; set; }
			public string robot1Auto { get; set; }
			public string robot2Auto { get; set; }
			public string robot3Auto { get; set; }
			public bool rotor1Auto { get; set; }
			public bool rotor2Auto { get; set; }
			public bool rotor1Engaged { get; set; }
			public bool rotor2Engaged { get; set; }
			public bool rotor3Engaged { get; set; }
			public bool rotor4Engaged { get; set; }
			public int rotorBonusPoints { get; set; }
			public bool rotorRankingPointAchieved { get; set; }
			public string tba_rpEarned { get; set; }
			public int teleOpFuelHigh { get; set; }
			public int teleOpFuelLow { get; set; }
			public int teleOpFuelPoints { get; set; }
			public int teleOpRotorPoints { get; set; }
			public int teleOpTakeoffPoints { get; set; }
			public string touchPadFar { get; set; }
			public string touchPadMiddle { get; set; }
			public string touchPadNear { get; set; }
		}
	}
}
