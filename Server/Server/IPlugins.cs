﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Server
{
    public interface IPlugins
    {
        // ##########################################################################################################################################
        StreamWriter Writer
        {
            set;
        }

        // ##########################################################################################################################################
        string PluginName
        {
            get;
        }

        // ##########################################################################################################################################
        bool IsPlugin
        {
            get;
            set;
        }

        // ##########################################################################################################################################
        void Init(string[] parameter);

        // ##########################################################################################################################################
        void Run();
        
    }
}
