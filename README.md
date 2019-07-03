# PBallUnityArcore
Mini-project for the course User-Interface HES -SO MSE
The project consists of creating an Augmented reality game with with Unity and ARCore

First at starting we have to touch on the screen and the app detects the planed surface in our environment.
Once a surface is detected we should touch on a point on this to instanciate virtual components of the game on it
The 2nd touch instantiates the area game with border
The 3rd touch instantiates the hole 
The 4th touch instantiates the second ball which we have to move to the hole
The last toouch places the ball which is our player

To scale object except the surface , we can usethe pinch gesture
After placing and scaling object, we can start the game by swiping the ballplayer in order to touch the ball. On collision 
with the player, the ball move, the goal is to push the ball in the hole, once the ball is in contact of the hole,
it disapears and the player win, if the the ball player touch the hole it disapear too, and the game is over.
