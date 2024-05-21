using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using ZPP_2.Models;
using ZPP_2.Repository;

namespace ZPP_2.Controllers
{
    public class ParentController : Controller
    {
        private readonly ITodoRepository _todoRepository;

        public ParentController(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = _todoRepository.GetAll();
            return View(model);
        }

        [HttpGet]
        public IActionResult AddTodo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddTodo(Todo model) 
        {
            if (ModelState.IsValid)
            {
                _todoRepository.Insert(model);
                _todoRepository.Save();
                return RedirectToAction("Index", "Parent");
            }
            return View();
        }
        [HttpGet]
        public IActionResult EditTodo(int id)
        {
            Todo model = _todoRepository.GetById(id);
            return View(model);
        }
        [HttpPost]
        public IActionResult EditTodo(Todo model)
        {
            if (ModelState.IsValid)
            {
                _todoRepository.Update(model);
                _todoRepository.Save();
                return RedirectToAction("Index", "Parent");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult DeleteTodo(int id)
        {
            Todo model = _todoRepository.GetById(id);
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _todoRepository.Delete(id);
            _todoRepository.Save();
            return RedirectToAction("Index", "Parent");
        }
    }
}
