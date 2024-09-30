using System;

namespace Application.Interfaces
{
    public interface ILogicService
    {
        void MainLogic();
        void StateRequests(DateTime startDay, DateTime endDay);
        void NonStateRequests(DateTime startDay, DateTime endDay);
        void FillStateWorkers(DateTime startDay, DateTime endDay);
        void FillNonStateWorkers(DateTime startDay, DateTime endDay);
        void Castlings(DateTime startDay, DateTime endDay);
        void CheckSundays(DateTime startDay, DateTime endDay);
    }
}
