
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SCore.BLL.Interfaces;
using SCore.BLL.Models;
using SCore.DAL.EF;
using SCore.DAL.Interfaces;
using SCore.Models;
using SCore.Models.Entities;

using System;
using System.Collections.Generic;
using System.IO;

namespace SCore.BLL.Services
{
    public class ProductService : IProductService
    {
        IUnitOfWork db { get; set; }
        private readonly IFileManager _fileManager;
        private readonly ApplicationDbContext context;
        IHostingEnvironment _appEnvironment;
        public ProductService(IUnitOfWork _db, IFileManager fileManager, IConfiguration config, ApplicationDbContext _context, IHostingEnvironment appEnvironment)
        {
            db = _db;
            _fileManager = fileManager;
            _appEnvironment = appEnvironment;
            context = _context;
        }

        public async void Create(ProductModel model)
        {
            Product product = new Product
            {
                ProductId = model.ProductId,
                Date = model.Date,
                Description = model.Description,
                Name = model.Name,
                Price = model.Price
            };
            db.Products.Create(product);
            db.Products.Save();
            foreach(var file in model.Images)
            {
                string path = "/Files/" + file.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                  await file.CopyToAsync(fileStream);
                }
                 FileModel newfile = new FileModel { Name = file.FileName, Path = path, ProductId = product.ProductId};
                context.Files.Add(newfile);
                context.SaveChanges();
                product.Files.Add(newfile);
            }         
        }

        public Product Get(int id)
        {
            return db.Products.Get(id);
        }

        public void Delete(int id)
        {
            db.Products.Delete(id);
            db.Products.Save();
        }

        public IEnumerable<Product> GetAll()
        {
            return db.Products.GetAll();
        }

        public void Edit(ProductModel model)
        {
            Product product = new Product
            {
                ProductId = model.ProductId,
                Date = model.Date,
                Description = model.Description,
                Name = model.Name,
                Price = model.Price
            };
            db.Products.Edit(product);
            db.Products.Save();
        }

        public void Dispose(bool disposing)
        {
            db.Dispose(disposing);
        }
    }
}
