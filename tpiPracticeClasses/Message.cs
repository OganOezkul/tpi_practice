using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tpiPracticeClasses
{
    public class Message
    {
        [Key]
        public int messageId { get; set; }

        public int userId { get; set; }
        public int groupchatId { get; set; }
        public string content { get; set; }
    }
}
