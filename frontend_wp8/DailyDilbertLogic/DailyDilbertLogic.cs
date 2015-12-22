using Microsoft.Phone.Shell;
using System;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DailyDilbert
{
	public static class DailyDilbertLogic
	{
		public static void UpdateTile(Uri imageUrl)
		{
			try
			{
				ShellTile.ActiveTiles.First().Update(new FlipTileData()
					{
						SmallBackgroundImage = imageUrl,
						BackgroundImage = imageUrl,
						WideBackgroundImage = imageUrl,
					});
			}
			catch (Exception)
			{
			}
		}

		public static async Task<Uri> GetImageUrl()
		{
			var httpClient = new HttpClient();
			var dailyDilbertImageUrl = await httpClient.GetStringAsync("http://dailydilbert2.azurewebsites.net/url?" + DateTime.Now.Ticks);
			return new Uri(dailyDilbertImageUrl, UriKind.Absolute);
		}


		private const string IS_DailyDilbertImageUrl_Key = "dailyDilbertImageUrl";
		public static void Cache(Uri dailyDilbertImageUrl)
		{
			var settings = IsolatedStorageSettings.ApplicationSettings;
			if (!settings.Contains(IS_DailyDilbertImageUrl_Key))
				settings.Add(IS_DailyDilbertImageUrl_Key, null);
			settings[IS_DailyDilbertImageUrl_Key] = dailyDilbertImageUrl;
			settings.Save();
		}

		public static Uri GetCachedDailyDilbertImageUrl()
		{
			var settings = IsolatedStorageSettings.ApplicationSettings;
			return settings.Contains(IS_DailyDilbertImageUrl_Key) ? settings[IS_DailyDilbertImageUrl_Key] as Uri : null;
		}
	}
}
