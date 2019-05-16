using System;
using System.Collections.Generic;
using System.Text;

namespace SCore.Models.Entities
{
    public class FileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
