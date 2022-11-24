Name: Bogdan Tsyganok, student id: 0886354
video url: https://youtu.be/GGayzqiHufY
The camera is not attached to anything right now, please use the unity editor window
Unity project, please load with 2020.3.25f1. Assets/maps contains 3 bmp maps. Coordinator object, BMPLoader scrip has a string field in which you can write a different map name, exclude the .bmp extension.
Relevant Code can be found in BMPLoader for reading and creating a map and FuzzyWander for how calcSteering was determined. Please note the script doesn't actually use Wandering steering logic and instead relies entirely on the "feelers"

Thank you for reading!