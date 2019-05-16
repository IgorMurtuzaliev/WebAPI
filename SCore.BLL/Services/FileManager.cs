using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SCore.BLL.Interfaces;
using SCore.DAL.EF;
using SCore.Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SCore.BLL.Services
{
    public class FileManager : IFileManager
    {
        private string _imagePath;
        public FileManager(IConfiguration config)
        {
            _imagePath = config["Path:Avatar"];
        }

        public FileStream ImageStream(string image)
        {
            return new FileStream(Path.Combine(_imagePath, image), FileMode.Open, FileAccess.Read);
        }
        public string SaveImage(IFormFile image)
        {
            try {
            var _savePath = Path.Combine(_imagePath);
            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }
            var mime = image.FileName.Substring(image.FileName.LastIndexOf('.'));
            var file_name = $"img_{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}{mime}";


            using (var fileStream = new FileStream(Path.Combine(_savePath, file_name), FileMode.Create))
            {
               image.CopyToAsync(fileStream);
            }
            return file_name;
 }
            catch
            {
                return "Error";
            }
        }

    }
}
