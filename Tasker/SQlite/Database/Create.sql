-- Create table to store basic data for everything --
CREATE TABLE IF NOT EXISTS Data (
    Id INTEGER PRIMARY KEY,

    Label TEXT NOT NULL CHECK(LENGTH(Label) <= 128),
    Description TEXT CHECK(LENGTH(Description <= 4096)),
    Created DATETIME2 DEFAULT CURRENT_TIMESTAMP NOT NULL,
    Updated DATETIME2 DEFAULT CURRENT_TIMESTAMP NOT NULL,
    Finished DATETIME2,
    DeleteOn DATETIME2
);



-- Create table for different difficulty stages --
CREATE TABLE IF NOT EXISTS Difficulty (
    Id INTEGER PRIMARY KEY,

    Label TEXT NOT NULL UNIQUE CHECK(LENGTH(Label) <= 128),
    Description TEXT CHECK(LENGTH(Description <= 4096)),
    Recommendation TEXT CHECK(LENGTH(Description <= 1024))
);

-- Create table for the different priority stages --
CREATE TABLE IF NOT EXISTS Priority (
    Id INTEGER PRIMARY KEY,

    Label TEXT NOT NULL UNIQUE CHECK(LENGTH(Label) <= 128),
    Ordering INTEGER NOT NULL UNIQUE CHECK(Ordering BETWEEN 0 AND 63)
);

-- Create table for the different types, that a timing can have. Primary used as a filter --
CREATE TABLE IF NOT EXISTS Type (
    Id INTEGER PRIMARY KEY,

    Label TEXT NOT NULL UNIQUE CHECK(LENGTH(Label) <= 128)
);



-- Create every variant of a time, that needs to be stored, like the start and end time for a task or for a worktime, etc. --
CREATE TABLE IF NOT EXISTS Timing (
    Id INTEGER PRIMARY KEY,

    TypeId INTEGER NOT NULL,

    Start DATETIME2,
    End DATETIME2,

    FOREIGN KEY(TypeId) REFERENCES Type(Id)
);

-- Create a table for specific things, that repeat (like a task for example) --
CREATE TABLE IF NOT EXISTS Repeater (
    Id INTEGER PRIMARY KEY,

    TypeId INTEGER NOT NULL,

    MonthlyInterval INTEGER CHECK(MonthlyInterval BETWEEN 0 AND 511),
    DailyInterval INTEGER NOT NULL CHECK(DailyInterval BETWEEN 0 AND 511),

    FOREIGN KEY(TypeId) REFERENCES Type(Id)
);



-- Create a table for all categories, that the user can create and switch between for different tasks --
CREATE TABLE IF NOT EXISTS Category (
    Id INTEGER PRIMARY KEY,

    DataId INTEGER NOT NULL,
    PriorityId INTEGER NOT NULL,

    FOREIGN KEY(DataId) REFERENCES Data(Id),
    FOREIGN KEY(PriorityId) REFERENCES Priority(Id)
);

-- Create a table for all projects, which may exist in different categories --
CREATE TABLE IF NOT EXISTS Project (
    Id INTEGER PRIMARY KEY,

    DataId INTEGER NOT NULL,
    CategoryId INTEGER NOT NULL,
    PriorityId INTEGER NOT NULL,

    Expiry DATETIME2,

    FOREIGN KEY(DataId) REFERENCES Data(Id),
    FOREIGN KEY(CategoryId) REFERENCES Category(Id),
    FOREIGN KEY(PriorityId) REFERENCES Priority(Id)
);

-- Create a table for all appointments, which may or may not exist within a project. in the database structure, they always have a project assigned to them. --
CREATE TABLE IF NOT EXISTS Appointment (
    Id INTEGER PRIMARY KEY,

    DataId INTEGER NOT NULL,
    ProjectId INTEGER NOT NULL,

    FOREIGN KEY(DataId) REFERENCES Data(Id),
    FOREIGN KEY(ProjectId) REFERENCES Project(Id)
);

-- Create a table for all tasks. All tasks are asssigned to a project within the database structure.
CREATE TABLE IF NOT EXISTS Task (
    Id INTEGER PRIMARY KEY,

    DataId INTEGER NOT NULL,
    ProjectId INTEGER NOT NULL,
    PriorityId INTEGER NOT NULL,
    DifficultyId INTEGER,

    Expiry DATETIME2,

    FOREIGN KEY(DataId) REFERENCES Data(Id),
    FOREIGN KEY(ProjectId) REFERENCES Project(Id),
    FOREIGN KEY(PriorityId) REFERENCES Priority(Id),
    FOREIGN KEY(DifficultyId) REFERENCES Difficulty(Id)
);


--  Create a table for all limits, you can set for a specific category. --
CREATE TABLE IF NOT EXISTS Worktimelimit (
    Id INTEGER PRIMARY KEY,

    CategoryId INTEGER NOT NULL,
    TypeId INTEGER NOT NULL,

    LimitInMinutes INTEGER NOT NULL CHECK(LimitInMinutes BETWEEN 5 AND 4096),

    FOREIGN KEY(CategoryId) REFERENCES Category(Id),
    FOREIGN KEY(TypeId) REFERENCES Type(Id)
);



-- Connections for different m to n relations --
CREATE TABLE IF NOT EXISTS TimingInCategory (
    TimingId INTEGER,
    CategoryId INTEGER,

    PRIMARY KEY (TimingId, CategoryId),

    FOREIGN KEY (TimingId) REFERENCES Timing(Id),
    FOREIGN KEY (CategoryId) REFERENCES Category(Id)
);

CREATE TABLE IF NOT EXISTS TimingInProject (
    TimingId INTEGER,
    ProjectId INTEGER,
            
    PRIMARY KEY (TimingId, ProjectId),
            
    FOREIGN KEY (TimingId) REFERENCES Timing(Id),
    FOREIGN KEY (ProjectId) REFERENCES Project(Id)
);

CREATE TABLE IF NOT EXISTS TimingInAppointment (
    TimingId INTEGER,
    AppointmentId INTEGER,
            
    PRIMARY KEY (TimingId, AppointmentId),
            
    FOREIGN KEY (TimingId) REFERENCES Timing(Id),
    FOREIGN KEY (AppointmentId) REFERENCES Appointment(Id)
);

CREATE TABLE IF NOT EXISTS TimingInTask (
    TimingId INTEGER,
    TaskId INTEGER,
            
    PRIMARY KEY (TimingId, TaskId),
            
    FOREIGN KEY (TimingId) REFERENCES Timing(Id),
    FOREIGN KEY (TaskId) REFERENCES Task(Id)
);