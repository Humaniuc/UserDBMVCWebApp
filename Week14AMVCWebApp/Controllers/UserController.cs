using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Week14AMVCWebApp.Models;
using AppLogic;
using Data;
using PagedList;

namespace Week14AMVCWebApp.Controllers
{
    
    public class UserController : Controller
    {
        private IUserRepository _repo;
        private readonly SqlConnection connection = Data.Connection.GetConnection();

        public UserController()
        {
            //UserRepository userRepo = new UserRepository(connection);

            _repo = new UserRepository(connection);
        }
        public ActionResult Index(int? page)
        {
            var allUsers = _repo.GetAll();
            var userListModels = allUsers.Select(x => new Models.User()
            {
                Id = x.Id,
                Email = x.Email,
                Username = x.Username
            });
            return View(userListModels.ToPagedList(page ?? 1, 10));
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(AppLogic.User user)
        {
            //user.Id = _repo.GetAll().Count + 1;
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

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(_repo.GetAll().Where(u => u.Id == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Edit(AppLogic.User user)
        { 
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            else
            {
                const string query = @"update Users set Username = @username, Email = @email, Description = @description, City = @city, Street = @street where Id = @id;";
                SqlParameter userId = new SqlParameter("@id", System.Data.SqlDbType.Int)
                {
                    Value = user.Id
                };
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
                command.Parameters.Add(userId);
                command.Parameters.Add(userFullName);
                command.Parameters.Add(userEmail);
                command.Parameters.Add(userDescr);
                command.Parameters.Add(userCity);
                command.Parameters.Add(userStreet);

                command.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                const string query = @"delete from users where Id = @id;";
                SqlParameter userId = new SqlParameter("@id", System.Data.SqlDbType.Int)
                {
                    Value = _repo.GetAll().Where(u => u.Id == id).FirstOrDefault().Id
                };
                
                var command = new SqlCommand
                {
                    CommandText = query,
                    Connection = connection
                };
                command.Parameters.Add(userId);
                
                command.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}