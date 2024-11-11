using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json; // says that it works elsewhere than windows btw!

namespace ShoppingList
{
    public class SavingSystem
    {
        public static string FilePath = FileSystem.AppDataDirectory;
        public static char Sep = Path.DirectorySeparatorChar;

        public static void SaveShoppingList(ShoppingListData data)
        {
            string content = JsonConvert.SerializeObject(data, Formatting.Indented);
            string path = FilePath + Sep + data.Name + ".json";
            File.WriteAllText(path, content);
            Console.WriteLine($"Saved {path}");
        }

        public static ShoppingListData LoadShoppingList(string name)
        {
            string path = FilePath + Sep + name + ".json";
            string content = File.ReadAllText(path);
            ShoppingListData data = JsonConvert.DeserializeObject<ShoppingListData>(content);
            Console.WriteLine($"Loaded {path}");
            return data;
        }

        public static List<string> GetSavedShoppingListNames()
        {
            List<string> paths = Directory.GetFiles(FilePath, "*.json", SearchOption.TopDirectoryOnly).ToList<string>();
            List<string> names = new();
            foreach (string path in paths)
            {
                names.Add(Path.GetFileNameWithoutExtension(path));
            }
            return names;
        }

        public static string GetFreeShoppingListName()
        {
            string startName = "ShoppingList ";
            for (int i = 1; i < 1000; i++)
            {
                string name = startName + i.ToString();
                if (!File.Exists(FilePath + Sep + name + ".json"))
                {
                    return name;
                }
            }

            return "InvalidName";
        }

        public static void DeleteShoppingList(string name)
        {
            string path = FilePath + Sep + name + ".json";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
