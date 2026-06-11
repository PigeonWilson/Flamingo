using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CommonLibrary
{
    public sealed class ProcessLauncher : BaseObject
    {
        // Integrity level constants
        private const int SECURITY_MANDATORY_LOW_RID = 0x1000;
        private const int SE_GROUP_INTEGRITY = 0x00000020;

        // Token access rights
        private const int TOKEN_DUPLICATE = 0x0002;
        private const int TOKEN_QUERY = 0x0008;
        private const int TOKEN_ADJUST_DEFAULT = 0x0080;
        private const int TOKEN_ASSIGN_PRIMARY = 0x0001;

        // Process creation flags
        private const int CREATE_NEW_CONSOLE = 0x00000010;

        private enum TOKEN_INFORMATION_CLASS
        {
            TokenIntegrityLevel = 25
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SID_AND_ATTRIBUTES
        {
            public IntPtr Sid;
            public int Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct TOKEN_MANDATORY_LABEL
        {
            public SID_AND_ATTRIBUTES Label;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SID_IDENTIFIER_AUTHORITY
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] Value;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct STARTUPINFO
        {
            public int cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public int dwX;
            public int dwY;
            public int dwXSize;
            public int dwYSize;
            public int dwXCountChars;
            public int dwYCountChars;
            public int dwFillAttribute;
            public int dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        // P/Invoke declarations
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, int DesiredAccess, out IntPtr TokenHandle);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool DuplicateTokenEx(
            IntPtr hExistingToken,
            int dwDesiredAccess,
            IntPtr lpTokenAttributes,
            int ImpersonationLevel,
            int TokenType,
            out IntPtr phNewToken);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetTokenInformation(
            IntPtr TokenHandle,
            TOKEN_INFORMATION_CLASS TokenInformationClass,
            IntPtr TokenInformation,
            int TokenInformationLength);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool AllocateAndInitializeSid(
            ref SID_IDENTIFIER_AUTHORITY pIdentifierAuthority,
            byte nSubAuthorityCount,
            int nSubAuthority0, int nSubAuthority1, int nSubAuthority2, int nSubAuthority3,
            int nSubAuthority4, int nSubAuthority5, int nSubAuthority6, int nSubAuthority7,
            out IntPtr pSid);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern IntPtr FreeSid(IntPtr pSid);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int GetLengthSid(IntPtr pSid);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CreateProcessAsUser(
            IntPtr hToken,
            string lpApplicationName,
            string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            bool bInheritHandles,
            int dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation);

        public static void LaunchLowIntegrityProcess(string command)
        {
            // Step 1: Open current process token
            if (!OpenProcessToken(Process.GetCurrentProcess().Handle,
                TOKEN_DUPLICATE | TOKEN_ADJUST_DEFAULT | TOKEN_QUERY | TOKEN_ASSIGN_PRIMARY,
                out IntPtr hToken))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Failed to open process token.");
            }

            // Step 2: Duplicate token
            if (!DuplicateTokenEx(hToken,
                TOKEN_ADJUST_DEFAULT | TOKEN_QUERY | TOKEN_ASSIGN_PRIMARY,
                IntPtr.Zero,
                2, // SecurityImpersonation
                1, // TokenPrimary
                out IntPtr hNewToken))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Failed to duplicate token.");
            }

            // Step 3: Create Low Integrity SID
            SID_IDENTIFIER_AUTHORITY authority = new SID_IDENTIFIER_AUTHORITY { Value = new byte[] { 0, 0, 0, 0, 0, 16 } };
            if (!AllocateAndInitializeSid(ref authority, 1, SECURITY_MANDATORY_LOW_RID,
                0, 0, 0, 0, 0, 0, 0, out IntPtr pSid))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Failed to allocate Low Integrity SID.");
            }

            try
            {
                // Step 4: Set token integrity level
                TOKEN_MANDATORY_LABEL tml = new TOKEN_MANDATORY_LABEL
                {
                    Label = new SID_AND_ATTRIBUTES
                    {
                        Sid = pSid,
                        Attributes = SE_GROUP_INTEGRITY
                    }
                };

                int tmlSize = Marshal.SizeOf(tml) + GetLengthSid(pSid);
                IntPtr tmlPtr = Marshal.AllocHGlobal(tmlSize);
                try
                {
                    Marshal.StructureToPtr(tml, tmlPtr, false);

                    if (!SetTokenInformation(hNewToken, TOKEN_INFORMATION_CLASS.TokenIntegrityLevel, tmlPtr, tmlSize))
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error(), "Failed to set token integrity level.");
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(tmlPtr);
                }

                // Step 5: Launch process
                STARTUPINFO si = new STARTUPINFO { cb = Marshal.SizeOf(typeof(STARTUPINFO)) };
                PROCESS_INFORMATION pi;

                if (!CreateProcessAsUser(hNewToken, null, command,
                    IntPtr.Zero, IntPtr.Zero, false, CREATE_NEW_CONSOLE,
                    IntPtr.Zero, null, ref si, out pi))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "Failed to create process at Low Integrity.");
                }
            }
            catch (Exception) { }
        }
    }
}