using MauiEmbedding.MauiControls;
using TreehouseInterface;
using Uno.Extensions.Maui.Platform;
using UnoApp.Loader;
using UnoTestModule;

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

	private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
	{
		views.Register(
			new ViewMap(ViewModel: typeof(ShellViewModel)),
			new ViewMap<MainModule, MainViewModel>(),
			new ViewMap<UnoTestPage, UnoTestViewModel>()
		);

		//var dllPath = pickedFile.Path;
		//test var dllPath = @"C:\DD\@Spike\Uno\Yuan\UnoTestModule\UnoTestModule\bin\Debug\net7.0-windows10.0.19041\UnoTestModule.dll";
		//test if (File.Exists(dllPath))
		//test {
		//test     var loadContext = new ModuleLoadContext(dllPath);
		//test     var assembly = loadContext.LoadFromAssemblyPath(dllPath);
		//test     var moduleType = assembly.DefinedTypes.First(i => i.FullName.Contains("UnoTestModule.UnoTestModule"));
		//test     var instance = assembly.CreateInstance(moduleType.FullName) as ITreehouseModule;
		//test     instance.OnInitialized(views, routes);
		//test }

		routes.Register(
			new RouteMap("", View: views.FindByViewModel<ShellViewModel>(),
				Nested: new RouteMap[]
				{
					new RouteMap("Main", View: views.FindByViewModel<MainViewModel>()),
					new RouteMap("UnoTest", View: views.FindByViewModel<UnoTestViewModel>())
				}
			));


		//ok var dllPath = @"C:\DD\@Spike\Uno\dd-uno.extensions\samples\UnoTestModule\UnoTestModule\bin\Debug\net7.0\UnoTestModule.dll";
		//ok if (File.Exists(dllPath))
		//ok {
		//ok 	var loadContext = new ModuleLoadContext(dllPath);
		//ok 	var assembly = loadContext.LoadFromAssemblyPath(dllPath);
		//ok 	var moduleType = assembly.DefinedTypes.First(i => i.FullName.Contains("UnoTestModule.UnoTestModule"));
		//ok 	var instance = assembly.CreateInstance(moduleType.FullName) as ITreehouseModule;
		//ok 	instance.OnInitialized(views, routes);
		//ok 
		//ok }
	}
}
