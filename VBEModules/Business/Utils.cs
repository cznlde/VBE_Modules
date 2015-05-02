﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.Vbe.Interop;
using VbeComponents.Resources;

namespace VbeComponents.Business
{
    public static class Utils
    {
        /// <summary>
        /// Checks if given path exists
        /// TODO: Make it faster for network paths
        /// </summary>
        /// <param name="path">a path to check</param>
        /// <returns>True if the path exists, otherwise False</returns>
        public static bool PathExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Check if at least one file in given path has extension of a VBA component
        /// The extension can be cls, bas, frm
        /// </summary>
        /// <param name="path">path to a folder where to check components</param>
        /// <returns>True if at least one file is valid VBA component, otherwise false</returns>
        /// <exception cref="IOException">thrown if given path doesn't exists</exception>
        public static bool HasComponent(string path)
        {
            if (!PathExists(path)) throw  new IOException(string.Format(strings.FolderDoesnExists, path));
            IEnumerable<string> files = Directory.EnumerateFiles(path);
            return files.Any(x => x.EndsWith(".cls") || x.EndsWith(".bas") || x.EndsWith(".frm"));
        }

        public static string DisplayFolderDialog()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog { ShowNewFolderButton = true };
            DialogResult result = fbd.ShowDialog();
            return result == DialogResult.OK ? fbd.SelectedPath : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IEnumerable<Component> GetComponents(string path )
        {
            IEnumerable<string> files = Directory.EnumerateFiles(path).Where(x => x.EndsWith(".cls") || x.EndsWith("frm") || x.EndsWith(".bas") );
            var enumerable = files as string[] ?? files.ToArray();
            if (!enumerable.Any()) return null;

            List<Component> components = new List<Component>();
            foreach (string file in enumerable)
            {
                string ext = file.Substring(file.Length - 3, 3);
                string name = Path.GetFileName(file);
                switch (ext)
                {
                    case "bas":
                        components.Add(new Component() { Name = name, Path = path, Type = vbext_ComponentType.vbext_ct_StdModule } );
                        break;
                    case "cls":
                        components.Add(new Component() { Name = name, Path = path, Type = vbext_ComponentType.vbext_ct_ClassModule });
                        break;
                    case "frm":
                        components.Add(new Component() { Name = name, Path = path, Type = vbext_ComponentType.vbext_ct_MSForm });
                        break;
                }    
            }
            return components;
        }

    }
}