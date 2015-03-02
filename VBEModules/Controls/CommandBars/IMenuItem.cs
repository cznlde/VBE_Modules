﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VBEModules.Controls.CommandBars
{
    /// <summary>
    /// Defines interface for a menu item
    /// </summary>
    public interface IMenuItem
    {
        string Name { get; set; }
        string DisplayName { get; set; }
        int IconId { get; set; }
        double Order { get; set; }
        bool HasSeparator { get; set; }
        ICommand Command { get; set; }
    }
}