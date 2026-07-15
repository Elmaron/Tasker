# Tasker
A Task-Planner and Tracker to help you understand your own work behavior.

*Eine deutsche Übersetzung befindet sich [hier](https://github.com/Elmaron/Tasker/tree/master/more-information/readme-german.md).*

# Development Status
## Current
**WARNING! This project is not ready to use in it's current state.**

After rethinking the base-design of the program, I'll be working on providing basic functionality with the database, so that I can publish the first alpha version.
Follow me on github to stay updated on the project!

State: **[Alpha](#alpha)** | [Beta](#beta) | [Release](#release-v10)

*finished*; **in Progress**; Upcoming Phase

# Features
This task planning program uses a local database to track your tasks on your device. The program then uses the collected data, to help you plan new tasks or appointments, if you already did them in the past.

For people who find it difficult to "just do" a task, it gives advice on how to start with something and get productive, if that's a problem. Still, they are only recommendations and shall give you more perspectives to become better at doing the things, that "need to be done".

In the end, the program should help the user to decide between tasks, that need to be done and help them to do them.

## Statement to AI
Generally I'm *not* against new technology and this also relates to the topic of AI. **However** I find most use cases for AI today unneccessary and inappropriate. So here I want to explain, *how* and *if* I use AI during development and what to expect in the future about the usage of AI in this program.

### Development
During development AI can definitely boost productivity, by writing parts of the code itself. That is a fact, I **cannot** deny. Still, I dislike the idea of letting *something* else write my code. Instead, I use AI for the following things during development:
* To learn new commands
* To find commands, I'm unaware of
* To understand the basic usage of something new (command, packages, etc.)
* To help find the cause of an error, if I'm unable to find it

If I need to learn something new, for example how to use the avalonia package, I concider the documentation first. If I'm unable to understand the concepts, I use AI to help me understand them. It often leads to other commands and/or packages, that I do not know. So I repeat the process, until I understand enough of it to use it.

If I'm unable to find the problem after getting an error, I may ask AI, to help me identify the problem. But before I post my code to AI, I always try to rewrite the code a little, so AI doesn't as easily know, what I'm doing exactly (for example renaming variables, posting only what I need, aso.). Unfortunately, AI always presents a solution to the problem, without me asking for it. So instead, I mostly ignore them. I never copy and paste the code from an AI. If I use code snippets, I always review them first, understand them and then retype them in my own program. I do not use AI to refine the code.

I could use AI more, so I would be faster at everything. But I want to understand everything I'm doing. And I also don't want to get dumber. So that's why I'm using AI the way I do. I'll always try to understand the error, before considering to ask an AI for help. I'm also planning to use stackoverflow or similar in the future instead a lot more.

If you want to know more about this topic, you can write a [mail](mailto:info@vindona.de) (fastest answer).

### AI Assistant
I do **not** find the idea of an "AI Assistant" in the program off-putting. **But not** as a "I can ask you anything and you do anything"-kinda program. I also dislike the idea of using another AI program and asking a server or anything else for help. **Currently, I do not plan to add an AI Assistant to the program.**
But if I do, here is, what you can and cannot expect from it:

* The AI Assistant is going to be integrated within the program or available as a plugin.
* The Assistant can be turned off completely.
* The AI is only available locally and can not connect to other AIs.
    * If you run a selfhosted version for the broswer, the AI Assistant only runs on your own server and can not connect to other AIs.
* You'll always get transparent information on what the AI assistant does and what it doesn't.
    * The Assistant is an alternative to the algorithm to analyse your times for tasks better.
    * The Assistant can recommend you tasks (for different days), but does never automatically plan them for you.
    * The Assistant can **not** analyze the content of your tasks, to recommend you new tasks, that may still be needed.
    * You can not interact in a chatlike environment with the AI.

## Planned Features
### Alpha
- [x] Create Github Project
- [ ] Update design
    - [ ] Redefine layout and design
- [ ] Create basic functionality
    - [ ] Read and write to the database (through SQLite-commands)
    - [ ] Create classes and commands for interactions with the database
    - [ ] Create layout
    - [ ] Connect layout with commands and classes
- [ ] Refine code
- [ ] Fixing bugs

*Phase shall be finished until 17th of July 2026.*

### Beta
*More objectives may be added while working on the program*
- [ ] Update design
    - [ ] Create switchable designs
- [ ] New Features
    - [ ] Export Data
    - [ ] Algorithm to help you plan your new tasks and appointments
    - [ ] Automatically show hidden task-ids, to differentiate between tasks, which have the same title and are in the same category and project
    - [ ] Customizable settings
        - [ ] Shortcuts
        - [ ] Autodelete data
            - [ ] After a certain amount of time
            - [ ] After a certain amount of disk space is used
            - [ ] Deactivate Autodelete
        - [ ] Always show hidden task ids
- [ ] Optimisations for different user types
    - [ ] Standard
    - [ ] People with ADHD
- [ ] Refine code
- [ ] Fixing more bugs

### Release (V1.0)
*More objectives may be added while working on the program*
- [ ] Update design
    - [ ] Enable custom designs
- [ ] New Features
    - [ ] Plugins
    - [ ] New Languages
        - [ ] German
    - [ ] Task Merger
        - [ ] Function to merge selected tasks
        - [ ] automatically recommands tasks to merge, which are in the same category, same project and have the same title
            - [ ] option, to ignore those recommendations
            - [ ] filter, to show ignored recommendations
- [ ] Update Features
    - [ ] Refine Algorithm
- [ ] Create guides
    - [ ] Online user guide (detailed)
    - [ ] Built in guide for beginners
- [ ] Create documentations
    - [ ] For developer
        - [ ] program/app development
        - [ ] plugin development
    - [ ] For designer
- [ ] Refine code
- [ ] Fixing even more bugs

## Planned Features after release
*These are only ideas for now, nothing promised.*
- [ ] Selfhostable version for the browser
- [ ] Server synchronisation for database
- [ ] AI Assistant for new tasks (only locally on your machine)
- [ ] A few funny rewards for doing your tasks!
    - [ ] You can create them yourself or let the program recommend you something
    - [ ] The Program has built-in Mini-Games, which act as a reward
- [ ] Reminders for Worktimes during Work

*This text has been created without the usage of ai.*

**this project is developed by Elmaron from Vindona**
