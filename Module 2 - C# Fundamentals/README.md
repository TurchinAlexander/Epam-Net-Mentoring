# C# Fundamentals

## 1. FileSystemVisitor

Create a class to show all files and subdirectories from the selected directory. When searching the files one of the four events can be raised:  
1. `FileFound`
Event is raised when a file is found inside the main directory or its' subdirectories.
2. `FileFilteredFound`  
Event is raise after the FileFound event when the filter is set up in FileSystemVisitor and the file has been successfully not filtered.
3. `DirectoryFound`  
Event is raised when a subdirectory is found inside the main directory or its' subdirectories.
4. `DirectoryFilteredFound`  
Event is raised after the DirectoryFound event when the filter is set up in FileSystemVisitor and the directory has been successfull not filtered.

Besides create a unit test to cover up FileSystemVisitor and check its' functionality.

## 2. File System Service

### Base Info
The project is used to set up the watch on the specified directories and when new file is transfered to the watched directories the job start looking through the filter to get information what it need to be done with the file.

All necessary work is done inside the configuration file under sections:
1. Console section (name - consoleSection)
    1. `cultureLocalization` - specify the language used to show the messages on the console. Either en-US or ru-RU.
2. Monitor section (name - monitorSection) - set up the service.  
    1. `Directories`  - set up the directories what need to be watched.  
        * `path` - you need to specify the path of the directory needed to be watched.
    2. Filters
    Set up the filters which should be used to determine what need to be done with incoming files.  
        * `pattern` - the pattern of file on which this filter should be used.  
        * `destinationFolder` - the output directory where the file should be put.  
        * `addPositionNumber` - true or false. Set up the number before file name in the output directory.  
        * `addMovementDate` - true or false. Set up the date when the file have been transfered to the output directory.  

To have the posibility to show message on the console in diffrent languages, the project uses .resx files to take culture variant messages to show.

## 3. Exception Hangling

There are two project in this section:
1. Show the first character of the string which an used entered. Handle the situation when the used enterd nothing.
2. Create custom string to int converter. To convert a developer can use TryConvert to have a posibility to check if the conversion has been done successfully. Or he can use Convert method but it will throw an exception when the input string is invalid.

## 4. Inversion of Control container

IoC container is used to connect the object to each other at Runtime without any need from developer side to explicitly use `new` clause in his project.

Main elements of the project:
1. Can be used to create value and reference types.
2. If you want to create a primitive object, you need no any additional moves to create it.
3. If you want to create an object to abstract class or an interface, you should firstly register them in container, using Register methods.
4. If you want to initialize the properties of an object, you can you can mark it by using `InitAttribute`.
5. If a class has a property of its class and marked for initialization, the container will throw CircularReference exception.
6. The container can register object as singleton. That means that when you take such an object, you local parameters will have same address.
