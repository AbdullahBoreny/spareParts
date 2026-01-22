using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spareParts.Models
{
    public class Message
{
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public bool IsIncoming { get; set; }
    public string TimeLabel => Timestamp.ToString("HH:mm");
}
}
