﻿namespace WormsApp.Domain.Services
{
    public interface IGameClocks
    {
        public delegate void OnTick(long currentTick);

        public void Launch(OnTick onTickCallback);
    }
}