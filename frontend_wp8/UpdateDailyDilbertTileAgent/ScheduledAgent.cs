﻿using Microsoft.Phone.Scheduler;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace UpdateDailyDilbertTileAgent
{
	public class ScheduledAgent : ScheduledTaskAgent
	{
		static ScheduledAgent()
		{
			Deployment.Current.Dispatcher.BeginInvoke(delegate
			{
				Application.Current.UnhandledException += UnhandledException;
			});
		}

		private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
		{
			if (Debugger.IsAttached)
			{
				Debugger.Break();
			}
		}

		protected override void OnInvoke(ScheduledTask task)
		{
			Task<Uri> dailyDilbertImageUrl = DailyDilbert.DailyDilbertLogic.GetImageUrl();
			dailyDilbertImageUrl.ContinueWith(t => 
			{
				DailyDilbert.DailyDilbertLogic.UpdateTile(t.Result);
				NotifyComplete();
			});
		}
	}
}