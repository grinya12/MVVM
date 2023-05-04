using MVVM.Models;
using MVVM.ViewModels;
using Plugin.Messaging;
using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;


namespace MVVM.ViewModels
{
    public class FriendViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        FriendsListViewModel lvm;

        public Friend Friend { get; private set; }

        public ICommand SendSmsCommand { get; }
        public ICommand SendEmailCommand { get; }
        public ICommand CallPhoneCommand { get; }

        public FriendViewModel()
        {
            Friend= new Friend();
            SendSmsCommand = new Command(SendSms);
            SendEmailCommand = new Command(SendEmail);
            CallPhoneCommand = new Command(CallPhone);
        }

        public FriendsListViewModel ListViewModel
        {
            get { return lvm; }
            set
            {
                if (lvm != value)
                {
                    lvm = value;
                    OnPropertyChanged("ListViewModel");
                }
            }
        }

        public string Name 
        {
            get { return Friend.Name; }
            set
            {
                if  (Friend.Name != value)
                {
                    Friend.Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Email
        {
            get { return Friend.Email; }
            set
            {
                if (Friend.Email != value)
                {
                    Friend.Email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        public string Phone
        {
            get { return Friend.Phone; }
            set
            {
                if (Friend.Phone != value)
                {
                    Friend.Phone = value;
                    OnPropertyChanged("Phone");
                }

            }
        }

        public DateTime Birthday
        {
            get { return Friend.BirthDate; }
            set
            {
                if (Friend.BirthDate != value)
                {
                    Friend.BirthDate = value;
                    OnPropertyChanged("Birthday");
                    OnPropertyChanged("Age");
                }
            }
        }

        public string Age
        {
            get
            {
                if (Friend.BirthDate == default(DateTime))
                {
                    return string.Empty;
                }
                var today = DateTime.Today;
                var age = today.Year - Friend.BirthDate.Year;
                if (Friend.BirthDate > today.AddYears(-age))
                {
                    age--;
                }
                return age.ToString();
            }
        }

        private void SendSms()
        {
            var smsMessenger = CrossMessaging.Current.SmsMessenger;
            if (smsMessenger.CanSendSms)
            {
                smsMessenger.SendSms(Phone, $"Привет, {Name}! Это сообщение отправлено из моего приложения!");
            }
        }

        private void SendEmail()
        {
            var emailMessenger = CrossMessaging.Current.EmailMessenger;
            if (emailMessenger.CanSendEmail)
            {
                emailMessenger.SendEmail(Email, "Поздравление с днем рождения", $"Привет, {Name}! Поздравляю тебя с днем рождения!");
            }
        }

        private void CallPhone()
        {
            var phoneDialer = CrossMessaging.Current.PhoneDialer;
            if (phoneDialer.CanMakePhoneCall)
                phoneDialer.MakePhoneCall("+37253584972");
        }

        public bool IsValid
        {
            get
            {
                return ((!string.IsNullOrEmpty(Name.Trim())) ||
                    (!string.IsNullOrEmpty(Phone.Trim())) ||
                    (!string.IsNullOrEmpty(Email.Trim())) ||
                    (!string.IsNullOrEmpty(Age.Trim())));
            }
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
