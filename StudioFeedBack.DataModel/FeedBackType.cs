using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioFeedBack.DataModel
{
    public enum FeedBackType
    {
        User = 1,
        Admin =2,
    }
    public enum FeedbackStatus
    {
        All = 0,
        Open = 1,
        Pending = 2,
        Closed = 3
    }
}
