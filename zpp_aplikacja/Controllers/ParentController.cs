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
        private string RewardsFilePath = "Data/rewards.json";
        private string TasksFilePath = "Data/tasks.json";



        public IActionResult Index()
        {
            // Sprawdź, czy użytkownik jest zalogowany i czy ma rolę rodzica
            if (HttpContext.Session.GetString("UserRole") != UserRole.Parent.ToString())
            {
                return RedirectToAction("Index", "Child");
            }

            // Pobierz użytkowników z pliku JSON
            var users = GetUsersFromJson();

            var yourClassInstanceR = new Reward();
            var rewards = yourClassInstanceR.GetRewardById();

            var yourClassInstance = new Tasks();
            var tasks = yourClassInstance.GetTaskById();

            // Przekaż dane do widoku
            return View(users);
        }



        public IActionResult CreateUsers(User user)
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
        private List<Tasks> GetTasksFromJson()
        {
            if (System.IO.File.Exists(TasksFilePath))
            {
                string jsonString = System.IO.File.ReadAllText(TasksFilePath);
                return JsonSerializer.Deserialize<List<Tasks>>(jsonString);
            }
            else
            {
                return new List<Tasks>();
            }
        }
        private List<Reward> GetRewardsFromJson()
        {
            if (System.IO.File.Exists(RewardsFilePath))
            {
                string jsonString = System.IO.File.ReadAllText(RewardsFilePath);
                return JsonSerializer.Deserialize<List<Reward>>(jsonString);
            }
            else
            {
                return new List<Reward>();
            }
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
       
        public IActionResult EditUsers(User user)
        {
            if (ModelState.IsValid)
            {
                // Dodaj nowego użytkownika do pliku JSON
                UpdateUsersFromJson(user);

                // Przekieruj do listy użytkowników
                return RedirectToAction("Index");
            }

            return View(user);
        }
        public IActionResult DeleteUsers(User user)
        {
            if (ModelState.IsValid)
            {
                // Dodaj nowego użytkownika do pliku JSON
               DeleteUserFromJson(user.Id);

                // Przekieruj do listy użytkowników
                return RedirectToAction("Index");
            }

            return View(user);
        }


        public IActionResult Parent()
            {
                // tutaj logika zarządzania użytkownikami
                return View(GetUsersFromJson());
            }

      
        // Metoda do zapisu listy zadań do pliku JSON
        private void SaveTasksToJson(List<Tasks> tasks)
        {
            string jsonString = JsonSerializer.Serialize(tasks);
            System.IO.File.WriteAllText(TasksFilePath, jsonString);
        }

        // Metoda do pobrania zadania według ID
        private Tasks GetTaskById(int taskId)
        {
            var tasks = GetTasksFromJson();
            return tasks.FirstOrDefault(t => t.Id == taskId);
        }

        // Metoda do sprawdzenia, czy zadanie istnieje
        private bool TaskExists(int taskId)
        {
            return GetTasksFromJson().Any(t => t.Id == taskId);
        }

        // Metoda do dodania nowego zadania
        private void AddTaskToJson(Tasks task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task), "Task cannot be null");

            var tasks = GetTasksFromJson();
            tasks.Add(task);
            SaveTasksToJson(tasks);
        }

        // Metoda do aktualizacji zadania
        private void UpdateTaskInJson(Tasks task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task), "Task cannot be null");

            var tasks = GetTasksFromJson();
            var existingTask = tasks.FirstOrDefault(t => t.Id == task.Id);

            if (existingTask != null)
            {
                if (existingTask.Name != task.Name || existingTask.Description != task.Description)
                {
                    existingTask.Name = task.Name;
                    existingTask.Description = task.Description;
                    SaveTasksToJson(tasks);
                }
            }
            else
            {
                throw new InvalidOperationException($"Task with Id {task.Id} does not exist.");
            }
        }

        // Metoda do usunięcia zadania
        private void DeleteTaskFromJson(int taskId)
        {
            var tasks = GetTasksFromJson();
            var taskToRemove = tasks.FirstOrDefault(t => t.Id == taskId);

            if (taskToRemove != null)
            {
                tasks.Remove(taskToRemove);
                SaveTasksToJson(tasks);
            }
            else
            {
                throw new InvalidOperationException($"Task with Id {taskId} does not exist.");
            }
        }
        public IActionResult Tasks()
        {
            // tutaj logika zarządzania zadaniami
            return View(GetTasksFromJson());
        }

        public IActionResult CreateTasks(Tasks tasks)
        {
            if (ModelState.IsValid)
            {
                // Dodaj nowego użytkownika do pliku JSON
                AddTaskToJson(tasks);

                // Przekieruj do listy użytkowników
                return RedirectToAction("Tasks");
            }

            // Wyświetl ponownie formularz z błędami
            return View(tasks);
        }

        public IActionResult EditTasks(Tasks tasks)
        {
            if (ModelState.IsValid)
            {
                // Dodaj nowego użytkownika do pliku JSON
                UpdateTaskInJson(tasks);

                // Przekieruj do listy użytkowników
                return RedirectToAction("Tasks");
            }

            return View(tasks);
        }
        public IActionResult DeleteTasks(Tasks tasks)
        {
            if (ModelState.IsValid)
            {
                // Dodaj nowego użytkownika do pliku JSON
                DeleteTaskFromJson(tasks.Id);

                // Przekieruj do listy użytkowników
                return RedirectToAction("Tasks");
            }

            return View(tasks);
        }
        // Metoda do zapisu listy nagród do pliku JSON
        private void SaveRewardsToJson(List<Reward> rewards)
        {
            string jsonString = JsonSerializer.Serialize(rewards);
            System.IO.File.WriteAllText(RewardsFilePath, jsonString);
        }

        // Metoda do pobrania nagrody według ID
        private Reward GetRewardById(int rewardId)
        {
            var rewards = GetRewardsFromJson();
            return rewards.FirstOrDefault(r => r.RewardId == rewardId);
        }

        // Metoda do sprawdzenia, czy nagroda istnieje
        private bool RewardExists(int rewardId)
        {
            return GetRewardsFromJson().Any(r => r.RewardId == rewardId);
        }

        // Metoda do dodania nowej nagrody
        private void AddRewardToJson(Reward reward)
        {
            if (reward == null) throw new ArgumentNullException(nameof(reward), "Reward cannot be null");

            var rewards = GetRewardsFromJson();
            rewards.Add(reward);
            SaveRewardsToJson(rewards);
        }

        // Metoda do aktualizacji nagrody
        private void UpdateRewardInJson(Reward reward)
        {
            if (reward == null) throw new ArgumentNullException(nameof(reward), "Reward cannot be null");

            var rewards = GetRewardsFromJson();
            var existingReward = rewards.FirstOrDefault(r => r.RewardId == reward.RewardId);

            if (existingReward != null)
            {
                if (existingReward.Name != reward.Name || existingReward.Description != reward.Description)
                {
                    existingReward.Name = reward.Name;
                    existingReward.Description = reward.Description;
                    SaveRewardsToJson(rewards);
                }
            }
            else
            {
                throw new InvalidOperationException($"Reward with Id {reward.RewardId} does not exist.");
            }
        }

        // Metoda do usunięcia nagrody
        private void DeleteRewardFromJson(int rewardId)
        {
            var rewards = GetRewardsFromJson();
            var rewardToRemove = rewards.FirstOrDefault(r => r.RewardId == rewardId);

            if (rewardToRemove != null)
            {
                rewards.Remove(rewardToRemove);
                SaveRewardsToJson(rewards);
            }
            else
            {
                throw new InvalidOperationException($"Reward with Id {rewardId} does not exist.");
            }
        }

        public IActionResult Rewards()
        {
            // tutaj logika zarządzania nagrodami
            return View(GetRewardsFromJson());
        }

        public IActionResult CreateRewards(Reward rewards)
        {
            if (ModelState.IsValid)
            {
                // Dodaj nową nagrodę do pliku JSON
                AddRewardToJson(rewards);

                // Przekieruj do listy nagród
                return RedirectToAction("Rewards");
            }

            // Wyświetl ponownie formularz z błędami
            return View(rewards);
        }

        public IActionResult EditRewards(Reward rewards)
        {
            if (ModelState.IsValid)
            {
                // Zaktualizuj nagrodę w pliku JSON
                UpdateRewardInJson(rewards);

                // Przekieruj do listy nagród
                return RedirectToAction("Rewards");
            }

            return View(rewards);
        }

        public IActionResult DeleteRewards(Reward rewards)
        {
            if (ModelState.IsValid)
            {
                // Usuń nagrodę z pliku JSON
                DeleteRewardFromJson(rewards.RewardId);

                // Przekieruj do listy nagród
                return RedirectToAction("Rewards");
            }

            return View(rewards);
        }

    }
}
