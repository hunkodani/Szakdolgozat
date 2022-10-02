using AppEvaluator.ViewModels.Teacher;

namespace AppEvaluator.Commands.Teacher
{
    internal class RemoveTestFileCmd : CommandBase
    {
        private readonly ManageTestsViewModel _manageTestsViewModel;

        public RemoveTestFileCmd(ManageTestsViewModel manageTestsViewModel)
        {
            this._manageTestsViewModel = manageTestsViewModel;
        }

        /// <summary>
        /// Removes a testfile from the TestFiles on _manageTestsViewModel
        /// </summary>
        /// <param name="parameter">the file to remove</param>
        public override void Execute(object parameter)
        {
            if (parameter != null)
            {
                _manageTestsViewModel.TestFiles.Remove(parameter as FileStructure);
            }
        }
    }
}
