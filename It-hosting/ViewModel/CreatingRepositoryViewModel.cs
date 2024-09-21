using It_hosting_2._0.Model.Tools;
using It_hosting_2._0.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace It_hosting_2._0.ViewModel
{
    internal class CreatingRepositoryViewModel
    {
        private CommandTemplate _creatingRepositoryCommand;
        private string _title;
        private string _description;
        private bool _isPrivate;
        private User _user;

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
        public CreatingRepositoryViewModel(User user)
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
        {
            User = user;
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
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public bool IsPrivate
        {
            get => _isPrivate;
            set
            {
                _isPrivate = value;
                OnPropertyChanged(nameof(IsPrivate));
            }
        }

        public CommandTemplate CreatingRepositoryCommand
        {
            get
            {
                if (_creatingRepositoryCommand == null && string.IsNullOrWhiteSpace(_title))
                {
                    _creatingRepositoryCommand = new CommandTemplate(obj =>
                    {
                        CreateRepository();
                    });
                }

#pragma warning disable CS8603 // Возможно, возврат ссылки, допускающей значение NULL.
                return _creatingRepositoryCommand;
#pragma warning restore CS8603 // Возможно, возврат ссылки, допускающей значение NULL.
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void CreateRepository()
        {
            using (ithostingContext db = new ithostingContext())
            {
                Repository repository = new Repository { UserId = _user.Id, Title = _title, Description = _description, IsPrivate = _isPrivate};
                db.Repositories.Add(repository);
                db.SaveChanges();
                Branch branch = new Branch { Title = "Main", RepositoryId = repository.Id, IsMain = true };
                db.Branches.Add(branch);
                db.SaveChanges();

                MessageBox.Show("Репозиторий создан.");
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
