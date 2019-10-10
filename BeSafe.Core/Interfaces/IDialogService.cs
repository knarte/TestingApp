using System;
using System.Collections.Generic;
using System.Text;

namespace BeSafe.Core.Interfaces
{
    public interface IDialogService
    {
        void Alert(
            string message,
            string title,
            string okbtnText);

        void Confirm(
            string title,
            string message,
            string okButtonTitle,
            string dismissButtonTitle,
            Action confirmed,
            Action dismissed);

        void Alert(
            string message,
            string title,
            string okbtnText,
            string dismissButtonTitle,
            Action confirmed);

        //void Confirm(
        //    string title,
        //    string message,
        //    string okButtonTitle,
        //    string dismissButtonTitle,
        //    Action confirmed,
        //    Action dismissed);
        //}
    }
}
