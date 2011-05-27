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
        install_stfu();
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
        var project_bin_path = @".\bin";
        if(!Directory.Exists(project_bin_path))
          Directory.CreateDirectory(project_bin_path);
          
        var compilerPath = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe";
        var executable_path = Path.Combine(project_bin_path, "stfu.exe");
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

  public static void install_stfu()
  {
    var install_path = @"C:\stfu";
    var install_file_path = Path.Combine(install_path, "stfu.exe");
    if(Directory.Exists(install_path))
      Directory.Delete(install_path, true);
    Directory.CreateDirectory(install_path);
    File.Copy(@".\bin\stfu.exe", install_file_path);
    var path_value = System.Environment.GetEnvironmentVariable("Path");
    if(!path_value.Contains(";" + install_file_path + ";"))
      System.Environment.SetEnvironmentVariable("Path", path_value + ";" + install_path, EnvironmentVariableTarget.User);

  }
}
