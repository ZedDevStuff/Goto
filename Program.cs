using System.Diagnostics;
using System.Text;

public class Program
{
    public static void Main(string[] args)
    {
        if (Path.GetDirectoryName(Environment.ProcessPath) == null) return;
        string path = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "data.dat");
        if (args.Length == 0)
        {
            return;
        }
        else if (args[0] == "-delete")
        {
            if (args.Length < 2)
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
                string savedData = File.ReadAllText(path);
                Dictionary<string, string> data = Load(path);
                if (data.ContainsKey(args[1]))
                {
                    data.Remove(args[1]);
                    File.WriteAllText(path, Serialize(data));
                    Console.WriteLine($"Entry {args[1]} deleted");
                }
            }

        }
        else if (args[0] == "-list")
        {
            Dictionary<string, string> data;
            if (!File.Exists(path))
            {
                data = new();
            }
            else
            {
                string savedData = File.ReadAllText(path);
                data = Load(path);
            }
            foreach (KeyValuePair<string, string> entry in data)
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
            Dictionary<string, string> data;
            if (!File.Exists(path))
            {
                data = new();
            }
            else
            {
                string savedData = File.ReadAllText(path);
                data = Load(path);
            }
            if (data.ContainsKey(args[1]))
            {
                Console.WriteLine($"Entry {args[1]} already exists");
            }
            if (Directory.Exists(args[2]))
            {
                data.Add(args[1], new DirectoryInfo(args[2]).FullName);
                File.WriteAllText(path, Serialize(data));
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
            string savedData = File.ReadAllText(path);
            Dictionary<string, string> data = Load(path);
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
    }
    public static string Serialize(Dictionary<string, string> data)
    {
        StringBuilder sb = new();
        foreach(KeyValuePair<string, string> kvp in data)
        {
            sb.Append(kvp.Key);
            sb.Append('=');
            sb.Append(kvp.Value);
            sb.AppendLine();
        }
        return sb.ToString();
    }
    public static Dictionary<string, string> Load(string path)
    {
        Dictionary<string, string> result = new();
        File.ReadAllLines(path).ToList().ForEach(line =>
        {
            string[] split = line.Split('=');
            if (split.Length == 2)
            {
                result.Add(split[0], split[1]);
            }
        });
        return result;
    }
}
