# User Data

The app saves your data in a local database in the app storage of your operating system.

You can find the files in the following folder:

* Windows: %LocalAppData%\Tasker (Just copy and paste it into the top bar in the file explorer)

* Linux: ~/.local/share/Tasker (You can use `ls` in a terminal to see, if appData.db is present)

If you want to backup your data, you can simply copy "appData.db" into another folder. You can also rename it. If you want to recover your data, just copy your backup into the app-folder. Don't forget to change the name to "appData.db", if you changed it.
If you want to create a new, empty database, simply delete "appData.db" from the app-folder. The program is automatically creating a new database for you.