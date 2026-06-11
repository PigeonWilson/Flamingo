using System.Text;
using ZTALauncher;
using ZTALauncher.Common;
using static ZTALauncher.Common.Enums;

void PrintAndTerminateErrorWrongArgument()
{
    StringBuilder sb = new StringBuilder();
    sb.Append(ApplicationContext.Instance.Localization.GetString(Localization.TruthSource.missingArgument));
    sb.Append(ApplicationContext.Instance.Localization.GetString(Localization.TruthSource.exitCodeInfoMsg));
    sb.Append(ArgumentValidator.FaultyExitCode);
    Console.WriteLine(sb.ToString());
    Environment.Exit(ArgumentValidator.FaultyExitCode);
}

try
{
    bool verbose = false;
    SupportedModes mode = SupportedModes.Vanilla;

    if (args.Length < 2)
    {
        PrintAndTerminateErrorWrongArgument();
    }

    string executablePath = args[0];
    string modeSelection = args[1];

    if (args.Length == 3)
    {
        verbose = true;
    }

    if (modeSelection.ToLower() == "steam") mode = SupportedModes.Steam;
    if (modeSelection.ToLower() == "vanilla") mode = SupportedModes.Vanilla;

    if (args.Length > 0)
    {
        var argumentValidator = new ArgumentValidator();
        argumentValidator.ExecutablePath = args[0];
        if (!ArgumentValidator.CheckExecutableAccessibility(argumentValidator.ExecutablePath))
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ApplicationContext.Instance.Localization.GetString(Localization.TruthSource.errorFileAccessibility));
            sb.Append(ApplicationContext.Instance.Localization.GetString(Localization.TruthSource.exitCodeInfoMsg));
            if (verbose) Console.WriteLine($"{sb.ToString()} {ArgumentValidator.FaultyExitCode}");
            Environment.Exit(ArgumentValidator.FaultyExitCode);
        }
        else
        {
            if (verbose) Console.WriteLine(ApplicationContext.Instance.Localization.GetString(Localization.TruthSource.launching));
            
            ApplicationContext.Instance.Set(mode, argumentValidator.ExecutablePath);
            ApplicationContext.Instance.LoadMode();
            if (ApplicationContext.Instance.LocalStore.Game != null 
                && !string.IsNullOrEmpty(ApplicationContext.Instance.LocalStore.Game.ExecutableSignature))
            {
                Console.WriteLine(ApplicationContext.Instance.LocalStore.Game.ExecutableSignature);
            }

            // TODO: generate an error if it was not possible to print the signature

            CommonLibrary.ProcessLauncher.LaunchLowIntegrityProcess(argumentValidator.ExecutablePath);

            Console.ReadKey();

            Environment.Exit(ArgumentValidator.SuccessExitCode);
        }
    }
    else
    {
        PrintAndTerminateErrorWrongArgument();
    }
}
catch (Exception e) 
{
    Console.WriteLine($"An unknown error occured. {e.Message}");
}