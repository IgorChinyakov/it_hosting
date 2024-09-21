using It_hosting_2._0.Model.Tools;
using It_hosting_2._0.Models.DBModels;
using It_hosting_2._0.View;
using Microsoft.EntityFrameworkCore;
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
    internal class RepositoryViewModel
    {
        private CommandTemplate _creatingBranch;
        private string _branchTitle;
        private Repository _repository;
        private Window _window;
        private ObservableCollection<Branch> _branches;
        private ObservableCollection<BranchesViewModel> _branchesViews;

        public RepositoryViewModel(Repository repository, Window window)
        {
            _repository = repository;
            _window = window;
            BranchesViews = new ObservableCollection<BranchesViewModel>();

            using (ithostingContext db = new ithostingContext())
            {
                _branches = new ObservableCollection<Branch>();
                foreach (var item in db.Branches.Where(x => x.RepositoryId == Repository.Id))
                {
                    _branches.Add(item);
                }
            }

            foreach (var item in _branches)
            {
                BranchesViews.Add(new BranchesViewModel(item, window));
            }
        }

        public ObservableCollection<BranchesViewModel> BranchesViews
        {
            get => _branchesViews;
            set
            {
                _branchesViews = value;
                OnPropertyChanged(nameof(BranchesViews));
            }
        }

        public string BranchTitle
        {
            get => _branchTitle;
            set
            {
                _branchTitle = value;
                OnPropertyChanged(nameof(BranchTitle));
            }
        }

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
        public CommandTemplate CreatingBranch
        {
            get
            {
                if (_creatingBranch == null)
                {
                    _creatingBranch = new CommandTemplate(obj =>
                    {
                        CreateBranch();
                    });
                }

                return _creatingBranch;
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public void CreateBranch()
        {
            if (string.IsNullOrWhiteSpace(BranchTitle) == false)
            {
                using (ithostingContext db = new ithostingContext())
                {
                    List<File> mainFiles = new List<File>();
                    mainFiles = db.Files.Where(x => x.BranchId == db.Branches.Where(x => x.IsMain == true && x.RepositoryId == Repository.Id).First().Id).ToList();
                    Branch branch = new Branch { Title = BranchTitle, RepositoryId = Repository.Id, IsMain = false };
                    db.Branches.Add(branch);
                    db.SaveChanges();

                    foreach (File file in mainFiles)
                    {
                        File currentFile = new File { Text = file.Text, Title = file.Title, BranchId = branch.Id } ;
                        db.Files.Add(currentFile);
                    }

                    db.SaveChanges();
                }

                MessageBox.Show("Ветка добавлена");
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    internal class BranchesViewModel
    {
        private CommandTemplate _openingBranchView;
        private Branch _branch;
        private Window _window;

        public BranchesViewModel(Branch branch, Window window)
        {
            _branch = branch;
            _window = window;
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

        public CommandTemplate OpeningBranchView 
        { 
            get 
            {
                if (_openingBranchView == null)
                {
                    _openingBranchView = new CommandTemplate(obj =>
                    {
                        OpenBranchView();
                    });
                }

                return _openingBranchView;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OpenBranchView()
        {
            BranchView branchView = new BranchView();
            BranchViewModel branchViewModel = new BranchViewModel(branchView, Branch);

            _window.Hide();

            branchView.DataContext = branchViewModel;
            branchView.ShowDialog();

            _window.ShowDialog();
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

