using System.Diagnostics;
using System.Text;
using ZTALauncher;


const string verbose_mode = "verbose";

try
{
    var localization = new Localization();
    bool verbose = false;

    if (args.Length == 2 && args[1] == verbose_mode)
    {
        verbose = true;
    }

    if (args.Length > 0)
    {
        var argumentValidator = new ArgumentValidator();
        argumentValidator.ExecutablePath = args[0];
        if (!ArgumentValidator.CheckExecutableAccessibility(argumentValidator.ExecutablePath))
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(localization.GetString(Localization.TruthSource.errorFileAccessibility));
            sb.Append(localization.GetString(Localization.TruthSource.exitCodeInfoMsg));
            if (verbose) Console.WriteLine($"{sb.ToString()} {ArgumentValidator.FaultyExitCode}");
            Environment.Exit(ArgumentValidator.FaultyExitCode);
        }
        else
        {
            if (verbose) Console.WriteLine(localization.GetString(Localization.TruthSource.launching));
            CommonLibrary.ProcessLauncher.LaunchLowIntegrityProcess(argumentValidator.ExecutablePath);
            var fileSignature = ArgumentValidator.SignFile(argumentValidator.ExecutablePath);
            var readableSignature = Convert.ToBase64String(fileSignature);
            Console.WriteLine(readableSignature);
            Environment.Exit(ArgumentValidator.SuccessExitCode);
        }
    }
    else
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(localization.GetString(Localization.TruthSource.missingArgument));
        sb.Append(localization.GetString(Localization.TruthSource.exitCodeInfoMsg));
        sb.Append(ArgumentValidator.FaultyExitCode);
        if (verbose) Console.WriteLine(sb.ToString());
        Console.WriteLine($"{localization.GetString("tip")} {verbose_mode}");
        Environment.Exit(ArgumentValidator.FaultyExitCode);
    }
}
catch (Exception e) 
{
    Console.WriteLine($"An unknown error occured. {e.Message}");
}