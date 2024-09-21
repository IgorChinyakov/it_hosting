using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Immutable;
using It_hosting_2._0.Model.Tools;
using It_hosting_2._0.View;
using Accessibility;
using System.Collections.ObjectModel;

namespace It_hosting_2._0.ViewModel
{
    internal class FileViewModel
    {
        private ObservableCollection<StringViewModel> _fileStringsViewModels;
        private CommandTemplate _commitsOpening;
        private string _fileTitle;
        private int _fileId;
        private Window _window;

        public FileViewModel(string fileTitle, string text, int fileId, Window window)
        {
            _fileTitle = fileTitle;
            _fileStringsViewModels = new ObservableCollection<StringViewModel>();
            _fileId = fileId;
            _window = window;

            List<string> strings = new List<string>();
            strings = text.Split("\n").ToList();

            for (int i = 0; i < strings.Count; i++)
            {
                _fileStringsViewModels.Add(new StringViewModel($"{i + 1}.  " + strings[i]));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public CommandTemplate CommitsOpening
        {
            get
            {
                if(_commitsOpening == null)
                {
                    _commitsOpening = new CommandTemplate(obj =>
                    {
                        OpenCommitsView();
                    });
                }

                return _commitsOpening;
            }
        }

        public ObservableCollection<StringViewModel> FileStringsViewModel
        {
            get => _fileStringsViewModels;
            set
            {
                _fileStringsViewModels = value;
                OnPropertyChanged(nameof(FileStringsViewModel));
            }
        }

        public string FileTitle
        {
            get => _fileTitle;
            set
            {
                _fileTitle = value;
                OnPropertyChanged(nameof(FileTitle));
            }
        }

        public int FileId
        {
            get => _fileId;
            set
            {
                _fileId = value;
                OnPropertyChanged(nameof(FileId));
            }
        }

        private void OpenCommitsView()
        {
            CommitsView commitsView = new CommitsView();
            CommitsViewModel commitsViewModel = new CommitsViewModel(FileId, commitsView);

            _window.Hide();
            
            commitsView.DataContext = commitsViewModel;
            commitsView.ShowDialog();

            _window.ShowDialog();
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    internal class StringViewModel
    {
        private string _fileString;

        public StringViewModel(string str)
        {
            _fileString = str;
        }

        public string FileString
        {
            get => _fileString;
            set
            {
                _fileString = value;
                OnPropertyChanged(nameof(FileString));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
