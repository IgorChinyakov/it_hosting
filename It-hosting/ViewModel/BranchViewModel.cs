using It_hosting_2._0.Model.Tools;
using It_hosting_2._0.Models.DBModels;
using It_hosting_2._0.View;
using Microsoft.Win32;
using my.utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using File = It_hosting_2._0.Models.DBModels.File;

namespace It_hosting_2._0.ViewModel
{
    internal class BranchViewModel
    {
        private Branch _branch;
        private CommandTemplate _uploadingFileCommand;
        private CommandTemplate _pullRequestOpeningCommand;
        private ObservableCollection<FilesViewModel> _filesViewModels;
        private Window _window;

        public BranchViewModel(Window window, Branch branch)
        {
            Branch = branch;
            FilesViewModels = new ObservableCollection<FilesViewModel>();
            _window = window;

            using (ithostingContext db = new ithostingContext())
            {
                List<File> files = new List<File>();
                files = db.Files.Where(x => x.BranchId == Branch.Id).ToList();

                foreach (var item in files)
                {
                    FilesViewModels.Add(new FilesViewModel(item, _window));
                }
            }
        }

        public Branch Branch
        {
            get => _branch;
            set
            {
                _branch = value;
                OnPropertyChanged(nameof(Branch));
            }
        }

        public CommandTemplate UploadingFileCommand
        {
            get
            {
                if (_uploadingFileCommand == null)
                {
                    _uploadingFileCommand = new CommandTemplate(obj =>
                    {
                        UploadFile();
                    });
                }

                return _uploadingFileCommand;
            }
        }

        public ObservableCollection<FilesViewModel> FilesViewModels
        {
            get => _filesViewModels;
            set
            {
                _filesViewModels = value;
                OnPropertyChanged(nameof(FilesViewModels));
            }
        }

        public CommandTemplate PullRequestOpeningCommand 
        { 
            get
            {
                if (_pullRequestOpeningCommand == null)
                {
                    _pullRequestOpeningCommand = new CommandTemplate(obj =>
                    {
                        OpenPullRequest();
                    });
                }

                return _pullRequestOpeningCommand;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OpenPullRequest()
        {
            PullRequestView pullRequestView = new PullRequestView();
            PullRequestViewModel pullRequestViewModel = new PullRequestViewModel(pullRequestView, Branch.RepositoryId, Branch.Id);

            _window.Hide();

            pullRequestView.DataContext = pullRequestViewModel;
            pullRequestView.ShowDialog();

            _window.ShowDialog();
        }

        public void UploadFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.txt; *.cs)| *.txt; *.cs|All files (*.*)|*.*";
            // Открываем окно диалога с пользователем.
            if (openFileDialog.ShowDialog() == true)
            {
                using (ithostingContext db = new ithostingContext())
                {
                    string path = openFileDialog.FileName;
                    string title = openFileDialog.FileName.Split(@"\").Last();
                    StreamReader sr = new StreamReader(openFileDialog.FileName);
                    string currentFile = sr.ReadToEnd();
                    List<File> uploadedFiles = db.Files.Where(x => x.BranchId == Branch.Id && x.Title == title).ToList();

                    if (uploadedFiles.Count != 0)
                    {
                        File file = db.Files.Where(x => x.BranchId == Branch.Id && x.Title == title).First();
                        //100-не работает
                        db.Commits.Add(new Commit { Text = MakeCommit(file.Text, currentFile), FileId = file.Id, CreatingDate = DateTime.Now });
                        db.Files.Where(x => x.BranchId == Branch.Id && x.Title == title).First().Text = currentFile;
                        db.SaveChanges();
                        return;
                    }

                    db.Files.Add(new File { BranchId = Branch.Id, Text = currentFile, Title = openFileDialog.FileName.Split(@"\").Last() });
                    db.SaveChanges();
                }
            }
        }

        public string MakeCommit(string firstFile, string secondFile)
        {
            string a_line = firstFile;
            string b_line = secondFile;
            string finalString = "";

            int[] a_codes = Diff.DiffCharCodes(a_line, true);
            int[] b_codes = Diff.DiffCharCodes(b_line, true);
            Diff diff = new Diff();
            Diff.Item[] diffs = diff.DiffInt(a_codes, b_codes);

            int pos = 0;
            for (int n = 0; n < diffs.Length; n++)
            {
                Diff.Item it = diffs[n];

                // write unchanged chars
                while ((pos < it.StartB) && (pos < b_line.Length))
                {
                    finalString += b_line[pos].ToString();
                    pos++;
                } // while

                // write deleted chars
                if (it.deletedA > 0)
                {
                    finalString += "<span class='-'>";
                    for (int m = 0; m < it.deletedA; m++)
                    {
                        finalString += a_line[it.StartA + m].ToString();
                    } // for
                    finalString += "</span>";
                }

                // write inserted chars
                if (pos < it.StartB + it.insertedB)
                {
                    finalString += "<span class='+'>";
                    while (pos < it.StartB + it.insertedB)
                    {
                        finalString += b_line[pos].ToString();
                        pos++;
                    } // while
                    finalString += "</span>";
                } // if
            } // while

            // write rest of unchanged chars
            while (pos < b_line.Length)
            {
                finalString += b_line[pos].ToString();
                pos++;
            } // while

            return finalString;
        }

        //private bool HasRegisteredFileExstension(string fileExstension)
        //{
        //    RegistryKey rkRoot = Registry.ClassesRoot;
        //    RegistryKey rkFileType = rkRoot.OpenSubKey(fileExstension);

        //    return rkFileType != null;
        //}

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    internal class FilesViewModel
    {
        private File _file;
        private string _fileTtile;
        private CommandTemplate _openingFile;
        private Window _window;

        public FilesViewModel(File file, Window window)
        {
            _file = file;
            FileTitle = file.Title;
            _window = window;
        }

        public File File
        {
            get => _file;
            set
            {
                _file = value;
                OnPropertyChanged(nameof(File));
            }
        }

        public string FileTitle
        {
            get => _fileTtile;
            set
            {
                _fileTtile = value;
                OnPropertyChanged(nameof(FileTitle));
            }
        }

        public CommandTemplate OpeningFile
        {
            get
            {
                if (_openingFile == null)
                {
                    _openingFile = new CommandTemplate(obj =>
                    {
                        OpenFile();
                    });
                }

                return _openingFile;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OpenFile()
        {
            FileView fileView = new FileView();
            FileViewModel fileViewModel = new FileViewModel(File.Title, File.Text, File.Id, fileView);

            _window.Hide();

            fileView.DataContext = fileViewModel;
            fileView.ShowDialog();

            _window.ShowDialog();
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
