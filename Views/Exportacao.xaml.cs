using cadastroSemaforico.Database;
using cadastroSemaforico.Models;
using SQLitePCL;

namespace cadastroSemaforico.Views;

public partial class Exportacao : ContentPage
{
    Task<List<CadastroSemaforico>> _listRegistros;

    SemaforicoPostgreSQLDB _cadastroPostgres;
    List<SemaforicoPostgreSQLDB> _listCadastrosPostgres;

    public Exportacao()
    {
        InitializeComponent();
        Count_Register();
    }

    private void Count_Register()
    {
        _listRegistros = new CadastroSQLiteDB().PesquisarAsync();
        LabelTotalRegistro.Text = _listRegistros.Result.Count.ToString();
    }

    private void OnButtonClicked_To_Menu(object sender, EventArgs e)
    {
        _listRegistros.Dispose();
        Navigation.PopAsync();
    }

    private async void OnButtonClicked_To_Export(object sender, EventArgs e)
    {
        int contador = 0;

        _listCadastrosPostgres = new List<SemaforicoPostgreSQLDB>();

        foreach (CadastroSemaforico cadastro in _listRegistros.Result)
        {
            _cadastroPostgres = new SemaforicoPostgreSQLDB();

            _cadastroPostgres.regional_der = "DR" + cadastro.Regional;
            _cadastroPostgres.rodovia = cadastro.Rodovia;
            _cadastroPostgres.codigo = cadastro.CodigoElemento;
            _cadastroPostgres.data_levantamento = cadastro.DataCadastro;
            _cadastroPostgres.km = cadastro.KM;
            _cadastroPostgres.lado_pista = cadastro.LadoPista;
            _cadastroPostgres.sentido = cadastro.Sentido;
            _cadastroPostgres.tipo_sinalização = cadastro.TipoSinalizacao;
            _cadastroPostgres.destinacao = cadastro.Destinacao;
            _cadastroPostgres.numero_indicacoes_luminosas = cadastro.IndicacaoLuminosa;
            _cadastroPostgres.sequencia_luminosa = cadastro.SequenciaLuminosa;
            _cadastroPostgres.forma_foco = cadastro.FormaFoco;
            _cadastroPostgres.latitude = cadastro.Latitude;
            _cadastroPostgres.longitude = cadastro.Longitude;
            _cadastroPostgres.nome_fotopanoramica = cadastro.FotoPanoramica;
            _cadastroPostgres.nome_fotodetalhe1 = cadastro.FotoDetalhe1;
            _cadastroPostgres.nome_fotodetalhe2 = cadastro.FotoDetalhe2;
            _cadastroPostgres.condicao = cadastro.EstadoConservacao;
            _cadastroPostgres.observacao_ec = cadastro.ObservacaoEC;
            _cadastroPostgres.atendimento_norma = cadastro.AtendimentoNorma;
            _cadastroPostgres.obs_an = cadastro.ObservacaoAN;
            _cadastroPostgres.observacao = cadastro.Observacao;
            _cadastroPostgres.usuariologado = cadastro.NomeUsuario;
            _cadastroPostgres.status_interno = cadastro.StatusInterno;
            _cadastroPostgres.alteracao_dia = "";
            _cadastroPostgres.observacao_sistema = "";
            _cadastroPostgres.data_inclusao_novo = DateTime.Now.ToString("dd-MM-yyyy_HHmmss");
            _cadastroPostgres.pista = "";
            _cadastroPostgres.auditoria = cadastro.Auditoria;
            _cadastroPostgres.numero_dispositivo = cadastro.IdDispositivo.ToString();
            _cadastroPostgres.usuario_campo = cadastro.NomeUsuario;

            _listCadastrosPostgres.Add(_cadastroPostgres);
        }

        await new CadastroPostgresSQLDB().CadastrarAsync(_listCadastrosPostgres);

        foreach (CadastroSemaforico cadastro in _listRegistros.Result)
        {
            CreateBackup(cadastro);

            contador += 1; 
            LabelTotalRegistroEnviados.Text = contador.ToString();
            await new CadastroSQLiteDB().ExcluirAsync(cadastro.Id);      
        }

        await DisplayAlert("Aviso", "Banco de dados atualizado com SUCESSO", "OK");
    }

    private void CreateBackup(CadastroSemaforico cadastro)
    {
        string nomeArquivo;
        string caminhoArquivo;

        nomeArquivo = cadastro.CodigoElemento + ".txt";
        caminhoArquivo = Path.Combine(Constantes.CaminhoDiretorioSave, nomeArquivo);

        using (StreamWriter writer = new StreamWriter(caminhoArquivo))
        {
            writer.WriteLine(
                cadastro.Id + ";" +
                cadastro.Rodovia + ";" +
                cadastro.Regional + ";" +
                cadastro.Sentido + ";" +
                cadastro.LadoPista + ";" +
                cadastro.AtendimentoNorma + ";" +
                cadastro.ObservacaoAN + ";" +
                cadastro.KM + ";" +
                cadastro.Destinacao + ";" +
                cadastro.TipoSinalizacao + ";" +
                cadastro.FormaFoco + ";" +
                cadastro.IndicacaoLuminosa + ";" +
                cadastro.SequenciaLuminosa + ";" +
                cadastro.EstadoConservacao + ";" +
                cadastro.ObservacaoEC + ";" +
                cadastro.Observacao + ";" +
                cadastro.Latitude + ";" +
                cadastro.Longitude + ";" +
                cadastro.FotoPanoramica + ";" +
                cadastro.FotoDetalhe1 + ";" +
                cadastro.FotoDetalhe2 + ";" +
                cadastro.CodigoElemento + ";" +
                cadastro.StatusInterno + ";" +
                cadastro.IdDispositivo + ";" +
                cadastro.NomeUsuario + ";" +
                cadastro.Auditoria + ";" +
                cadastro.DataCadastro
                );
        }
    }

}