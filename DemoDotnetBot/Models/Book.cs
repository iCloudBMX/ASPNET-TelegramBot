using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDotnetBot.Models
{
    public class Book
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string FileId { get; set; }
    }
}
