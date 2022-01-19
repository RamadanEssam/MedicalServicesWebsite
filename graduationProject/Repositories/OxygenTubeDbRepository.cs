using graduationProject.Data;
using graduationProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace graduationProject.Repositories
{
    public class OxygenTubeDbRepository : InterfaceRepository<OxygenTube>
    {
        ApplicationDbContext db;
        public OxygenTubeDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }
        public void Add(OxygenTube entity)
        {
            db.OxygenTubes.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var oxygenTube = Find(id);
            db.OxygenTubes.Remove(oxygenTube);
            db.SaveChanges();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public OxygenTube Find(int id)
        {
            //هستخدم انكلود عشان اجيب الجدولين مع بعض
            var oxygenTube = db.OxygenTubes.Include(d => d.Department).SingleOrDefault(h => h.OxgnId == id);
            return oxygenTube;
        }

        public IList<OxygenTube> List()
        {
            return db.OxygenTubes.Include(d => d.Department).Include(u => u.User).AsNoTracking().ToList();
        }

        public void Update(int id, OxygenTube newentity)
        {
            db.Update(newentity);
            db.SaveChanges();
        }
    }
}
