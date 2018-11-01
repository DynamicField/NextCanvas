using System;

namespace NextCanvas.Interactivity.Dialogs
{
    public interface IUserRequestInteraction : IUserInteraction<DialogResultEventArgs>
    {
        string Title { get; set; }
        string Content { get; set; }
    }

    public class DialogResultEventArgs : EventArgs
    {
        public string ChosenButtonText { get; }

        public DialogResultEventArgs(string chosen)
        {
            ChosenButtonText = chosen;
        }
    }
}