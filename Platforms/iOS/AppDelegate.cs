using Foundation;
using UIKit;

namespace cadastroSemaforico;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        SQLitePCL.Batteries.Init();
        return base.FinishedLaunching(application, launchOptions);
    }
}
