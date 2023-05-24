# game-golfyou
2D platformer created as a deliverable for CS4730 at the University of Virginia  
I (Willl99) created the player logic, found the player sprite, created custom sprites for animations, created the physics logic, level logic using Tiled CS, and created collision logic, as well as the control logic.  
Overall concept worked well with some minor issues like the player being able to skip some parts of the level  
Biggest issue was the collision, as players can hit themselves through certain tiles if they're travelling fast enough, likely due to the collision logic not updating quickly enough to account for the player's high speed.  
  
Controls:  
a/d to move left/right  
space to enter putting mode, space again to choose angle, space again to choose velocity  
c to cancel out of putting mode  
q to change putting mode  
