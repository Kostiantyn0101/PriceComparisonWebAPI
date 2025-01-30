using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Configuration
{
    public class FileStorageOptions
    {
        public string MediaFolder { get; set; }
        public string ImagesFolder { get; set; }
        public string InstructionsFolder { get; set; }
    }
}
