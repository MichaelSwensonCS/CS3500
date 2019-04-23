///-----------------------------------------------------------------
///   Description:    Readme file.
///   Author:     Michael Swenson and Seth Jackson    Date: 12/6/18
///-----------------------------------------------------------------

We had a hard time firguring out how the networking works till we found that we need to make two loops one to keep
getting the commands from the client and one to update the world. We had a problem with things going into overdrive
until we saw that it was modifying the world every client instead of just once per frame. We had a problem with
projectiles staying after hiting the edge. We found that we need to send alive = false first before removing it.
In our extra feature we had trouble with the negative y axis but after realising that the star wraped like it was
supose to.

12/6/18 - Ready For Grading.
12/6/18 - Additional comments
12/6/18 - Documentation, refactoring, and readability
12/5/18 - Merge branch master
12/5/18 - Unit Tests
12/5/18 - Add files Via Upload
12/5/18 - Read Me?
12/5/18 - Extra part done.
12/3/18 - Caused a problem. The ship was still being modified after death. Fixed now.
12/3/18 - Added ReadMe. Fixed when client leaves. Fixed projectiles hitting the sun.
11/30/18 - Fixed multiple client issue because instead of giving the update a socket we now start update after a 
client connects and then at the end of updating the world send it to every client.
11/30/18 - Fixed the score falling into the health bar, the issue was in receive name we weren't stripping the 
newline off so it was "name\n". Also fixed the projectiles not disappearing, this is because the client needs to see 
the projectile alive = false, we can't just remove it immediately so I added a counter that will remove projectiles 
after 100 frame updates so that the world doesn't get too full.
11/28/18 - projectile is kind of working counting 6 frames dosen't
11/27/18 - Fixed the problems we didn't have before.
11/27/18 - It shoots a bullet for about one frame and then the gui throws an error comment out the projectile 
stuff to get it back to flying around and working... 
(note on start I don't think you can rotate left and if you rotate right you just keep spinning)
11/27/18 - Thrust is working now, (I sorta broke the client by changing projectile from Dict<int proj> to 
Dict<int, List<proj>> but the server works( for thrust) after separating the logic out of process message. 
I basically added a dictionary for every command and it gets linked to that players ID we check if that key 
has commands waiting when we update, if it does we do those things then clear those commands.
11/27/18 - I am about to try and do some major over hauls on process message and update. 
This is the last current "working version" I have. Currently a projectile can be created and will track when it dies 
but the code is messy and the fire command seems to pause the server sometimes.
11/26/18 - Getting basic position calculations working, drawing the world, thrusting, currently having issues with firing.
11/26/18 - Started on server got up to approximately step 3 in the week 1 sprint.

Extra part: The sun moves right until it hits the edge of the screen then appears randomly on the bottom and moves
up till it hits the edge appears randomly on the left side of the screen moving right again. When multiple suns are
added it speeds up.