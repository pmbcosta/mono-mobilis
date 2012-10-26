using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Mobilis.Lib.Util;
using Mobilis.Lib.DataServices;
using Mobilis.Lib.Database;
using Mobilis.Lib;
using Mobilis.Lib.ViewModel;

namespace MonoMobilis
{
	public partial class MonoMobilisViewController : UIViewController
	{

		private CoursesViewController coursesPage;
		private LoginViewModel loginViewModel;

		public MonoMobilisViewController () : base ("MonoMobilisViewController", null)
		{

		}
		
		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			loginViewModel = new LoginViewModel();

			this.submit.TouchUpInside += (sender, e) => {
			
				loginViewModel.submitLoginData(login.Text,password.Text,() =>
				{
					getCourses();
				});

			};
		}

		public void getCourses() 
		{
			loginViewModel.requestCourses(() =>
			{
				ServiceLocator.Dispatcher = new DispatchAdapter(this);
				ServiceLocator.Dispatcher.invoke(() =>
				{
					coursesPage = new CoursesViewController();
					this.NavigationController.PushViewController(this.coursesPage,true);
				});

			});
		}

		/* Navegacao
		 * coursesPage = new CoursesViewController();
		   this.NavigationController.PushViewController(this.coursesPage,true);
		 */


		partial void actionSubmit (NSObject sender)
		{
			System.Diagnostics.Debug.WriteLine("Teste");
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;
			
			ReleaseDesignerOutlets ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}

		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			foreach (var item in this) 
			{
				var tf = item as UITextField;
				if (tf != null && tf.IsFirstResponder) 
				{
				tf.ResignFirstResponder ();
				}		
			}
		base.TouchesEnded (touches, evt);
		}
	}
}

