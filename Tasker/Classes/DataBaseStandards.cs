using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Classes
{
    public static class DataBaseStandards
    {
        private static List<Dictionary<string, object>> _difficulty = new List<Dictionary<string, object>>
        {
            new Dictionary<string, object> {
                {"Label", "easy"},
                {"Description", "These Tasks should be simple to do. This means, that they are mostly quick to do, simple or uncomplicated."},
                {"Recommendation", "Put these tasks at the beginning of a work phase (especially on work days with hard tasks)."}
            },
            new Dictionary<string, object> {
                {"Label", "normal"},
                {"Description", "Even though these tasks are not hard, you still might use a little boost to do them. This Category is for simple tasks, that you are not motivated for, and for tasks, which take a little longer."},
                {"Recommendation", "Do these tasks after an easy one. May also be a good final task for the day, because theyre still doable."}
            },
            new Dictionary<string, object> {
                {"Label", "hard"},
                {"Description", "Repetetive, boring, complex tasks and those, that you simply have no motivation for, are hard, because theyre usually hard to do."},
                {"Recommendation", "Do these tasks after a few smaller successes. If your motivation fails you, while doing the task, do a simpler one as a motivation booster."}
            }
        };

        private static List<Dictionary<string, object>> _priority = new List<Dictionary<string, object>>
        {
            new Dictionary<string, object> {
                {"Label", "very low"},
                {"Ordering", 15}
            },
            new Dictionary<string, object> {
                {"Label", "low"},
                {"Ordering", 23}
            },
            new Dictionary<string, object> {
                {"Label", "normal"},
                {"Ordering", 31}
            },
            new Dictionary<string, object> {
                {"Label", "high"},
                {"Ordering", 39}
            },
            new Dictionary<string, object> {
                {"Label", "very high"},
                {"Ordering", 47}
            }
        };

        private static List<Dictionary<string, object>> _type = new List<Dictionary<string, object>>
        {
            new Dictionary<string, object>
            {
                {"Label", "WORKTIME"}
            },
            new Dictionary<string, object>
            {
                {"Label", "PROCESSTIME"}
            },
            new Dictionary<string, object>
            {
                {"Label", "REMINDER"}
            },
            new Dictionary<string, object>
            {
                {"Label", "REPEATER"}
            },
            new Dictionary<string, object>
            {
                {"Label", "WORKTIME_LIMIT_WEEKLY"}
            },
            new Dictionary<string, object>
            {
                {"Label", "WORKTIME_LIMIT_MONTHLY"}
            },
            new Dictionary<string, object>
            {
                {"Label", "WORKTIME_LIMIT_MONDAY"}
            },
            new Dictionary<string, object>
            {
                {"Label", "WORKTIME_LIMIT_TUESDAY"}
            },
            new Dictionary<string, object>
            {
                {"Label", "WORKTIME_LIMIT_WEDNESDAY"}
            },
            new Dictionary<string, object>
            {
                {"Label", "WORKTIME_LIMIT_THURSDAY"}
            },
            new Dictionary<string, object>
            {
                {"Label", "WORKTIME_LIMIT_FRIDAY"}
            },
            new Dictionary<string, object>
            {
                {"Label", "WORKTIME_LIMIT_SATURDAY"}
            },
            new Dictionary<string, object>
            {
                {"Label", "WORKTIME_LIMIT_SUNDAY"}
            }
        };

        private static List<Dictionary<string, object>> _data = new List<Dictionary<string, object>>
        {
            new Dictionary<string, object>
            {
                {"Label", "RESERVED_NOCATEGORY"},
                {"Description", "All tasks, appointments and projects, which have not been added to a category. Only visible if content available."}
            },
            new Dictionary<string, object>
            {
                {"Label", "RESERVED_NOPROJECT"},
                {"Description", "All tasks and appointments, which have not been added to a project. Only visible if content available."}
            }
        };

        private static List<Dictionary<string, object>> _category = new List<Dictionary<string, object>>
        {
            new Dictionary<string, object>
            {
                {"DataId", new Dictionary<string, object> { {"RESERVED_TABLE", "Data"}, { "Label", "RESERVED_NOCATEGORY" } } },
                {"PriorityId", new Dictionary<string, object> { { "RESERVED_TABLE", "Priority"}, { "Label", "normal" } } }
            }
        };

        private static List<Dictionary<string, object>> _project = new List<Dictionary<string, object>>
        {
            new Dictionary<string, object>
            {
                {"CategoryId", new Dictionary<string, object> { { "RESERVED_TABLE", "Category" }, { "DataId", new Dictionary<string, object> { { "RESERVED_TABLE", "Data" }, { "Label", "RESERVED_NOCATEGORY" } } } } },
                {"DataId", new Dictionary<string, object> { { "RESERVED_TABLE", "Data" }, { "Label", "RESERVED_NOPROJECT" } } },
                {"PriorityId", new Dictionary<string, object> { { "RESERVED_TABLE", "Priority" }, { "Label", "normal" } } }
            }
        };

        public static List<Dictionary<string, object>> Difficulty { get => _difficulty; }
        public static List<Dictionary<string, object>> Priority { get => _priority; }
        public static List<Dictionary<string, object>> Type { get => _type; }
        public static List<Dictionary<string, object>> Data { get => _data; }
        public static List<Dictionary<string, object>> Category { get => _category; }
        public static List<Dictionary<string, object>> Project { get => _project; }

    }
}
