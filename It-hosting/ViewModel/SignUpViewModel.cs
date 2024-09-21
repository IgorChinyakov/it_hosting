using It_hosting_2._0.Model.Tools;
using It_hosting_2._0.Models.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace It_hosting_2._0.ViewModel
{
    internal class SignUpViewModel
    {
        private CommandTemplate _addUserCommand;
        private string _userName;
        private string _login;
        private SecureString _password;
        private SecureString _confirmPassword;

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
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

        public SecureString Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public SecureString ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }

        #region Commands
        public CommandTemplate AddUserCommand
        {
            get
            {
                if (_addUserCommand == null)
                {
                    _addUserCommand = new CommandTemplate(obj =>
                    {
                        AddUser();
                    });
                }

                return _addUserCommand;
            }
        }
        #endregion

        public void AddUser()
        {
            using (ithostingContext db = new ithostingContext())
            {
                string password = new NetworkCredential("", Password).Password;
                string confirmPassword = new NetworkCredential("", ConfirmPassword).Password;
                if (string.IsNullOrWhiteSpace(_login) == false && string.IsNullOrWhiteSpace(_userName) == false
                    && string.IsNullOrWhiteSpace(password) == false)
                {
                    if (password == confirmPassword)
                    {
                        User user = new User { Login = _login, Password = password, Nickname = _userName };
                        db.Users.Add(user);
                        db.SaveChanges();

                        MessageBox.Show("Пользователь добавлен успешно !");
                    }
                }
                else
                {
                    MessageBox.Show("Поля не должны быть пустыми !");
                }
            }
        }

        public void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
