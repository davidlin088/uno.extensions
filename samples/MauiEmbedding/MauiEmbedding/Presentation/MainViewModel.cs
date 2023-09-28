namespace MauiEmbedding.Presentation;

public partial class MainViewModel : ObservableObject
{
	private INavigator _navigator;

	[ObservableProperty]
	private string? name;

	public MainViewModel(
		IStringLocalizer localizer,
		IOptions<AppConfig> appInfo,
		INavigator navigator)
	{
		_navigator = navigator;
		Title = "Main";
		Title += $" - {localizer["ApplicationName"]}";
		Title += $" - {appInfo?.Value?.Environment}";
		DynamicLoad = new AsyncRelayCommand(GoToSecondView);
		StaticLoad = new AsyncRelayCommand(GoToStaticLoad);
	}
	public string? Title { get; }
	public ICommand DynamicLoad { get; }
	public ICommand StaticLoad { get; }
	private async Task GoToSecondView()
	{
		await _navigator.NavigateRouteAsync(this, "UnoTest");
	}

	private async Task GoToStaticLoad()
	{
		await _navigator.NavigateRouteAsync(this, "UnoStaticLoad");
	}

}
