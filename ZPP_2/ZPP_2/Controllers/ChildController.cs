using Microsoft.AspNetCore.Mvc;
using ZPP_2.Models;
using ZPP_2.Repository;

namespace ZPP_2.Controllers
{
    public class ChildController : Controller
    {
        private readonly ITodoRepository _todoRepository;

        public ChildController(ITodoRepository todoRepository)
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
                return RedirectToAction("Index", "Child");
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
                return RedirectToAction("Index", "Child");
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
            return RedirectToAction("Index", "Child");
        }
    }
}
