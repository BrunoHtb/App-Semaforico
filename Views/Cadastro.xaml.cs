using cadastroSemaforico.Database;
using cadastroSemaforico.Models;
using Mopups.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace cadastroSemaforico.Views;

public partial class Cadastro : ContentPage
{
    CancellationTokenSource _cancelTokenSource;
    bool _isCheckingLocation;
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
        try
        {
            _isCheckingLocation = true;

            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

            _cancelTokenSource = new CancellationTokenSource();

            Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

            EntryLatitude.Text = location.Latitude.ToString();
            EntryLongitude.Text = location.Longitude.ToString();   
        }
        catch (Exception ex)
        {
        }
        finally
        {
            _isCheckingLocation = false;
        }
    }

    private async void OnClick_To_GetPhotoPanoramica(object sender, EventArgs e)
    {
        var pagina = new PopupFoto();
        await MopupService.Instance.PushAsync(pagina, true);
        var result = await pagina.Show();

        if(result && !pagina.opcao)
        {

        }
        else
        {

        }
    }

    private void OnClick_To_GetPhotoDetalhe1(object sender, EventArgs e)
    {

    }

    private void OnClick_To_GetPhotoDetalhe2(object sender, EventArgs e)
    {

    }

    private async void GetPhoto_To_Camera()
    {
        await CrossMedia.Current.Initialize();

        if (!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
        {
            await DisplayAlert("Ops", "Nenhuma câmera detectada.", "OK");
            return;
        }

        if (PckDR.SelectedIndex == -1 || string.IsNullOrEmpty(EntryRodovia.Text) || PckLadoDaPista.SelectedIndex == -1 || PckSentido.SelectedIndex == -1)
        {
            await DisplayAlert("Alerta de campo sem preenchimento", "Campo obrigatório não preenchido", "OK");
            return;
        }
    }

    private async void SavePhoto()
    {
        string nomeFoto;
        var file = await CrossMedia.Current.TakePhotoAsync(
            new StoreCameraMediaOptions
            {
                SaveToAlbum = true,
                //Name = _nomeDaFotoDetalhe1,
                Directory = "PRU",
                CompressionQuality = 60
            });

        if (file != null)
        {
            //FotoDetalhe1.Text = "Foto Detalhe 1 ✔";
            //FotoDetalhe1.BackgroundColor = System.Drawing.Color.LimeGreen;
            //FotoDetalhe1.TextColor = System.Drawing.Color.White;
            return;
        }

    }


}