using cadastroSemaforico.Database;
using cadastroSemaforico.Models;
using The49.Maui.BottomSheet;

namespace cadastroSemaforico.Views;

public partial class ListaCadastro : ContentPage
{
    CadastroSemaforico _cadastroSemaforico;

    public ListaCadastro()
	{
		InitializeComponent(); 
    }

    [Obsolete]
    protected override async void OnAppearing()
    {
        try
        {
            base.OnAppearing();

            //Coloca na thread principal e com a parte visual
            Device.BeginInvokeOnMainThread(async () => {
                CVListaCadastro.ItemsSource = await new CadastroSQLiteDB().PesquisarAsync();
                CountElement(); 
                LabelTotalElementos.Text = "Total: " + CountElement().ToString();
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Aviso!", "Sistema de coleta " + ex.Message, "OK");
        }
    }

    private int CountElement()
    {
        int contador = 0;

        foreach (var item in CVListaCadastro.ItemsSource)
        {
            contador++;
        }
        return contador;
    }

    private async void OnButtonClicked_To_CadastroNovo(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Cadastro());
    }

    private async void Selection_Item_Changed(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            if (e.CurrentSelection.FirstOrDefault() is CadastroSemaforico selectedItem)
            {
                var bottomSheet = new ListaCadastroTelaInferior(selectedItem, this);
               
                bottomSheet.IsModal = true;
                bottomSheet.ShowHandle = true;
                bottomSheet.Detents = new DetentsCollection()
                {
                    new FullscreenDetent(),
                    new ContentDetent(),
                };


                bottomSheet.Show(Window);

                _cadastroSemaforico = selectedItem;
            }
        }
        catch(Exception ex)
        {
            await DisplayAlert("Aviso!", "Erro " + ex.Message, "FECHAR");
        }
    }

    public async void OnButtonClicked_To_CadastroEditar()
    {
        try
        {
            await Navigation.PushAsync(new Cadastro(_cadastroSemaforico));
        }
        catch(Exception ex) 
        {
            await DisplayAlert("Aviso!", "Erro " + ex.Message, "FECHAR");
        }
    }

}