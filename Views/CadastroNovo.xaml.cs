using cadastroSemaforico.Database;
using cadastroSemaforico.Models;
using Location = Microsoft.Maui.Devices.Sensors.Location;
using CommunityToolkit.Maui.Views;


namespace cadastroSemaforico.Views;

public partial class Cadastro : ContentPage
{
    CancellationTokenSource _cancelTokenSource;
    bool _isCheckingLocation;
    private string _nomeFotoPanoramica = "";
    private string _nomeFotoDetalhe1 = "";
    private string _nomeFotoDetalhe2 = "";
    private string _dateRegisterBegin;

    public Cadastro()
	{
		InitializeComponent();
        _dateRegisterBegin = DateTime.Now.ToString("dd-MM-yyyy_HHmmss");
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
        cadastroSemaforico.FotoPanoramica = _nomeFotoPanoramica;
        cadastroSemaforico.FotoDetalhe1 = _nomeFotoDetalhe1;
        cadastroSemaforico.FotoDetalhe2 = _nomeFotoDetalhe2;
        cadastroSemaforico.CodigoElemento = GetCode_Register();
        cadastroSemaforico.DataCadastro = _dateRegisterBegin;

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
        NamePhoto_And_SaveDirectory(photo, ultimoDigito);
    }
    private async void GetPhoto_To_Gallery(int ultimoDigito)
    {
        FileResult photo = await MediaPicker.Default.PickPhotoAsync();
        NamePhoto_And_SaveDirectory(photo, ultimoDigito);
    }
    private async void NamePhoto_And_SaveDirectory(FileResult photo, int ultimoDigito)
    {
        try
        {
            string nomeFoto = "Semaforica_DR" + GetCode_Register();

            if (photo != null)
            {
                nomeFoto += "_" + ultimoDigito + ".jpg";

                photo.FileName = nomeFoto;

                string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);

                await sourceStream.CopyToAsync(localFileStream);

                Button_Take_PhotoOK(ultimoDigito, nomeFoto);
                return;
            }
        }
        catch { }
    }

    private void Button_Take_PhotoOK(int ultimoDigito, string nomeFoto) 
    {
        Button selectButton;
        
        if(ultimoDigito == 1)
        {
            selectButton = BtnFotoPanoramica;
            _nomeFotoPanoramica = nomeFoto;
        }
        else if(ultimoDigito == 2)
        {
            selectButton = BtnFotoDetalhe1;
            _nomeFotoDetalhe1 = nomeFoto;
        }
        else
        {
            selectButton = BtnFotoDetalhe2;
            _nomeFotoDetalhe2 = nomeFoto;
        }

        selectButton.Text = "Foto ✔";
        selectButton.BackgroundColor = Color.FromRgb(50, 205, 50);
        selectButton.TextColor = Color.FromRgb(255, 255, 255);
    }
    private void Button_Take_PhotoClear(int ultimoDigito)
    {
        Button selectButton;
         if (ultimoDigito == 1)
        {
            selectButton = BtnFotoPanoramica;
            _nomeFotoPanoramica = "";
        }
        else if (ultimoDigito == 2)
        {
            selectButton = BtnFotoDetalhe1;
            selectButton = BtnFotoDetalhe1;
        }
        else
        {
            selectButton = BtnFotoDetalhe2;
            _nomeFotoDetalhe2 = "";
        }

        selectButton.BackgroundColor = Color.FromRgb(128, 128, 128);
        selectButton.TextColor = Color.FromRgb(255, 255, 255);
    }

    private string GetCode_Register()
    {
        return PckDR.Items[PckDR.SelectedIndex] + "_" + EntryRodovia.Text.ToUpper().Replace(" ", "") + "_KM_" + EntryKM.Text +
            "_" + PckSentido.Items[PckSentido.SelectedIndex].Substring(0, 3) + "_" + PckLadoDaPista.Items[PckLadoDaPista.SelectedIndex].Substring(0, 1) +
            _dateRegisterBegin;
    }

    private void OnClick_To_Clear(object sender, EventArgs e)
    {
        EntryRodovia.Text = string.Empty;
        PckDR.SelectedIndex = -1;
        PckSentido.SelectedIndex = -1;
        PckLadoDaPista.SelectedIndex = -1;
        PckAtendimentoNorma.SelectedIndex = -1;
        EntryObsAN.Text = string.Empty;
        EntryKM.Text = string.Empty;
        PckDestinacao.SelectedIndex = -1;
        PckTipoSinalizacao.SelectedIndex = -1;
        PckForma.SelectedIndex = -1;
        PckIndicacaoLuminosa.SelectedIndex = -1;
        PckSequencia.SelectedIndex = -1;
        PckEstadoConservacao.SelectedIndex = -1;
        EntryObsEC.Text = string.Empty;
        EntryObs.Text = string.Empty;
        EntryLatitude.Text = string.Empty;
        EntryLongitude.Text = string.Empty;
        EntryCodElemento.Text = string.Empty;
        Button_Take_PhotoClear(1);
        Button_Take_PhotoClear(2);
        Button_Take_PhotoClear(3);
    }

    private async void OnClick_To_Cancel(object sender, EventArgs e)
    {
        bool cancelar = await DisplayAlert("Atenção", "Tem certeza que deseja cancelar esse cadastro?", "SIM", "NÃO");
        if (cancelar == true)
        {
            RemoverPageRelacaoElementos();
            await App.Current.MainPage.Navigation.PushAsync(new ListaCadastro());
        }
        else
        {
            return;
        }
    }
    private void RemoverPageRelacaoElementos()
    {
        var removePage = this.Navigation.NavigationStack;
        for (int i = 0; i < removePage.Count; i++)
        {
            var name = removePage[i].Title;
            if (name == "Cadastro Semafórico")
            {
                Navigation.RemovePage(this.Navigation.NavigationStack[i]);
            }
        }
    }
}