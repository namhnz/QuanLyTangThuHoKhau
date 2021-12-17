using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using log4net;

namespace QuanLyTangThuHoKhau
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            //Thay doi Logging level de log tat ca moi thu: https://stackoverflow.com/questions/8926409/log4net-hierarchy-and-logging-levels
            Log.Error(e.ExceptionObject);
            MessageBox.Show("Đã có lỗi xảy ra");
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            Log.Info("Phan mem quan ly tang thu ho khau khoi chay");
            base.OnStartup(e);

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

    }
}
