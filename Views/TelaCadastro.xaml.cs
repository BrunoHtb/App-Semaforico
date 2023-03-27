using cadastroSemaforico.Database;
using cadastroSemaforico.Models;
using Location = Microsoft.Maui.Devices.Sensors.Location;
using CommunityToolkit.Maui.Views;
using cadastroSemaforico.Static;

namespace cadastroSemaforico.Views;

public partial class Cadastro : ContentPage
{
    private CadastroSemaforico _cadastroSemaforico;
    private List<DadoLogin> _lista;
    private bool update;
    CancellationTokenSource _cancelTokenSource;
    private bool _isCheckingLocation;
    private string _nomeFotoPanoramica = "";
    private string _nomeFotoDetalhe1 = "";
    private string _nomeFotoDetalhe2 = "";
    private string _dateRegisterBegin;

    public Cadastro()
	{
		InitializeComponent();

        update = false;
        _cadastroSemaforico = new CadastroSemaforico();
        _dateRegisterBegin = DateTime.Now.ToString("dd-MM-yyyy_HHmmss");

        if (!string.IsNullOrEmpty(VariaveisEstaticas.Regional) && !string.IsNullOrEmpty(VariaveisEstaticas.Rodovia))
        {
            EntryRodovia.Text = VariaveisEstaticas.Rodovia;
            PckDR.SelectedItem = VariaveisEstaticas.Regional;
        }

        GetDadosLogin();
    }

    public Cadastro(CadastroSemaforico cadastroSemaforico)
    {
        InitializeComponent();

        update = true;
        _cadastroSemaforico = cadastroSemaforico;
        FillFields();
    }

    private async void GetDadosLogin()
    {
        _lista = await new CadastroSQLiteDB().PesquisarLoginAsync();
        EntryAuditoria.Text = _lista.FirstOrDefault().Auditoria.ToString();
    }

    private void FillFields()
    {
        EntryRodovia.Text = _cadastroSemaforico.Rodovia;
        PckDR.SelectedItem = _cadastroSemaforico.Regional;
        PckSentido.SelectedItem = _cadastroSemaforico.Sentido;
        PckLadoDaPista.SelectedItem = _cadastroSemaforico.LadoPista;
        PckAtendimentoNorma.SelectedItem = _cadastroSemaforico.AtendimentoNorma;
        EntryObsAN.Text = _cadastroSemaforico.ObservacaoAN;
        EntryKM.Text = _cadastroSemaforico.KM;
        PckDestinacao.SelectedItem = _cadastroSemaforico.Destinacao;
        PckTipoSinalizacao.SelectedItem = _cadastroSemaforico.TipoSinalizacao;
        PckForma.SelectedItem = _cadastroSemaforico.FormaFoco;
        PckIndicacaoLuminosa.SelectedItem = _cadastroSemaforico.IndicacaoLuminosa;
        PckSequencia.SelectedItem = _cadastroSemaforico.SequenciaLuminosa;
        PckEstadoConservacao.SelectedItem = _cadastroSemaforico.EstadoConservacao;
        EntryObsEC.Text = _cadastroSemaforico.ObservacaoEC;
        EntryObs.Text = _cadastroSemaforico.Observacao;
        EntryLatitude.Text = _cadastroSemaforico.Latitude;
        EntryLongitude.Text = _cadastroSemaforico.Longitude;
        EntryCodElemento.Text = _cadastroSemaforico.CodigoElemento;
        _nomeFotoPanoramica = _cadastroSemaforico.FotoPanoramica;
        _nomeFotoDetalhe1 = _cadastroSemaforico.FotoDetalhe1;
        _nomeFotoDetalhe2 = _cadastroSemaforico.FotoDetalhe2;
        _dateRegisterBegin = _cadastroSemaforico.DataCadastro;

        if(_nomeFotoPanoramica != "")
        {
            Button_Take_PhotoOK(1, _nomeFotoPanoramica);
        }
        if (_nomeFotoDetalhe1 != "")
        {
            Button_Take_PhotoOK(2, _nomeFotoDetalhe1);
        }
        if (_nomeFotoDetalhe2 != "")
        {
            Button_Take_PhotoOK(3, _nomeFotoDetalhe2);
        }
    }

