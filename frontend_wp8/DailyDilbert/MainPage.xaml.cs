﻿using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DailyDilbert
{
	public partial class MainPage : PhoneApplicationPage
	{
		public MainPage()
		{
			InitializeComponent();
			headerDate.Text = DateTime.Now.ToShortDateString();
			Loaded += MainPage_Loaded;
		}

		async void MainPage_Loaded(object sender, RoutedEventArgs e)
		{
			new UpdateBackgroundAgent().StartPeriodicAgent();

			var cachedDailyDilbertImageUrl = DailyDilbertLogic.GetCachedDailyDilbertImageUrl();
			if (cachedDailyDilbertImageUrl != null)
			{
				DailyDilbertImage.Source = new BitmapImage() { UriSource = cachedDailyDilbertImageUrl };
			}
			else
			{
				ReloadDailyDilbertImage();
			}
		}

		async void ReloadDailyDilbertImage()
		{
			var dailyDilbertImageUrl = await DailyDilbertLogic.GetImageUrl();
			DailyDilbertImage.Source = new BitmapImage() { UriSource = dailyDilbertImageUrl };
		}

		async void ApplicationBarIconButton_Click(object sender, EventArgs e)
		{
			DailyDilbertImage.Source = null;
			loadingProgressBar.Visibility = System.Windows.Visibility.Visible;
			ReloadDailyDilbertImage();
		}

		private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
		{
			new WebBrowserTask { Uri = new Uri("http://dilbert.com", UriKind.Absolute) }.Show();
		}

		#region zoom and pan logic from Microsoft: http://code.msdn.microsoft.com/wpapps/Image-Recipes-0c0b8fee
		const double MaxScale = 10;

		double _scale = 1.0;
		double _minScale;
		double _coercedScale;
		double _originalScale;

		Size _viewportSize;
		bool _pinching;
		Point _screenMidpoint;
		Point _relativeMidpoint;

		BitmapImage _bitmap;

		/// <summary> 
		/// Either the user has manipulated the image or the size of the viewport has changed. We only 
		/// care about the size. 
		/// </summary> 
		void viewport_ViewportChanged(object sender, System.Windows.Controls.Primitives.ViewportChangedEventArgs e)
		{
			Size newSize = new Size(viewport.Viewport.Width, viewport.Viewport.Height);
			if (newSize != _viewportSize)
			{
				_viewportSize = newSize;
				CoerceScale(true);
				ResizeImage(false);
			}
		}

		/// <summary> 
		/// Handler for the ManipulationStarted event. Set initial state in case 
		/// it becomes a pinch later. 
		/// </summary> 
		void OnManipulationStarted(object sender, ManipulationStartedEventArgs e)
		{
			_pinching = false;
			_originalScale = _scale;
		}

		/// <summary> 
		/// Handler for the ManipulationDelta event. It may or may not be a pinch. If it is not a  
		/// pinch, the ViewportControl will take care of it. 
		/// </summary> 
		/// <param name="sender"></param> 
		/// <param name="e"></param> 
		void OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
		{
			if (e.PinchManipulation != null)
			{
				e.Handled = true;

				if (!_pinching)
				{
					_pinching = true;
					Point center = e.PinchManipulation.Original.Center;
					_relativeMidpoint = new Point(center.X / DailyDilbertImage.ActualWidth, center.Y / DailyDilbertImage.ActualHeight);

					var xform = DailyDilbertImage.TransformToVisual(viewport);
					_screenMidpoint = xform.Transform(center);
				}

				_scale = _originalScale * e.PinchManipulation.CumulativeScale;

				CoerceScale(false);
				ResizeImage(false);
			}
			else if (_pinching)
			{
				_pinching = false;
				_originalScale = _scale = _coercedScale;
			}
		}

		/// <summary> 
		/// The manipulation has completed (no touch points anymore) so reset state. 
		/// </summary> 
		void OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
		{
			_pinching = false;
			_scale = _coercedScale;
		}


		/// <summary> 
		/// When a new image is opened, set its initial scale. 
		/// </summary> 
		void OnImageOpened(object sender, RoutedEventArgs e)
		{
			loadingProgressBar.Visibility = Visibility.Collapsed;

			_bitmap = (BitmapImage)DailyDilbertImage.Source;

			// Set scale to the minimum, and then save it. 
			_scale = 0;
			CoerceScale(true);
			_scale = _coercedScale;

			ResizeImage(true);
		}

		/// <summary> 
		/// Adjust the size of the image according to the coerced scale factor. Optionally 
		/// center the image, otherwise, try to keep the original midpoint of the pinch 
		/// in the same spot on the screen regardless of the scale. 
		/// </summary> 
		/// <param name="center"></param> 
		void ResizeImage(bool center)
		{
			if (_coercedScale != 0 && _bitmap != null)
			{
				double newWidth = canvas.Width = Math.Round(_bitmap.PixelWidth * _coercedScale);
				double newHeight = canvas.Height = Math.Round(_bitmap.PixelHeight * _coercedScale);

				xform.ScaleX = xform.ScaleY = _coercedScale;

				viewport.Bounds = new Rect(0, 0, newWidth, newHeight);

				if (center)
				{
					viewport.SetViewportOrigin(
						new Point(
							Math.Round((newWidth - viewport.ActualWidth) / 2),
							Math.Round((newHeight - viewport.ActualHeight) / 2)
							));
				}
				else
				{
					Point newImgMid = new Point(newWidth * _relativeMidpoint.X, newHeight * _relativeMidpoint.Y);
					Point origin = new Point(newImgMid.X - _screenMidpoint.X, newImgMid.Y - _screenMidpoint.Y);
					viewport.SetViewportOrigin(origin);
				}
			}
		}

		/// <summary> 
		/// Coerce the scale into being within the proper range. Optionally compute the constraints  
		/// on the scale so that it will always fill the entire screen and will never get too big  
		/// to be contained in a hardware surface. 
		/// </summary> 
		/// <param name="recompute">Will recompute the min max scale if true.</param> 
		void CoerceScale(bool recompute)
		{
			if (recompute && _bitmap != null && viewport != null)
			{
				// Calculate the minimum scale to fit the viewport 
				double minX = viewport.ActualWidth / _bitmap.PixelWidth;
				double minY = viewport.ActualHeight / _bitmap.PixelHeight;

				_minScale = Math.Min(minX, minY);
			}

			_coercedScale = Math.Min(MaxScale, Math.Max(_scale, _minScale));

		}
		#endregion
	}
}