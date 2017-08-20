using System;
using System.Threading.Tasks;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;

namespace VitruvianApp2017
{
	public class FirebaseAccess
	{
		public static async Task<bool> checkExistingMatchData(FirebaseClient database, string path){
			var dataCheck = await database
							.Child(GlobalVariables.regionalPointer)
							.Child(path)
							.OnceSingleAsync<MatchData>();
			if (dataCheck != null)
				return true;
			else
				return false;
		}

		public static void saveMatchData(FirebaseClient database, string path, MatchData data) {
			var dataCheck = database
							.Child(GlobalVariables.regionalPointer)
							.Child(path)
							.Child(data.matchID)
							.PutAsync(data);
		}

		public static void saveMatchActions(FirebaseClient database, string path, ActionData[] data) {
			var dataCheck = database
							.Child(GlobalVariables.regionalPointer)
							.Child(path)
							.PutAsync(data);
		}
	}
}
