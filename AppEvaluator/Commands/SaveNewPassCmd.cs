using AppEvaluator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEvaluator.Commands
{
    internal class SaveNewPassCmd : CommandBase
    {
        private readonly SettingsViewModel _settingsViewModel;

        public SaveNewPassCmd(SettingsViewModel settingsViewModel)
        {
            _settingsViewModel = settingsViewModel;
        }

        public override void Execute(object parameter)
        {

        }
    }
}
