[ Unreleased ]

## Added

* Added a roadmap and previews for version 1.0 to the github page.

## Changed

* Restructered the github page, to make simpler to understand and find things quicker.

## Fixed

## Removed

# v0.1.0

## Added

* Added an update button, which can automatically update the app. (Get more information [here](./github-content/documentation/updates.md))

* Added a way to create tasks in projects.

* Added a way to create projects in categories.

* Added a way to create categories.

* Tasks can be started and stopped.

* Tasks are stored in a local database.

* Tasks, projects and categories can be deleted with right-click.

    * Deleting a project or a category deletes the content within.

* Set things like priority, difficulty, see how long you've been working on a task and change the taskname later.

* Get a detailed view of the task data, like when it was created, when it was last updated (changed) and so on.

## Deprecated

* SQlite/Database/Create.sql -> The control over the database is going to be fully written in code instead of sql files.