using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;

string path = Path.Combine(new FileInfo(Assembly.GetCallingAssembly().Location).DirectoryName, "data.json");

if (args.Length == 0)
{
    return;
}
else if (args[0] == "-delete")
{
    if(args.Length < 2)
    {
        Console.WriteLine("Please provide the entry to delete");
    }
    else
    {
        if (!File.Exists(path))
        {
            File.Create(path);
            Console.WriteLine($"No entry named {args[0]}");
            return;
        }
        string json = File.ReadAllText(path);
        Dictionary<string, string>? data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        if (data.ContainsKey(args[1]))
        {
            data.Remove(args[1]);
            File.WriteAllText(path, JsonSerializer.Serialize(data));
            Console.WriteLine($"Entry {args[1]} deleted");
        }
    }

}
else if (args[0] == "-list")
{
    Dictionary<string,string> data;
    if (!File.Exists(path))
    {
        data = new();
    }
    else
    {
        string json = File.ReadAllText(path);
        data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
    }
    foreach(KeyValuePair<string,string> entry in data)
    {
        Console.WriteLine($"{entry.Key} -> \"{entry.Value}\"");
    }
}
else if (args[0] == "-add")
{
    if (args.Length < 2)
    {
        Console.WriteLine("Please provide a name");
        return;
    }
    if (args.Length < 3)
    {
        Console.WriteLine("Please provide a path");
        return;
    }
    Dictionary<string, string>? data;
    if (!File.Exists(path))
    {
        data = new();
    }
    else
    {
        string json = File.ReadAllText(path);
        data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
    }
    if (data.ContainsKey(args[1]))
    {
        Console.WriteLine($"Entry {args[1]} already exists");
    }
    if (Directory.Exists(args[2]))
    {
        data.Add(args[1], new DirectoryInfo(args[2]).FullName);
        File.WriteAllText(path, JsonSerializer.Serialize(data));
        Console.WriteLine($"Entry {args[1]} added");
    }
    else
    {
        Console.WriteLine($"\"{args[2]} is not a valid directory or is a file\"");
    }
}
else
{
    if (!File.Exists(path))
    {
        File.Create(path);
        Console.WriteLine($"No location named {args[0]}");
        return;
    }
    string json = File.ReadAllText(path);
    Dictionary<string, string>? data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
    if (data != null)
    {
        if (data.ContainsKey(args[0]))
        {
            Process.Start("explorer.exe", data[args[0]]);
        }
        else
        {
            Console.WriteLine($"No location named {args[0]}");
        }
    }
}
