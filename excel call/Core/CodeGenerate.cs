﻿using System;
using System.Collections.Generic;
using System.IO;
using DreamLib.Editor.Unity.Extensition;
using ScriptGenerate;

namespace DreamExcel.Core
{
    public static class CodeGenerate
    {
        private static Action<Generate.Info, Generate> Defalut(List<GenerateConfigTemplate> customClass, GenerateConfigTemplate core)
        {
            Action<Generate.Info, Generate> action = (info, g) =>
            {
                if (info.PlaceHolder == "CustomClass")
                {
                    for (var i = 0; i < customClass.Count; i++)
                    {
                        var custom = customClass[i];
                        g.SetReplace("Class", custom.Class.Name);
                        g.SetReplace("Attribute", custom.Class.Attribute);
                        g.BeginGroup("NestedField");
                        var properties = custom.Properties;
                        for (var j = 0; j < properties.Count; j++)
                        {
                            g.SetReplace("Name", properties[j].Name);
                            g.SetReplace("Type", properties[j].Type);
                            g.Apply();
                        }
                        g.EndGroup();
                        g.BeginGroup("NestedProperty");
                        for (var j = 0; j < properties.Count; j++)
                        {
                            g.SetReplace("Name", properties[j].Name);
                            g.SetReplace("Type", properties[j].Type);
                            g.Apply();
                        }
                        g.EndGroup();
                        g.Apply();
                    }
                }
                if (info.PlaceHolder == "CoreClass")
                {
                    g.SetReplace("Class", core.Class.Name);
                    g.SetReplace("Attribute", core.Class.Attribute);
                    g.SetReplace("KeyType", core.Class.Type);
                    g.BeginGroup("Field");
                    var properties = core.Properties;
                    for (var i = 0; i < properties.Count; i++)
                    {
                        g.SetReplace("Name", properties[i].Name);
                        g.SetReplace("Type", properties[i].Type);
                        g.Apply();
                    }
                    g.EndGroup();
                    g.BeginGroup("Property");
                    for (var i = 0; i < properties.Count; i++)
                    {
                        g.SetReplace("Name", properties[i].Name);
                        g.SetReplace("Type", properties[i].Type);
                        g.Apply();
                    }
                    g.EndGroup();
                    g.Apply();
                }
                if (info.PlaceHolder == "ScriptableObject")
                {
                    g.SetReplace("Class", core.Class.Name);
                    g.Apply();
                }
            };
            return action;
        }

        public static string Start(Action<Generate.Info, Generate> action,string fileName)
        {
            var path = Config.Instance.ScriptTemplatePath;
            string g = new Generate(path, action).StartWrite();
            g = Generate.FormatScript(g);
            if (!Directory.Exists(Config.Instance.SaveScriptPath))
                Directory.CreateDirectory(Config.Instance.SaveScriptPath);
            File.WriteAllText(Config.Instance.SaveScriptPath + fileName + ".cs",g);
            return g;
        }

        public static string Start(List<GenerateConfigTemplate> customClass, GenerateConfigTemplate core, string fileName)
        {
            var action = Defalut(customClass, core);
            return Start(action,fileName);
        }
    }
}
