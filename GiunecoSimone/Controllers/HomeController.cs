using GiunecoSimone.Class;
using GiunecoSimone.Enumerator;
using GiunecoSimone.Interfaces;
using GiunecoSimone.Models;
using GiunecoSimone.Models.Extend;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace GiunecoSimone.Controllers
{
    public class HomeController : Controller
    {
        private static List<Users> useredit;

        [HttpGet]
        public ActionResult Home(int? id, int? choose)
        {
            if (choose != null)
                ViewBag.Choose = choose;
            else
                ViewBag.Choose = 0;

            State state;
            if (id != null)
            {
                state = (State)id;
            }
            else
            {
                state = (State)2;
            }
            Session["stato"] = state.GetDescription();
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

        [HttpGet]
        public ActionResult Delete(string id)
        {
            var status = false;
            var message = string.Empty;

            if (Session["UserId"] != null)
            {
                ITasks service;
                service = new ServiceTasks();
                var result = service.DeleteTask(id);
                if (result)
                {
                    status = true;
                    message = "Task cancellato!";
                }
                else
                    message = "Errore nella cancellazione del task";

                ViewBag.Message = message;
                ViewBag.Status = status;

                return RedirectToAction("Home", "Home");
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult EditTask(string id, string idUserTasks)
        {
            var userTask = new EditTaskMetaData();
            if (Session["UserId"] != null)
            {
                if (id != null)
                {
                    useredit = new List<Users>();
                    ITasks service;
                    service = new ServiceTasks();
                    var task = service.GetTasks(id);
                    Session["TaskId"] = id;
                    //Session["UserTaskId"] = idUserTasks;
                    ViewBag.TitleTask = task.Title;
                    userTask.Description = task.Description;
                    userTask.Date = task.Date;
                    State state = (State)task.State;
                    userTask.State = state;
                    ViewBag.State = state;
                    userTask.newComment = string.Empty;
                    userTask.TotalWorkedHour = service.GetTotalWorkedHour(id);
                    userTask.Comments = service.GetComments(id);
                    ViewBag.Comments = new List<Comments>();
                    ViewBag.Comments = userTask.Comments;
                    userTask.Users = service.GetUsersForTask(id);
                    ViewBag.Users = new List<Users>();
                    ViewBag.Users = userTask.Users;
                    userTask.UsersAvailable = service.GetUsers(id);
                    ViewBag.UsersAvailable = new List<Users>();
                    ViewBag.UsersAvailable = userTask.UsersAvailable;
                    return View(userTask);
                }
                return View(userTask);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult EditTask(EditTaskMetaData editTaskMetaData)
        {
            var message = string.Empty;
            var status = false;

            ITasks service;
            service = new ServiceTasks();
            if (Session["UserId"] != null && Session["TaskId"] != null)
            {
                try
                {
                    var tasks = new Tasks();
                    var users = new List<Users>();
                    var usertasks = new List<UsersTasks>();
                    Guid idTask = Guid.Parse(Session["TaskId"].ToString());
                    Guid idUser = Guid.Parse(Session["UserId"].ToString());
                    editTaskMetaData.Users = new List<Users>();
                    editTaskMetaData.Users = useredit;

                    using (giuneco_Entities ue = new giuneco_Entities())
                    {
                        tasks = ue.Tasks.Where(x => x.Id.Equals(idTask)).FirstOrDefault();

                        usertasks = ue.UsersTasks.Where(x => x.IdTask.Equals(idTask)).ToList();
                        var useTasksInsert = new UsersTasks();
                        var description = ue.Tasks.Where(x => x.Id.Equals(idTask)).FirstOrDefault().Description = editTaskMetaData.Description;
                        var state = ue.Tasks.Where(x => x.Id.Equals(idTask)).FirstOrDefault().State = (int)editTaskMetaData.State;

                        tasks.Id = idTask;
                        tasks.Description = editTaskMetaData.Description;
                        tasks.State = (int)editTaskMetaData.State;
                        


                        if (!string.IsNullOrEmpty(editTaskMetaData.newComment))
                        {
                            useTasksInsert.Comment = editTaskMetaData.newComment;
                            useTasksInsert.CommentDate = DateTime.Now;
                        }
                        useTasksInsert.WorkedHour = editTaskMetaData.WorkedHour;
                        useTasksInsert.Id = Guid.NewGuid();
                        useTasksInsert.IdTask = idTask;
                        useTasksInsert.IdUser = idUser;
                        ue.UsersTasks.Add(useTasksInsert);
                        ue.SaveChanges();

                        foreach(var adduser in useredit)
                        {
                            if (!idUser.Equals(adduser.Id))
                            {
                                var usertasksAdd = new UsersTasks();
                                usertasksAdd.IdUser = adduser.Id;
                                usertasksAdd.IdTask = idTask;
                                usertasksAdd.Comment = null;
                                usertasksAdd.CommentDate = null;
                                usertasksAdd.WorkedHour = 0;
                                usertasksAdd.Id = Guid.NewGuid();
                                ue.UsersTasks.Add(usertasksAdd);
                                ue.SaveChanges();
                            }
                        }
                        Session["TaskId"] = null;

                        message = "Aggiornamento completato!";
                        status = true;
                    }
                    ViewBag.Message = message;
                    ViewBag.Status = status;
                    return RedirectToAction("Home", "Home");
                }
                catch(Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    ViewBag.Status = status;
                    return RedirectToAction("Home", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public void EditTaskUser(List<string> users)
        {
            ITasks service;
            service = new ServiceTasks();
            if (Session["UserId"] != null)
            {
                try
                {
                    if (users != null)
                    {
                        useredit = new List<Users>();
                        foreach (var edit in users)
                        {
                            var email = edit.Split(new string[] { "(" }, StringSplitOptions.None)[1].Split(new string[] { ")" }, StringSplitOptions.None)[0];
                            var us = service.GetUser(email);
                            useredit.Add(us);
                        }
                    }
                }
                catch 
                {
                }
            }
        }
    }
}