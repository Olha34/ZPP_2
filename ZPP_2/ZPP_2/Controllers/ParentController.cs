using Microsoft.AspNetCore.Mvc;
using zpp_aplikacja.Pages.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace zpp_aplikacja.Pages.Controllers
{
    public class ParentController : Controller
    {
        private string UsersFilePath = "Data/users.json";

        public IActionResult Index()
        {
            // Sprawdź, czy użytkownik jest zalogowany i czy ma rolę rodzica
            if (HttpContext.Session.GetString("UserRole") != UserRole.Parent.ToString())
            {
                return RedirectToAction("Index", "Child");
            }

            // Pobierz użytkowników z pliku JSON
            var users = GetUsersFromJson();

            // Przekaż dane do widoku
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            // Walidacja danych
            if (ModelState.IsValid)
            {
                // Dodaj nowego użytkownika do pliku JSON
                AddUserToJson(user);

                // Przekieruj do listy użytkowników
                return RedirectToAction("Index");
            }

            // Wyświetl ponownie formularz z błędami
            return View(user);
        }

        // ... inne metody

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
