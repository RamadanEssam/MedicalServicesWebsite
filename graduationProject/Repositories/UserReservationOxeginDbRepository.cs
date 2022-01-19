using graduationProject.Data;
using graduationProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace graduationProject.Repositories
{
    public class UserReservationOxeginDbRepository : InterfaceRepository<UserReservationOxegin>
    {
        ApplicationDbContext db;
        public UserReservationOxeginDbRepository(ApplicationDbContext _db)
        {
            db = _db;
        }
        public void Add(UserReservationOxegin entity)
        {
            db.UserReservationOxegins.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var userReserve = Find(id);
            db.UserReservationOxegins.Remove(userReserve);
            db.SaveChanges();
        }

        public void DeleteById(int id)
        {
            var userReserve = db.UserReservationOxegins.SingleOrDefault(g => g.Id == id);
            db.UserReservationOxegins.Remove(userReserve);
            db.SaveChanges();
        }

        public UserReservationOxegin Find(int id)
        {
            var userReserve = db.UserReservationOxegins.Include(h => h.OxygenTube).Include(u => u.User).SingleOrDefault(g => g.Oxygen_Id == id);
            return userReserve;
        }

        public IList<UserReservationOxegin> List()
        {
            var userReserve = db.UserReservationOxegins.Include(h => h.OxygenTube).Include(u => u.User).ToList();

            return userReserve;
        }

        public void Update(int id, UserReservationOxegin newentity)
        {
            db.Update(newentity);
            db.SaveChanges();
        }
    }
}
