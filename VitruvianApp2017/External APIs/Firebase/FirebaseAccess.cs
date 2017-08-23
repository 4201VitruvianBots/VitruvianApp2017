using System;
using System.Collections.Generic;
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

		public static void saveData(FirebaseClient database, string path, Object data) {
			var dataCheck = database
							.Child(GlobalVariables.regionalPointer)
							.Child(path)
							.PutAsync(data);
		}

		/*
		public async Task getSingleData(FirebaseClient database, string path, Type dataType) {
			return await database
							.Child(GlobalVariables.regionalPointer)
							.Child(path)
							.OnceSingleAsync<TypeOf(dataType)>();
		}
		*/
	}
}
