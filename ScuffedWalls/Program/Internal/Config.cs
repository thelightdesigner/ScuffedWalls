namespace ScuffedWalls
{
    public partial class Config
    {
        public string MapFolderPath { get; set; }
        public string SWFilePath { get; set; }
        public string InfoPath { get; set; }
        public string MapFilePath { get; set; }
        public string OldMapPath { get; set; }
        public bool IsAutoImportEnabled { get; set; }
        public bool IsBackupEnabled { get; set; }
        public bool IsAutoSimplifyPointDefinitionsEnabled { get; set; }
        public bool PrettyPrintJson { get; set; }
        public Backup BackupPaths { get; set; }
        public class Backup
        {
            public string BackupFolderPath { get; set; }
            public string BackupSWFolderPath { get; set; }
            public string BackupMAPFolderPath { get; set; }
        }

    }



}
