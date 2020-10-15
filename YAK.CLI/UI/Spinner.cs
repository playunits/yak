using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static YAK.CLI.UI.Utils;

namespace YAK.CLI.UI
{
    public class Spinner : CliUiElement
    {
        public int Delay { get; set; }

        public string Message { get; set; }

        private char[] _Sequence { get; set; } = new char[] { '/', '-', '\\', '|' };

        private ConsoleColor SpinnerColor { get; set; }
        private ConsoleColor TextColor { get; set; }

        private int _SeqPos { get; set; } = 0;

        public Spinner(string message, int delay = 100, ConsoleColor spinnerColor = ConsoleColor.Yellow, ConsoleColor textColor = ConsoleColor.White)
        {
            this.Message = message;
            this.Delay = delay;
            this.SpinnerColor = spinnerColor;
            this.TextColor = textColor;
        }
        public override void Run()
        {
            while (this._Active)
            {
                if (!this._Paused)
                {
                    Turn();
                    System.Threading.Thread.Sleep(this.Delay);
                }
            }
        }

        public void Start(string message)
        {
            this.Message = message;
            this.Start();
        }

        public void Stop(IOStatus status, string message)
        {
            Utils.ClearCurrentLine();
            Utils.Status(status);
            Console.Write($" {message}");
            Console.Write("\n");
        }

        public override void CreateThread()
        {
            this.Thread = new Thread(Run);
        }

        public void Turn()
        {
            Utils.ClearCurrentLine();
            Console.ForegroundColor = this.SpinnerColor;
            Console.Write($"{this._Sequence[this._SeqPos % this._Sequence.Length]}");
            Console.ForegroundColor = this.TextColor;
            Console.Write(this.Message);
            this._SeqPos++;
            if (this._SeqPos == this._Sequence.Length)
                this._SeqPos = 0;
        }

        public void StartNext(IOStatus status, string finishMessage, string message)
        {
            this._Paused = true;
            Utils.ClearCurrentLine();
            Utils.Status(status);
            Console.WriteLine(finishMessage);
            this.Message = message;
            this._Paused = false;
        }
    }
}
