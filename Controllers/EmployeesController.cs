using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TAApi.Data;
using TAApi.Dtos;
using TAApi.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace TAApi.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeRepo _repository;
        private readonly IMapper _mapper;

        public EmployeesController(IEmployeeRepo employeeRepo, IMapper mapper)
        {
            _repository = employeeRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult <IEnumerable<EmployeeReadDto>> GetAllEmployees()
        {
            IEnumerable<Employee> employees = _repository.GetAllEmployees();
            return Ok(_mapper.Map<IEnumerable<EmployeeReadDto>>(employees));
        }

        [HttpGet("{id}", Name="GetEmployeeByID")]
        public ActionResult<EmployeeReadDto> GetEmployeeById(int id)
        {
            Employee employee = _repository.GetEmployeeById(id);
            if(employee != null)
            {
                return Ok(_mapper.Map<EmployeeReadDto>(employee));
            }
             
            return NotFound();
        }

        [HttpPost]
        public ActionResult<EmployeeReadDto> CreateEmployee(EmployeeCreateDto employeeCreateDto)
        {
            var employeeModel = _mapper.Map<Employee>(employeeCreateDto);
            employeeModel.CreateDateTime = DateTime.Now;
            _repository.CreateEmployee(employeeModel);
            _repository.SaveChanges();

            var employeeReadDto = _mapper.Map<EmployeeReadDto>(employeeModel);
            return CreatedAtRoute(nameof(GetEmployeeById), new {ID = employeeReadDto.Id}, employeeReadDto);        
        }

        [HttpPut("{id}")]
        public ActionResult<EmployeeReadDto> UpdateEmployee(int id, EmployeeUpdateDto employeeUpdateDto)
        {
            var employeeModelFromRepo = _repository.GetEmployeeById(id);
            if(employeeModelFromRepo == null)
            {
                return NotFound();
            }

            employeeModelFromRepo.UpdateDateTime = DateTime.Now;
            _mapper.Map(employeeUpdateDto, employeeModelFromRepo);

            _repository.UpdateEmployee(employeeModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<EmployeeUpdateDto> patchDoc)
        {
            var employeeModelFromRepo = _repository.GetEmployeeById(id);
            if(employeeModelFromRepo == null)
            {
                return NotFound();
            }

            var employeeToPatch = _mapper.Map<EmployeeUpdateDto>(employeeModelFromRepo);
            patchDoc.ApplyTo(employeeToPatch, ModelState);

            if(!TryValidateModel(employeeToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(employeeToPatch, employeeModelFromRepo);

             _repository.UpdateEmployee(employeeModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteEmployee(int id)
        {
            var employeeModelFromRepo = _repository.GetEmployeeById(id);
            if(employeeModelFromRepo == null)
            {
                return NotFound();
            }

            _repository.DeleteEmployee(employeeModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}