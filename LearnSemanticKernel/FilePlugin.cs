using Microsoft.SemanticKernel;

namespace LearnSemanticKernel;

public class FilePlugin
{
    public string RootFolder { get; set; }

    public FilePlugin()
    {
        RootFolder = @"c:\sk-temp";
        if (!Directory.Exists(RootFolder))
        {
            Directory.CreateDirectory(RootFolder);
        }
    }

    [KernelFunction("get_root_folder")]
    public string GetRootFolder()
    {
        return RootFolder;
    }

    [KernelFunction("create_folder")]
    public void CreateFolder(string folderPath)
    {
        Guard(folderPath);
        Directory.CreateDirectory(folderPath);
    }

    [KernelFunction("create_file")]
    public void CreateFile(string filePath, string content)
    {
        Guard(filePath);
        File.WriteAllText(filePath, content);
    }

    [KernelFunction("get_content_of_file")]
    public string GetContentOfFile(string filePath)
    {
        Guard(filePath);
        return File.ReadAllText(filePath);
    }

    [KernelFunction("move_file")]
    public void MoveFile(string sourceFilePath, string targetFilePath)
    {
        Guard(sourceFilePath);
        Guard(targetFilePath);
        File.Move(sourceFilePath, targetFilePath);
    }

    [KernelFunction("delete_file")]
    public void DeleteFile(string filePath)
    {
        Guard(filePath);
        File.Delete(filePath);
    }

    [KernelFunction("move_folder")]
    public void MoveFolder(string sourceFolderPath, string targetFolderPath)
    {
        Guard(sourceFolderPath);
        Guard(targetFolderPath);
        Directory.Move(sourceFolderPath, targetFolderPath);
    }

    [KernelFunction("delete_folder")]
    public void DeleteFolder(string folderPath)
    {
        Guard(folderPath);
        Directory.Delete(folderPath);
    }

    [KernelFunction("get_files_for_folder")]
    public string[] GetFiles(string folderPath)
    {
        Guard(folderPath);
        return Directory.GetFiles(folderPath);
    }

    [KernelFunction("get_folders_for_folder")]
    public string[] GetFolders(string folderPath)
    {
        Guard(folderPath);
        return Directory.GetDirectories(folderPath);
    }

    private void Guard(string folderPath)
    {
        if (!folderPath.StartsWith(RootFolder))
        {
            throw new Exception("No you don't!");
        }
    }
}