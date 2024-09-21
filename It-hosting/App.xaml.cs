using It_hosting_2._0.View;
using It_hosting_2._0.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace It_hosting_2._0
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            SignInView window = new SignInView();
            SignInViewModel viewModel = new SignInViewModel(window);

            window.DataContext = viewModel;
            window.Show();
        }
    }
}
