﻿using Microsoft.AspNetCore.Mvc;
using zpp_aplikacja.Pages.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace YourProjectName.Controllers
{
    public class ChildController : Controller
    {
        private string UsersFilePath = "Data/users.json";
        private string TasksFilePath = "Data/tasks.json";

        public IActionResult Index()
        {
            // Sprawdź, czy użytkownik jest zalogowany i czy ma rolę dziecka
            if (HttpContext.Session.GetString("UserRole") != UserRole.Child.ToString())
            {
                return RedirectToAction("Index", "Parent");
            }

            // Pobierz dane użytkownika z sesji
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = GetUserById(userId.Value);

            // Pobierz zadania z pliku JSON
            var yourClassInstance = new Tasks();
            var tasks = yourClassInstance.GetTaskById();

            // Przekaż dane do widoku
            ViewBag.Points = user.Points;
            return View(tasks);
        }
        private List<Task> GetTasksFromJson()
        {
            if (System.IO.File.Exists(TasksFilePath))
            {
                string jsonString = System.IO.File.ReadAllText(TasksFilePath);
                return JsonSerializer.Deserialize<List<Task>>(jsonString);
            }
            else
            {
                return new List<Task>();
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        private List<User> GetUsersFromJson()
        {
            if (System.IO.File.Exists(UsersFilePath))
            {
                string jsonString = System.IO.File.ReadAllText(UsersFilePath);
                return JsonSerializer.Deserialize<List<User>>(jsonString);
            }
            else
            {
                return new List<User>();
            }
        }

        private void AddUserToJson(User user)
        {
            var users = GetUsersFromJson();
            users.Add(user);
            SaveUsersToJson(users);
        }

        private void SaveUsersToJson(List<User> users)
        {
            string jsonString = JsonSerializer.Serialize(users);
            System.IO.File.WriteAllText(UsersFilePath, jsonString);
        }

        private User GetUserById(int userId)
        {
            var users = GetUsersFromJson();
            return users.FirstOrDefault(u => u.Id == userId);
        }

        private void UpdateUsersFromJson(User user)
        {
            var users = GetUsersFromJson();
            var existingUser = users.FirstOrDefault(u => u.Id == user.Id);

            if (existingUser != null)
            {
                // Zaktualizuj dane użytkownika
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Email = user.Email;

                SaveUsersToJson(users);
            }
        }
        private void DeleteUserFromJson(int userId)
        {
            var users = GetUsersFromJson();
            var userToRemove = users.FirstOrDefault(u => u.Id == userId);

            if (userToRemove != null)
            {
                users.Remove(userToRemove);
                SaveUsersToJson(users);
            }
        }
    }
}
