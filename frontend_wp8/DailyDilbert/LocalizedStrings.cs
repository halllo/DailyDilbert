﻿using DailyDilbert.Resources;

namespace DailyDilbert
{
	public class LocalizedStrings
	{
		private static AppResources _localizedResources = new AppResources();

		public AppResources LocalizedResources { get { return _localizedResources; } }
	}
}