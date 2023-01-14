using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Services
{
    public class LogService
    {
        private ILogRepository repository { get; set; }

        public LogService(ILogRepository _repository)
        {
            this.repository = _repository;
        }

        public void Log(string msg, string ipAddress, string user)
        {
            repository.Log(msg, ipAddress, user);
        }

        public void Log(Exception exeption, string ipAddress, string user)
        {
            repository.Log(exeption, ipAddress, user);
        }
    }
}
