using It_hosting_2._0.Model.Tools;
using It_hosting_2._0.Models.DBModels;
using It_hosting_2._0.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace It_hosting_2._0.ViewModel
{
    internal class UserProfileViewModel
    {
        private CommandTemplate _openingRepositoriesCommand;
        private Window _window;
        private string _userName;
        private string _login;
        private string _imageSource;
        private User _user;

        public UserProfileViewModel(Window window, User user)
        {

            //пофиксить
            User = user;
            _window = window;
            UserName = user.Nickname;
            Login = user.Login;
            ImageSource = "avatarka-pustaya-vk_0.jpg";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #region Commands
        public CommandTemplate OpeningRepositoriesCommand
        {
            get
            {
                if (_openingRepositoriesCommand == null)
                {
                    _openingRepositoriesCommand = new CommandTemplate(obj =>
                    {
                        OpenRepositories();
                    });
                }

                return _openingRepositoriesCommand;
            }
        }
        #endregion

        public User User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        public string ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        private void OpenRepositories()
        {
            RepositoriesView repositoriesView = new RepositoriesView(); 
            RepositoriesViewModel repositoriesViewModel = new RepositoriesViewModel(repositoriesView, User);

            _window.Hide();

            repositoriesView.DataContext = repositoriesViewModel;
            repositoriesView.ShowDialog();

            _window.ShowDialog();
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
