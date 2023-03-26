namespace cadastroSemaforico.Views;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

    private async void OnButtonClicked_To_Menu(object sender, EventArgs e)
    {	
		if (string.IsNullOrEmpty(EntryID.Text) || string.IsNullOrEmpty(EntryUsuario.Text) || string.IsNullOrEmpty(EntryAuditoria.Text))
		{
			await DisplayAlert("ERRO!","Por favor, preencha todos os campos.","OK");
		} else 
		{ 
			await Navigation.PushAsync(new Menu());
        }
    }
}