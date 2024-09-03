using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using zpp_aplikacja.Pages.Models;

namespace zpp_aplikacja.Pages.Controllers
{
    public class TasksController : Controller
    {
        private readonly string _dataFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "tasks.json");

        // Metoda do pobrania listy zadań z pliku JSON
        private List<Tasks> GetTasksFromJson()
        {
            if (System.IO.File.Exists(_dataFilePath))
            {
                string jsonString = System.IO.File.ReadAllText(_dataFilePath);
                return JsonSerializer.Deserialize<List<Tasks>>(jsonString) ?? new List<Tasks>();
            }
            else
            {
                return new List<Tasks>();
            }
        }

        // Metoda do zapisu listy zadań do pliku JSON
        private void SaveTasksToJson(List<Tasks> tasks)
        {
            string jsonString = JsonSerializer.Serialize(tasks);
            System.IO.File.WriteAllText(_dataFilePath, jsonString);
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
    }
}
