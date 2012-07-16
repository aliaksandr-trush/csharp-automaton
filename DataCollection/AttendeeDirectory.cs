namespace RegOnline.RegressionTest.DataCollection
{
    public class AttendeeDirectory
    {
        public AttendeeDirectory(string directoryName)
        {
            this.DirectoryName = directoryName;
        }

        public string DirectoryName;
        public bool ShareDirectory;
        public bool RequireLogin;
    }
}
