using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entites.DTOs.Image
{
    public class DeleteImageDto
    {
        public string FullName { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
    }
}
