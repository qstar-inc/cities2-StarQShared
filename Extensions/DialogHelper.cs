using System;
using System.Threading.Tasks;
using Game.SceneFlow;
using Game.UI;
using Game.UI.Localization;

namespace StarQ.Shared.Extensions
{
    public static class DialogHelper
    {
        public static void ShowMessageDialog(
            LocalizedString title,
            LocalizedString message,
            LocalizedString confirmAction,
            Action<int> action,
            bool log = true,
            LogLevel logLevel = LogLevel.Info
        )
        {
            GameManager.instance.userInterface.appBindings.ShowMessageDialog(
                new MessageDialog(title, message, confirmAction),
                action
            );
            if (log)
                LogHelper.SendLog(message.value, logLevel);
        }

        public static void ShowConfirmationDialog(
            LocalizedString title,
            LocalizedString message,
            LocalizedString confirmAction,
            LocalizedString cancelAction,
            LocalizedString[]? otherActions,
            Action<int> action
        )
        {
            GameManager.instance.userInterface.appBindings.ShowConfirmationDialog(
                new ConfirmationDialog(title, message, confirmAction, cancelAction, otherActions),
                action
            );
        }

        public static async Task<int> ShowConfirmationDialogAndWait(
            LocalizedString title,
            LocalizedString message,
            LocalizedString confirmAction,
            LocalizedString cancelAction,
            LocalizedString[]? otherActions
        )
        {
            return await GameManager.instance.userInterface.appBindings.ShowConfirmationDialogAndWait(
                new ConfirmationDialog(title, message, confirmAction, cancelAction, otherActions)
            );
        }
    }
}
