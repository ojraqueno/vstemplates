using Core1.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core1.Features.Timezone
{
    public class TimezoneHelper
    {
        static TimezoneHelper()
        {
            TimezoneLookupItems = TimeZoneInfo
                .GetSystemTimeZones()
                .Select(tz => (ILookupItem)new LookupItem
                {
                    Text = tz.DisplayName,
                    Value = tz.BaseUtcOffset.TotalMinutes.ToString()
                })
                .ToList();
        }

        public static IEnumerable<ILookupItem> TimezoneLookupItems { get;  }
    }
}