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
            var contact = db.Contact.Include(c => c.CompanyInfo).Include(c => c.VolunteerInfo).Where(x => x.company == false);
            return View(contact.ToList());
        }

        //[HttpPost]
        //public ActionResult Index(bool Companies, bool Members, bool Stakeholders, bool Volunteers, bool InternalEmployees)
        //{
        //    db = new Models.DFUNK();
        //    var contact = db.Contact.Include(c => c.CompanyInfo).Include(c => c.VolunteerInfo).Where(x => x.company == Companies && x.member == Members && x.stakeholder == Stakeholders
        //    && x.volunteer == Volunteers && x.internalEmployee == InternalEmployees);

        //    return View(contact.ToList());
        //}

        [HttpPost]
        public ActionResult RemoveFromGroup(int Group1, int contact_id)
        {
            db.Contact.Find(contact_id).Group1.Remove(db.Group.Find(Group1));
            db.SaveChanges();

            return RedirectToAction("Details", new { id = contact_id });
        }

        [HttpPost]
        public ActionResult AddToGroup(int Group1, int contact_id)
        {
            db.Contact.Find(contact_id).Group1.Add(db.Group.Find(Group1));
            db.SaveChanges();

            return RedirectToAction("Details", new { id = contact_id });
        }

        [HttpPost]
        public ActionResult Index(int MembersOrCompanies)
        {
            db = new Models.DFUNK();

            var contact = db.Contact.Include(c => c.CompanyInfo).Include(c => c.VolunteerInfo).Where(x => x.company == false);

            if (MembersOrCompanies == 2)
                contact = db.Contact.Include(c => c.CompanyInfo).Include(c => c.VolunteerInfo).Where(x => x.company == true);

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
        public ActionResult Create([Bind(Include = "contact_id,name,surname,dateOfBirth,street,zipCode,city,phoneNr,email,company,member,stakeholder,volunteer,internalEmployee")] Contact contact, [Bind(Include = "tshirtSize,vegetarian,drivingLicense")] VolunteerInfo volunteerInfo, [Bind(Include = "contactPerson,role,email,phone")] CompanyInfo companyInfo)
        {
            if (ModelState.IsValid)
            {
                contact.VolunteerInfo = volunteerInfo;
                contact.CompanyInfo = companyInfo;
                contact.registerDate = DateTime.Now;
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
        public ActionResult Edit([Bind(Include = "contact_id,name,surname,dateOfBirth,street,zipCode,city,phoneNr,email,company,registerDate,member,stakeholder,volunteer,internalEmployee")] Contact contact, [Bind(Include = "tshirtSize,vegetarian,drivingLicense")] VolunteerInfo volunteerInfo, [Bind(Include = "contactPerson,role,email,phone")] CompanyInfo companyInfo)
        {
            if (ModelState.IsValid)
            {
                if (contact.volunteer)
                {
                    volunteerInfo.contact_id = contact.contact_id;
                    db.Entry(volunteerInfo).State = EntityState.Modified;
                }
                if (contact.company)
                {
                    companyInfo.contact_id = contact.contact_id;
                    db.Entry(companyInfo).State = EntityState.Modified;
                }              
                
                //contact.VolunteerInfo = volunteerInfo;
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
