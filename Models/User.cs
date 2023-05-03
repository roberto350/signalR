using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace apiChat.Models
{
   
    public class User
    {
        [Key]
        public string usuario { get; set; }
        public string grupo { get; set; }
    }
}
