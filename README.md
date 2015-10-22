Speeching Server
================

**Speeching summary:**

>This application has been made as a tool for those who wish to get feedback about their speech. By completing the various available practice tasks on a regular basis you should be able to track your progress towards the goal of having clearer speech.

>While using the application you can complete various simple scenarios, assessments and tasks. These activities have been designed to help assess your speech clarity and record your vocal responses. Upon completion of an activity, you can choose to upload these recordings. This is where the fun begins!

![Speeching structure](https://raw.githubusercontent.com/GSDan/Speeching_Client/master/Droid_PeopleWithParkinsons/Resources/drawable/diagram.png)

>If you choose to upload your recordings, they will be distibuted anonymously to a 'crowd' of users. These users will not know who you are, nor will you know who they are. They'll be asked to give feedback on various aspects of your speech, such as clarity, pacing and pitch. We then find the average for these statistics and return them to you!

>Soon after submitting your results for an activity, your main feed will start to fill up with 'cards' like the one above. Each card will contain a measurement of a different area of speech. Use these measurements and their accompanying tips to help improve your speech!

**Speeching Server**

This repository contains the serverside component, built in C# with asp.net

The serverside portion of the Speeching project serves the mobile application users with tasks and scenarios to complete and posts the users' responses to CrowdFlower for feedback. This feedback is then collected and presented back to the user in an easily digestible manner.

The serverside application is currently only an API, with no webview implementation.

In order to successfully deploy, web config files (containing your server's connection strings) and static class containing various login details must be provided. These files have been ignored from the repository for security purposes.