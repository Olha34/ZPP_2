using Microsoft.CodeAnalysis.CSharp;
using System.Diagnostics.Metrics;
using ZPP_2.Models;

namespace ZPP_2.Data
{
    public class DbInitializer
    {
        public static void Initialize(TodoDbContext context)
        {
            if (context.Todos.Any())
            {
                return;
            }
            var todos = new Todo[]
            {
                new Todo { Id = 1, Name = "Sprzątanie pokoju", Description = "Wycieranie kurzy, zbieranie zabawek", Status = false, Points = 20},
                new Todo { Id = 2, Name = "Odrabianie lekcji", Description = "Odrabianie wszystkich bieżących zadań domowych", Status = false, Points = 10},
                new Todo { Id = 3, Name = "Wynoszenie smieci", Description = "Wyniesienie przynajmniej jednego worka z śmieciami, punkty za każdy worek", Status = false, Points = 5}
            };
            foreach (Todo t in todos)
            {
                context.Todos.Add(t);
            }
            context.SaveChanges();

        }
    }
}
