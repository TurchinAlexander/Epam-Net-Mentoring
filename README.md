# Epam-Net-Mentoring

## 2. C# Fundamentals

### 2.1. FileSystemVisitor

Create a class to show all files and subdirectories from the selected directory. When searching the files one of the four events can be raised:  
1. FileFound
2. FileFilteredFound
3. DirectoryFound
4. DirectoryFilteredFound

<b>FileFound</b> event is raised when a file is found inside the main directory or its' subdirectories.

<b>FileFilteredFound</b> event is raise after the FileFound event when the filter is set up in FileSystemVisitor and the file has been successfully not filtered.

<b>DirectoryFound</b> event is raised when a subdirectory is found inside the main directory or its' subdirectories.

<b>DirectoryFilteredFound</b> event is raise after the DirectoryFound event when the filter is set up in FileSystemVisitor and the directory has been successfull not filtered.

Besides create a unit test to cover up FileSystemVisitor and check its' functionality.