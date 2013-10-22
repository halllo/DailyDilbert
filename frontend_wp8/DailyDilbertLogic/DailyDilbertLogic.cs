using Microsoft.Phone.Shell;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DailyDilbert
{
	public static class DailyDilbertLogic
	{
		public static void UpdateTile(Uri imageUrl)
		{
			ShellTile.ActiveTiles.First().Update(new FlipTileData()
			{
				SmallBackgroundImage = imageUrl,
				BackgroundImage = imageUrl,
				WideBackgroundImage = imageUrl,
			});
		}

		public static async Task<Uri> GetImageUrl()
		{
			var httpClient = new HttpClient();
			var dailyDilbertImageUrl = await httpClient.GetStringAsync("http://dailydilbert.eu01.aws.af.cm/url?" + DateTime.Now.Ticks);
			return new Uri(dailyDilbertImageUrl, UriKind.Absolute);
		}
	}
}
