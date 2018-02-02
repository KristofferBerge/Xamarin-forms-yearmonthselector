using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace dateSelector
{
    public class YearMonthSelectorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<string> months = new List<string>();
        public List<String> Months
        {
            set
            {
                if (months != value)
                {
                    months = value;
                    NotifyPropertyChanged(nameof(Months));
                }
            }
            get
            {
                return months;
            }
        }

        private int selectedMonthIndex;
        public int SelectedMonthIndex
        {
            get
            {
                return selectedMonthIndex;
            }
            set
            {
                if (selectedMonthIndex != value)
                {
                    if (value != -1)
                        selectedMonthIndex = value;
                    NotifyPropertyChanged(nameof(SelectedMonthIndex));
                }
            }
        }
        private List<int> years = new List<int>();
        public List<int> Years
        {
            set
            {
                if (years != value)
                {
                    years = value;
                    NotifyPropertyChanged(nameof(Years));
                }
            }
            get
            {
                return years;
            }
        }

        private int selectedYearIndex;
        public int SelectedYearIndex
        {
            get
            {
                return selectedYearIndex;
            }
            set
            {
                if (selectedYearIndex != value)
                {
                    selectedYearIndex = value;
                    SetMonthRange();
                    NotifyPropertyChanged(nameof(SelectedYearIndex));
                }
            }
        }
        public DateTime MaxDate { set; get; }
        public void SetDateRange(DateTime minDate, DateTime maxDate, DateTime selectedDate)
        {
            Years = getYears(minDate.Year, maxDate.Year);
            MaxDate = maxDate;
            SetMonthRange();
            var selectedYear = years.Where(x => x == selectedDate.Year).FirstOrDefault();
            SelectedYearIndex = years.IndexOf(selectedYear);
            var selectedMonth = months[selectedDate.Month - 1];
            SelectedMonthIndex = months.IndexOf(selectedMonth);
        }
        public void SetMonthRange()
        {
            // If on max year, limit months to max month
            if (SelectedYearIndex != -1 && years[SelectedYearIndex] == MaxDate.Year)
            {
                var newMonths = getMonths(MaxDate.Month);
                if (Months.Count != newMonths.Count)
                    Months = newMonths;
            }
            else
            {
                var newMonths = getMonths(12);
                if (Months.Count != newMonths.Count)
                    Months = newMonths;
            }
            // If we adjust month range, the selected month may be outside the new range
            // Set to last of outside after adjusting range
            if (SelectedMonthIndex > Months.Count)
                SelectedMonthIndex = Months.Count - 1;
        }
        private List<string> getMonths(int maxMonth)
        {
            var months = new List<string>();
            var d = new DateTime(DateTime.Now.Year, 1, 1);
            for (int i = 0; i < maxMonth; i++)
            {
                months.Add(d.ToString("MMMM"));
                d = d.AddMonths(1);
            }
            return months;
        }

        private List<int> getYears(int minYear, int maxYear)
        {
            var years = new List<int>();
            while (minYear <= maxYear)
            {
                years.Add(minYear++);
            }
            return years;
        }

        public DateTime GetSelectedDate()
        {
            var y = Years[SelectedYearIndex];
            var m = DateTime.ParseExact(Months[SelectedMonthIndex], "MMMM", CultureInfo.InvariantCulture).Month;
            return new DateTime(y, m, 1);
        }

        public void SetSelectedDate(DateTime newDate)
        {
            // Set selected or highest possible year
            if (newDate.Year < Years.LastOrDefault())
            {
                SelectedYearIndex = Years.IndexOf(newDate.Year);
            }
            else
            {
                SelectedYearIndex = Years.Count - 1;
            }
            SetMonthRange();
            // Set selected or highest possible month
            if (newDate.Month < Months.Count)
            {
                SelectedMonthIndex = newDate.Month -1;
            }
            else
            {
                SelectedMonthIndex = Months.Count - 1;
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
