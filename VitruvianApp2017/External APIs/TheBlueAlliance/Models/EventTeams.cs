using System;

namespace TheBlueAlliance.Models
{
	public class EventTeamsHttp
	{
		public class Team
		{
			public int team_number { get; set; }
			public string nickname { get; set; }
			public string name { get; set; }
			public string city { get; set; }
			public string state_prov { get; set; }
			public string country { get; set; }
			public string address { get; set; }
			public string postal_code { get; set; }
			public string gmaps_place_id { get; set; }
			public string gmaps_url { get; set; }
			public double lat { get; set; }
			public double lng { get; set; }
			public string location_name { get; set; }
			public string website { get; set; }
			public int rookie_year { get; set; }
			public string motto { get; set; }
			//public home_championship { get; set; }
		}
	}
}
