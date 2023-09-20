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
		Login = new AsyncRelayCommand(GoToSecondView);
	}
	public string? Title { get; }
	public ICommand Login { get; }

	private async Task GoToSecondView()
	{
		//test-ok await _navigator.NavigateRouteAsync(this, "Second");
		await _navigator.NavigateRouteAsync(this, "UnoTest");
		//org await _navigator.NavigateViewModelAsync<SecondViewModel>(this, data: new Entity(Name!));
	}

}
