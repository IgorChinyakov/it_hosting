using It_hosting_2._0.Model.Tools;
using It_hosting_2._0.Models.DBModels;
using It_hosting_2._0.View;
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
    internal class PullRequestViewModel
    {
        private ObservableCollection<PRBranchesViewModel> _PRBranchesViewModels;
        private Repository _repository;
        private Window _window;

        public PullRequestViewModel(Window window, int repositoryId, int currentBranchId)
        {

            PRBranchesViewModels = new ObservableCollection<PRBranchesViewModel>();
            _window = window;

            using (ithostingContext db = new ithostingContext())
            {
                List<Branch> Branches = new List<Branch>();
                Branches = db.Branches.Where(x => x.RepositoryId == repositoryId && x.Id != currentBranchId).ToList();

                foreach (var item in Branches)
                {
                    PRBranchesViewModels.Add(new PRBranchesViewModel(item, _window, currentBranchId));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<PRBranchesViewModel> PRBranchesViewModels
        {
            get => _PRBranchesViewModels;
            set
            {
                _PRBranchesViewModels = value;
                OnPropertyChanged(nameof(PRBranchesViewModels));
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    internal class PRBranchesViewModel
    {
        private CommandTemplate _openingMergeViewCommand;
        private Branch _branch;
        private Window _window;
        private int _currentBranchId;

        public PRBranchesViewModel(Branch branch, Window window, int currentBranchId)
        {
            _branch = branch;
            _window = window;
            _currentBranchId = currentBranchId;
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

        public int CurrentBranchId
        {
            get => _currentBranchId;
            set
            {
                _currentBranchId = value;
                OnPropertyChanged(nameof(CurrentBranchId));
            }
        }

        public CommandTemplate OpeningMergeViewCommand
        {
            get
            {
                if (_openingMergeViewCommand == null)
                {
                    _openingMergeViewCommand = new CommandTemplate(obj =>
                    {
                        OpenMergeView();
                    });
                }

                return _openingMergeViewCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OpenMergeView()
        {
            MergeView mergeView = new MergeView();
            MergeViewModel mergeViewModel = new MergeViewModel(CurrentBranchId, Branch.Id, _window);

            _window.Hide();

            mergeView.DataContext = mergeViewModel;
            mergeView.ShowDialog();

            _window.ShowDialog();
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
