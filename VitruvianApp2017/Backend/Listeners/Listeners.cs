using System;
using System.Threading;
using System.Collections;
using Android.Gms.Tasks;

namespace VitruvianApp2017
{
	internal class CompleteListener : Java.Lang.Object, IOnCompleteListener
	{
		// This is probably implemented very poorly/could be improved
		public string data { get; set; }
		public SemaphoreSlim signal = new SemaphoreSlim(0, 1);


		public void OnComplete(Android.Gms.Tasks.Task task)
		{
			if (task.IsComplete)
			{
				signal.Release();
				try {
					data = task.Result.ToString();
					Console.WriteLine("Task Completed");
				}
				catch (Exception ex) {
					Console.WriteLine("Error: " + ex);
				}
			}
			else {
				Console.WriteLine("Task Failed");
			}
		}
		/*
		public event EventHandler taskSuccessful;
		public event EventHandler isComplete
		{
			add { taskSuccessful += value; }
			remove { taskSuccessful -= value; }
		}
		*/
	}

	internal class SuccessListener : Java.Lang.Object, IOnSuccessListener
	{
		public void OnSuccess(Java.Lang.Object obj)
		{

		}
	}
}
