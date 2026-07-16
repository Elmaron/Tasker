-- Initalise default values for the difficulties --
INSERT OR IGNORE INTO Difficulty(Label, Description, Recommendation)
VALUES (
	'easy',
	'These Tasks should be simple to do. This means, that they are mostly quick to do, simple or uncomplicated.',
	'Put these tasks at the beginning of a work phase (especially on work days with hard tasks).'
);

INSERT OR IGNORE INTO Difficulty(Label, Description, Recommendation)
VALUES (
	'normal',
	'Even though these tasks are not hard, you still might use a little boost to do them. This Category is for simple tasks, that you are not motivated for, and for tasks, which take a little longer.',
	'Do these tasks after an easy one. May also be a good final task for the day, because theyre still doable.'
);

INSERT OR IGNORE INTO Difficulty(Label, Description, Recommendation)
VALUES (
	'hard',
	'Repetetive, boring, complex tasks and those, that you simply have no motivation for, are hard, because theyre usually hard to do.',
	'Do these tasks after a few smaller successes. If your motivation fails you, while doing the task, do a simpler one as a motivation booster. Giving yourself a reward after doing a hard task could be a good way to keep your motivatio up in the future for hard tasks.'
);


-- Initalise default values for the priorites --
INSERT OR IGNORE INTO Priority(Label, Ordering)
VALUES (
	'very low',
	'15'
);

INSERT OR IGNORE INTO Priority(Label, Ordering)
VALUES (
	'low',
	'23'
);

INSERT OR IGNORE INTO Priority(Label, Ordering)
VALUES (
	'normal',
	'31'
);

INSERT OR IGNORE INTO Priority(Label, Ordering)
VALUES (
	'high',
	'39'
);

INSERT OR IGNORE INTO Priority(Label, Ordering)
VALUES (
	'very high',
	'47'
);


-- Initalise unchangable filters for the timings. --
INSERT OR IGNORE INTO Type(Label)
VALUES (
	'WORKTIME'
);

INSERT OR IGNORE INTO Type(Label)
VALUES (
	'PROCESSTIME'
);

INSERT OR IGNORE INTO Type(Label)
VALUES (
	'REMINDER'
);

INSERT OR IGNORE INTO Type(Label)
VALUES (
	'REPEATER'
);

INSERT OR IGNORE INTO Type(Label)
VALUES (
	'WORKTIME_LIMIT_WEEKLY'
);

INSERT OR IGNORE INTO Type(Label)
VALUES (
	'WORKTIME_LIMIT_MONTHLY'
);

INSERT OR IGNORE INTO Type(Label)
VALUES (
	'WORKTIME_LIMIT_MONDAY'
);

INSERT OR IGNORE INTO Type(Label)
VALUES (
	'WORKTIME_LIMIT_TUESDAY'
);

INSERT OR IGNORE INTO Type(Label)
VALUES (
	'WORKTIME_LIMIT_WEDNESDAY'
);

INSERT OR IGNORE INTO Type(Label)
VALUES (
	'WORKTIME_LIMIT_THURSDAY'
);

INSERT OR IGNORE INTO Type(Label)
VALUES (
	'WORKTIME_LIMIT_FRIdAY'
);

INSERT OR IGNORE INTO Type(Label)
VALUES (
	'WORKTIME_LIMIT_SATURDAY'
);

INSERT OR IGNORE INTO Type(Label)
VALUES (
	'WORKTIME_LIMIT_SUNDAY'
);


-- Initalise structure for categories and projects. --
-- DEPRECATED: just for testing because of bug, is going to be moved to code in the future --
INSERT OR IGNORE INTO Data(Label, Description)
VALUES (
	'RESERVED_NOCATEGORY',
	'All tasks, appointments and projects, which have not been added to a category. Only visible if content available.'
);

INSERT OR IGNORE INTO Category(DataId, PriorityId)
VALUES (
	last_insert_rowid(),
	(SELECT Id FROM Priority WHERE Label = 'normal')
);

INSERT OR IGNORE INTO Data(Label, Description)
VALUES (
	'RESERVED_NOPROJECT',
	'All tasks and appointments, which have not been added to a project. Only visible if content available.'
);

INSERT OR IGNORE INTO Project(DataId, CategoryId, PriorityId)
VALUES (
	last_insert_rowid(),
	(SELECT Id FROM Category WHERE DataId = (SELECT Id FROM Data WHERE Label = "RESERVED_NOCATEGORY")),
	(SELECT Id FROM Priority WHERE Label = 'normal')
);

INSERT OR IGNORE INTO Data(Label, Description)
VALUES (
	'Just a testproject',
	'just for the purpose of testing the code :)'
);

INSERT OR IGNORE INTO Project(DataId, CategoryId, PriorityId)
VALUES (
	last_insert_rowid(),
	(SELECT Id FROM Category WHERE DataId = (SELECT Id FROM Data WHERE Label = "RESERVED_NOCATEGORY")),
	(SELECT Id FROM Priority WHERE Label = 'normal')
);

INSERT OR IGNORE INTO Task(DataId, ProjectId, PriorityId)
VALUES (
	(SELECT Id FROM Data WHERE Label = 'Just a testproject'),
	last_insert_rowid(),
	(SELECT Id FROM Priority WHERE Label = 'normal')
);