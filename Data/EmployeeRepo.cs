using System;
using System.Collections.Generic;
using System.Linq;
using TAApi.Models;

namespace TAApi.Data
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly TADBContext _context;

        public EmployeeRepo(TADBContext context)
        {
            _context = context;
        }

        public void CreateEmployee(Employee employee)
        {
            if(employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            
            _context.Employees.Add(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            if(employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            _context.Employees.Remove(employee);
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _context.Employees.ToList();
        }

        public Employee GetEmployeeById(int id)
        {
            return _context.Employees.FirstOrDefault(d=> d.Id == id);
        }

        public bool SaveChanges()
        {
           return (_context.SaveChanges() >= 0);
        }

        public void UpdateEmployee(Employee employee)
        {
            //Nothing
        }
    }
}