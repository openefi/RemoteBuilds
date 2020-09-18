using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.ComponentModel;

namespace OpenEFI_RemoteBuild.Workers
{
    public static class ProcessWorkers
    {
        public static string MakeBuild(ILogger _logger)
        {
            var BuildWorker = new BackgroundWorker()
            {
                WorkerReportsProgress = false,
                WorkerSupportsCancellation = false
            };

            BuildWorker.DoWork += Build;
            BuildWorker.RunWorkerCompleted += FinishBuild;
            string _hash = SHA512("asdasda");
            BuildWorker.RunWorkerAsync(new { Hash = _hash, logger = _logger });
            return _hash;
        }

        public static void Build(object sender, DoWorkEventArgs e)
        {
            dynamic args = e.Argument;
            string txt = args.Hash ?? "no mandaste un pingo";
            ILogger _logger = args.logger;

            using (var process = new Process())
            {
                process.StartInfo.FileName = @"/home/fdsoftware/.platformio/penv/bin/pio";
                process.StartInfo.Arguments = @"run -d ./files/demo";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                //process.OutputDataReceived += (sender, data) => _logger.LogInformation(data.Data);
                //process.ErrorDataReceived += (sender, data) => _logger.LogError(data.Data);
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                var exited = process.WaitForExit(1000 * 100);
                var code = exited ? process.ExitCode : -1;
                _logger.LogInformation($"VAMO MENEEE QUE TERMINO {code}");
            }
            e.Result = "eso lo revoleo al void para cuando termino el trabajo";
        }

        public static void FinishBuild(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Build finalizado (Aca tendria que tirar la data a la DB pero tanto no agarro la pala)");
            Console.WriteLine(e.Result);
        }


        public static string SHA512(string input)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }
    }


}
