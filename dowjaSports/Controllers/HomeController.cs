using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dowjaSports.Models;

namespace dowjaSports.Controllers
{
    public class HomeController : Controller
    {
        dojawsportsdb1Entities db = new dojawsportsdb1Entities();
        public ActionResult Index()
        {
            //this is To add data in table

            Product p1 = new Product();
            //string subCat = null;
         

            //    string[] filePaths = Directory.GetFiles(Server.MapPath("~/Items/Uniforms/Rugby Uniforms/"));
            //    IList<string> abc = new List<string>();

            //    foreach (string filePath in filePaths)
            //    {
            //        abc.Add(Path.GetFileName(filePath));
            //        p1.Category = "Uniforms";
            //        p1.SubCategory = "Rugby Uniforms";
            //        p1.ProductURL = Path.GetFileName(filePath);
            //        db.Products.Add(p1);
            //        db.SaveChanges();
            //    }

            






            return View();
        }

        public ActionResult About()
        {
            IQueryable<Product> myProducts = from abc in db.Products
                                             where abc.Category == "boxing gear" && abc.SubCategory == "Arm Shields"
                                             select abc;

          

            return View(myProducts);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}