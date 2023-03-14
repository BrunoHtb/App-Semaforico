using cadastroSemaforico.Views;

namespace cadastroSemaforico;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new NavigationPage(new Login());
	}
}
