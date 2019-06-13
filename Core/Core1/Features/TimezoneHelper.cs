using Core1.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core1.Features
{
    public static class TimezoneHelper
    {
        private static IList<ILookupItem> _timezoneLookupItems;
        public static IList<ILookupItem> TimezoneLookupItems
        {
            get
            {
                if (_timezoneLookupItems == null)
                {
                    _timezoneLookupItems = TimeZoneInfo
                        .GetSystemTimeZones()
                        .Select(tz => (ILookupItem)new LookupItem
                        {
                            Text = tz.DisplayName,
                            Value = tz.BaseUtcOffset.TotalMinutes.ToString()
                        })
                        .ToList();
                }

                return _timezoneLookupItems;
            }
        }
    }
}