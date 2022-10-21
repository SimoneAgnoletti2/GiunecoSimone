﻿using GiunecoSimone.Class;
using GiunecoSimone.Enumerator;
using GiunecoSimone.Interfaces;
using GiunecoSimone.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;

namespace GiunecoSimone.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Home(int? id)
        {
            State state;
            if(id != null)
            {
                state = (State)id;
            }
            else
            {
                state = (State)2;
            }
            ViewBag.Stato = state;
            if (Session["UserId"] != null)
            {
                var idUser = Session["UserId"].ToString();
                ITasks service;
                service = new ServiceTasks();
                var progress = service.GetTasks(state, idUser);
                return View(progress);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult CreateTask()
        {
            if (Session["UserId"] != null)
            {
                return View(new TasksMetaData());
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult CreateTask(Tasks newTask)
        {
            if (Session["UserId"] != null)
            {
                var status = false;
                var message = string.Empty;
                try
                {
                    newTask.Id = Guid.NewGuid();
                    newTask.State = (int)State.Backlog;
                    newTask.Date = DateTime.Now;

                    UsersTasks ut = new UsersTasks();
                    ut.IdUser = Guid.Parse(Session["UserID"].ToString());
                    ut.IdTask = newTask.Id;
                    ut.WorkedHour = 0;

                    using (giuneco_Entities ue = new giuneco_Entities())
                    {
                        ue.Tasks.Add(newTask);

                        ue.UsersTasks.Add(ut);

                        ue.SaveChanges();

                        status = true;
                        message = "Attività registrata con successo";
                    }
                }
                catch (Exception ex)
                {
                    status = true;
                    message = $"Errore:  {ex.Message}";
                }

                ViewBag.Message = message;
                ViewBag.Status = status;

                return View();
            }
            return RedirectToAction("Login", "Account");
        }
    }
}