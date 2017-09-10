using System;

namespace CoreMVC.WebApp.Features.Shared
{
    public class Error
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !String.IsNullOrEmpty(RequestId);
    }
}