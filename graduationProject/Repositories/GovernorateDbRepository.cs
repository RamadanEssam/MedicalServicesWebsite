using graduationProject.Data;
using graduationProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace graduationProject.Repositories
{
    public class GovernorateDbRepository : InterfaceRepository<Governorate>
    {
        ApplicationDbContext db;
        public GovernorateDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }
        public void Add(Governorate entity)
        {
            db.Governorates.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var governorate = Find(id);
            db.Governorates.Remove(governorate);
            db.SaveChanges();
        }

        public Governorate Find(int id)
        {
            var governorate = db.Governorates.SingleOrDefault(g => g.Gov_Id == id);
            return governorate;
        }

        public IList<Governorate> List()
        {
            return db.Governorates.AsNoTracking().ToList();
        }

        public void Update(int id, Governorate newentity)
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
