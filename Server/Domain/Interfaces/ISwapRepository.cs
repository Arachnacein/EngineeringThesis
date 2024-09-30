using Domain.Entites;
using System;
using System.Collections.Generic;

namespace Domain.Interfaces.Requests
{
    public interface ISwapRepository
    {
        IEnumerable<Swap> Get(int user_id, int id_duty, DateTime date);
        IEnumerable<Swap> GetAll();
        IEnumerable<Swap> GetAllConfirmed();
        IEnumerable<Swap> GetAllUnconfirmed();
        IEnumerable<Swap> GetAllByDate(DateTime date);
        IEnumerable<Swap> GetAllByUserId(int user_id);
        Swap GetSwap(int user_id1, int user_id2, DateTime date1, DateTime date2, int id_duty1, int id_duty2);
        Swap GetById(int id);
        Swap Add(Swap sw);
        void Update(Swap sw);
        void Delete(Swap sw);
    }
}
