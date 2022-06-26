using AppEvaluator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvaluator.ViewModels
{
    internal class SubjectViewModel: ViewModelBase
    {
        private readonly Subject _subject;

        public string Code => _subject.SubjectCode;
        public string Name => _subject.SubjectName;
        public string FolderLocation => _subject.FolderLocation;

        public SubjectViewModel(Subject subject)
        {
            _subject = subject;
        }
    }
}
