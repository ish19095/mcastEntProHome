using DataAccess.context;
using Domain.Interface;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public class LogViaDBRepository : ILogRepository
    {

        private FileSharingContext fileSharingContext;

        public LogViaDBRepository(FileSharingContext _fileSharingContext)
        {
            this.fileSharingContext = _fileSharingContext;
        }

        public void Log(string msg, string ipaddress, string user)
        {
            var log = new Log()
            {
                Id = Guid.NewGuid(),
                Msg = msg,
                Ipaddress = ipaddress,
                User = user,
                TimeStamp = DateTime.Now
            };

            fileSharingContext.Logs.Add(log);
            fileSharingContext.SaveChanges();
        }

        public void Log(Exception exception, string ipaddress, string user)
        {
            var log = new Log()
            {
                Id = Guid.NewGuid(),
                Msg = exception.Message,
                Ipaddress = ipaddress,
                User = user,
                TimeStamp = DateTime.Now
            };

            fileSharingContext.Logs.Add(log);
            fileSharingContext.SaveChanges();
        }
    }
}
