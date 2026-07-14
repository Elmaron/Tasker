-- Initalise default values for the difficulties --
INSERT OR IGNORE INTO Difficulty(
	'easy',
	'These Tasks should be simple to do. This means, that they are mostly quick to do, simple or uncomplicated.',
	'Put these tasks at the beginning of a work phase (especially on work days with hard tasks).'
);

INSERT OR IGNORE INTO Difficulty(
	'normal',
	'Even though these tasks are not hard, you still might use a little boost to do them. This Category is for simple tasks, that you are not motivated for, and for tasks, which take a little longer.',
	'Do these tasks after an easy one. May also be a good final task for the day, because theyre still doable.'
);

INSERT OR IGNORE INTO Difficulty(
	'hard',
	'Repetetive, boring, complex tasks and those, that you simply have no motivation for, are hard, because theyre usually hard to do.',
	'Do these tasks after a few smaller successes. If your motivation fails you, while doing the task, do a simpler one as a motivation booster. Giving yourself a reward after doing a hard task could be a good way to keep your motivatio up in the future for hard tasks.'
);


-- Initalise default values for the priorites --
INSERT OR IGNORE INTO Priority(
	'very low',
	'15'
);

INSERT OR IGNORE INTO Priority(
	'low',
	'23'
);

INSERT OR IGNORE INTO Priority(
	'normal',
	'31'
);

INSERT OR IGNORE INTO Priority(
	'high',
	'39'
);

INSERT OR IGNORE INTO Priority(
	'very high',
	'47'
);


-- Initalise unchangable filters for the timings. --
INSERT OR IGNORE INTO Type(
	'WORKTIME'
);

INSERT OR IGNORE INTO Type(
	'PROCESSTIME'
);

INSERT OR IGNORE INTO Type(
	'REMINDER'

INSERT OR IGNORE INTO Type(
	'REPEATER'
);

INSERT OR IGNORE INTO Type(
	'WORKTIME_LIMIT_WEEKLY'
);

INSERT OR IGNORE INTO Type(
	'WORKTIME_LIMIT_MONTHLY'
);

INSERT OR IGNORE INTO Type(
	'WORKTIME_LIMIT_MONDAY'
);

INSERT OR IGNORE INTO Type(
	'WORKTIME_LIMIT_TUESDAY'
);

INSERT OR IGNORE INTO Type(
	'WORKTIME_LIMIT_WEDNESDAY'
);

INSERT OR IGNORE INTO Type(
	'WORKTIME_LIMIT_THURSDAY'
);

INSERT OR IGNORE INTO Type(
	'WORKTIME_LIMIT_FRIdAY'
);

INSERT OR IGNORE INTO Type(
	'WORKTIME_LIMIT_SATURDAY'
);

INSERT OR IGNORE INTO Type(
	'WORKTIME_LIMIT_SUNDAY'
);


-- Initalise structure for categories and projects. --
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