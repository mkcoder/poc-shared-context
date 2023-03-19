using System.ComponentModel.DataAnnotations;

namespace poc_other_project_ms
{
    public class DependencyProjectionSync
    {
        [Key]
        public int Id { get; set; }
        public string DepenendencyName { get; set; }
        public DateTime LastSyncTime { get; set; }
    }
}

