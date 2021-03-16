using System.Collections.Generic;
using Newtonsoft.Json;

public class Config
{
    public string applicationID { get; set; }
    public string applicationSecret { get; set; }

    public List<SharpStatus> statusList { get; set; }

    public int updateInterval { get; set; } = 10;
}
