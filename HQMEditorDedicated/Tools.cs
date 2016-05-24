using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQMEditorDedicated
{
    public class Tools
    {
        /// <summary>
        /// Pauses time on the clock
        /// </summary>
        public static void PauseGame()
        {
            MemoryEditor.WriteBytes(new byte[] { 75 }, 0x0040B65A);
            MemoryEditor.WriteBytes(new byte[] { 0 }, 0x004078FB);
        }

        /// <summary>
        /// Resumes time on the clock
        /// </summary>
        public static void ResumeGame()
        {
            MemoryEditor.WriteBytes(new byte[] { 0 }, 0x0040B65A);
            MemoryEditor.WriteBytes(new byte[] { 1 }, 0x004078FB);
        }

        /// <summary>
        /// Forces a faceoff
        /// </summary>
        public static void ForceFaceoff()
        {
            MemoryEditor.WriteBytes(new byte[] { 1 }, 0x01893200);
        }
    }
}
