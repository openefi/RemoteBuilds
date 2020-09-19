using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using static OpenEFI_RemoteBuild.DB.DBController;


namespace OpenEFI_RemoteBuild.Workers
{
    public static class ProcessWorkers
    {
        public static string MakeBuild(ILogger _logger, BuildRequest data)
        {
            var BuildWorker = new BackgroundWorker()
            {
                WorkerReportsProgress = false,
                WorkerSupportsCancellation = false
            };

            BuildWorker.DoWork += Build;
            BuildWorker.RunWorkerCompleted += FinishBuild;
            string _hash = SHA512(JsonSerializer.Serialize(data));
            UpdateBuildStatus(_hash, "PREBUILD_CHECKS");

            if (PreBuildChecks(data))
            {
                UpdateBuildStatus(_hash, "PREBUILD_INIT_FILES");
                BuildWorker.RunWorkerAsync(new { Hash = _hash, logger = _logger });
            }
            else
            {
                _logger.LogCritical("fail in prebuilds checks");
                UpdateBuildStatus(_hash, "PREBUILD_CHECK_FAIL");
            }

            return _hash;
        }

        public static void Build(object sender, DoWorkEventArgs e)
        {
            dynamic args = e.Argument;
            string hash = args.Hash ?? "no mandaste un pingo";
            ILogger _logger = args.logger;
            UpdateBuildStatus(hash, "BUILD_INIT");
            int code = 0;
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
                code = exited ? process.ExitCode : -1;

                _logger.LogInformation($"VAMO MENEEE QUE TERMINO {code}");
            }
            e.Result = new { hash = hash, code = code, logger = _logger };
        }

        public static void FinishBuild(object sender, RunWorkerCompletedEventArgs e)
        {
            dynamic args = e.Result;
            int code = args.code;
            string hash = args.hash;
            ILogger logger = args.logger;

            if (code == 0)
            {
                logger.LogInformation($"Build ID {hash.Substring(0, 8)}, finished");
                UpdateBuildStatus(hash, "BUILD_SUCCES");
            }
            else
            {
                logger.LogCritical($"Build ID {hash.Substring(0, 8)}, has error in build");
                UpdateBuildStatus(hash, "BUILD_FAILED");

            }
        }

        public static bool PreBuildChecks(BuildRequest data)
        {
            //TODO: agarrar la pala
            return true;
        }

        public static bool InitBuildFiles(string BUILD_ID)
        {
            //TODO: agarrar la pala x2
            return true;
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
