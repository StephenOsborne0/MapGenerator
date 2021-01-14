# MapGenerator
A console app to split a map image into tileset data.

Takes in a directory of image files as a parameter.
Image files should be something similar to this: https://static.wikia.nocookie.net/zelda/images/b/b2/Holodrum_Coordinate_Map.png/revision/latest?cb=20100430223108
It will be split into screens (the larger grid) and individual tiles. 
This then gets exported to a json file with the coordinates of all the tiles.
Also generates tilesets from the unique tiles.

Screen, tile and tileset sizes are currently hardcoded in the Constants.cs file.
