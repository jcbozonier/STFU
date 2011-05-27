using System;
using System.Diagnostics;
using System.IO;

public static class Program
{
  public static void Main(string[] args)
  {
    var command_name = args[0];

    switch(command_name)
    {
      case "install":
        Console.WriteLine("installing stfu");

        Console.WriteLine("stfu installed successfully");
        break;
      case "new":
        var new_project_name = args[1];
        var new_project_src_path = Path.Combine(new_project_name, "src");
        var new_project_bin_path = Path.Combine(new_project_name, "bin");

        Directory.CreateDirectory(new_project_name);
        Directory.CreateDirectory(new_project_src_path);
        Directory.CreateDirectory(new_project_bin_path);

        var new_project_main_class_path = Path.Combine(new_project_src_path, "main.cs");

        File.Create(new_project_main_class_path);
        break;
      case "clean":
        Directory.Delete(@".\bin", true);
        Directory.CreateDirectory(@".\bin");
        break;
      case "build":
        Console.WriteLine("Starting build of main.cs");
        var compilerPath = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe";
        var executable_path = @".\bin\stfu.exe";
        var src = @".\src\main.cs";
        using (var compilerProcess = new Process())
        {
            compilerProcess.StartInfo.FileName = compilerPath;
            compilerProcess.StartInfo.Arguments = "/out:\"" + executable_path + "\" \"" + src + "\"";
            compilerProcess.StartInfo.UseShellExecute = false;
            compilerProcess.StartInfo.RedirectStandardOutput = true;
            compilerProcess.StartInfo.CreateNoWindow = true;

            compilerProcess.Start();
            compilerProcess.WaitForExit();
            
            var compilerMessages = compilerProcess.StandardOutput.ReadToEnd();
            Console.WriteLine(compilerMessages);
        }

        Console.WriteLine("Done building stfu");
        break;
      default:
        Console.WriteLine("I don't know command " + args[0]);
        break;
    }

  }
}
