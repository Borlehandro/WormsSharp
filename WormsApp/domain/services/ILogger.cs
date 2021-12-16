﻿using System;

namespace WormsApp.Domain.Services
{
    public interface ILogger: IDisposable
    {
        public void Log(string content);
    }
}