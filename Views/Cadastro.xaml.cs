using cadastroSemaforico.Database;
using cadastroSemaforico.Models;
using Microsoft.Maui.Media;
using Location = Microsoft.Maui.Devices.Sensors.Location;
using Microsoft.Maui.Graphics;
using CommunityToolkit.Maui.Views;
using Plugin.Media;

namespace cadastroSemaforico.Views;

public partial class Cadastro : ContentPage
{
    CancellationTokenSource _cancelTokenSource;
    bool _isCheckingLocation;
    private string nomeFotoPanoramica = "";
    private string nomeFotoDetalhe1 = "";
    private string nomeFotoDetalhe2 = "";
    private string dateRegisterBegin;
    public Cadastro()
	{
		InitializeComponent();
        dateRegisterBegin = DateTime.Now.ToString("dd-MM-yyyy_HHmmss");
    }

    private async void OnClick_To_Save(object sender, EventArgs e)
    {
        if (PckDR.SelectedIndex == -1 || string.IsNullOrEmpty(EntryRodovia.Text) || PckLadoDaPista.SelectedIndex == -1 || PckSentido.SelectedIndex == -1)
        {
            await DisplayAlert("Alerta de campo sem preenchimento", "Campo obrigatório não preenchido", "OK");
            return;
        }

        //TODO - Pegar os dados da Tela e Criar uma tarefa
        CadastroSemaforico cadastroSemaforico = new CadastroSemaforico();

        //TODO - Validação dos dados
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
        cadastroSemaforico.CodigoElemento = GetCodeRegister();

        //TODO - Salvar a Tarefa no Banco
        await new CadastroDB().CadastrarAsync(cadastroSemaforico);

        //TODO - MessagingCenter Retornar a Tarefa para a tela de listagem.
        await DisplayAlert("Dados Salvos", "As informações foram salvas com sucesso", "OK");
        await App.Current.MainPage.Navigation.PushAsync(new ListaCadastro());
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

    private async void OnClick_To_GetPhoto(object sender, EventArgs e)
    {
        if (PckDR.SelectedIndex == -1 || string.IsNullOrEmpty(EntryRodovia.Text) || PckLadoDaPista.SelectedIndex == -1 || PckSentido.SelectedIndex == -1)
        {
            await DisplayAlert("Alerta de campo sem preenchimento", "Campo obrigatório não preenchido", "OK");
            return;
        }
        int ultimoDigito;
        if (sender == BtnFotoPanoramica)
        {
            ultimoDigito = 1;
        }
        else if(sender == BtnFotoDetalhe1)
        {
            ultimoDigito = 2;
        }
        else
        {
            ultimoDigito = 3;
        }

        var page = new PopupFoto();
        this.ShowPopup(page);
        var result = await page.Show();
 
        if (!result && page.opcao)
        {
            GetPhoto_To_Camera(ultimoDigito);
        }
        else
        {
            GetPhoto_To_Gallery(ultimoDigito);
        }
        page.Close();
        
    }

    private async void GetPhoto_To_Camera(int ultimoDigito)
    {
        FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
        NamePhotoAndSaveDirectory(photo, ultimoDigito);
    }
    private async void GetPhoto_To_Gallery(int ultimoDigito)
    {
        FileResult photo = await MediaPicker.Default.PickPhotoAsync();
        NamePhotoAndSaveDirectory(photo, ultimoDigito);
    }
    private async void NamePhotoAndSaveDirectory(FileResult photo, int ultimoDigito)
    {
        try
        {
            string nomeFoto = "Semaforica_DR" + GetCodeRegister();

            if (photo != null)
            {
                nomeFoto += "_" + ultimoDigito + ".jpg";

                photo.FileName = nomeFoto;

                string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);

                await sourceStream.CopyToAsync(localFileStream);

                ButtonTakePhotoOK(ultimoDigito, nomeFoto);
                return;
            }
        }
        catch { }
    }

    private void ButtonTakePhotoOK(int ultimoDigito, string nomeFoto) 
    {
        Button selectButton;
        
        if(ultimoDigito == 1)
        {
            selectButton = BtnFotoPanoramica;
            nomeFotoPanoramica = nomeFoto;
        }
        else if(ultimoDigito == 2)
        {
            selectButton = BtnFotoDetalhe1;
            nomeFotoDetalhe1 = nomeFoto;
        }
        else
        {
            selectButton = BtnFotoDetalhe2;
            nomeFotoDetalhe2 = nomeFoto;
        }

        selectButton.Text = "Foto ✔";
        selectButton.BackgroundColor = Color.FromRgb(50, 205, 50);
        selectButton.TextColor = Color.FromRgb(255, 255, 255);
    }

    private string GetCodeRegister()
    {
        return PckDR.Items[PckDR.SelectedIndex] + "_" + EntryRodovia.Text.ToUpper().Replace(" ", "") + "_KM_" + EntryKM.Text +
            "_" + PckSentido.Items[PckSentido.SelectedIndex].Substring(0, 3) + "_" + PckLadoDaPista.Items[PckLadoDaPista.SelectedIndex].Substring(0, 1) +
            dateRegisterBegin;
    }

}