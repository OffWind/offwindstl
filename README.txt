OffWind Elevation
=================

The OffWind Elevation is a code for generating terrain data in the STL format[0]. The elevation data is fetched 
from the Google Elevation API[1].


Instruction for building the software
=====================================
The code is written in C# using Visual Studio 2010 and should compile "out of the box".


Usage
=====

The software is executed as a command line tool taking the following arguments:

--dms-latitude <DD:MM:SSSS>           WGS84 latitude[2]
--dms-longitude <DD:MM:SSSS>          WGS84 longitude
--num-cells-I <N>                     Number of I-cells in a IxJ grid to be generated
--num-cells-J <N>                     Number of J-cells in a IxJ grid to be generated
--distance-ew <d>                     Distance (in meters) from east to west
--distance-ns <d>                     Distance (in meters) from north to south
--output <filename>                   Output filename (in STL format)

Example:

elevation --dms-latitude 61:53:37.20 --dms-longitude 9:51:43.92 --num-cells-I 10 --num-cells-J 10 --distance-ew 2000.0 --distance-ns 2000.0 --output rondane.stl

Known limits
============
Please take note of the Usage Limits of the Google Elevation API. The current
implementaion of the OffWind Elevation does not handle grids larger than 10x10 cells well - although this could 
easily be extended by sending multiple requests to the Google service. As of this writing there is a limit of
2,500 requests per day, and no more than 25,000 locations total per day.

The DMS latitude is limited to the northern hemisphere.


[0] http://en.wikipedia.org/wiki/STL_(file_format)
[1] https://developers.google.com/maps/documentation/elevation/
[2] http://en.wikipedia.org/wiki/World_Geodetic_System