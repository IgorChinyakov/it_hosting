using It_hosting_2._0.Model.Tools;
using It_hosting_2._0.Models.DBModels;
using It_hosting_2._0.View;
using my.utils;
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
    internal class MergeViewModel
    {
        private CommandTemplate _mergeCommand;
        private Branch _firstBranch;
        private Branch _secondBranch;
        private Window _window;

        public MergeViewModel(int firstBranchId, int secondBranchId, Window window)
        {
            _window = window;
            using (ithostingContext db = new ithostingContext())
            {
                _firstBranch = db.Branches.Where(x => x.Id == firstBranchId).First();
                _secondBranch = db.Branches.Where(x => x.Id == secondBranchId).First();
            }
        }

        public Branch FirstBranch
        {
            get => _firstBranch;
            set
            {
                _firstBranch = value;
                OnPropertyChanged(nameof(SecondBranch));
            }
        }

        public Branch SecondBranch
        {
            get => _secondBranch;
            set
            {
                _secondBranch = value;
                OnPropertyChanged(nameof(SecondBranch));
            }
        }

        public CommandTemplate MergeCommand
        {
            get
            {
                if (_mergeCommand == null)
                {
                    _mergeCommand = new CommandTemplate(obj =>
                    {
                        Merge();
                    });
                }

                return _mergeCommand;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Merge()
        {
            using (ithostingContext db = new ithostingContext())
            {
                List<File> firstBranchFiles = db.Files.Where(x => x.BranchId == FirstBranch.Id).ToList();
                List<File> secondBranchFiles = db.Files.Where(x => x.BranchId == SecondBranch.Id).ToList();

                foreach (var item in firstBranchFiles)
                {
                    if (secondBranchFiles.Any(x => x.Title == item.Title))
                    {
                        File oldFile = secondBranchFiles.Where(x => x.Title == item.Title).First();
                        string newFileText = firstBranchFiles.Where(x => x.Title == oldFile.Title).First().Text;
                        db.Commits.Add(new Commit { Text = MakeCommit(oldFile.Text, newFileText), CreatingDate = DateTime.Now, FileId = oldFile.Id});
                        db.Files.Where(x => x.Id == oldFile.Id).First().Text = newFileText;
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Files.Add(new File { BranchId = SecondBranch.Id, Title = item.Title, Text = item.Text});
                        db.SaveChanges();
                    }
                }

                MessageBox.Show("Ветки слиты");
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
}
