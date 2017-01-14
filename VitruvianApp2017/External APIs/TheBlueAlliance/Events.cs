﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using ModernHttpClient;
using Newtonsoft.Json;
using TheBlueAlliance.Models;

namespace TheBlueAlliance
{
	public class EventsHttp
	{
		static string headerString = "frc4201:VitruvianApp2017:v01";
		/*
		public static EventTeams.Team[] GetEventTeamsList(string eventKey)
		{
			var teamList = new List<EventTeams.Team>();
			var wc = new WebClient();
			wc.Headers.Add("X-TBA-App-Id", headerString);
			try
			{
				var url = ("https://www.thebluealliance.com/api/v2/event/" + eventKey + "/teams");
				teamList = JsonConvert.DeserializeObject<List<EventTeams.Team>>(wc.DownloadString(url));
			}
			catch (Exception webError)
			{
				Console.WriteLine("Error Message: " + webError.Message);
			}
			return teamList.ToArray();
		}
		*/

		public static async Task<EventTeams.Team[]> GetEventTeamsListHttp(string eventKey)
		{
			var teamList = new List<EventTeams.Team>();
			var url = ("http://www.thebluealliance.com/api/v2/event/" + eventKey + "/teams");

			try
			{
				var client = new HttpClient(new NativeMessageHandler());
				client.DefaultRequestHeaders.Add("X-TBA-App-Id", headerString);
				using (client)
					using (HttpResponseMessage response = await client.GetAsync(url))
						using (HttpContent content = response.Content)
						{
							var result = await content.ReadAsStreamAsync();
							var jsonString = new StreamReader(result).ReadToEnd();
							teamList = JsonConvert.DeserializeObject<List<EventTeams.Team>>(jsonString);

							// Sort team number in descending order
							teamList.Sort((x, y) => x.team_number.CompareTo(y.team_number));
						}
			}
			catch (Exception webError)
			{
				Console.WriteLine("Error Message: " + webError.Message);
			}
			return teamList.ToArray();
		}
	}
}