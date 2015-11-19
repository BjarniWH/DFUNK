using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DFUNK.Models;
using PagedList;

namespace DFUNK.Controllers
{
    public class ContactsController : Controller
    {
        private Models.DFUNK db = new Models.DFUNK();
        

        // GET: Contacts
        //public ActionResult Index()
        //{
        //    var contact = db.Contact.Include(c => c.CompanyInfo).Include(c => c.VolunteerInfo).Where(x => x.company == false);
        //    return View(contact.ToList());
        //}

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.LastNameSortParm = sortOrder == "surName" ? "surName_desc" : "surName";
            //var contacts = from c in db.Contact
            //               select c;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var contacts = db.Contact.Include(c => c.CompanyInfo).Include(c => c.VolunteerInfo);

            if (!String.IsNullOrEmpty(searchString))
            {
                //var searchStrings = searchString.Split(' ');

                //var searchcontacts = contacts.Where(c => c.email.Equals("Nothing"));

                //foreach(string word in searchStrings)
                //{
                //    searchcontacts = searchcontacts.Concat(contacts.Where(s => s.name.Contains(searchString)
                //                       || s.surname.Contains(searchString)));
                //}

                //contacts = searchcontacts;

                contacts = contacts.Where(c => (c.name.ToString().TrimEnd() + " " + c.surname.ToString().TrimEnd()).Contains(searchString));

                //contacts = contacts.Where(s => s.name.Contains(searchString)
                //                       || s.surname.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    contacts = contacts.OrderByDescending(s => s.name);
                    break;
                case "surName":
                    contacts = contacts.OrderBy(s => s.surname);
                    break;
                case "surName_desc":
                    contacts = contacts.OrderByDescending(s => s.surname);
                    break;
                default:
                    contacts = contacts.OrderBy(s => s.name);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(contacts.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult RemoveFromGroup(int Group1, int contact_id)
        {
            db.Contact.Find(contact_id).Group1.Remove(db.Group.Find(Group1));
            db.SaveChanges();

            return RedirectToAction("Details", new { id = contact_id });
        }

        [HttpPost]
        public ActionResult RemoveFromEvent(int Events1, int contact_id)
        {
            db.Contact.Find(contact_id).Events1.Remove(db.Events.Find(Events1));
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

        public ActionResult AddToEvent(int Events1, int contact_id)
        {
            db.Contact.Find(contact_id).Events1.Add(db.Events.Find(Events1));
            db.SaveChanges();

            return RedirectToAction("Details", new { id = contact_id });
        }

        //[HttpPost]
        //public ActionResult Index(int MembersOrCompanies)
        //{
        //    db = new Models.DFUNK();

        //    var contact = db.Contact.Include(c => c.CompanyInfo).Include(c => c.VolunteerInfo).Where(x => x.company == false);

        //    if (MembersOrCompanies == 2)
        //        contact = db.Contact.Include(c => c.CompanyInfo).Include(c => c.VolunteerInfo).Where(x => x.company == true);

        //    return View(contact.ToList());
        //}

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
                //if (contact.volunteer)
                //{
                //    volunteerInfo.contact_id = contact.contact_id;
                //    if (db.VolunteerInfo.Find(contact.contact_id) != null)
                //        db.Entry(volunteerInfo).State = EntityState.Modified;
                //    else
                //        db.VolunteerInfo.Add(volunteerInfo);
                //}
                //if (contact.company)
                //{
                //    companyInfo.contact_id = contact.contact_id;
                //    if (db.CompanyInfo.Find(contact.contact_id) != null)
                //        db.Entry(companyInfo).State = EntityState.Modified;
                //    else
                //        db.CompanyInfo.Add(companyInfo);                    
                //}              
                
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
            //if (db.VolunteerInfo.Find(id) != null)
            //    db.VolunteerInfo.Remove(db.VolunteerInfo.Find(id));
            //if (db.CompanyInfo.Find(id) != null)
            //    db.CompanyInfo.Remove(db.CompanyInfo.Find(id));
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
