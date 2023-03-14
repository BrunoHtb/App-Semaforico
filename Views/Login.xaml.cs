namespace cadastroSemaforico.Views;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

    private void OnButtonClicked_To_Menu(object sender, EventArgs e)
    {
		Navigation.PushAsync(new Menu());
    }
}