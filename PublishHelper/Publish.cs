using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PublishHelper
{

    static class Dotnet
    {

        static Process process;
        public static Task Publish(string profile, Action<string> onProgress)
        {

            if (process != null)
                CancelPublish();

            process = new Process();

            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = "publish " + Strings.project + " /p:PublishProfile=" + profile;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.OutputDataReceived += (s, e) => onProgress?.Invoke(e.Data);
            process.Start();
            process.BeginOutputReadLine();

            return Task.Run(() => { process.WaitForExit(); CancelPublish(); });

        }

        public static void CancelPublish()
        {

            if (!process?.HasExited ?? false)
                process.Kill();
            process = null;

        }

    }

}
