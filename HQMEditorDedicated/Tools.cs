using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQMEditorDedicated
{
    class Tools
    {
        /// <summary>
        /// Pauses time on the clock
        /// </summary>
        public static void PauseGame()
        {
            MemoryEditor.WriteBytes(0x0040B65A, new byte[] { 75 });
            MemoryEditor.WriteBytes(0x004078FB, new byte[] { 0 });
        }

        /// <summary>
        /// Resumes time on the clock
        /// </summary>
        public static void ResumeGame()
        {
            MemoryEditor.WriteBytes(0x0040B65A, new byte[] { 0 });
            MemoryEditor.WriteBytes(0x004078FB, new byte[] { 1 });
        }

        /// <summary>
        /// Forces a faceoff
        /// </summary>
        public static void ForceFaceoff()
        {
            MemoryEditor.WriteBytes(0x01893200, new byte[] { 1 });
        }
    }
}
