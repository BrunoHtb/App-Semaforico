using cadastroSemaforico.Database;
using cadastroSemaforico.Models;

namespace cadastroSemaforico.Views;

public partial class Login : ContentPage
{
    private DadoLogin _dadoLogin;
    private List<DadoLogin> _lista;

    public Login()
    {
        InitializeComponent();

        VerifyDB();
    }

    private async void VerifyDB()
    {
        _lista = await new CadastroSQLiteDB().PesquisarLoginAsync();
        if (_lista.Count != 0)
        {
            _dadoLogin = _lista.FirstOrDefault();
            EntryID.Text = _dadoLogin.IdDispositivo.ToString();
            EntryUsuario.Text = _dadoLogin.NomeUsuario;
            EntryAuditoria.Text = _dadoLogin.Auditoria.ToString();
        }
        else
        {
            _dadoLogin = new DadoLogin();
        }
    }

    private async void OnButtonClicked_To_Menu(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(EntryID.Text) || string.IsNullOrEmpty(EntryUsuario.Text) || string.IsNullOrEmpty(EntryAuditoria.Text))
            {
                await DisplayAlert("ERRO!", "Por favor, preencha todos os campos.", "OK");
                return;
            }

            CreateDirectory();

            if (_lista.Count == 0)
            {
                _dadoLogin.IdDispositivo = Int32.Parse(EntryID.Text);
                _dadoLogin.NomeUsuario = EntryUsuario.Text;
                _dadoLogin.Auditoria = Int32.Parse(EntryAuditoria.Text);
                await new CadastroSQLiteDB().CadastrarLoginAsync(_dadoLogin);
            }
            else if (
                (EntryID.Text != _dadoLogin.IdDispositivo.ToString()) ||
                (EntryUsuario.Text != _dadoLogin.NomeUsuario) ||
                (EntryAuditoria.Text != _dadoLogin.Auditoria.ToString()))
            {
                _dadoLogin.IdDispositivo = Int32.Parse(EntryID.Text);
                _dadoLogin.NomeUsuario = EntryUsuario.Text;
                _dadoLogin.Auditoria = Int32.Parse(EntryAuditoria.Text);
                await new CadastroSQLiteDB().AtualizarLoginAsync(_dadoLogin);
            }

            await Navigation.PushAsync(new Menu(), false);
        }
        catch(Exception ex)
        {
            await DisplayAlert("Erro", "Erro: " + ex, "OK");
        }
    }

    private  void CreateDirectory()
    {
        if (!Directory.Exists(Constantes.CaminhoDiretorioSave))
        {
            Directory.CreateDirectory(Constantes.CaminhoDiretorioSave);
        }
    }

    private void Entry_Changed(object sender, TextChangedEventArgs e)
    {
        {
            Entry entry = sender as Entry;
            #if ANDROID
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                //Access the CursorPosition via the Entry's x:Name 
                this.EntryUsuario.CursorPosition = entry.Text.Length + 1;
            }
            #endif
        }
    }
}