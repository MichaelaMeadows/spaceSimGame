using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceSimulation.Commands
{
    enum CommandState
    {
        CREATED,
        FAILED,
        SUCCESS,
        PROGRESS,
        PENDING
    }
}
