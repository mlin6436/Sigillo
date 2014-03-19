using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Sigillo.App.ViewModel
{
    public class RelativeAndFriendViewModel : ViewModelBase, IDataErrorInfo
    {
        #region Commands

        public RelayCommand AddNewRelativeAndFriendInfo { get; private set; }
        public RelayCommand RemoveRelativeAndFriendInfo { get; private set; }

        #endregion

        public RelativeAndFriendViewModel()
        {
            AddNewRelativeAndFriendInfo =
                new RelayCommand(() => Messenger.Default.Send(new NotificationMessage(this, "AddNewRelativeAndFriendInfo")));
            RemoveRelativeAndFriendInfo =
                new RelayCommand(() => Messenger.Default.Send(new NotificationMessage(this, "RemoveRelativeAndFriendInfo")));
        }

        public string this[string columnName]
        {
            get { return string.Empty; }
        }

        public string Error { get; private set; }
    }
}
