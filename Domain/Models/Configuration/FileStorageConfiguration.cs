namespace Domain.Models.Configuration
{
    public class FileStorageConfiguration
    {
        public const string Position = "FileStorage";
        public string MediaFolder { get; set; }
        public string ImagesFolder { get; set; }
        public int ImageSizeMB { get; set; }        
        public string InstructionsFolder { get; set; }
    }
}
