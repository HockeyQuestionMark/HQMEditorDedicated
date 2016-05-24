
using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace HQMEditorDedicated
{
    public static class MemoryEditor
    {
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        static extern bool GetExitCodeProcess(int hProcess, ref int lpExitCode);

        private const int PROCESS_ALL_ACCESS = 0x1F0FFF;
        private static Process hockeyProcess = null;
        private static IntPtr hockeyProcessHandle;        

        // <summary>
        /// Attaches to hockeydedicated.exe. Must be called before anything else. Make sure hockey is running.
        /// returns true if attach was succesful
        /// </summary>
        public static bool Init()
        {
            try
            {
                hockeyProcess = Process.GetProcessesByName("hockeydedicated")[0];
            }
            catch (System.IndexOutOfRangeException e)  // CS0168
            {
                return false;
            }
            hockeyProcessHandle = OpenProcess(PROCESS_ALL_ACCESS, false, hockeyProcess.Id);
            return true;
            
        }

        /// <summary>
        /// Attaches to hockeydedicated.exe. Must be called before anything else. Make sure hockey is running. 
        /// Use this if your hockeydedicated.exe is named differently.
        /// returns true if attach was succesful
        /// </summary>
        /// <param name="processName">the address to read from</param>
        public static bool Init(string processName)
        {
            try
            {
                hockeyProcess = Process.GetProcessesByName(processName)[0];
            }
            catch (System.IndexOutOfRangeException e)  // CS0168
            {
                return false;
            }
            hockeyProcessHandle = OpenProcess(PROCESS_ALL_ACCESS, false, hockeyProcess.Id);
            return true;
        }

        // <summary>
        /// Returns true if the memory editor is still attached to hockey
        /// </summary>
        public static bool IsAttached()
        {
            int ret = 0;
            GetExitCodeProcess((int)hockeyProcessHandle, ref ret);
            return ret == 259;
        }

        /// <summary>
        /// Read a 32 bit integer from memory.
        /// </summary>
        /// <param name="address">the address to read from</param>
        public static int ReadInt(int address)
        {
            int bytesRead = 0;
            byte[] buffer = new byte[4];
            ReadProcessMemory((int)hockeyProcessHandle, address, buffer, buffer.Length, ref bytesRead);

            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// Write a 32 bit integer to memory.
        /// </summary>
        /// <param name="value">the integer to write</param>
        /// <param name="address">the address to write to</param>
        public static void WriteInt(int value, int address)
        {
            int bytesWritten = 0;
            byte[] buffer = BitConverter.GetBytes(value);

            if (!WriteProcessMemory((int)hockeyProcessHandle, address, buffer, buffer.Length, ref bytesWritten))
                Console.WriteLine("failed to write "+value+" to "+address);
        }

        /// <summary>
        /// Read a float from memory.
        /// </summary>
        /// <param name="address">the address to read from</param>
        public static float ReadFloat(int address)
        {
            int bytesRead = 0;
            byte[] buffer = new byte[4];
            ReadProcessMemory((int)hockeyProcessHandle, address, buffer, buffer.Length, ref bytesRead);

            return BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>
        /// Write a float to memory.
        /// </summary>
        /// <param name="value">the float to write</param>
        /// <param name="address">the address to write to</param>
        public static void WriteFloat(float value, int address)
        {
            int bytesWritten = 0;
            byte[] buffer = BitConverter.GetBytes(value);

            WriteProcessMemory((int)hockeyProcessHandle, address, buffer, buffer.Length, ref bytesWritten);
        }

        /// <summary>
        /// Read a string from memory
        /// </summary>
        /// <param name="address">the address to read from</param>
        /// <param name="length">the length of the string to read</param>
        public static string ReadString(int address, int length)
        {
            int bytesRead = 0;
            byte[] buffer = new byte[length];
            ReadProcessMemory((int)hockeyProcessHandle, address, buffer, buffer.Length, ref bytesRead);

            // Read up until a \0 is encounted
            return Encoding.ASCII.GetString(buffer).Split('\0')[0];
        }

        /// <summary>
        /// Write a string to memory
        /// </summary>
        /// /// <param name="str">the string to write</param>
        /// <param name="address">the address to write to</param>        
        public static void WriteString(string str, int address)
        {
            int bytesWritten = 0;
            byte[] buffer = Encoding.ASCII.GetBytes(str + "\0");

            WriteProcessMemory((int)hockeyProcessHandle, address, buffer, buffer.Length, ref bytesWritten);
        }     

        /// <summary>
        /// Writes a Vector3 to memory.
        /// </summary>
        /// <param name="v">HQMVector to write</param>
        /// <param name="address"> The address of the vector to write.</param>
        public static void WriteHQMVector(HQMVector v, int address)
        {
            int bytesWritten = 0;
            byte[] buffer = new byte[12];
            float[] posArray = new float[] { v.X, v.Y, v.Z };
            Buffer.BlockCopy(posArray, 0, buffer, 0, buffer.Length);

            WriteProcessMemory((int)hockeyProcessHandle, address, buffer, buffer.Length, ref bytesWritten);
        }

        /// <summary>
        /// Reads a Vector3 from memory
        /// </summary>
        /// <param name="address">The address of the Vector3 to write</param>
        /// <returns>HQMVector </returns>
        public static HQMVector ReadHQMVector(int address)
        {
            int bytesRead = 0;
            byte[] buffer = new byte[12];

            ReadProcessMemory((int)hockeyProcessHandle, address, buffer, buffer.Length, ref bytesRead);

            float[] posArray = new float[3];
            Buffer.BlockCopy(buffer, 0, posArray, 0, buffer.Length);
            HQMVector v = new HQMVector(posArray[0], posArray[1], posArray[2]);

            return v;
        }

        /// <summary>
        /// Writes bytes to memory
        /// </summary>
        /// <param name="bytes">bytes to write</param>
        /// <param name="address">The address to write the bytes to</param>
        /// <returns>number of bytes written </returns>
        public static int WriteBytes(byte[] bytes, int address)
        {
            int bytesWritten = 0;
            WriteProcessMemory((int)hockeyProcessHandle, address, bytes, bytes.Length, ref bytesWritten);
            return bytesWritten;
        }

        /// <summary>
        /// Read bytes from memory
        /// </summary>
        /// <param name="address">The address to read the bytes from</param>
        /// <param name="numBytes">The number of bytes to read</param>
        /// <returns>bytes read from memory</returns>
        public static byte[] ReadBytes(int address, int numBytes)
        {
            int bytesRead = 0;
            byte[] buffer = new byte[numBytes];
            ReadProcessMemory((int)hockeyProcessHandle, address, buffer, buffer.Length, ref bytesRead);
            return buffer;
        }
    }
}

