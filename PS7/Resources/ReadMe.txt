///-----------------------------------------------------------------
///   Description:    Readme file.
///   Author:     Michael Swenson and Seth Jackson    Date: 11/17/18
///-----------------------------------------------------------------

Our plan is to work with together to finish the networking and world objects.

Had a problem where Visual studio didn't see Json at all. Finally relised that I needed to uninstall and 
reinstall it.
We couldn't figure out why nothing was drawing until we saw that we were using Invalidate on the form not the
drawing panel.
We were having trouble using multiple keys so we used four bool values and changed them to true when that key
had a key down event and false when it had a key up event and just tested which values were true every time we
got a message from the server.
The panels in the score borad kept leaving when a ship was killed. We found that the ship was being taken out of
the world object when hp was zero and the panel was conected.


Polish**  The scoreboard sorts by the highest player score and also changes color to red when the player has 1 hp left.