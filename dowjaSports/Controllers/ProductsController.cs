using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dowjaSports.Models;

using Newtonsoft.Json;
using static System.Collections.Specialized.NameObjectCollectionBase;
using System.Net.Mail;

namespace dowjaSports.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        int totalItems = 0;
        dojawsportsdb1Entities db = new dojawsportsdb1Entities();
        public ActionResult Items(string cat,string subCat)
        {
            
            IQueryable<Product> myProducts = from abc in db.Products
                                             where abc.Category == cat && abc.SubCategory == subCat
                                             select abc;
            foreach(Product abc in myProducts)
            {
                string aaa = abc.Category;
            }

            return View(myProducts);
        }

        public ActionResult itemDetail(int id)
        {

            Product item = (from abc in db.Products
                            where abc.Id == id
                            select abc).Single<Product>();

            return View(item);
        }


        public int addToCart(int id,string quantity,string payment,string size)
        {

            cart item = new cart();
            item.id = id;
            item.payment = payment;
            item.quantity = quantity;
            item.size = size;


            
            
           

            //totalItems = Request.Cookies["totalItems"];

            totalItems = Request.Cookies.Count;

            if (totalItems != 0)
            {
                //Request.Cookies.Add(cb);
                //string list = Request.Cookies.Last().Key;
                //int key = Convert.ToInt16(list);
                //key = key + 1;
                //string name = key.ToString();
                //Response.Cookies.Append(name, item1, cb);
                ////Response.Cookies.Append("totalItmes", name, cb);

                string key = DateTime.Now.ToString();

                string nameCookie = (key).ToString();

                item.key = nameCookie;
                string item1 = JsonConvert.SerializeObject(item);

                HttpCookie cookie = new HttpCookie(nameCookie);
                cookie.Value = item1;


                cookie.Expires = System.DateTime.Now.AddDays(1);
                Response.Cookies.Add(cookie);

            }

            else
            {
                //Response.Cookies.Append("1", item1, cb);
                HttpCookie cookie = new HttpCookie("1");
                item.key = "1";
                string item1 = JsonConvert.SerializeObject(item);
                cookie.Value = item1;
                cookie.Expires = System.DateTime.Now.AddDays(1);
                Response.Cookies.Add(cookie);

            }


            return 1;
        }


        public int totalCookies()
        {
            int items= Request.Cookies.Count; 
            return items;
        }

        public ActionResult cartItems()
        {
            KeysCollection list = Request.Cookies.Keys;

            IList<cart> allObj = new List<cart>();


            foreach (string a in list)
            {
                string obj = Request.Cookies[a].Value;
                cart obj2 = JsonConvert.DeserializeObject<cart>(obj);

                Product item = (from abc in db.Products
                                where abc.Id == obj2.id
                                select abc).Single<Product>();
                obj2.productName = item.ProductName;
                obj2.imgUrl = "/dojawsports.com/Items/" + item.Category + "/" + item.SubCategory + "/" + item.ProductURL;

                allObj.Add(obj2);


            }
            ViewBag.item_Keys = list;

            return View("cartitems", allObj);
           
        }


        public bool deleteItem(string key)
        {
            HttpCookie cb = new HttpCookie(key);
            cb.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(cb);
            return true;
        }

        public int checkout(string username, string phone, string email, string address)
        {
           
             KeysCollection list = Request.Cookies.Keys;

            IList<cart> allObj = new List<cart>();


            foreach (string a in list)
            {
                string obj = Request.Cookies[a].Value;
                cart obj2 = JsonConvert.DeserializeObject<cart>(obj);

                Product item = (from abc in db.Products
                                where abc.Id == obj2.id
                                select abc).Single<Product>();
                obj2.productName = item.ProductName;
                obj2.imgUrl = "/dojawsports.com/Items/" + item.Category + "/" + item.SubCategory + "/" + item.ProductURL;

                allObj.Add(obj2);

            }


            KeysCollection items_keys = Request.Cookies.Keys;
            MailMessage EmailObject = new MailMessage();
            EmailObject.From = new MailAddress("fahadsherwani01@gmail.com", "Order");
            EmailObject.To.Add(new MailAddress("info@dojawsports.com", "tuDcJSMQB2DZ"));

            EmailObject.Subject = username;
            //EmailObject.Body = "<h1>this is body of the email</h1>";
            EmailObject.IsBodyHtml = true;


            string body = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
            body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";
            body += "</HEAD><BODY><div>Name  :'" + username + "'</div><div>Address  :'" + address + "'</div><div>Email  :'" + email + "'</div><div>Phone  :'" + phone + "'</div><table class='table table-responsive'>";



            int i = 0;

            List<LinkedResource> resources = new List<LinkedResource>();

            foreach (var item in allObj)
            {
                string imageTag = string.Format("<tr><td><img src=cid:chart'" + i + "' /></td><td><h5>'" + item.quantity + "'</h5></td></tr>");
                body += imageTag;

                LinkedResource img = new LinkedResource(Server.MapPath(item.imgUrl), System.Net.Mime.MediaTypeNames.Image.Jpeg);
                img.ContentId = "chart" + i;

                resources.Add(img);
                i++;
            }

            body += "</table></BODY></HTML>";

            



            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
            //LinkedResource logo = new LinkedResource("wwwroot/BEAUTY_INSTRUMENT/CUTICLEE SCISSORS/cuticle-nippers-1419075075-RB-7701.jpg");
            //logo.ContentId = "companylogo";

            //htmlView.LinkedResources.Add(logo);

            resources.ForEach(x => htmlView.LinkedResources.Add(x));

            EmailObject.AlternateViews.Add(htmlView);
            EmailObject.Body = body;
            SmtpClient SC = new SmtpClient("smtpout.asia.secureserver.net", 3535);
            SC.Credentials = new System.Net.NetworkCredential("info@dojawsports.com", "tuDcJSMQB2DZ");
            SC.EnableSsl = false;
            SC.Send(EmailObject);

            return 1;
           
           }

        public ActionResult userForm()
        {
            return View();
        }

    }
}