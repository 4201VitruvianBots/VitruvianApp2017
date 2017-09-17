using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TheBlueAlliance.Models
{
	public class EventOprs
	{
		[JsonProperty("oprs")]
		public Dictionary<string, double> oprs { get; set; }
		[JsonProperty("dprs")]
		public Dictionary<string, double> dprs { get; set; }
		[JsonProperty("ccwms")]
		public Dictionary<string, double> ccwms { get; set; }
	}
}

