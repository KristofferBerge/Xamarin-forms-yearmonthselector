using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace dateSelector
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class YearMonthSelector : ContentView
    {
        public YearMonthSelectorViewModel vm;

        // Exposed bindable properties
        public static readonly BindableProperty MaximumDateProperty = BindableProperty.Create(
            propertyName: nameof(MaximumDate),
            returnType: typeof(DateTime),
            declaringType: typeof(YearMonthSelector),
            defaultValue: new DateTime(DateTime.Now.Year + 5, 11, 1),
            propertyChanged: OnDateRangeSettingsChanged
            );
        public DateTime MaximumDate
        {
            set
            {
                base.SetValue(MaximumDateProperty, value);
                vm.SetDateRange(MinimumDate, value, SelectedDate);
            }
            get
            {
                return (DateTime)base.GetValue(MaximumDateProperty);
            }
        }

        public static readonly BindableProperty MinimumDateProperty = BindableProperty.Create(
            propertyName: nameof(MinimumDate),
            returnType: typeof(DateTime),
            declaringType: typeof(YearMonthSelector),
            defaultValue: new DateTime(DateTime.Now.Year - 5, 11, 1),
            propertyChanged: OnDateRangeSettingsChanged
        );
        public DateTime MinimumDate
        {
            set
            {
                base.SetValue(MinimumDateProperty, value);
            }
            get
            {
                return (DateTime)base.GetValue(MinimumDateProperty);
            }
        }

        public static readonly BindableProperty SelectedDateProperty = BindableProperty.Create(
            propertyName: nameof(SelectedDate),
            returnType: typeof(DateTime),
            declaringType: typeof(YearMonthSelector),
            defaultValue: new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
            propertyChanged: OnSelectedDateChanged
            );
        public DateTime SelectedDate
        {
            set
            {
                base.SetValue(SelectedDateProperty, value);
            }
            get
            {
                return (DateTime)base.GetValue(SelectedDateProperty);
            }
        }

        public YearMonthSelector()
        {
            InitializeComponent();
            vm = new YearMonthSelectorViewModel();
            //Limit context to specific view to avoid capturing own bindable properties existing context too
            PickerLayout.BindingContext = vm;
            vm.SetDateRange(MinimumDate, MaximumDate, SelectedDate);
        }

        private void OnSelectedDateChanged(object sender, EventArgs e)
        {
            // Only update the bindable selected date if the date actually changed
            // This event may be triggered multiple times when user selects
            if (((DateTime)GetValue(SelectedDateProperty)).Ticks != vm.GetSelectedDate().Ticks)
            {
                SetValue(SelectedDateProperty, vm.GetSelectedDate());
                //Debug.WriteLine("NEW DATE: " + vm.GetSelectedDate().ToShortDateString());
            }
        }

        private static void OnDateRangeSettingsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var minDate = (DateTime)((YearMonthSelector)bindable).GetValue(MinimumDateProperty);
            var maxDate = (DateTime)((YearMonthSelector)bindable).GetValue(MaximumDateProperty);
            var selectedDate = (DateTime)((YearMonthSelector)bindable).GetValue(SelectedDateProperty);
            ((YearMonthSelector)bindable).vm.SetDateRange(minDate, maxDate, selectedDate);
        }
        private static void OnSelectedDateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            Debug.WriteLine(oldValue + " " + newValue);
        }
    }
}