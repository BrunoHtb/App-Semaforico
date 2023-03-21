using CommunityToolkit.Maui.Views;

namespace cadastroSemaforico.Views;


public partial class PopupFoto : Popup
{
    public bool opcao;
    TaskCompletionSource<bool> _tcs = null;
    public bool Opcao
    {
        get { return opcao; }   // get method
        set { opcao = value; }  // set method
    }

    public PopupFoto()
    {
        InitializeComponent();
    }

    private async void OnClick_To_GetGallery(object sender, EventArgs e)
    {
        opcao = false;
        _tcs?.SetResult(true);
    }

    private async void OnClick_To_GetCamera(object sender, EventArgs e)
    {
        opcao = true;
        _tcs?.SetResult(!opcao);

    }
    public async Task<bool> Show()
    {
        _tcs = new TaskCompletionSource<bool>();
        return await _tcs.Task;
    }
}