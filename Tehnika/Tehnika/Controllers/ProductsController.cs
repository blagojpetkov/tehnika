using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tehnika.Models;

namespace Tehnika.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        static ShoppingCart cart = new ShoppingCart();

        // GET: Products
        
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        [Authorize]
        public ActionResult Buy()
        {
            cart.products.Clear();
            return View();
        }

        public ActionResult ShoppingCart()
        {
            //cart.products = new List<Product> { new Product { Id = 2222, Category = "asdf", Description = "asdf", Name = "Ime", Price = 200, Discount = 12, ImageURL = "asdf", Warranty = 22222 } };
            return View(cart.products);
        }

        public ActionResult DeleteCart(int id)
        {
            foreach(Product p in cart.products.ToList())
            {
                if (p.Id.Equals(id))
                {
                    cart.products.Remove(p);
                }
            }
            return View("ShoppingCart", cart.products);
        }

        public ActionResult Search(string id)
        {
            ViewBag.Message = "Your application description page.";
            if(id.Equals("all"))
                return View("Index", db.Products.ToList());
            List<Product> products = new List<Product>();
            foreach(Product p in db.Products.ToList())
            {
                if (p.Category.Equals(id))
                {
                    products.Add(p);
                }
            }
            return View("Index", products);
        }

        public ActionResult SearchDetailed(string id)
        {
            
            foreach (Product p in db.Products.ToList())
            {
                if (p.Id.Equals(int.Parse(id)))
                {
                    return View(p);
                }
            }
            return HttpNotFound();
            
        }

        public ActionResult AddToCart(string id)
        {
            foreach (Product p in db.Products.ToList())
            {
                if (p.Id.Equals(int.Parse(id)))
                {
                    cart.products.Add(p);
                    return View("Index", db.Products.ToList());
                }
            }
            return HttpNotFound();
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,ImageURL,Price,Discount,Warranty,Category")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,ImageURL,Price,Discount,Warranty,Category")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
