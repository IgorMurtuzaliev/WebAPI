using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SCore.BLL.Interfaces
{
    public interface IFileManager
    {
        string SaveImage(IFormFile image);
        FileStream ImageStream(string image);
    }
}
