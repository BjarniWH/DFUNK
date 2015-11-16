using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DFUNK.Models;

namespace DFUNK.Controllers
{
    public class PaymentsController : Controller
    {
        private Models.DFUNK db = new Models.DFUNK();

        // GET: Payments
        public ActionResult Index()
        {
            var payments = db.Payments.Include(p => p.Contact).Include(p => p.PaymentMethod).Include(p => p.Projects);
            var result = payments.ToList();
            decimal total = 0;
            foreach(Payments payment in result)
            {
                total = total + (decimal)payment.amount;
            }

            return View(result);
        }

        // GET: Payments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payments payments = db.Payments.Find(id);
            if (payments == null)
            {
                return HttpNotFound();
            }
            return View(payments);
        }

        // GET: Payments/Create
        public ActionResult Create()
        {
            ViewBag.contact_id = new SelectList(db.Contact, "contact_id", "name");
            ViewBag.payment_method = new SelectList(db.PaymentMethod, "paymentMethod_id", "name");
            ViewBag.project_id = new SelectList(db.Projects, "project_id", "name");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "payment_id,date,amount,payment_method,contact_id,project_id")] Payments payments)
        {
            if (ModelState.IsValid)
            {
                payments.date = DateTime.Now;
                db.Payments.Add(payments);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.contact_id = new SelectList(db.Contact, "contact_id", "name", payments.contact_id);
            ViewBag.payment_method = new SelectList(db.PaymentMethod, "paymentMethod_id", "name", payments.payment_method);
            ViewBag.project_id = new SelectList(db.Projects, "project_id", "name", payments.project_id);
            return View(payments);
        }

        // GET: Payments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payments payments = db.Payments.Find(id);
            if (payments == null)
            {
                return HttpNotFound();
            }
            ViewBag.contact_id = new SelectList(db.Contact, "contact_id", "name", payments.contact_id);
            ViewBag.payment_method = new SelectList(db.PaymentMethod, "paymentMethod_id", "name", payments.payment_method);
            ViewBag.project_id = new SelectList(db.Projects, "project_id", "name", payments.project_id);
            return View(payments);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "payment_id,date,amount,payment_method,contact_id,project_id")] Payments payments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.contact_id = new SelectList(db.Contact, "contact_id", "name", payments.contact_id);
            ViewBag.payment_method = new SelectList(db.PaymentMethod, "paymentMethod_id", "name", payments.payment_method);
            ViewBag.project_id = new SelectList(db.Projects, "project_id", "name", payments.project_id);
            return View(payments);
        }

        // GET: Payments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payments payments = db.Payments.Find(id);
            if (payments == null)
            {
                return HttpNotFound();
            }
            return View(payments);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payments payments = db.Payments.Find(id);
            db.Payments.Remove(payments);
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
