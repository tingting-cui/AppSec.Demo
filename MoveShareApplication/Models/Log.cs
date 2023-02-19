using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MoveShareApplication.Models
{
    public class Log
    {
        [Key]
        public int Log_id { get; set; }
        public string? LogLevel { get; set; }

        public string Thread_id { get; set; }

        public string Event_id { get; set; }

        public string? EventName { get; set; }

        public string? ExceptionMessage { get; set; }

        public string? ExceptionStackTrace { get; set; }

        public string? ExceptionSource { get; set; }

    }
}
