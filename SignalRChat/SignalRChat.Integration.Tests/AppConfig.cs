using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpecsFor.Mvc;

namespace SignalRChat.Integration.Tests
{
    [TestClass]
    public class AppConfig
    {
        private static SpecsForIntegrationHost _integrationHost;

        [AssemblyInitialize()]
        public static void MyAssemblyInitialize(TestContext testContext)
        {
            var config = new SpecsForMvcConfig();

            config.UseIISExpress()
                .With(Project.Named("SignalRChat"))
                .ApplyWebConfigTransformForConfig("Debug");

            config.BuildRoutesUsing(RouteConfig.RegisterRoutes);

            config.UseBrowser(BrowserDriver.Chrome);

            _integrationHost = new SpecsForIntegrationHost(config);
            _integrationHost.Start();
        }

        [AssemblyCleanup()]
        public static void MyAssemblyCleanup()
        {
            _integrationHost.Shutdown();
        }
    }
}
