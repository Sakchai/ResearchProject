﻿using System.Threading.Tasks;

namespace AdminLTE.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
