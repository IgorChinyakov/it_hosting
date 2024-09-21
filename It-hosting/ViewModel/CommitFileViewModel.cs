using It_hosting_2._0.Model.Tools;
using It_hosting_2._0.View;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace It_hosting_2._0.ViewModel
{
    internal class CommitFileViewModel
    {
        private ObservableCollection<StringCommitFileViewModel> _commitFileStringsViewModels;
        private string _fileTitle;
        private int _fileId;

        public CommitFileViewModel(string text)
        {
            _commitFileStringsViewModels = new ObservableCollection<StringCommitFileViewModel>();

            List<string> strings = new List<string>();
            text = text.Replace("<span class='+'>\r\n", "\n<span class='+'>");
            text = text.Replace("<span class='-'>\r\n", "\n<span class='-'>");
            strings = text.Split("\n").ToList();

            List<bool> checkList = CheckIsStringChanged(strings[0]);
            bool isAddStringOver = checkList[2];
            bool isRemoveStringOver = checkList[1];
            bool isChanged = checkList[0];

            strings[0] = strings[0].Replace("<span class='-'>", "-〈");
            strings[0] = strings[0].Replace("<span class='+'>", "+〈");
            strings[0] = strings[0].Replace("</span>", "〉");

            if (isChanged)
            {
                _commitFileStringsViewModels.Add(new StringCommitFileViewModel($"{1}.  " + strings[0], true));
            }
            else
            {
                _commitFileStringsViewModels.Add(new StringCommitFileViewModel($"{1}.  " + strings[0], false));
            }

            for (int i = 1; i < strings.Count; i++)
            {
                checkList = CheckIsStringChanged(strings[i]);
                isChanged = checkList[0];

                strings[i] = strings[i].Replace("<span class='-'>", "-〈");
                strings[i] = strings[i].Replace("<span class='+'>", "+〈");
                strings[i] = strings[i].Replace("</span>", "〉");

                if (isChanged || !isRemoveStringOver || !isAddStringOver)
                {
                    _commitFileStringsViewModels.Add(new StringCommitFileViewModel($"{i + 1}.  " + strings[i], true));
                    if (!isRemoveStringOver || !isAddStringOver)
                    {
                        continue;
                    }
                }
                else
                {
                    _commitFileStringsViewModels.Add(new StringCommitFileViewModel($"{i + 1}.  " + strings[i], false));
                }
                isAddStringOver = checkList[2];
                isRemoveStringOver = checkList[1];
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<StringCommitFileViewModel> CommitFileStringsViewModels
        {
            get => _commitFileStringsViewModels;
            set
            {
                _commitFileStringsViewModels = value;
                OnPropertyChanged(nameof(CommitFileStringsViewModels));
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

        private List<bool> CheckIsStringChanged(string str)
        {
            bool isAddStringOver = false;
            bool isRemoveStringOver = false;
            bool isChanged = false;


            //for (int i = 0; i < strings.Count; i++)
            //{
            if (str.Contains("<span class='+'>") || str.Contains("<span class='-'>") || str.Contains("</span>"))
            {
                isChanged = true;
                if (str.Contains("<span class='+'>"))
                {
                    string temp = "";
                    for (int j = str.IndexOf("<span class='+'>"); j < str.Length; j++)
                    {
                        temp += str[j].ToString();
                        if (temp.Contains("</span>"))
                        {
                            isAddStringOver = true;
                            break;
                        }
                    }
                }

                if (str.Contains("<span class='-'>"))
                {
                    string temp = "";
                    for (int j = str.IndexOf("<span class='-'>"); j < str.Length; j++)
                    {
                        temp += str[j].ToString();
                        if (temp.Contains("</span>"))
                        {
                            isRemoveStringOver = true;
                            break;
                        }
                    }
                }
                //}
            }
            if (isChanged == false)
            {
                isAddStringOver = true;
                isRemoveStringOver = true;
            }

            return new List<bool> { isChanged, isRemoveStringOver, isAddStringOver };
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    internal class StringCommitFileViewModel
    {
        private string _fileString;
        private string _background;

        public StringCommitFileViewModel(string str, bool isChanged)
        {
            _fileString = str;
            if (isChanged)
            {
                Background = "Yellow";
            }
            else
            {
                Background = "DimGray";
            }
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

        public string Background
        {
            get => _background;
            set
            {
                _background = value;
                OnPropertyChanged(nameof(Background));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
