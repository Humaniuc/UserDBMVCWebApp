using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Week14AMVCWebApp.Models;
using AppLogic;
using Data;

namespace Week14AMVCWebApp.Controllers
{
    
    public class UserController : Controller
    {
        private readonly IUserRepository _repo;
        private readonly SqlConnection connection = Data.Connection.GetConnection();

        public UserController()
        {
            //UserRepository userRepo = new UserRepository(connection);

            _repo = new UserRepository(connection);
        }
        public ActionResult Index()
        {
            var allUsers = _repo.GetAll();
            var userListModels = allUsers.Select(x => new Models.User()
            {
                Id = x.Id,
                Email = x.Email,
                Username = x.Username
            });
            return View(userListModels);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(AppLogic.User user)
        {
            if(!ModelState.IsValid)
            {
                return View("Add", user);
            }
            else
            {
                user.Id = _repo.GetAll().Count + 1;
                const string query = @"insert into Users (Username, Email, Description, City, Street) values 
                                        (@username, @email, @description, @city, @street); select cast(scope_identity() as int);";
                SqlParameter userFullName = new SqlParameter("@username", System.Data.DbType.String)
                {
                    Value = user.Username
                };
                SqlParameter userEmail = new SqlParameter("@email", System.Data.DbType.String)
                {
                    Value = user.Email
                };
                SqlParameter userDescr = new SqlParameter("@description", System.Data.DbType.String)
                {
                    Value = user.Description
                };
                SqlParameter userCity = new SqlParameter("@city", System.Data.DbType.String)
                {
                    Value = user.City
                };
                SqlParameter userStreet = new SqlParameter("@street", System.Data.DbType.String)
                {
                    Value = user.Street
                };

                var command = new SqlCommand
                {
                    CommandText = query,
                    Connection = connection
                };
                command.Parameters.Add(userFullName);
                command.Parameters.Add(userEmail);
                command.Parameters.Add(userDescr);
                command.Parameters.Add(userCity);
                command.Parameters.Add(userStreet);

                command.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }


    }
}