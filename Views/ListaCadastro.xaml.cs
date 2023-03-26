using cadastroSemaforico.Database;
using cadastroSemaforico.Models;
using Microsoft.Maui.Controls;

namespace cadastroSemaforico.Views;

public partial class ListaCadastro : ContentPage
{
    CadastroSemaforico _cadastroSemaforico;

	public ListaCadastro()
	{
		InitializeComponent();

        //Executa em uma thread de forma paralela
        Task.Run(() => {
            //Coloca na thread principal e com a parte visual
            Device.BeginInvokeOnMainThread(async () => {
                CVListaCadastro.ItemsSource = await new CadastroSQLiteDB().PesquisarAsync();
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
    private async void OnButtonClicked_To_CadastroNovo(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Cadastro());
    }

    private void Selection_Item_Changed(object sender, SelectionChangedEventArgs e)
    {
        if(e.CurrentSelection.FirstOrDefault() is CadastroSemaforico selectedItem)
        {
            LabelRodovia.Text = selectedItem.Rodovia;
            LabelCodigoElemento.Text = selectedItem.CodigoElemento;

            _cadastroSemaforico = selectedItem;
        }
    }

    private async void OnButtonClicked_To_CadastroEditar(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new Cadastro(_cadastroSemaforico));
        }
        catch(Exception ex) { }
    }

}