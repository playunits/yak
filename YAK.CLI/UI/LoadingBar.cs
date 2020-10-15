using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace YAK.CLI.UI
{
    public class LoadingBar : CliUiElement
    {
        public int Progress { get; set; }
        public int RefreshDelay { get; set; }

        public LoadingBar(int refreshDelay = 100)
        {
            this.RefreshDelay = refreshDelay;
        }

        public override void Run()
        {
            while (this._Active)
            {
                if (!this._Paused)
                {
                    Display();
                    System.Threading.Thread.Sleep(this.RefreshDelay);
                }
            }
        }

        public override void CreateThread()
        {
            this.Thread = new Thread(Run);
        }

        public void Display()
        {
            Utils.ClearCurrentLine();            

            string progressString = $"({this.Progress}%)";
            int loadWidth = Console.WindowWidth - (progressString.Length + 6 + 3);

            string inner = "";

            float tmpLen = (this.Progress / 100.0f);
            int len = (int)(tmpLen * loadWidth);
            for (int i = 0; i< len; i++)
            {
                inner += "=";
            }

            if(loadWidth - inner.Length  > 0)
            {
                inner += ">";
            }

            for(int i = inner.Length; i <= loadWidth; i++){
                inner += " ";
            }

            Console.Write($"   [{inner}] {progressString}");
        }
    }
}
