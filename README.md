# BEP_meta1
Building an application that lets the user create a virtual world using the Meta 1 that can be used to train rescuers in emergency flooding situations.

 
# 1 Project structure
## 1.1 Core
Core is the project which contains the core classes. These are classes like datatypes and utilities which are frequently used throughout the application.

### 1.1.1 Core.Test
This project tests the files in the Core project.

## 1.2 UserLocalisation
UserLocalisation is the project in which we try to locate the user in the map. The project contains classes for sensor input and filters to determine the location based on multiple sensor data.

### 1.2.1 UserLocalisation.Test
This project tests the files in the UserLocalisation project.

## 1.3 Unity
Unity is the project for everything that is related to Unity or Meta 1. This includes the classes that couple the sensors to the classes in UserLocalisation and handling the game engines world.

# 2 Dependencies
The three projects require dependencies of eachother as follows: (x->y means x is a dependency for y)
* Core -> Core.Test
* Core -> UserLocalisation
* Core -> Unity
* UserLocalisation -> UserLocalisation.Test
* UserLocalisation -> Unity

## 2.1 External Dependencies
The external third party dependencies for the projects. Between brackets the projects are mentioned which depend on these dependencies.

### 2.1.1 
* MathNet.Numerics v3.11.1 (Core, UserLocalisation, Unity)
* TaskParallelLibrary v1.0.2856.0, MathNet.Numerics dependency for .NET 3.5 target framework (Core, UserLocalisation, Unity)
* Meta SDK v1.3.4.308 (Unity)

### 2.1.2 Testing Dependencies
* NUnit v3.2.1 (Core.Test, UserLocalisation.Test)
* NUnit3TestAdapter v3.0.10, for running NUnit tests in Visual Studio (Core.Test, UserLocalisation.Test)
* Moq v4.2.1510.2205 (UserLocalisation.Test)

## 2.2 Build Order
The different projects have the following build order:
1. Core
2. Core.Test
3. UserLocalisation
4. UserLocalisation.Test
Note that the Unity project is not in the order, because the engine builds the code itself.
