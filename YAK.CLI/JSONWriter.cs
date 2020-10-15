using System;
using System.IO;
using System.Text.Json;

namespace AnhangExporter.Core.Serialization
{
    public class JSONWriter
    {
        public static (bool success, string message) Write<T>(T element, string datei)
        {
            try
            {
                var wert = Serialize<T>(element);

                if (wert.success)
                {
                    File.WriteAllText(datei, wert.data);
                    return (true, "Erfolgreich geschrieben");
                }
                else
                {
                    return (false, wert.message);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public static (bool success, T element, string message) Read<T>(string datei)
        {
            try
            {
                string data = File.ReadAllText(datei);
                if (String.IsNullOrWhiteSpace(data))
                    return (false, default, "Der Inhalt der Datei war leer");

                var wert = Deserialize<T>(data);

                if (wert.success)
                {
                    return (true, wert.element, "Erfolgreich gelesen");
                }
                else
                {
                    return (false, default, wert.message);
                }
            }
            catch (Exception ex)
            {
                return (false, default, ex.Message);
            }
        }

        public static (bool success, string data, string message) Serialize<T>(T element)
        {
            try
            {
                return (true, JsonSerializer.Serialize<T>(element), "Erfolgreich serialisiert");
            }
            catch (Exception ex)
            {
                return (false, "", ex.Message);
            }
        }

        public static (bool success, T element, string message) Deserialize<T>(string data)                                                                                                                                                                                                                                                                      
        {
            try
            {
                return (true, JsonSerializer.Deserialize<T>(data), "Erfolgreich deserialisiert");
            }
            catch (Exception ex)
            {
                return (false, default, ex.Message);
            }
        }
    }
}
