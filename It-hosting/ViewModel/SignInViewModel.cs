using It_hosting_2._0.Model;
using It_hosting_2._0.Model.Tools;
using It_hosting_2._0.Models.DBModels;
using It_hosting_2._0.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace It_hosting_2._0.ViewModel
{
    internal class SignInViewModel
    {
        private CommandTemplate _signingInCommand;
        private CommandTemplate _signingUpCommand;
        private string _login;
        private SecureString _securePassword;
        private User _user;
        private Window _window;
        
        public SignInViewModel(Window window)
        {
            _window = window;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        public SecureString SecurePassword
        {
            get => _securePassword;
            set
            {
                _securePassword = value;
                OnPropertyChanged(nameof(SecurePassword));
            }
        }

        public User User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        #region commands
        public CommandTemplate SigningUpCommand
        {
            get
            {
                if (_signingUpCommand == null)
                {
                    _signingUpCommand = new CommandTemplate(obj =>
                    {
                        OpenSignUpView();
                    });
                }

                return _signingUpCommand;
            }
        }

        public CommandTemplate SigningInCommand
        {
            get
            {
                if (_signingInCommand == null)
                {
                    _signingInCommand = new CommandTemplate(obj =>
                    {
                        User = SignIn(User);
                        OpenUserProfileView(User);
                    });
                }

                return _signingInCommand;
            }
        }
        #endregion

        private void OpenUserProfileView(User user)
        {
            if (user != null)
            {
                UserProfileView userProfileView = new UserProfileView();
                UserProfileViewModel userProfileViewModel = new UserProfileViewModel(userProfileView, user);

                _window.Hide();

                userProfileView.DataContext = userProfileViewModel;
                userProfileView.ShowDialog();

                _window.ShowDialog();
            }
            else
            {
                MessageBox.Show("не");
            }
        }

        public User SignIn(User user)
        {
            using (ithostingContext db = new ithostingContext())
            {
                user = null;
                List<User> users = db.Users.ToList();

                if (Login != null && SecurePassword != null)
                {
                    foreach (User item in users)
                    {
                        string theString = new NetworkCredential("", SecurePassword).Password;
                        if (item.Login == Login && item.Password == theString)
                        {
                            user = item;
                            return user;
                        }
                    }
                }

                return null;
            }
        }

        public void OpenSignUpView()
        {
            SignUpView signUpWindow = new SignUpView();
            SignUpViewModel signUpViewModel = new SignUpViewModel();

            _window.Hide();

            signUpWindow.DataContext = signUpViewModel;
            signUpWindow.ShowDialog();

            _window.ShowDialog();
        }

        public void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
