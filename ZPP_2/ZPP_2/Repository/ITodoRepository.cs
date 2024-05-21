using Newtonsoft.Json.Bson;
using ZPP_2.Models;

namespace ZPP_2.Repository
{
    public interface ITodoRepository
    {
        IEnumerable<Todo> GetAll();
        Todo GetById(int id);
        void Insert(Todo item);
        void Update(Todo item);
        void Delete(int id);
        void Save();
    }
}
