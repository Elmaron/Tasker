INSERT OR IGNORE INTO Difficulties(
	'leicht',
	'Diese Aufgaben sollten leicht zu erledigen sein. Dazu gehoeren in erster Linie kurze, kleine und unkomplizierte Aufgaben.',
	'Diese Aufgaben (besonders an schweren Tagen) am besten an den Anfang einer Arbeitsphase setzen.'
);

INSERT OR IGNORE INTO Difficulties(
	'mittel',
	'Diese Aufgaben sind noch nicht schwer zu erledigen, allerdings kostet es moeglicherweise trotzdem etwas Ueberwindung, diese anzufangen. Leichte Aufgaben, für die weniger Motivation vorhanden ist, sowie etwas laenger andauernde Aufgaben fallen in diese Kategorie.',
	'Diese Aufgaben am besten erst nach einer leichten beginnen. Eignet sich auch gut für den Abschluss des Tages, da sie trotzdem schaffbar sein sollten.'
);

INSERT OR IGNORE INTO Difficulties(
	'schwer',
	'Bei diesen Aufgaben handelt es sich vor allem um Aufgaben, bei denen die Motivation nicht da ist, die sehr komplex oder nur sehr langweilig sind oder als repetetiv gelten.',
	'Diese Aufgaben sollten erst nach einigen kleineren Erfolgserlebnissen begonnen werden. Falls die Motivation zwischendurch wegfaellt, koennte eine leichtere Aufgabe die Motivation moeglicherweise wieder herstellen. Belohnungen nach diesen Aufgaben koennten hilfreich sein.'
);



INSERT OR IGNORE INTO Priorities(
	'sehr niedrig',
	'15'
);

INSERT OR IGNORE INTO Priorities(
	'niedrig',
	'23'
);

INSERT OR IGNORE INTO Priorities(
	'normal',
	'31'
);

INSERT OR IGNORE INTO Priorities(
	'hoch',
	'39'
);

INSERT OR IGNORE INTO Priorities(
	'sehr hoch',
	'47'
);



INSERT OR IGNORE INTO Types(
	'Arbeitszeit'
);

INSERT OR IGNORE INTO Types(
	'Bearbeitungszeit'
);

INSERT OR IGNORE INTO Types(
	'Erinnerung'

INSERT OR IGNORE INTO Types(
	'Wiederholung'
);

INSERT OR IGNORE INTO Types(
	'maximale Arbeitszeit pro Woche'
);

INSERT OR IGNORE INTO Types(
	'maximale Arbeitszeit pro Monat'
);

INSERT OR IGNORE INTO Types(
	'maximale Arbeitszeit Montag'
);

INSERT OR IGNORE INTO Types(
	'maximale Arbeitszeit Dienstag'
);

INSERT OR IGNORE INTO Types(
	'maximale Arbeitszeit Mittwoch'
);

INSERT OR IGNORE INTO Types(
	'maximale Arbeitszeit Donnerstag'
);

INSERT OR IGNORE INTO Types(
	'maximale Arbeitszeit Freitag'
);

INSERT OR IGNORE INTO Types(
	'maximale Arbeitszeit Samstag'
);

INSERT OR IGNORE INTO Types(
	'maximale Arbeitszeit Sonntag'
);



INSERT OR IGNORE INTO Data(Label, Description)
VALUES (
	'RESERVED General',
	'Alle Aufgaben und Projekte, die keiner Kategorie zugeordnert wurden. Wird nur angezeigt, wenn es Inhalt gibt.'
);

INSERT OR IGNORE INTO Categories(DataID, PriorityID)
VALUES (
	last_insert_rowid(),
	(SELECT ID FROM Priorities WHERE Label = 'normal')
);

INSERT OR IGNORE INTO Data(Label, Description)
VALUES (
	'RESERVED Other',
	'Alle Aufgaben und Termine, die keinem Projekt zugeordnert wurden. Wird nur angezeigt, wenn es Inhalt gibt.'
);

INSERT OR IGNORE INTO Projects(DataID, CategoryID, PriorityID)
VALUES (
	last_insert_rowid(),
	(SELECT ID FROM Categories WHERE DataID = (SELECT ID FROM Data WHERE Label = "RESERVED General")),
	(SELECT ID FROM Priorities WHERE Label = 'normal')
);