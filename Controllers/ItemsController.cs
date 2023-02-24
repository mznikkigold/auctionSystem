using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using auctionSystem.Models;

namespace auctionSystem.Controllers
{
    public class ItemsController : Controller
    {
        private readonly MyDatabaseEntities db = new MyDatabaseEntities();

        // GET: Items
        public ActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine(User.IsInRole("Administrator")); 
            var items = db.Items.Include(i => i.User);
            return View(items.ToList());
        }

        // GET: Items/Details/5
        public ActionResult Details(int? id, string heading = "")
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Admin"))
                ViewBag.UserRole = "Admin";
            else if (User.IsInRole("Vendor") || User.IsInRole("Customer"))
            {
                ViewBag.heading = heading;
                ViewBag.UserRole = "Users";
            }
            else
                ViewBag.UserRole = "Gost";

            var productQuery = from b in db.BidTables
                               where b.ItemId == id
                               orderby b.BidId descending
                               select b;
            var limitedProductQuery = productQuery.Take(10);

            int count = limitedProductQuery.Count();
            long[] prices = new long[count];
            string[] bidders = new string[count];
            string[] times = new string[count];
            string[] states = new string[count];
            int i = 0;
            foreach (var bid in limitedProductQuery)
            {
                prices[i] = bid.BidId;
                bidders[i] = bid.User.FirstName;
              //  times[i] = bid.BidDate.ToString(@"dd\:hh\:mm\:ss");
                states[i++] = "Open";
            }
            if (item.state == "Sold")
            {
                states[0] = "Sold";
            }
            ViewBag.count = count;
            ViewBag.prices = prices;
            ViewBag.bidders = bidders;
            ViewBag.times = times;
            ViewBag.states = states;
            ViewBag.IDAuction = id;
            ViewBag.heading = heading;
            return View(item);
        }

        // GET: Items/Create
        [AutorizedClass(Roles = "Administrator, Vendor")]
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "UserID", "FirstName");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

      //  [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AutorizedClass(Roles = "Administrator, Vendor")]
        public ActionResult Create([Bind(Include = "ItemId,ItemName,ItemPrice,ItemDesc,PictureToUpload,open_date_time,close_date_time,UserId,UserName,itemQuantity")] Item item)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(item.PictureToUpload.FileName);
                string extension = Path.GetExtension(item.PictureToUpload.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                item.picture = "../Images/" + fileName;
                fileName = Path.Combine(Server.MapPath("../Images/"), fileName);
                item.PictureToUpload.SaveAs(fileName);
                item.DateCreated = DateTime.Now;
                item.state = "Ready";
                item.lastbidder = " ";
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "UserID", "FirstName", item.UserId);
            return View(item);
        }

        // GET: Items/Edit/5
        [AutorizedClass(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "UserID", "FirstName", item.UserId);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AutorizedClass(Roles = "Administrator")]
        public ActionResult Edit([Bind(Include = "ItemId,ItemName,PictureToUpload,ItemPrice,ItemDesc,UserId,UserName,itemQuantity,open_date_time,close_date_time")] Item item)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(item.PictureToUpload.FileName);
                string extension = Path.GetExtension(item.PictureToUpload.FileName);
          //      fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                item.picture = "../Images/" + fileName;
                fileName = Path.Combine(Server.MapPath("../Images/"), fileName);
                item.PictureToUpload.SaveAs(fileName);
                item.DateCreated = DateTime.Now;
                item.state = "Ready";
                item.lastbidder = " ";
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "UserID", "FirstName", item.UserId);
            return View(item);
        }

        // GET: Items/Delete/5
        [AutorizedClass(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AutorizedClass(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
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



  
//namespace ClubMember.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ActionResult Index()
//        {
//            return View();
//        }

//        public ActionResult About()
//        {
//            ViewBag.Message = "Your application description page.";

//            return View();
//        }

//        public ActionResult Contact()
//        {
//            ViewBag.Message = "Your contact page.";

//            return View();
//        }

//        [HttpGet]
//        public ActionResult ContactForm()
//        {

//            return View();
//        }

//        [HttpPost]
//        public ActionResult ContactForm(MemberModel membervalues)
//        {
//            //Use Namespace called :  System.IO  
//            string FileName = Path.GetFileNameWithoutExtension(membervalues.ImageFile.FileName);

//            //To Get File Extension  
//            string FileExtension = Path.GetExtension(membervalues.ImageFile.FileName);

//            //Add Current Date To Attached File Name  
//            FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;

//            //Get Upload path from Web.Config file AppSettings.  
//            string UploadPath = ConfigurationManager.AppSettings["UserImagePath"].ToString();

//            //Its Create complete path to store in server.  
//            membervalues.ImagePath = UploadPath + FileName;

//            //To copy and save file into server.  
//            membervalues.ImageFile.SaveAs(membervalues.ImagePath);


//            //To save Club Member Contact Form Detail to database table.  
//            var db = new ClubMemberDataClassesDataContext();

//            tblMember _member = new tblMember();

//            _member.ImagePath = membervalues.ImagePath;
//            _member.MemberName = membervalues.Name;
//            _member.PhoneNumber = membervalues.PhoneNumber;

//            db.tblMembers.InsertOnSubmit(_member);
//            db.SubmitChanges();

//            return View();
//        }

//    }
//}
