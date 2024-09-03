using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Data
{
    private static Data gi;
    public static Data Gi()
    {
        if (gi == null) gi = new Data(); 
        return gi;
    }
    public Setting Setting { get; set; }
    public School_Schedule Schedule { get; set; }
    public Calendar_personal Personal { get; set; }

}
