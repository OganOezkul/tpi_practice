using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace tpiPracticeClasses
{
    public class Groupchat
    {
        [Key]
        [JsonPropertyName("grouchatId")]
        public int GroupchatId { get; set; }

        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
