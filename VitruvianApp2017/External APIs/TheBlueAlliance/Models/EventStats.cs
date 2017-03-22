using System;
using System.Collections.Generic;

namespace TheBlueAlliance.Models
{
	public class EventStatsHttp
	{
		public List<OPR> oprs { get; set; }
		public List<CCWM> ccwms { get; set; }
		public List<DPR> dprs { get; set; }
	}

	public class OPR
	{
		public int team_number { get; set; }
		public double opr { get; set; }

	}

	public class CCWM
	{
		public int team_number { get; set; }
		public double ccwm { get; set; }

	}

	public class DPR
	{
		public int team_number { get; set; }
		public double dpr { get; set; }

	}
}
