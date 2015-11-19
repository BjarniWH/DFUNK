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
    public class ProjectsController : Controller
    {
        private Models.DFUNK db = new Models.DFUNK();

        // GET: Projects
        public ActionResult Index()
        {
            var projects = db.Projects.Include(p => p.Contact);
            return View(projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        [HttpPost]
        public ActionResult RemoveGroup(int Group, int project_id)
        {
            db.Projects.Find(project_id).Group.Remove(db.Group.Find(Group));
            db.SaveChanges();

            return RedirectToAction("Details", new { id = project_id });
        }

        [HttpPost]
        public ActionResult AddGroup(int Group, int project_id)
        {
            db.Projects.Find(project_id).Group.Add(db.Group.Find(Group));
            db.SaveChanges();

            return RedirectToAction("Details", new { id = project_id });
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            ViewBag.leader = new SelectList(db.Contact, "contact_id", "name");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "project_id,name,budget,leader,contactNr,contactEmail")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(projects);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.leader = new SelectList(db.Contact, "contact_id", "name", projects.leader);
            return View(projects);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            ViewBag.leader = new SelectList(db.Contact, "contact_id", "name", projects.leader);
            return View(projects);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "project_id,name,budget,leader,contactNr,contactEmail")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projects).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.leader = new SelectList(db.Contact, "contact_id", "name", projects.leader);
            return View(projects);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Projects projects = db.Projects.Find(id);
            db.Projects.Remove(projects);
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
