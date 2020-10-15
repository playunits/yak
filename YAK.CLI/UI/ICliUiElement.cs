using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace YAK.CLI.UI
{
    public interface ICliUiElement 
    {        
        public void Start();
        public void Stop();
        public void Pause();
        public void UnPause();
        public void Run();
        public bool IsRunning();
    }

    public class CliUiElement : ICliUiElement, IDisposable
    {
        public Thread Thread { get; set; }
        public bool _Active { get; set; }
        public bool _Paused { get; set; }

        public void Dispose()
        {
            this.Stop();
        }

        public bool IsRunning()
        {
            if (this.Thread == null)
                return false;
            if (!_Active)
                return false;
            if (_Paused)
                return false;

            return true;
        }

        public void Pause()
        {
            this._Paused = true;
        }

        public virtual void Run()
        {
            
        }

        public void Start()
        {
            this._Active = true;
            this._Paused = false;
            if(this.Thread == null)
            {
                CreateThread();
            }

            if (!Thread.IsAlive)
                Thread.Start();
        }

        public virtual void CreateThread()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            this._Active = false;            
            this.Thread = null;
        }

        public void UnPause()
        {
            this._Paused = false;
        }
    }
}
