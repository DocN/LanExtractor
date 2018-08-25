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
        private static string SHARE_DIR = "D";
        private string fileDir = "";
        private string filePath = "";
        private Socket handler;
        private string filename = "";

        

        public RarExtractor(String fileDIR, Socket handler)
        {
            this.handler = handler;
            this.fileDir = fileDIR;
            //this.fileDir = "D:\\Family.Guy.S14E14.720p.HDTV.x264-FLEET\\family.guy.s14e14.720p.hdtv.x264-fleet.r00";
            this.ReplaceDriveLetter();
            this.SwapSlash();
            this.DirToFilename(this.fileDir);
            this.getDirectory();
            this.ExtractFile();

        }

        void ExtractFile()
        {
            this.makeFolder();
            string strCmdText = "unrar x";

            //strCmdText = "unrar x C:/Family.Guy.S14E14.720p.HDTV.x264-FLEET/family.guy.s14e14.720p.hdtv.x264-fleet.r00 C:/Family.Guy.S14E14.720p.HDTV.x264-FLEET -o+";
            strCmdText = "unrar x " + this.fileDir + " "  + this.filePath + "/" + this.filename + " -o+";
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

        void makeFolder()
        {
            
            var processStartInfo = new ProcessStartInfo();
            string strCmdText = "mkdir " + this.filePath + "/" + this.filename;
            strCmdText = this.SwapSlashString(strCmdText);
            Console.WriteLine(strCmdText);
            processStartInfo.FileName = "cmd.exe";
            processStartInfo.Arguments = "/C " + strCmdText;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            Process proc = Process.Start(processStartInfo);
        }
        void SwapSlash()
        {
            this.fileDir = this.fileDir.Replace('\\', '/');
        }

        string SwapSlashString(string directory)
        {
            directory = directory.Replace('/', '\\');
            return directory;
        }

        void getDirectory()
        {
            int indexOfLastSlash = this.fileDir.LastIndexOf('/');
            if(indexOfLastSlash == -1)
            {
                this.filePath = "blank";
            }
            this.filePath = this.fileDir.Substring(0, indexOfLastSlash);
        }

        void DirToFilename(string filedir)
        {
            string filename = "";
            if (filedir == null || filedir.Length <= 0)
            {
                this.filename = filename;
            }
            int lastIndex = filedir.LastIndexOf('/');

            if (lastIndex == -1)
            {
                this.filename = filename;
            }

            if (lastIndex < filedir.Length)
            {
                filename = filedir.Substring(lastIndex + 1);
            }
            int extIndex = filename.LastIndexOf('.');
            this.filename = filename.Substring(0, extIndex);
        }

        void ReplaceDriveLetter()
        {
            if(this.fileDir != null && this.fileDir.Length >= 0)
            {
                this.fileDir = SHARE_DIR + this.fileDir.Substring(1);
            }
        }
    }
}
