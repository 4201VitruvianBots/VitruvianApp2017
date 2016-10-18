using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using Newtonsoft.Json;
using TheBlueAlliance.Models;

namespace TheBlueAlliance
{
	public class Events
	{
		public static EventTeams.Team[] GetEventTeamsList(string eventKey)
		{
			var teamList = new List<EventTeams.Team>();
			var wc = new WebClient();
			try
			{
				var url = ("http://www.thebluealliance.com/api/v2/event/" + eventKey + "/teams");
				teamList = JsonConvert.DeserializeObject<List<EventTeams.Team>>(wc.DownloadString(url));
			}
			catch (Exception webError)
			{
				Console.WriteLine("Error Message: " + webError.Message);
			}
			return teamList.ToArray();
		}
	}
}
