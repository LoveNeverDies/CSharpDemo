using System.IO;

namespace CSharpDemo
{
    public class TorrentMatching
    {
        public TorrentMatching()
        {

        }

        public void Start()
        {
            FileAttributes f = new FileAttributes();
            f = FileAttributes.Archive | FileAttributes.Directory;
            if (f.ToString().Contains(FileAttributes.Directory.ToString()))
                System.Console.WriteLine();
            string modifyPath = @"D:/SoftWare/HoneySelect/Mods";
            System.IO.DirectoryInfo directory = new DirectoryInfo(modifyPath);
            var dirChildren = directory.GetFileSystemInfos();
            foreach (var item in dirChildren)
            {
                CopyToOtherLocation(item.FullName);
            }
            //ClearTorrent();
        }

        public void ClearTorrent(string modifyPath = @"D:/SoftWare/HoneySelect/Mods")
        {
            System.IO.DirectoryInfo directory = new DirectoryInfo(modifyPath);
            foreach (var dir in directory.GetDirectories())
            {
                var dirChildren = dir.GetDirectories();
                if (dirChildren.Length > 0)
                {
                    ClearTorrent(dir.FullName);
                }
                else
                {
                    foreach (var file in dir.GetFiles())
                    {
                        string newName = file.FullName.Replace(".bt.xltd", "");
                        File.Move(file.FullName, newName);
                    }
                }
            }
        }

        public void CopyToOtherLocation(string modifyPath = @"D:/SoftWare/HoneySelect/Mods", string moveToPath = @"D:/SoftWare/HoneySelect/Data", string repetitionFile = @"D:/RepetitionFile")
        {
            System.IO.DirectoryInfo directory = new DirectoryInfo(modifyPath);
            foreach (var dir in directory.GetFileSystemInfos())
            {
                string moveOtherToPath = Path.Combine(moveToPath, dir.Name);
                if (dir.Attributes == FileAttributes.Directory)
                {
                    Directory.CreateDirectory(moveOtherToPath);
                    CopyToOtherLocation(Path.Combine(modifyPath, dir.Name), moveOtherToPath);
                }
                else
                {
                    if (File.Exists(moveOtherToPath) == false)
                        File.Move(dir.FullName, moveOtherToPath);
                    else
                    {
                        //不为空
                        using (FileStream fs = new FileStream(Path.Combine(repetitionFile, "RepetitionFile.txt"), FileMode.Append, FileAccess.Write))
                        {//创建写入文件
                            StreamWriter sw = new StreamWriter(fs);
                            sw.WriteLine(dir.FullName);//开始写入值
                            sw.WriteLine();
                            sw.Close();
                            fs.Close();
                        }
                    }//IF ELSE END
                }//IF ELSE END
            }//FOREACH END
        }//FUNCTION END

    }
}
