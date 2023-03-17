using cadastroSemaforico.Database;
using cadastroSemaforico.Models;
using Mopups.Services;
using Xamarin.Essentials;
namespace cadastroSemaforico.Views;

public partial class Cadastro : ContentPage
{
    IGeolocation _geolocation;
    private string nomeFotoPanoramica = "";
    private string nomeFotoDetalhe1 = "";
    private string nomeFotoDetalhe2 = "";
    private string codigo = "";

    public Cadastro()
	{
		InitializeComponent();
	}

    private async void OnClick_To_Save(object sender, EventArgs e)
    {
        //TODO - Pegar os dados da Tela e Criar uma tarefa
        CadastroSemaforico cadastroSemaforico = new CadastroSemaforico();

        cadastroSemaforico.Rodovia = EntryRodovia.Text;
        cadastroSemaforico.Regional = (PckDR.SelectedIndex == -1) ? "" : PckDR.Items[PckDR.SelectedIndex];
        cadastroSemaforico.Sentido = (PckSentido.SelectedIndex == -1) ? "" : PckSentido.Items[PckSentido.SelectedIndex];
        cadastroSemaforico.LadoPista = (PckLadoDaPista.SelectedIndex == -1) ? "" : PckLadoDaPista.Items[PckLadoDaPista.SelectedIndex];
        cadastroSemaforico.AtendimentoNorma = (PckAtendimentoNorma.SelectedIndex == -1) ? "" : PckAtendimentoNorma.Items[PckAtendimentoNorma.SelectedIndex];
        cadastroSemaforico.ObservacaoAN = EntryObsAN.Text;
        cadastroSemaforico.KM = EntryKM.Text;
        cadastroSemaforico.Destinacao = (PckDestinacao.SelectedIndex == -1) ? "" : PckDestinacao.Items[PckDestinacao.SelectedIndex];
        cadastroSemaforico.TipoSinalizacao = (PckTipoSinalizacao.SelectedIndex == -1) ? "" : PckTipoSinalizacao.Items[PckTipoSinalizacao.SelectedIndex];
        cadastroSemaforico.FormaFoco = (PckForma.SelectedIndex == -1) ? "" : PckForma.Items[PckForma.SelectedIndex];
        cadastroSemaforico.IndicacaoLuminosa = (PckIndicacaoLuminosa.SelectedIndex == -1) ? "" : PckIndicacaoLuminosa.Items[PckIndicacaoLuminosa.SelectedIndex];
        cadastroSemaforico.SequenciaLuminosa = (PckSequencia.SelectedIndex == -1) ? "" : PckSequencia.Items[PckSequencia.SelectedIndex];
        cadastroSemaforico.EstadoConservacao = (PckEstadoConservacao.SelectedIndex == -1) ? "" : PckEstadoConservacao.Items[PckEstadoConservacao.SelectedIndex];
        cadastroSemaforico.ObservacaoEC = EntryObsEC.Text;
        cadastroSemaforico.Observacao = EntryObs.Text;
        cadastroSemaforico.Latitude = EntryLatitude.Text;
        cadastroSemaforico.Longitude = EntryLongitude.Text;
        cadastroSemaforico.FotoPanoramica = nomeFotoPanoramica;
        cadastroSemaforico.FotoDetalhe1 = nomeFotoDetalhe1;
        cadastroSemaforico.FotoDetalhe2 = nomeFotoDetalhe2;
        cadastroSemaforico.CodigoElemento = codigo;

        //TODO - Validação dos dados

        //TODO - Salvar a Tarefa no Banco
        await new CadastroDB().CadastrarAsync(cadastroSemaforico);

        //TODO - MessagingCenter Retornar a Tarefa para a tela de listagem.
    }

    private async void OnClick_To_GetCoordinates(object sender, EventArgs e)
    {
        var location = await Xamarin.Essentials.Geolocation.GetLocationAsync();

        if (location != null)
        {
            await DisplayAlert("Problema com as Coordenadas", $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}", "OK");
        }
        else
        {
            EntryLatitude.Text = location.Latitude.ToString();
            EntryLongitude.Text = location.Longitude.ToString();
        }
    }

    private void OnClick_To_GetPhoto(object sender, EventArgs e)
    {
        MopupService.Instance.PushAsync(new PopupFoto(), true);
    }
}