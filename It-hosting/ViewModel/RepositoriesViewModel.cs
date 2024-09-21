using It_hosting_2._0.Model.Tools;
using It_hosting_2._0.Models.DBModels;
using It_hosting_2._0.View;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    internal class RepositoriesViewModel
    {
        private CommandTemplate _openingCreatingRepositoryCommand;
        private Window _window;
        private User _user;
        private ObservableCollection<Repository> _repositories;
        private ObservableCollection<RepositoryStackPanelViewModel> _repositoriesView;

        public RepositoriesViewModel(Window window, User user)
        {
            _window = window;
            _user = user;
            RepositoriesView = new ObservableCollection<RepositoryStackPanelViewModel>();

            using (ithostingContext db = new ithostingContext())
            {
                _repositories = new ObservableCollection<Repository>();
                foreach (var item in db.Repositories.Where(x => x.UserId == User.Id).ToList())
                {
                    _repositories.Add(item);
                }
            }

            foreach (var item in _repositories)
            {
                RepositoriesView.Add(new RepositoryStackPanelViewModel(item, window));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public User User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        public ObservableCollection<RepositoryStackPanelViewModel> RepositoriesView
        {
            get => _repositoriesView;
            set
            {
                _repositoriesView = value;
                OnPropertyChanged(nameof(RepositoriesView));
            }
        }

        public ObservableCollection<Repository> Repositories
        {
            get => _repositories;
            set
            {
                _repositories = value;
                OnPropertyChanged(nameof(Repositories));
            }
        }

        #region Commands
        public CommandTemplate OpeningCreatingRepositoryCommand
        {
            get
            {
                if (_openingCreatingRepositoryCommand == null)
                {
                    _openingCreatingRepositoryCommand = new CommandTemplate(obj =>
                    {
                        OpenCreatingRepositoryWindow();
                    });
                }

                return _openingCreatingRepositoryCommand;
            }
        }
        #endregion

        private void OpenCreatingRepositoryWindow()
        {
            CreatingRepositoryView creatingRepositoryView = new CreatingRepositoryView();
            CreatingRepositoryViewModel creatingRepositoryViewModel = new CreatingRepositoryViewModel(User);

            _window.Hide();

            creatingRepositoryView.DataContext = creatingRepositoryViewModel;
            creatingRepositoryView.ShowDialog();

            _window.ShowDialog();
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    internal class RepositoryStackPanelViewModel
    {
        //private ICollection<Repository> _repositories;
        //private User _user;
        private Repository _repository;
        private CommandTemplate _openingRepositoryCommand;
        private Window _window;

        public RepositoryStackPanelViewModel(Repository repository, Window window)
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
        {
            Repository = repository;
            _window = window;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Repository Repository
        {
            get => _repository;
            set
            {
                _repository = value;
                OnPropertyChanged(nameof(Repository));
            }
        }

        #region Commands
        public CommandTemplate OpeningRepositoryViewCommand
        {
            get
            {
                if (_openingRepositoryCommand == null)
                {
                    _openingRepositoryCommand = new CommandTemplate(obj =>
                    {
                        OpenRepositoryWindow();
                    });
                }

                return _openingRepositoryCommand;
            }
        }
        #endregion

        private void OpenRepositoryWindow()
        {
            RepositoryView repositoryView = new RepositoryView();
            RepositoryViewModel repositoryViewModel = new RepositoryViewModel(Repository, repositoryView);

            _window.Hide();

            repositoryView.DataContext = repositoryViewModel;
            repositoryView.ShowDialog();

            _window.ShowDialog();
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
