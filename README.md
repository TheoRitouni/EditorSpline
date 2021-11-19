# EditorSpline
Mathematic project / Unity c#

## Resume

This study project is a plugin that can produce splines on Unity. 
It was realized by Théo Ritouni student at Isart Digital.
This plugin has been developed on Unity in C#.
Version of Unity : 2020.3.17f

With this tool you can produce different types of spline.
List of spline types :
	- Bézier
	- Hermite
	- B Spline
	- Catmull Rom

## How to us it 

To use the plugin you just have to create an empty "GameObject" and add to it
the script EditorSpline.cs located in the project at the following path "Assets\ScriptsEditorSpline.cs".

Once the script is associated with the "GameObject" you can see the interface of the plugin.
It is composed of several distinct parts with their options.

Here is a description of the parts and their options to help you.

- **Select Spline** in this section you can select the type of spline you want to model.  

- **Point Control** this section allows you to create the control points that influence your spline.
The first piece of information available to you is the number of control points you have created.
***Warning*** on BSpline and CatmullRom splines you need a minimum of four control points. 
Then you have the list of points with their coordinates and a button to delete the point.
Just below you have the button to add points.  

- **Object Movement** you have the possibility to select a "GameObject" to move it according to the curve.
The first option is a boolean to activate the move. The next one is an area to place the object you want to move.
you want to move. And finally the speed of the moving object.
***Warning*** the object moves only when you launch Unity in play mode.

- **Manager Data** The last section allows you to save the curve at the moment you click on the save button.
The second button allows you to return to the first state.

In the provided project you have an example map at the following path "Assets\Scenes\ExampleSpline.unity".

## Features

- Hermite, Bézier, BSpline, CatmullRom point calculation
- Display of curves
- Real time modification of control points
- Saving and loading data
- Animation of an object along a spline
- Add / Remove control points
- Changing the type of spline in real time

## To Do

- Plus d'options 
- Retravailler le code 
- Documentation