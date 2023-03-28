using cadastroSemaforico.Models;
using System;
using The49.Maui.BottomSheet;


namespace cadastroSemaforico.Views;

public partial class ListaCadastroTelaInferior
{
    CadastroSemaforico _selectedItem;
    ListaCadastro _listaCadastro;
    public ListaCadastroTelaInferior(CadastroSemaforico selectedItem, ListaCadastro listaCadastro)
	{
		InitializeComponent();

        _selectedItem = selectedItem;
        _listaCadastro = listaCadastro;
        FillField();
    }

    private void FillField()
    {
        LabelRodovia.Text +=  _selectedItem.Rodovia;
        LabelKM.Text += _selectedItem.KM;

        LabelCodigoElemento.Text += _selectedItem.CodigoElemento;
        LabelSentido.Text += _selectedItem.Sentido;
        LabelLadoPista.Text += _selectedItem.LadoPista;
        LabelStatusInterno.Text += _selectedItem.StatusInterno;
        LabelAuditoria.Text += _selectedItem.Auditoria.ToString();
    }

    public async void OnButtonClicked_To_CadastroEditar(object sender, EventArgs e)
    {
        await this.Dismiss();
        _listaCadastro.OnButtonClicked_To_CadastroEditar();
    }
}
