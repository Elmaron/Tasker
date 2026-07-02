INSERT OR IGNORE INTO Data(Label, Description)
VALUES (
	@newLabel,
	@newDescription
);

INSERT INTO Projects(DataID, CategoryID, PriorityID)
VALUES (
	@last_insert_rowid(),
	@selectedCategory,
	@selectedPriority
);