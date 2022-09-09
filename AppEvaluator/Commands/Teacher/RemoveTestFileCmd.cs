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

        public override void Execute(object parameter)
        {
            if (parameter != null)
            {
                _manageTestsViewModel.TestFiles.Remove(parameter as FileStructure);
            }
        }
    }
}
