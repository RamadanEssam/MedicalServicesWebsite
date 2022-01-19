using graduationProject.Data;
using graduationProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace graduationProject.Repositories
{
    public class DepartmentDbRepository : InterfaceRepository<Department>
    {
        ApplicationDbContext db;
        public DepartmentDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }
        public void Add(Department entity)
        {
            db.Departments.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var department = Find(id);
            db.Departments.Remove(department);
            db.SaveChanges();
        }

        public Department Find(int id)
        {
            //هستخدم انكلود عشان اجيب الجدولين مع بعض

            var department = db.Departments.Include(g => g.Governorate).SingleOrDefault(d => d.Dept_Id == id);
            return department;
        }

        public IList<Department> List()
        {
            //هستخدم انكلود عشان اجيب الجدولين مع بعض
            return db.Departments.Include(d => d.Governorate).AsNoTracking().ToList();
        }

        public void Update(int id, Department newentity)
        {
            db.Update(newentity);
            db.SaveChanges();
        }
        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
