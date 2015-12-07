using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jitter.Controllers
{
    public class JitterController : Controller
    {
        // GET: Jitter
        // Maybe the Public feed here?
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult TopFavs()
        {
            return View();
        }

        [Authorize]
        public ActionResult UserFeed()
        {
            return View();
        }

        // GET: Jitter/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Jitter/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Jitter/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Jitter/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Jitter/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Jitter/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Jitter/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
