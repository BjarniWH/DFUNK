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
    public class EventsController : Controller
    {
        private Models.DFUNK db = new Models.DFUNK();

        // GET: Events
        public ActionResult Index()
        {
            var events = db.Events.Include(e => e.Contact);
            return View(events.ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }

            //db.Projects.ToList();

            return View(events);
        }

        [HttpPost]
        public ActionResult RemoveProject(int Projects, int event_id)
        {
            db.Events.Find(event_id).Projects.Remove(db.Projects.Find(Projects));
            db.SaveChanges();

            return RedirectToAction("Details", new { id = event_id });
        }

        [HttpPost]
        public ActionResult AddProject(int Projects, int event_id)
        {
            db.Events.Find(event_id).Projects.Add(db.Projects.Find(Projects));
            db.SaveChanges();

            return RedirectToAction("Details", new { id = event_id });
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            ViewBag.leader = new SelectList(db.Contact, "contact_id", "name");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "event_id,name,minPeople,maxPeople,address,leader,startDate,endDate")] Events events)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(events);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.leader = new SelectList(db.Contact, "contact_id", "name", events.leader);
            return View(events);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            ViewBag.leader = new SelectList(db.Contact, "contact_id", "name", events.leader);
            return View(events);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "event_id,name,minPeople,maxPeople,address,leader,startDate,endDate,startTime")] Events events)
        {
            if (ModelState.IsValid)
            {
                db.Entry(events).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.leader = new SelectList(db.Contact, "contact_id", "name", events.leader);
            return View(events);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            return View(events);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Events events = db.Events.Find(id);
            db.Events.Remove(events);
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
