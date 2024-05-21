using ZPP_2.Models;
using ZPP_2.Data;

namespace ZPP_2.Repository
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;
        public TodoRepository(TodoDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Todo> GetAll()
        {
            return _context.Todos.ToList();
        }
        public Todo GetById(int id)
        {
            return _context.Todos.Find(id);
        }
        public void Insert(Todo todo)
        {
            _context.Todos.Add(todo);
        }
        public void Update(Todo todo)
        {
            _context.Entry(todo).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Delete(int id)
        {
            Todo todo = _context.Todos.Find(id);
            if(todo != null)
            {
                _context.Todos.Remove(todo);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
