using System;

namespace SWP.UI.Components.AdminBlazorComponents.ViewModels
{
    public class LogRecordViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime Created { get; set; }
    }
}
