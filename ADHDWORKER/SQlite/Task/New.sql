INSERT INTO Data(Label, Description)
VALUES (
	@newLabel,
	@newDescription
);

INSERT INTO Task(DataID, ProjectID, PriorityID)
VALUES (
	last_insert_rowid(),
	@selectedProject,
	@selectedPriority
);