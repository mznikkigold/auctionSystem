using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using auctionSystem.Models;
using System.Data.Entity;
using System.Net;
//using Newtonsoft.Json;

namespace auctionSystem.Controllers
{
    public class TopBiddersController : Controller
    {
        private MyDatabaseEntities db = new MyDatabaseEntities();
        // GET: TopBidders
        public ActionResult TopBidders()
        {
            var bidTables = db.BidTables.GroupBy(x => x.ItemId)
           .Select(y => y.OrderByDescending(z => z.Bidprice).FirstOrDefault()).Where(d => d.Bid_status != "Approved").ToList();
            return View(bidTables);
        }
        public ActionResult PaymentPage()
        {
            return View();
        }
        public ActionResult AllBids()
        {
            var bidTables = db.BidTables.Include(b => b.Item).Include(b => b.User);
            return View(bidTables.ToList());
        }
        [HttpPost]
        public ActionResult ApproveBid(bool approve_bid, string Bid_status, int itemQuantity,int itemTotalQuantity,int itemId, BidTable bidTable)
        {
            //string message = "";
            System.Diagnostics.Debug.WriteLine(approve_bid);
            System.Diagnostics.Debug.WriteLine(itemQuantity);
            System.Diagnostics.Debug.WriteLine(itemTotalQuantity);
            //if (db.BidTables.Any(o => o.approve_bid == null))
            //{
                var p = Guid.NewGuid().ToString("N").Substring(0, 5);
                var userID = db.Users.FirstOrDefault(x => x.EmailID == System.Web.HttpContext.Current.User.Identity.Name).UserID;

                var objCourse = db.BidTables.Single(d => d.BidId == bidTable.BidId);
                objCourse.approve_bid = approve_bid;
                objCourse.Bid_status = Bid_status;
                db.SaveChanges();
        
                var itemUpdate = db.Items.Single(d => d.ItemId == itemId);
                itemUpdate.itemQuantity = itemTotalQuantity - itemQuantity;
                db.SaveChanges();

                return RedirectToAction("PaymentPage", "TopBidders");
            //}
            //else
            //{
            //    //  message = "Invalid credential provided";
            //    return Json(new { status = "error", message = "Account Does Not Match" });
            //};
            //  ViewBag.Message = message;
            return View();
        }

    }
}