namespace RegOnline.RegressionTest.AutoIt
{
    using System.Diagnostics;

    public class UploadFile
    {
        public static void UploadAFile(string windowName, string fileSource)
        {
            Process p = new Process();
            p.StartInfo.FileName = "UploadAFile.exe";
            p.StartInfo.Arguments = "\""+ windowName + "\"" + " " + fileSource;
            p.StartInfo.UseShellExecute = false;
            p.Start();
            p.WaitForExit();
            System.Threading.Thread.Sleep(3000);
        }
    }
}
