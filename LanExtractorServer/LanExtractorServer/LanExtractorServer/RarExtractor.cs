using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanExtractorServer
{
    class RarExtractor
    {
        private string fileDir = "";
        private string directoryName = "";
        public RarExtractor()
        {
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
            strCmdText = "unrar x Z:/Family.Guy.S14E14.720p.HDTV.x264-FLEET/family.guy.s14e14.720p.hdtv.x264-fleet.r00 Z:/Family.Guy.S14E14.720p.HDTV.x264-FLEET";
            Console.WriteLine(strCmdText);
            var processStartInfo = new ProcessStartInfo();
            processStartInfo.WorkingDirectory = @"C:\Program Files\WinRAR";
            processStartInfo.FileName = "cmd.exe";
            processStartInfo.Arguments = "/C " + strCmdText;
            Process proc = Process.Start(processStartInfo);
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
