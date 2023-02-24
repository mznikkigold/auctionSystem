using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using auctionSystem.Models;

namespace auctionSystem.Controllers
{
    public class BidTablesController : Controller
    {
        private MyDatabaseEntities db = new MyDatabaseEntities();

        // GET: BidTables
        public ActionResult Index()
        {
            var userID = db.Users.FirstOrDefault(x => x.EmailID == System.Web.HttpContext.Current.User.Identity.Name).UserID;
            // var userID = Context.USER.Where(user => user.UserName == login.userName).select(user => user.UserID);
            var bidTables = db.BidTables.Include(b => b.Item).Include(b => b.User).Where(a => a.UserId == userID);
            System.Diagnostics.Debug.WriteLine(User.Identity.Name);
            return View(bidTables.ToList());
        }

        // GET: BidTables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BidTable bidTable = db.BidTables.Find(id);
            if (bidTable == null)
            {
                return HttpNotFound();
            }
            return View(bidTable);
        }

        // GET: BidTables/Create
        public ActionResult Create()
        {
            ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName");
            ViewBag.UserId = new SelectList(db.Users, "UserID", "FirstName");
            return View();
        }

        // POST: BidTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BidId,BidderName,BidderText,ItemId,ItemName,UserId,Bidprice,BidQuantity,BidDate,DateCreated")] BidTable bidTable)
        {
            if (ModelState.IsValid)
            {
                db.BidTables.Add(bidTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName", bidTable.ItemId);
            ViewBag.UserId = new SelectList(db.Users, "UserID", "FirstName", bidTable.UserId);
            return View(bidTable);
        }

        // GET: BidTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BidTable bidTable = db.BidTables.Find(id);
            if (bidTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName", bidTable.ItemId);
            ViewBag.UserId = new SelectList(db.Users, "UserID", "FirstName", bidTable.UserId);
            return View(bidTable);
        }

        // POST: BidTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BidId,BidderName,BidderText,ItemId,ItemName,UserId,Bidprice,BidQuantity,BidDate,DateCreated")] BidTable bidTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bidTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemId = new SelectList(db.Items, "ItemId", "ItemName", bidTable.ItemId);
            ViewBag.UserId = new SelectList(db.Users, "UserID", "FirstName", bidTable.UserId);
            return View(bidTable);
        }

        // GET: BidTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BidTable bidTable = db.BidTables.Find(id);
            if (bidTable == null)
            {
                return HttpNotFound();
            }
            return View(bidTable);
        }

        // POST: BidTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BidTable bidTable = db.BidTables.Find(id);
            db.BidTables.Remove(bidTable);
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
