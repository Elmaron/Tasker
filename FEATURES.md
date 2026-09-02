# FEATURES

This task planning program uses a local database to track your tasks on your device. The program then uses the collected data, to help you plan new tasks or appointments, if you already did them in the past.

For people who find it difficult to "just do" a task, it gives advice on how to start with something and get productive, if that's a problem. Still, they are only recommendations and shall give you more perspectives to become better at doing the things, that "need to be done".

In the end, the program should help the user to decide between tasks, that need to be done and help them to do them.

## Planned

The current alpha is going to be reworked into a program with different tabs, which is going to be the final structure for version 1.0. Everything is going to be organised a little more easy, so you're not getting overwhelmed by the sheer amount of functions, this program should be able to do in version 1.0. Tasker is meant to be a "simple by default, powerfull and customizable at will" program. Please make sure to leave Feedback, if you find certain things helpful or unhelpful. The program is going to be organised into the following tabs:

### Organise

The alpha 0.1 view gets reworked into the organise tab, so that you can organise everything for yourself. It contains the same detailed view for tasks, but also for appointments, projects and categories.

![Preview for version 1.0 - Organise Tab](./github-content/images/PROGRAM%20PREVIEW/ORGANISE%20Tab.jpg)

### Plan

The program also gets a plan tab, so that you can plan beforehand, when to do what task. The program is going to able to predict, which tasks can and should be done on specific days. If you've already done a task a few times, the program is going to predict, how many tasks or/and appointments you can do on that day.

![Preview for version 1.0 - Plan Tab](./github-content/images/PROGRAM%20PREVIEW/PLAN%20Tab.jpg)

### Perform

The perform tab is the most interesting for you, when you start with your tasks or appointments. It's a very customizable view, that presents you information about your upcoming tasks. If you tend to feel overwhelmed by the sheer amount of tasks you still need to do, you can simply let the program show you less tasks in this view.

![Preview for version 1.0 - Perform Tab](./github-content/images/PROGRAM%20PREVIEW/PERFORM%20Tab.jpg)

While doing your tasks the perform tab might still feel overwhelming, when you start working. That's when the concentration mode comes in. You can use this mode, while working on just a few tasks. Here you can only view up to three tasks, start and stop them and exit the mode. You will be able to customize this mode in the perform or settings tab.

![Preview for version 1.0 - Perform Tab using the concentration mode](./github-content/images/PROGRAM%20PREVIEW/PERFORM%20Tab%20Concentration%20Mode.jpg)

### Export

The export tab is a way to export your data into a visual document, so you can view or share statistics about your work behaviour. You can use predefined export templates or even create your own. For now the planned export-formats are PDF, CSV and odf.

### Overview

The overview tab gives you simple statistics about your recent work behaviour, often repeated tasks and so on. You can fully customize this tab to your liking.

### Settings

The program is going to be very customizable. A lot of options are going to be available in the released version, like being able to change the labels or the count of the priority and difficulty functions. You can change and load custom themes and colors.

### Plugins

Tasker is going to implement a way to support plugins in version 1.0. It's going to be one of the last features, that is going to be implemented, but a few standard functions come as plugins, so you can easily disable them, if you don't want to usse them at all.

## Available Features

Create or delete categories and projects. Add tasks into projects and start recording the time you need on specific tasks! Everything is stored in a local database on your machine. Get details about the tasks, like when it was created or at what times you worked on it. 

## Statement

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