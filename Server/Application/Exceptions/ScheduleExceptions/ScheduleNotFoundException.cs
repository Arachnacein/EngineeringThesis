using System;

namespace Application.Exceptions
{
    public class ScheduleNotFoundException : Exception
    {
        public ScheduleNotFoundException()
            :base("ScheduleNotFoundException")
        { }
        public ScheduleNotFoundException(string message) :
            base($"! { message}")
        { }        
    }
}
