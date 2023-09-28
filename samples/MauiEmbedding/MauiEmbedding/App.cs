using MauiEmbedding.MauiControls;
using TreehouseInterface;
using UnoApp.Loader;
using UnoStaticLoadModule;

namespace MauiEmbedding;

public class App : EmbeddingApplication
{
	protected Window? MainWindow { get; private set; }
	protected IHost? Host { get; private set; }

	protected async override void OnLaunched(LaunchActivatedEventArgs args)
	{
		var builder = this.CreateBuilder(args)
			// Add navigation support for toolkit controls such as TabBar and NavigationView
			.UseToolkitNavigation()
			.UseMauiEmbedding<MauiControls.App>(maui => maui.UseCustomLibrary())
			.Configure(host => host
#if DEBUG
				// Switch to Development environment when running in DEBUG
				.UseEnvironment(Environments.Development)
#endif
				.UseLogging(configure: (context, logBuilder) =>
				{
					// Configure log levels for different categories of logging
					logBuilder
						.SetMinimumLevel(
							context.HostingEnvironment.IsDevelopment() ?
								LogLevel.Information :
								LogLevel.Warning)

						// Default filters for core Uno Platform namespaces
						.CoreLogLevel(LogLevel.Warning);

					// Uno Platform namespace filter groups
					// Uncomment individual methods to see more detailed logging
					//// Generic Xaml events
					//logBuilder.XamlLogLevel(LogLevel.Debug);
					//// Layouter specific messages
					//logBuilder.XamlLayoutLogLevel(LogLevel.Debug);
					//// Storage messages
					//logBuilder.StorageLogLevel(LogLevel.Debug);
					//// Binding related messages
					//logBuilder.XamlBindingLogLevel(LogLevel.Debug);
					//// Binder memory references tracking
					//logBuilder.BinderMemoryReferenceLogLevel(LogLevel.Debug);
					//// RemoteControl and HotReload related
					//logBuilder.HotReloadCoreLogLevel(LogLevel.Information);
					//// Debug JS interop
					//logBuilder.WebAssemblyLogLevel(LogLevel.Debug);

				}, enableUnoLogging: true)
				.UseSerilog(consoleLoggingEnabled: true, fileLoggingEnabled: true)
				.UseConfiguration(configure: configBuilder =>
					configBuilder
						.EmbeddedSource<App>()
						.Section<AppConfig>()
				)
				// Enable localization (see appsettings.json for supported languages)
				.UseLocalization()
				.ConfigureServices((context, services) =>
				{
					// TODO: Register your services
					//services.AddSingleton<IMyService, MyService>();
				})
				.UseNavigation(RegisterRoutes)
			);
		MainWindow = builder.Window;

		Host = await builder.NavigateAsync<Shell>();

		//MainWindow.Content = new MainPage();
	}

	private void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
	{
		views.Register(
			new ViewMap(ViewModel: typeof(ShellViewModel)),
			new ViewMap<MainPage, MainViewModel>(),
			new ViewMap<UnoStaticLoadPage, UnoStaticLoadViewModel>() 
		);

		routes.Register(
			new RouteMap("", View: views.FindByViewModel<ShellViewModel>()),
			new RouteMap("Main", View: views.FindByViewModel<MainViewModel>()),
			new RouteMap("UnoStaticLoadModule", View: views.FindByViewModel<UnoStaticLoadViewModel>())
		);

		//Dynamic load mvvm dll -- PLEASE MODIFY THE DLL PATH FOR YOUR TESTING 
		var dllPath = @"C:\DD\@Spike\Treehouse\Uno.DD\dd.uno.extensions\samples\UnoTestModule\UnoTestModule\bin\Debug\net7.0-windows10.0.19041\UnoTestModule.dll";
		if (File.Exists(dllPath))
		{
			var loadContext = new ModuleLoadContext(dllPath);
			var assembly = loadContext.LoadFromAssemblyPath(dllPath);
			var moduleType = assembly.DefinedTypes.First(i => i.FullName.Contains("UnoTestModule.UnoTestModule"));
			var instance = assembly.CreateInstance(moduleType.FullName) as ITreehouseModule;
			instance.OnInitialized(views, routes);

			//Try to add xmlTypeInfo, but it still fail
			var originType = assembly.GetType("UnoTestModule.UnoTestModule_XamlTypeInfo.XamlMetaDataProvider");
			var provider = Activator.CreateInstance(originType!) as Microsoft.UI.Xaml.Markup.IXamlMetadataProvider;

			var appProviderProperty = Current.GetType().GetProperty("_AppProvider", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			var appProvider = appProviderProperty!.GetValue(this)!;
			var appProviderProviderProperty = appProvider.GetType().GetProperty("Provider", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			var appProviderProvider = appProviderProviderProperty!.GetValue(appProvider);
			var othersProperty = appProviderProvider!.GetType().GetProperty("OtherProviders", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			var otherProviders = othersProperty!.GetValue(appProviderProvider) as List<Microsoft.UI.Xaml.Markup.IXamlMetadataProvider>;

			var testProperty = otherProviders[0].GetType().GetProperty("Provider", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			var testProvider0 = testProperty!.GetValue(otherProviders[0]);
			var testProviderProperty = testProvider0.GetType().GetProperty("OtherProviders", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			var testOtherProviders = testProviderProperty!.GetValue(testProvider0) as List<Microsoft.UI.Xaml.Markup.IXamlMetadataProvider>;
			testOtherProviders.Add(provider);
		}
	}
}
