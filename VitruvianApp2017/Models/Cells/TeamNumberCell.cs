using System;
using Xamarin.Forms;

namespace VitruvianApp2017
{
	public class TeamNumberCell:TextCell
	{
		public TeamNumberCell() : this(null, null) {

		}

		public TeamNumberCell(string dataheader, string data) {
			Text = dataheader;
			Detail = data;
		}

		protected override void OnTapped() {

		}
	}
}
