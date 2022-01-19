using graduationProject.Data;
using graduationProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace graduationProject.Repositories
{
    public class UserReserveDbRepository : InterfaceRepository<UserReservation>
    {
        ApplicationDbContext db;
        public UserReserveDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }
        public void Add(UserReservation entity)
        {
            db.UserReservations.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var userReserve = Find(id);
            db.UserReservations.Remove(userReserve);
            db.SaveChanges();
        }

        public void DeleteById(int id)
        {
            var userReserve = db.UserReservations.SingleOrDefault(g => g.Id == id);
            db.UserReservations.Remove(userReserve);
            db.SaveChanges();
        }

        public UserReservation Find(int id)
        {
            var userReserve = db.UserReservations.Include(h => h.Hospital).Include(u => u.User).SingleOrDefault(g => g.Hos_Id == id);
            return userReserve;
        }

        public IList<UserReservation> List()
        {
            var userReserve = db.UserReservations.Include(h => h.Hospital).Include(u => u.User).ToList();

            return userReserve;
        }

        public void Update(int id, UserReservation newentity)
        {
            db.Update(newentity);
            db.SaveChanges();
        }
    }
}
