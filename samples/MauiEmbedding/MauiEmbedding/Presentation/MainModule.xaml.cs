namespace MauiEmbedding.Presentation;

public sealed partial class MainModule : Page
{
	public MainViewModel? Vm => DataContext as MainViewModel;

	public MainModule()
	{
		this.InitializeComponent();

		DataContextChanged += MainPage_DataContextChanged;
	}

	private void MainPage_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
	{

	}

	private void OnVisualElementChanged(object sender, VisualElementChangedEventArgs args)
	{

	}
}
