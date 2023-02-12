using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASiNet.Searcher.API;
public struct SearchOptions
{
    public SearchOptions()
    {
        MaxFoldersVisualization = 10;
        MaxFoldersVisualization = 10;
    }

    public int MaxFoldersVisualization { get; set; }
    public int MaxFilesVisualization { get; set; }

}
