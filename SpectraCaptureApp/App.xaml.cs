using NIR4.ViaviCapture.Model;
using ReactiveUI;
using Serilog;
using SpectraCaptureApp.Model;
using SpectraCaptureApp.View;
using SpectraCaptureApp.ViewModel;
using Splat;
using Splat.Serilog;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace SpectraCaptureApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            InitialiseConfiguration();
            RegisterDependencies();
            Log.Debug("Initialising main window");
            InitialiseMainWindow();
            base.OnStartup(e);
        }

        private void InitialiseConfiguration()
        {
            var settingsManager = new SettingsManager<UserSettings>("MySettings.json");
            Locator.CurrentMutable.RegisterConstant<SettingsManager<UserSettings>>(settingsManager);
        }

        private void InitialiseMainWindow()
        {
            var window = new MainWindow();
            window.SizeToContent = SizeToContent.WidthAndHeight;
            window.Show();
        }

        private void RegisterDependencies()
        {
            RegisterLoggers();
            RegisterViews();
            RegisterMisc();
        }

        private void RegisterLoggers()
        {
            //Locator.CurrentMutable.UseSerilogFullLogger();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RollingFile(@"Logs\SpectraCaptureAppLogs.log")
                .CreateLogger();
        }

        private void RegisterViews()
        {
            //TODO faster to do manually when I get round to doing it...
            //Locator.CurrentMutable.Register(() => new HomeView(), typeof(IViewFor<HomeViewModel>));
            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
        }

        private void RegisterMisc()
        {
            Locator.CurrentMutable.Register(() => new MainWindowViewModel(), typeof(IScreen));
            Locator.CurrentMutable.Register(() => new MyWrappedViaviScanningWorkflow(AppSettings.SpectrumSaveDirectory), typeof(IScanningWorkflow));

//#if DEBUG
//            Locator.CurrentMutable.Register(() => new MockScanningWorkflow(AppSettings.SpectrumSaveDirectory), typeof(IScanningWorkflow));
//#else
//            Locator.CurrentMutable.Register(() => new MyWrappedViaviScanningWorkflow(AppSettings.SpectrumSaveDirectory), typeof(IScanningWorkflow));                                
//#endif

        }
    }
}
