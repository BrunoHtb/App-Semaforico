namespace cadastroSemaforico.Views;

public partial class Menu : ContentPage
{
	public Menu()
	{
		InitializeComponent();
	}

    private async void Clicked_To_ListaCadastro(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ListaCadastro());
    }
}