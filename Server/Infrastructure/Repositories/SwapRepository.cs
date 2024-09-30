using Domain.Entites;
using Domain.Interfaces.Requests;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class SwapRepository : ISwapRepository
    {
        private readonly Context _context;

        public SwapRepository(Context context)
        {
            _context = context;
        }
        public IEnumerable<Swap> Get(int user_id, int id_duty, DateTime date)
        {
            return _context.Swaps.Where(x => (x.Id_User1 == user_id && x.Id_Duty1 == id_duty && x.Date1 == date) || (x.Id_User2 == user_id && x.Id_Duty2 == id_duty && x.Date2 == date));           
        }
        
        public IEnumerable<Swap> GetAll() => _context.Swaps.ToList();
        
        public IEnumerable<Swap> GetAllConfirmed() =>  _context.Swaps.Where(x => x.IsConfirmed == true);
        
        public IEnumerable<Swap> GetAllUnconfirmed() =>  _context.Swaps.Where(x => x.IsConfirmed == false);
        
        public Swap GetById(int id) => _context.Swaps.SingleOrDefault(x => x.Id == id);
        public Swap GetSwap(int user_id1, int user_id2, DateTime date1, DateTime date2, int id_duty1, int id_duty2)
        {
            return _context.Swaps.SingleOrDefault(x =>
                                                        x.Id_User1 == user_id1 && 
                                                        x.Id_User2 == user_id2 &&
                                                        x.Date1 == date1 &&
                                                        x.Date2 == date2 &&
                                                        x.Id_Duty1 == id_duty1 &&
                                                        x.Id_Duty2 == id_duty2);
        }

        public IEnumerable<Swap> GetAllByDate(DateTime date) => _context.Swaps.Where(x => x.Date1 == date);

        public IEnumerable<Swap> GetAllByUserId(int user_id) => _context.Swaps.Where(x => x.Id_User1 == user_id);

        public Swap Add(Swap sw)
        {
            _context.Add(sw);
            _context.SaveChanges();
            return sw;
        }
        public void Update(Swap sw)
        {
            _context.Update(sw);
            _context.SaveChanges();
        }
        public void Delete(Swap sw)
        {
            _context.Remove(sw);
            _context.SaveChanges();
        }

    }
}
