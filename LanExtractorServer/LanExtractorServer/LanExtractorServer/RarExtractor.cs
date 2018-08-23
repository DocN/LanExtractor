using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LanExtractorServer
{
    class RarExtractor
    {
        private string fileDir = "";
        private string directoryName = "";
        private Socket handler;
        public RarExtractor()
        {
            this.directoryName = "1";
            this.fileDir = "C:\\Family.Guy.S14E14.720p.HDTV.x264-FLEET\\family.guy.s14e14.720p.hdtv.x264-fleet.r00";
            this.SwapSlash();
            this.directoryName = DirToFilename(this.fileDir);
            this.ExtractFile();
        }

        public RarExtractor(Socket handler)
        {
            this.handler = handler;
            this.directoryName = "1";
            this.fileDir = "C:\\Family.Guy.S14E14.720p.HDTV.x264-FLEET\\family.guy.s14e14.720p.hdtv.x264-fleet.r00";
            this.SwapSlash();
            this.directoryName = DirToFilename(this.fileDir);
            this.ExtractFile();
        }

        void ExtractFile()
        {
            string strCmdText = "unrar x";
            strCmdText = strCmdText + " " + this.fileDir + " " + this.directoryName;
            //System.Diagnostics.Process.Start("CMD.exe", strCmdText);
            strCmdText = "unrar x C:/Family.Guy.S14E14.720p.HDTV.x264-FLEET/family.guy.s14e14.720p.hdtv.x264-fleet.r00 C:/Family.Guy.S14E14.720p.HDTV.x264-FLEET -o+";
            Console.WriteLine(strCmdText);
            var processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = "cmd.exe";
            processStartInfo.Arguments = "/C " + strCmdText;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            Process proc = Process.Start(processStartInfo);
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                Console.WriteLine(line);
                byte[] msg = Encoding.ASCII.GetBytes(line);

                handler.Send(msg);
                // do something with line
            }
        }

        void SwapSlash()
        {
            this.fileDir = this.fileDir.Replace('\\', '/');
        }

        string DirToFilename(string filedir)
        {
            string filename = "";
            if (filedir == null || filedir.Length <= 0)
            {
                return filename;
            }
            int lastIndex = filedir.LastIndexOf('/');
            if (lastIndex == -1)
            {
                return filename;
            }

            if (lastIndex < filedir.Length)
            {
                filename = filedir.Substring(lastIndex + 1);
            }
            return filename;
        }

        void replaceDriveLetter(string filedir)
        {

        }
    }
}
