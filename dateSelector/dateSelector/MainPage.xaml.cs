using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace dateSelector
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        public DateTime MaxDate { set; get; } = DateTime.Now;
        private DateTime myDate;
        public DateTime MyDate
        {
            set
            {
                if (myDate != value)
                {
                    myDate = value;
                    OnPropertyChanged(nameof(MyDate));
                }
            }
            get
            {
                return myDate;
            }
        }
        public MainPage()
        {
            BindingContext = this;
            InitializeComponent();
        }
    }
}
