namespace cadastroSemaforico.Views;

public partial class ListaCadastro : ContentPage
{
	public ListaCadastro()
	{
		InitializeComponent();
	}

    private async void OnButtonClicked_To_Cadastro(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Cadastro());
    }
}