using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        [Obsolete]
        private IHostingEnvironment _he;
        [Obsolete]
        public ProductController(ApplicationDbContext db, IHostingEnvironment he)
        {
            _db = db;
            _he = he;
        }
        public IActionResult Index()
        {
            return View(_db.Products.Include(c => c.ProductTypes).ToList());
        }
        //Get Create method
        public IActionResult Create()
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");

            return View();

        }
        //Post Create method
        [HttpPost]
        [Obsolete]
        public async Task<IActionResult> Create(Products products, IFormFile image)
        {

            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    products.Image = "Images/" + image.FileName;
                }
                if (image == null)
                {
                    products.Image = "Images/noimage.jpg";
                }

                _db.Products.Add(products);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }
        //Get Edit Action Method
        [HttpPost]
        public ActionResult Edit(int? id)
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            if (id == null)
            {

                return NotFound();
            }
            var product = _db.Products.Include(c => c.ProductTypes).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }



        //POST EDIT METHOD
        //        [HttpPost]
        //        public async Task<IActionResult> Edit(Products products, IFormFile image);
        //                {

        //                    if (ModelState.IsValid)
        //                    {
        //                        if (image != null)
        //                        {
        //                            var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
        //                   await image.CopyToAsync(new FileStream(name, FileMode.Create));
        //                            products.Image = "Images/" + image.FileName;
        //                        }
        //                        if (image == null)
        //                        {
        //                            products.Image = "Images/noimage.jpg";
        //                        }

        //              _db.Products.Update(products);
        //              await _db.SaveChangesAsync();
        //              return RedirectToAction(nameof(Index));
        //                    }
        //                    return View(products);
        //                }


        // }
        //// GET DETAILS METHOD
        //public ActionResult Details( int id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();

        //    }
        //    var product = _db.Products.Include(c => c.ProductTypes).FirstOrDefault(c => c.Id = id);

        //    if (product==null)
        //    {
        //        return NotFound();
        //    }
        //    return View();

        // }


        //GET  DELETE METHOD

        [HttpPost]
        public ActionResult Delete(int id)

        {
            if (id==null)
            {

                return NotFound();
            }
            var product = _db.Products.Include(c => c.ProductTypes).Where(c => c.Id == id).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }

}
