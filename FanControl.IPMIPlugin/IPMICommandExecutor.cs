using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace FanControl.IPMIPlugin
{

    public static class IPMICommandExecutor
    {
        private static string IPMITOOL_FILE = "ipmitool.exe";

        private static string FetchPath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), IPMITOOL_FILE);
        }

        public static string RunCommand(string command, string arguments)
        {
            string result = string.Empty;
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = command;
                startInfo.Arguments = arguments;
                startInfo.RedirectStandardOutput = true;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                using (
                    Process process = new Process()
                    )
                {
                    process.StartInfo = startInfo;
                    process.Start();
                    result = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                // 处理异常
                Console.WriteLine("Error: " + ex.Message);
            }

            return result;
        }
        public static void SetFanSpeedByPercentage(IPMIRemoteInfo client, FanEntiry fanIndex, int val)
        {
            if (fanIndex.index == "")
            {
                Console.WriteLine("fan index is empty");
                return;
            }

            string args = $"{client.Print()} raw 0x2e 0x30 00 {fanIndex.index} {val}";
            Console.WriteLine($"run cmd: {FetchPath()} {args}");
            string result = RunCommand(FetchPath(), args);
            Console.WriteLine($"result: {result}");
        }

        public static float GetFanSpeed(IPMIRemoteInfo client, FanEntiry fanIndex)
        {
            float ret = 0;
            if (fanIndex.name == "")
            {
                Console.WriteLine("fan name is empty");
                return ret;

            }

            string args = $"{client.Print()} sensor reading \"{fanIndex.name}\"";
            Console.WriteLine($"run cmd: {FetchPath()} {args}");
            string result = RunCommand(FetchPath(), args);
            Console.WriteLine($"result: {result}");

            int lastIndex = result.LastIndexOf('|');
            if (lastIndex != -1)
            {
                // 获取竖线后面的部分
                string numberString = result.Substring(lastIndex + 1).Trim();
                // 尝试将字符串转换为float类型
                if (float.TryParse(numberString, out float resultFloat))
                {
                    // 转换成功，result中存储了转换后的float值
                    ret = resultFloat;
                }
                else
                {
                    Console.WriteLine("Unable to convert the string to float.");
                }
            }
            else
            {
                Console.WriteLine("Unable to find the separator '|' in the input string.");
            }

            Console.WriteLine($"fan speed: {ret}, cmd: {FetchPath()} {args}");
            return ret;
        }
    }
}