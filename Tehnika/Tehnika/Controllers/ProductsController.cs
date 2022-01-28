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
        //static ShoppingCart cart = new ShoppingCart();

        // GET: Products
        
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        [Authorize]
        public ActionResult Buy()
        {
            string username = User.Identity.Name;
            ApplicationUser user = db.Users.Single(x => x.UserName.Equals(username));
            
            ShoppingCart cart = (ShoppingCart)Session["cart"];
            if (cart == null)
            {
                cart = new ShoppingCart();
                Session["cart"] = cart;
            }

            ShoppingCart databaseCart = new ShoppingCart();
            databaseCart.address = user.Address;
            databaseCart.username = user.UserName;
            foreach(Product p in cart.products)
            {
                databaseCart.products.Add(db.Products.Single(x => x.Name.Equals(p.Name)));
            }
            db.Carts.Add(databaseCart);
            db.SaveChanges();
            cart.products.Clear();
            return View();
        }

        public ActionResult ShoppingCart()
        {
            ShoppingCart cart = (ShoppingCart) Session["cart"];
            if (cart == null)
            {
                cart = new ShoppingCart();
                Session["cart"] = cart;
            }

            return View(cart.products);
        }

        public ActionResult DeleteCart(int id)
        {
            ShoppingCart cart = (ShoppingCart)Session["cart"];
            if (cart == null)
            {
                cart = new ShoppingCart();
                Session["cart"] = cart;
            }

            foreach (Product p in cart.products.ToList())
            {
                if (p.Id.Equals(id))
                {
                    cart.products.Remove(p);
                    Session["cart"] = cart;
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
            
            foreach (Product p in db.Products.Include(x=>x.ProductComments.Select(productcomment=>productcomment.user)).ToList())
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
            ShoppingCart cart = (ShoppingCart)Session["cart"];
            if (cart == null)
            {
                cart = new ShoppingCart();
                Session["cart"] = cart;
            }

            foreach (Product p in db.Products.ToList())
            {
                if (p.Id.Equals(int.Parse(id)))
                {
                    cart.products.Add(p);
                    Session["cart"] = cart;
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
                product.ProductComments = new List<ProductComment>();
                product.Grade = 0;
                product.Graders = 0;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Administrator,Moderator")]
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
        [Authorize(Roles = "Administrator,Moderator")]
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
            Product product = db.Products.Include(x=>x.ProductComments).FirstOrDefault(p => p.Id == id);
            product.ProductComments.Clear();
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Grade(int id, int grade)
        {
            Product product = db.Products.Find(id);
            product.Grade = (product.Grade * product.Graders + grade) / (product.Graders + 1);
            product.Graders++;
            db.SaveChanges();
            return RedirectToAction("SearchDetailed", new { id = id });
        }

        public ActionResult AddComment(int id, string comment)
        {
            string username = User.Identity.Name;
            ApplicationUser user = db.Users.Single(x=>x.UserName.Equals(username));
            Product product = db.Products.Find(id);
            if (product.ProductComments == null)
                product.ProductComments = new List<ProductComment>();
            product.ProductComments.Add(new ProductComment() { comment = comment, user = user });
            db.SaveChanges();
            return RedirectToAction("SearchDetailed", new { id = id });
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult BuyList()
        {
            List<ShoppingCart> carts = db.Carts.Include(x=>x.products).ToList();
            return View(carts);
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
