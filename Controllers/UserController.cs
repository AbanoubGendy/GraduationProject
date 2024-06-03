using Graduation.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Graduation.DAL.Models;
using AutoMapper;
using Graduation.PL.ViewModels;
using Graduation.PL.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Graduation.PL.Controllers
{
	public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepositry _employeeRepo;
        private readonly IMapper _mapper;

        public UserController(IUnitOfWork unitOfWork
            /*IEmployeeRepositry employee*/
            , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            //_employeeRepo = employee;
            _mapper = mapper;
        }

        public IActionResult Index(string SearchByName)
        {
            var Employees = Enumerable.Empty<User>();
            if (string.IsNullOrEmpty(SearchByName))
            {
                Employees = _unitOfWork.UserRepository.GetAll();

            }
            else
            {
                Employees = _unitOfWork.UserRepository.GetUserByName(SearchByName.ToLower());

            }
            var Employeesvm = _mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(Employees);
            return View(Employeesvm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            //ViewData["GetAllDept"] = _departmentRepo.GetAll();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel uservm)
        {
            if (ModelState.IsValid)
            {
                uservm.ImageName = await DocumentSettings.UplodeFile(uservm.Image, "Images");
                var Model = _mapper.Map<UserViewModel, User>(uservm);
                _unitOfWork.UserRepository.Add(Model);
                var roweffect = await _unitOfWork.CompleteAsync();

                //update Department
                //Delete Project 

                if (roweffect > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(uservm);
        }
        // /Employee/Details/10
        // /Employee/Details
        [HttpGet]
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest();
            var emp = _unitOfWork.UserRepository.Get(id.Value);
            var EmployeeVm = _mapper.Map<User, UserViewModel>(emp);
            if (emp is null)
                return NotFound();
            return View(ViewName, EmployeeVm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            ///if (!id.HasValue)
            ///    return BadRequest();
            ///var emp = _employeeRepo.GetById(id.Value);
            ///if (emp is null)
            ///    return NotFound();
            ///return View(emp);
            ///


            return  Details(id, "Update");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserViewModel uservm)
        {
            if (ModelState.IsValid)
            {
                var currentName = uservm.ImageName;

                if (uservm.Image is not null)
                    uservm.ImageName = await DocumentSettings.UplodeFile(uservm.Image, "Images");

                if (currentName is not null && currentName != uservm.ImageName)
                    DocumentSettings.DeleteFile(currentName, "Images");

                var Model = _mapper.Map<UserViewModel, User>(uservm);
                _unitOfWork.UserRepository.Update(Model);
                var roweffect = await _unitOfWork.CompleteAsync();
                if (roweffect > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(uservm);

        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            ///if (!id.HasValue)
            ///    return BadRequest();
            ///var emp = _employeeRepo.GetById(id.Value);
            ///if (emp is null)
            ///    return NotFound();
            ///return View(emp);
            ///

            return Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserViewModel uservm)
        {
            var roweffect = 0;
            if (uservm.ImageName is not null)
                DocumentSettings.DeleteFile(uservm.ImageName, "Images");
            var Model = _mapper.Map<UserViewModel, User>(uservm);
            try
            {
                _unitOfWork.UserRepository.Delete(Model);
                roweffect = await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(uservm);
            }


            return roweffect > 0 ? RedirectToAction(nameof(Index)) /*true*/: /*Else*/ RedirectToAction(nameof(Index));
        }

        public IActionResult Images()
        {
            var Employees = Enumerable.Empty<User>();
            
            
                Employees = _unitOfWork.UserRepository.GetAll();

            var Employeesvm = _mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(Employees);
            return View(Employeesvm);
        }
    }
}
