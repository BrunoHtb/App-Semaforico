using cadastroSemaforico.Database;
using cadastroSemaforico.Models;
using System;
using System.Collections.ObjectModel;
using The49.Maui.BottomSheet;


namespace cadastroSemaforico.Views;

public partial class ListaCadastroTelaInferior
{
    CadastroSemaforico _selectedItem;
    ListaCadastro _listaCadastro;
    private int TotalClickNavegarFotos = 1;
    public VisualElement Divider => BoxviewDivisor; 

    public ListaCadastroTelaInferior(CadastroSemaforico selectedItem, ListaCadastro listaCadastro)
	{
		InitializeComponent();

        _selectedItem = selectedItem;
        _listaCadastro = listaCadastro;
        FillField();
    }
    void Resize()
    {
        BoxviewDivisor.HeightRequest = 34;
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
        GetPhoto();
    }

    public async void OnButtonClicked_To_CadastroEditar(object sender, EventArgs e)
    {
        await this.Dismiss();
        _listaCadastro.OnButtonClicked_To_CadastroEditar();
    }

    public void GetPhoto()
    {
        LabelNomeFoto.Text = GetNamePhoto(1);
        ImageFoto.Source = ImageSource.FromFile(GetFullPathPhoto(1));
    }

    public string GetFullPathPhoto(int itemFoto)
    {
        if(itemFoto == 1)
        {
            return (_selectedItem.FotoPanoramica == "") ? "galeria.png" : Path.Combine(Constantes.CaminhoDiretorioSave, _selectedItem.FotoPanoramica);
        }
        else if(itemFoto == 2) 
        {
            return (_selectedItem.FotoDetalhe1 == "") ? "galeria.png" : Path.Combine(Constantes.CaminhoDiretorioSave, _selectedItem.FotoDetalhe1);
        }
        else
        {
            return (_selectedItem.FotoDetalhe2 == "") ? "galeria.png" : Path.Combine(Constantes.CaminhoDiretorioSave, _selectedItem.FotoDetalhe2);
        }      
    }
    public string GetNamePhoto(int itemFoto)
    {
        if(itemFoto == 1)
        {
            return "Foto Panorâmica";
        }
        else if (itemFoto == 2)
        {
            return "Foto Detalhe 1";
        }
        else
        {
            return "Foto Detalhe 2";
        }
    }

    private void Go_To_NextPhoto(object sender, EventArgs e)
    {
        TotalClickNavegarFotos += 1;
        switch(TotalClickNavegarFotos)
        {
            case 1:
                LabelNomeFoto.Text = GetNamePhoto(TotalClickNavegarFotos);
                ImageFoto.Source = ImageSource.FromFile(GetFullPathPhoto(TotalClickNavegarFotos));
                break;
            case 2:
                LabelNomeFoto.Text = GetNamePhoto(TotalClickNavegarFotos);
                ImageFoto.Source = ImageSource.FromFile(GetFullPathPhoto(TotalClickNavegarFotos));
                break;
            case 3:
                LabelNomeFoto.Text = GetNamePhoto(TotalClickNavegarFotos);
                ImageFoto.Source = ImageSource.FromFile(GetFullPathPhoto(TotalClickNavegarFotos));
                TotalClickNavegarFotos = 0;
                break;
        }
    }
}
