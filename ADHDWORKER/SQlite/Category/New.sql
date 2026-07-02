INSERT INTO Data(Label, Description)
VALUES (
	@newLabel,
	@newDescription
);

INSERT INTO Categories(DataID, PriorityID)
VALUES (
	last_insert_rowid(),
	@selectedPriority
);

INSERT INTO Projects(DataID, CategoryID, PriorityID)
VALUES (
	(SELECT ID FROM Data WHERE Label = 'RESERVED Other'),
	last_insert_rowid(),
	@selectedPriority
);