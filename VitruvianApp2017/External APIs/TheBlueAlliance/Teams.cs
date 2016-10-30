using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using Newtonsoft.Json;
using TheBlueAlliance.Models;

namespace TheBlueAlliance
{
	public class Teams
	{
		static string headerString = "frc4201:VitruvianApp2017:v01";

		public static TeamInformation GetTeamInformation(string teamKey)
		{
			var teamInformationToReturn = new TeamInformation();
			var wc = new WebClient();
			wc.Headers.Add("X-TBA-App-Id", headerString);
			try
			{
				var url = ("https://www.thebluealliance.com/api/v2/team/" + teamKey);
				teamInformationToReturn = JsonConvert.DeserializeObject<TeamInformation>(wc.DownloadString(url));
			}
			catch (Exception webError)
			{
				Console.WriteLine("Error Message: " + webError.Message);
			}
			return teamInformationToReturn;
		}
	}
}
