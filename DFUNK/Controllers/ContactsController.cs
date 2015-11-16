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
    public class ContactsController : Controller
    {
        private Models.DFUNK db = new Models.DFUNK();

        // GET: Contacts
        public ActionResult Index()
        {
            var contact = db.Contact.Include(c => c.CompanyInfo).Include(c => c.VolunteerInfo);
            return View(contact.ToList());
        }

        // GET: Contacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contact.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            ViewBag.contact_id = new SelectList(db.CompanyInfo, "contact_id", "contactPerson");
            ViewBag.contact_id = new SelectList(db.VolunteerInfo, "contact_id", "tshirtSize");
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "contact_id,name,surname,dateOfBirth,street,zipCode,city,phoneNr,email,company,registerDate,member,stakeholder,volunteer,internalEmployee")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Contact.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.contact_id = new SelectList(db.CompanyInfo, "contact_id", "contactPerson", contact.contact_id);
            ViewBag.contact_id = new SelectList(db.VolunteerInfo, "contact_id", "tshirtSize", contact.contact_id);
            return View(contact);
        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contact.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            ViewBag.contact_id = new SelectList(db.CompanyInfo, "contact_id", "contactPerson", contact.contact_id);
            ViewBag.contact_id = new SelectList(db.VolunteerInfo, "contact_id", "tshirtSize", contact.contact_id);
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "contact_id,name,surname,dateOfBirth,street,zipCode,city,phoneNr,email,company,registerDate,member,stakeholder,volunteer,internalEmployee")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.contact_id = new SelectList(db.CompanyInfo, "contact_id", "contactPerson", contact.contact_id);
            ViewBag.contact_id = new SelectList(db.VolunteerInfo, "contact_id", "tshirtSize", contact.contact_id);
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contact.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = db.Contact.Find(id);
            db.Contact.Remove(contact);
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
