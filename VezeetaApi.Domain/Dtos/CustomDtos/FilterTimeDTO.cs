using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VezeetaApi.Domain.Dtos.CustomDtos
{
    public class FilterTimeDTO
    {
        public Time TimeId { get; set; }


        public DateTime CurrentDate { get; set; } = DateTime.UtcNow;

        private DateTime LastDay
        {
            get { return CurrentDate.AddHours(-24); }
        }

        private DateTime LastWeek
        {
            get { return CurrentDate.AddDays(-7); }
        }

        private DateTime LastMonth
        {
            get { return CurrentDate.AddMonths(-1); }
        }

        private DateTime LastYear
        {
            get { return CurrentDate.AddYears(-1); }
        }


        public DateTime FilterByTime(Time TimeId)
        {
            switch (TimeId)
            {
                case Time.Day:return LastDay;

                case Time.Week: return LastWeek;

                case Time.Month:return LastMonth;

                case Time.Year:return LastYear;

                default: return CurrentDate;
            }
        }
    }

    public enum Time
    {
        CurrentDate = 0,
        Day = 1,
        Week = 2,
        Month = 3,
        Year = 4,
    }
}

