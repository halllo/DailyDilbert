using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows;

namespace DailyDilbert
{
	public partial class MainPage : PhoneApplicationPage
	{
		public MainPage()
		{
			InitializeComponent();
			headerDate.Text = DateTime.Now.ToShortDateString();
		}

		private void ApplicationBarIconButton_Click(object sender, EventArgs e)
		{
			CreateTile(new Uri("http://dailydilbert.eu01.aws.af.cm/image"));
		}

		private static void CreateTile(Uri image)
		{
			try
			{
				ShellTile.Create(
					new Uri("/MainPage.xaml?tileid=" + 0, UriKind.Relative),
					new FlipTileData() { SmallBackgroundImage = image, BackgroundImage = image, WideBackgroundImage = image, },
					true);
			}
			catch (Exception)
			{
				MessageBox.Show("you already have a tile on your home screen", "could not create tile", MessageBoxButton.OK);
			}
		}
	}
}