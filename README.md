# 3D COMPUTER GRAPHICS AND ANIMATION
# PROGRAMMING ASSIGNMENT 15

## Visible Surface Detection: Warnock & Scan Line Algorithms
## Difficulty: 5/5

Create an application that allows the user to manipulate two pyramids (one triangular pyramid, one square pyramid). You decide the object coordinates of both pyramids. The pyramids are represented with traingle meshes. The faces of the pyramids must have different colors. Both pyramids are placed on the world coordinate system (WCS) which is viewed with a perspective projection. You decide the initial position of the pyramids on the WCS.

The program must allow the user to:
+ Move both pyramids along the x, y, and z axes.
+ Rotate both pyramids (on the x, y, and z axis). The user must be able to move and rotate each pyramid separately.
+ Perform back face culling and visible surface detection on the pyramids. Only the faces facing the viewer is seen. If a pyramid should be in front of the other, part of the rear pyramid which is obstructed by the front pyramid is hidden. 
++ The visible surface detection methods used are the Warnock and Scan Line algorithms.
+ Allow the user to add other objects to the program (by loading an object from a file). 
+ Bonus points for user friendliness. Negative points for extreme user unfirendliness.
