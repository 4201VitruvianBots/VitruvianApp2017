using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using ModernHttpClient;
using Newtonsoft.Json;
using TheBlueAlliance.Models;
using VitruvianApp2017;

namespace TheBlueAlliance
{
	public class EventsHttp
	{
		static string headerString = GlobalVariables.TBAAuthKey;
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
			var url = ("http://www.thebluealliance.com/api/v3/event/" + eventKey + "/teams");

			try
			{
				var client = new HttpClient(new NativeMessageHandler());
				client.DefaultRequestHeaders.Add("X-TBA-Auth-Key", headerString);
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

		public static async Task<EventMatches.Match[]> GetEventMatchesHttp(string eventKey) {
			var dataList = new List<EventMatches.Match>();
			var url = ("http://www.thebluealliance.com/api/v3/event/" + eventKey + "/matches");

			try {
				var client = new HttpClient(new NativeMessageHandler());
				client.DefaultRequestHeaders.Add("X-TBA-Auth-Key", headerString);
				using (client)
				using (HttpResponseMessage response = await client.GetAsync(url))
				using (HttpContent content = response.Content) {
					var result = await content.ReadAsStreamAsync();
					var jsonString = new StreamReader(result).ReadToEnd();
					dataList = JsonConvert.DeserializeObject<List<EventMatches.Match>>(jsonString);

					//Remove elimination matches
					//dataList.RemoveAll((obj) => obj.comp_level.Equals("qf"));
					//dataList.RemoveAll((obj) => obj.comp_level.Equals("sf"));
					//dataList.RemoveAll((obj) => obj.comp_level.Equals("f"));
						
					// Sort match in descending order
					dataList.Sort((x, y) => x.time.CompareTo(y.time));
					foreach (var match in dataList)
						Console.WriteLine("TBA Match Fetch: " + match.match_number);
					 
				}
			} catch (Exception webError) {
				Console.WriteLine("Error Message: " + webError.Message);
			}
			
			return dataList.ToArray();
		}

		public static async Task<EventStatsHttp[]> GetEventStatsHttp(string eventKey) {
			var dataList = new List<EventStatsHttp>();
			var url = ("http://www.thebluealliance.com/api/v3/event/" + eventKey + "/stats");

			try {
				var client = new HttpClient(new NativeMessageHandler());
				client.DefaultRequestHeaders.Add("X-TBA-Auth-Key", headerString);
				using (client)
				using (HttpResponseMessage response = await client.GetAsync(url))
				using (HttpContent content = response.Content) {
					var result = await content.ReadAsStreamAsync();
					var jsonString = new StreamReader(result).ReadToEnd();
					Console.WriteLine("JSON String: " + jsonString);
					dataList = JsonConvert.DeserializeObject<List<EventStatsHttp>>(jsonString);

					// Sort match in descending order
					foreach (var stats in dataList)
						foreach(var opr in stats.oprs)
							Console.WriteLine("TBA Stat Fetch - OPR: " + opr);

				}
			} catch (Exception webError) {
				Console.WriteLine("Error Message: " + webError.Message);
			}

			return dataList.ToArray();
		}
	}
}
