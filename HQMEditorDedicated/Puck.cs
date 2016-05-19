using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQMEditorDedicated
{
    /// <summary>
    /// Contains information about the Puck
    /// </summary>
    public class Puck
    {
        const int PUCK_TRANSFORM_ADDRESS = 0x0187B6F0;
        const int PUCK_VELOCITY_OFFSET = 0x3C;
        const int PUCK_ROTATIONAL_VELOCITY_OFFSET = 0x54;

        /// <summary>
        /// The position of the puck
        /// </summary>
        public static HQMVector Position
        {
            get { return MemoryEditor.ReadHQMVector(PUCK_TRANSFORM_ADDRESS ); }
            set { MemoryEditor.WriteHQMVector(value, PUCK_TRANSFORM_ADDRESS); }
        }

        /// <summary>
        /// The velocity of the puck
        /// </summary>
        public static HQMVector Velocity
        {
            get { return MemoryEditor.ReadHQMVector(PUCK_TRANSFORM_ADDRESS + PUCK_VELOCITY_OFFSET); }
            set { MemoryEditor.WriteHQMVector(value, PUCK_TRANSFORM_ADDRESS + PUCK_VELOCITY_OFFSET); }
        }

        /// <summary>
        /// The spin or rotational velocity of the puck
        /// </summary>
        public static HQMVector RotationalVelocity
        {
            get { return MemoryEditor.ReadHQMVector(PUCK_TRANSFORM_ADDRESS + PUCK_ROTATIONAL_VELOCITY_OFFSET); }
            set { MemoryEditor.WriteHQMVector(value, PUCK_TRANSFORM_ADDRESS + PUCK_ROTATIONAL_VELOCITY_OFFSET); }
        }
    }
}
