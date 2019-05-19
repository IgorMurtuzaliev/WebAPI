
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
using System.Threading.Tasks;

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

        public async Task Create(ProductModel model)
        {
            Product product = new Product
            {
                ProductId = model.ProductId,
                Date = model.Date,
                Description = model.Description,
                Name = model.Name,
                Price = model.Price
            };
            await db.Products.Create(product);
            await db.Products.Save();
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

        public async Task<Product> Get(int id)
        {
            return await db.Products.Get(id);
        }

        public async Task Delete(int id)
        {
            await db.Products.Delete(id);
            await db.Products.Save();
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await db.Products.GetAll();
        }

        public async Task Edit(ProductModel model)
        {
            Product product = new Product
            {
                ProductId = model.ProductId,
                Date = model.Date,
                Description = model.Description,
                Name = model.Name,
                Price = model.Price
            };
            await db.Products.Edit(product);
            await db.Products.Save();
        }

        public void Dispose(bool disposing)
        {
            db.Dispose(disposing);
        }
    }
}
