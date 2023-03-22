using cadastroSemaforico.Database;


namespace cadastroSemaforico.Views;

public partial class ListaCadastro : ContentPage
{
	public ListaCadastro()
	{
		InitializeComponent();

        //Executa em uma thread de forma paralela
        Task.Run(() => {
            //Coloca na thread principal e com a parte visual
            Device.BeginInvokeOnMainThread(async () => {
                CVListaCadastro.ItemsSource = await new CadastroDB().PesquisarAsync();
            });         
        });

        CreateDirectory();
    }
    private void CreateDirectory()
    {
        var root = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        string path = root + "/Semaforico";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
    private async void OnButtonClicked_To_Cadastro(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Cadastro());
    }

    private void CVListaCadastro_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }
}