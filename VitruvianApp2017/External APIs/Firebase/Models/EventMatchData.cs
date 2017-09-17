using System;

namespace VitruvianApp2017
{
	public class EventMatchData
	{
		public string matchNumber { get; set; }
		public int[] Red { get; set; }
		public int[] Blue { get; set; }
		public long matchTime { get; set; }
	}

	public class TableauMatchShedule
	{
		public string matchID { get; set; }
		public string matchNumber { get; set; }
		public string alliance { get; set; }
		public int alliancePos { get; set; }
		public int teamNumber { get; set; }
		public long matchTime { get; set; }
	}
}
 