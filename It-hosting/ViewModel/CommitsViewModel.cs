using It_hosting_2._0.Model.Tools;
using It_hosting_2._0.Models.DBModels;
using It_hosting_2._0.View;
using Microsoft.EntityFrameworkCore.Update.Internal;
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
    internal class CommitsViewModel
    {
        private ObservableCollection<CommitViewModel> _commitsViewModels;
        Window _window;

        public CommitsViewModel(int fileId, Window window)
        {
            CommitsViewModels = new ObservableCollection<CommitViewModel>();
            _window = window;
            using (ithostingContext db = new ithostingContext())
            {
                List<Commit> commits = db.Commits.Where(x => x.FileId == fileId).ToList();
                commits = commits.OrderBy(x => x.CreatingDate).ToList();

                foreach (Commit commit in commits)
                {
                    CommitsViewModels.Add(new CommitViewModel(_window, commit.Text, (DateTime)commit.CreatingDate));
                }
            }
        }

        public ObservableCollection<CommitViewModel> CommitsViewModels
        {
            get => _commitsViewModels;
            set
            {
                _commitsViewModels = value;
                OnPropertyChanged(nameof(CommitsViewModels));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    internal class CommitViewModel
    {
        private string _text;
        private DateTime _date;
        private CommandTemplate _openingCommitFileCommand;
        private Window _window;

        public CommitViewModel(Window window, string text, DateTime date)
        {
            Text = text;
            _date = date;
            _window = window;
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public CommandTemplate OpeningCommitFileCommand
        {
            get
            {
                if (_openingCommitFileCommand == null)
                {
                    _openingCommitFileCommand = new CommandTemplate(obj =>
                    {
                        OpenCommitFileView();
                    });
                }

                return _openingCommitFileCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OpenCommitFileView()
        {
            CommitFileView commitFileView = new CommitFileView();
            CommitFileViewModel commitFileViewModel = new CommitFileViewModel(Text);

            _window.Hide();

            commitFileView.DataContext = commitFileViewModel;
            commitFileView.ShowDialog();

            _window.ShowDialog();
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
