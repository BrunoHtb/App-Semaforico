namespace cadastroSemaforico.Views;

public partial class Exportacao : ContentPage
{
	public Exportacao()
	{
		InitializeComponent();
	}
    private async void OnButtonClicked_To_Exportacao(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Exportacao());
    }
    private void OnButtonClicked_To_Menu(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}