    private async void OnClick_To_Save(object sender, EventArgs e)
    {
        try
        {
            if (PckDR.SelectedIndex == -1 || string.IsNullOrEmpty(EntryRodovia.Text) || PckLadoDaPista.SelectedIndex == -1 || PckSentido.SelectedIndex == -1)
            {
                await DisplayAlert("Alerta de campo sem preenchimento", "Campo obrigatório não preenchido", "OK");
                return;
            }

            //TODO - Validação dos dados
            _cadastroSemaforico.Rodovia = EntryRodovia.Text;
            _cadastroSemaforico.Regional = (PckDR.SelectedIndex == -1) ? "" : PckDR.Items[PckDR.SelectedIndex];
            _cadastroSemaforico.Sentido = (PckSentido.SelectedIndex == -1) ? "" : PckSentido.Items[PckSentido.SelectedIndex];
            _cadastroSemaforico.LadoPista = (PckLadoDaPista.SelectedIndex == -1) ? "" : PckLadoDaPista.Items[PckLadoDaPista.SelectedIndex];
            _cadastroSemaforico.AtendimentoNorma = (PckAtendimentoNorma.SelectedIndex == -1) ? "" : PckAtendimentoNorma.Items[PckAtendimentoNorma.SelectedIndex];
            _cadastroSemaforico.ObservacaoAN = EntryObsAN.Text;
            _cadastroSemaforico.KM = EntryKM.Text;
            _cadastroSemaforico.Destinacao = (PckDestinacao.SelectedIndex == -1) ? "" : PckDestinacao.Items[PckDestinacao.SelectedIndex];
            _cadastroSemaforico.TipoSinalizacao = (PckTipoSinalizacao.SelectedIndex == -1) ? "" : PckTipoSinalizacao.Items[PckTipoSinalizacao.SelectedIndex];
            _cadastroSemaforico.FormaFoco = (PckForma.SelectedIndex == -1) ? "" : PckForma.Items[PckForma.SelectedIndex];
            _cadastroSemaforico.IndicacaoLuminosa = (PckIndicacaoLuminosa.SelectedIndex == -1) ? "" : PckIndicacaoLuminosa.Items[PckIndicacaoLuminosa.SelectedIndex];
            _cadastroSemaforico.SequenciaLuminosa = (PckSequencia.SelectedIndex == -1) ? "" : PckSequencia.Items[PckSequencia.SelectedIndex];
            _cadastroSemaforico.EstadoConservacao = (PckEstadoConservacao.SelectedIndex == -1) ? "" : PckEstadoConservacao.Items[PckEstadoConservacao.SelectedIndex];
            _cadastroSemaforico.ObservacaoEC = EntryObsEC.Text;
            _cadastroSemaforico.Observacao = EntryObs.Text;
            _cadastroSemaforico.Latitude = EntryLatitude.Text;
            _cadastroSemaforico.Longitude = EntryLongitude.Text;
            _cadastroSemaforico.FotoPanoramica = _nomeFotoPanoramica;
            _cadastroSemaforico.FotoDetalhe1 = _nomeFotoDetalhe1;
            _cadastroSemaforico.FotoDetalhe2 = _nomeFotoDetalhe2;
            _cadastroSemaforico.CodigoElemento = GetCode_Register();
            _cadastroSemaforico.DataCadastro = _dateRegisterBegin;
            _cadastroSemaforico.StatusInterno = "NOVO";

            //Salvando Informações da tela de Login
            _cadastroSemaforico.IdDispositivo = _lista.FirstOrDefault().IdDispositivo;
            _cadastroSemaforico.NomeUsuario = _lista.FirstOrDefault().NomeUsuario;
            _cadastroSemaforico.Auditoria = Int32.Parse(EntryAuditoria.Text);

            //TODO - Salvar a Tarefa no Banco
            if (update)
            {
                await new CadastroSQLiteDB().AtualizarAsync(_cadastroSemaforico);
            }
            else
            {
                //Salvando Rodovia e Regional na classe estatica
                VariaveisEstaticas.Regional = _cadastroSemaforico.Regional;
                VariaveisEstaticas.Rodovia = _cadastroSemaforico.Rodovia;

                await new CadastroSQLiteDB().CadastrarAsync(_cadastroSemaforico);
            }
            //TODO - MessagingCenter Retornar a Tarefa para a tela de listagem.
            await DisplayAlert("Dados Salvos", "As informações foram salvas com sucesso", "OK");
        }
        catch(Exception ex)
        {
            await DisplayAlert("Aviso!", "Erro " + ex.Message, "FECHAR");
        }
        finally
        {
            await Navigation.PopAsync();
        }    
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
            await DisplayAlert("Aviso!", "Erro " + ex.Message, "FECHAR");
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
        FileResult photo;
        if (!result && page.opcao)
        {
            photo = await MediaPicker.Default.CapturePhotoAsync();
        }
        else
        {
            photo = await MediaPicker.Default.PickPhotoAsync();
        }

        NamePhoto_And_SaveDirectory(photo, ultimoDigito);
        page.Close();      
    }

    private async void NamePhoto_And_SaveDirectory(FileResult photo, int ultimoDigito)
    {
        try
        {
            string nomeFoto = "Semaforica_DR" + GetCode_Register() + "_" + _dateRegisterBegin;

            if (photo != null)
            {
                nomeFoto += "_" + ultimoDigito + ".jpg";
                photo.FileName = nomeFoto;

                string photosDir = Constantes.CaminhoDiretorioSave;

                string localFilePath = Path.Combine(photosDir, photo.FileName);

                if (!Directory.Exists(photosDir))
                {
                    Directory.CreateDirectory(photosDir);
                }
                
                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);

                await sourceStream.CopyToAsync(localFileStream);

                Button_Take_PhotoOK(ultimoDigito, nomeFoto);
                return;
            }
        }
        catch(Exception ex) 
        {
            await DisplayAlert("Aviso!", "Erro " + ex.Message, "FECHAR");
        }
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
            "_" + PckSentido.Items[PckSentido.SelectedIndex].Substring(0, 3) + "_" + PckLadoDaPista.Items[PckLadoDaPista.SelectedIndex].Substring(0, 1);
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