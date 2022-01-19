using graduationProject.Data;
using graduationProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace graduationProject.Repositories
{
    public class HospitalDbRepository : InterfaceRepository<Hospital>
    {
        ApplicationDbContext db;
        public HospitalDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }
        public void Add(Hospital entity)
        {
            db.Hospitals.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var hospital = Find(id);
            db.Hospitals.Remove(hospital);
            db.SaveChanges();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public Hospital Find(int id)
        {
            //هستخدم انكلود عشان اجيب الجدولين مع بعض
            var hospital = db.Hospitals.Include(d => d.Department).SingleOrDefault(h => h.Hos_Id == id);
            return hospital;
        }

        public IList<Hospital> List()
        {
            return db.Hospitals.Include(d => d.Department).Include(u => u.User).AsNoTracking().ToList();
        }

        public void Update(int id, Hospital newentity)
        {
            db.Update(newentity);
            db.SaveChanges();
        }
    }
}
