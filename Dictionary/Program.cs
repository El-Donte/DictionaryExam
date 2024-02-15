using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary
{
    internal class Program
    {
        class DictionaryApp
        {
            private Dictionary<string, Dictionary<string, List<string>>> dictionaries;
            public DictionaryApp()
            {
                dictionaries = new Dictionary<string, Dictionary<string, List<string>>>();
            }
            public void Run()
            {
                Console.WriteLine("Добро пожаловать в приложение \"Словари\"!");

                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("\nМеню:");
                    Console.WriteLine("1. Создать словарь");
                    Console.WriteLine("2. Добавить слово и его перевод");
                    Console.WriteLine("3. Заменить слово или его перевод");
                    Console.WriteLine("4. Удалить слово или перевод");
                    Console.WriteLine("5. Найти перевод слова");
                    Console.WriteLine("6. Экспортировать словарь");
                    Console.WriteLine("7. Отобразить словарь");
                    Console.WriteLine("8. Выйти из программы");

                    Console.Write("Выберите действие: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            CreateDictionary();
                            break;
                        case "2":
                            AddWordTranslation();
                            break;
                        case "3":
                            ReplaceWordOrTranslation();
                            break;
                        case "4":
                            RemoveWordOrTranslation();
                            break;
                        case "5":
                            FindTranslation();
                            break;
                        case "6":
                            ExportDictionary();
                            break;
                        case "7":
                            ShowDictionary();
                            break;
                        
                        case "8":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Неверный ввод. Пожалуйста, выберите действие из меню.");
                            break;
                    }
                }
            }

            private void CreateDictionary()
            {
                Console.Write("Введите название нового словаря: ");
                string dictName = Console.ReadLine();
                if (!dictionaries.ContainsKey(dictName))
                {
                    Console.WriteLine("Выберите тип словаря:");
                    Console.WriteLine("1. Англо-русский");
                    Console.WriteLine("2. Русско-английский");
                    Console.Write("Ваш выбор: ");
                    string typeChoice = Console.ReadLine();
                    Dictionary<string, List<string>> newDict = new Dictionary<string, List<string>>();
                    dictionaries.Add(dictName, newDict);
                    SaveDictionary(dictName);
                    Console.WriteLine($"\nСловарь \"{dictName}\" успешно создан.");
                }
                else
                {
                    Console.WriteLine($"\nСловарь с именем \"{dictName}\" уже существует.");
                }

            }

            private void AddWordTranslation()
            {
                Console.Write("Введите название словаря, в который нужно добавить слово: ");
                string dictionaryName = Console.ReadLine();
                LoadDictionaryFromFile(dictionaryName);
                if (dictionaries.ContainsKey(dictionaryName))
                {
                    Console.Write("Введите слово: ");
                    string word = Console.ReadLine();
                    Console.Write("Введите перевод(ы) через запятую: ");
                    string translationsInput = Console.ReadLine();
                    translationsInput = translationsInput.Replace(" ", "");
                    string[] translations = translationsInput.Split(',');
                    if (dictionaries[dictionaryName].ContainsKey(word))
                    {
                        dictionaries[dictionaryName][word].AddRange(translations);
                    }
                    else
                    {
                        dictionaries[dictionaryName][word] = translations.ToList();
                    }
                    Console.WriteLine($"\nСлово(а) \"{word}\" успешно добавлено в словарь \"{dictionaryName}\".");
                    SaveDictionary(dictionaryName);
                }
                else
                {
                    Console.WriteLine($"\nСловарь с названием \"{dictionaryName}\" не найден.");
                }
            }

            private void ReplaceWordOrTranslation()
            {
                Console.Write("Введите название словаря, в котором нужно заменить слово или его перевод: ");
                string dictionaryName = Console.ReadLine();
                LoadDictionaryFromFile(dictionaryName);
                if (dictionaries.ContainsKey(dictionaryName))
                {
                    Console.WriteLine("\nЧто вы хотите сделать");
                    Console.WriteLine("1. Заменить слово");
                    Console.WriteLine("2. Заменить перевод слова");
                    Console.Write("Выбор: ");
                    string choice = Console.ReadLine();
                    string word;
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Введите слово,которое вы хотите заменить: ");
                            word = Console.ReadLine();
                            if (dictionaries[dictionaryName].ContainsKey(word))
                            {
                                Console.Write("Введите новое слово: ");
                                string newWord = Console.ReadLine();
                                List<string> value = dictionaries[dictionaryName][word];
                                dictionaries[dictionaryName].Remove(word);
                                dictionaries[dictionaryName][newWord] = value;
                                SaveDictionary(dictionaryName);
                                Console.WriteLine($"\nСлово \"{word}\" в словаре \"{dictionaryName}\" успешно заменен(ы).");
                                break;
                            }
                            else
                            {
                                Console.WriteLine($"\nСлово \"{word}\" не найдено в словаре \"{dictionaryName}\".");
                                break;
                            }
                            
                        case "2":
                            Console.Write("Введите слово,перевод которого вы хотите заменить: ");
                            word = Console.ReadLine();
                            if (dictionaries[dictionaryName].ContainsKey(word))
                            {
                                Console.Write("Введите новый перевод(ы) через запятую: ");
                                string translationsInput = Console.ReadLine();
                                translationsInput = translationsInput.Replace(" ", "");
                                string[] translations = translationsInput.Split(',');
                                dictionaries[dictionaryName][word] = translations.ToList();
                                SaveDictionary(dictionaryName);
                                Console.WriteLine($"\nПеревод(ы) слова \"{word}\" в словаре \"{dictionaryName}\" успешно заменен(ы).");
                                break;
                            }
                            else
                            {
                                Console.WriteLine($"\nСлово \"{word}\" не найдено в словаре \"{dictionaryName}\".");
                                break;
                            }
                        default:
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"\nСловарь с названием \"{dictionaryName}\" не найден.");
                }
            }

            private void RemoveWordOrTranslation()
            {
                Console.Write("Введите название словаря, из которого нужно удалить слово или перевод: ");
                string dictionaryName = Console.ReadLine();
                LoadDictionaryFromFile(dictionaryName);
                if (dictionaries.ContainsKey(dictionaryName))
                {
                    Console.WriteLine("\nЧто вы хотите сделать");
                    Console.WriteLine("1. Удалить слово");
                    Console.WriteLine("2. Удалить перевод слова");
                    string choice = Console.ReadLine();
                    string word;
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Введите слово, которое нужно удалить: ");
                            word = Console.ReadLine();
                            if (dictionaries[dictionaryName].ContainsKey(word))
                            {
                                dictionaries[dictionaryName].Remove(word);
                                SaveDictionary(dictionaryName);
                                Console.WriteLine($"\nСлово \"{word}\" успешно удалено из словаря \"{dictionaryName}\".");
                                break;
                            }
                            else
                            {
                                Console.WriteLine($"\nСлово \"{word}\" не найдено в словаре \"{dictionaryName}\".");
                                break;
                            }
                        case "2":
                            Console.Write("Введите слово,перевод которого вы хотите удалить: ");
                            word = Console.ReadLine();
                            if (dictionaries[dictionaryName].ContainsKey(word))
                            {
                                Console.Write("Введите перевод слова, который вы хотие удалить: ");
                                string wordTraslation = Console.ReadLine();
                                if (dictionaries[dictionaryName][word].Contains(wordTraslation))
                                {
                                    int count = dictionaries[dictionaryName][word].Count;
                                    if (count == 1)
                                    {
                                        Console.WriteLine($"\nПеревод слова \"{word}\" нельзя удалить из " +
                                            $"словаря \"{dictionaryName}\"так, как он единственный");
                                        break;
                                    }
                                    else
                                    {
                                        dictionaries[dictionaryName][word].Remove(wordTraslation);
                                        SaveDictionary(dictionaryName);
                                        Console.WriteLine($"\nПеревод слова \"{word}\" успешно удален из словаря \"{dictionaryName}\".");
                                        break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Этот перевод слова \"{word}\" не найден в словаре \"{dictionaryName}\".");
                                    break;
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Слово \"{word}\" не найдено в словаре \"{dictionaryName}\".");
                                break;
                            }

                        default:
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"Словарь с названием \"{dictionaryName}\" не найден.");
                }
            }

            private void FindTranslation()
            {
                Console.Write("Введите название словаря, в котором нужно найти перевод слова: ");
                string dictionaryName = Console.ReadLine();
                LoadDictionaryFromFile(dictionaryName);
                if (dictionaries.ContainsKey(dictionaryName))
                {
                    Console.Write("Введите слово, перевод которого нужно найти: ");
                    string searchWord = Console.ReadLine();
                    bool found = false;
                    Console.WriteLine($"\nСловарь {dictionaryName}:");
                    foreach (var dict in dictionaries[dictionaryName])
                    {
                        if (dict.Key == searchWord)
                        {
                            Console.WriteLine($"{searchWord}: {string.Join(", ", dict.Value)}");
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        Console.WriteLine($"\nСлово \"{searchWord}\" не найдено в словарях.");
                    }
                }
                else
                {
                    Console.WriteLine($"\nСловарь с названием \"{dictionaryName}\" не найден.");
                }
            }

            private void ShowDictionary()
            {
                Console.Write("Введите название словаря который вы хотите отобразить: ");
                string dictionaryName = Console.ReadLine();
                LoadDictionaryFromFile(dictionaryName);
                if (dictionaries.ContainsKey(dictionaryName))
                {
                    Console.WriteLine($"\nСловарь {dictionaryName}:");
                    foreach (var dict in dictionaries[dictionaryName])
                    { 
                        Console.WriteLine($"{dict.Key}: {string.Join(", ", dict.Value)}");
                    }
                }
            }

            private void SaveDictionary(string dictionaryName)
            {

                if (dictionaries.ContainsKey(dictionaryName))
                {
                    StreamWriter writer = new StreamWriter($"{dictionaryName}.txt");
                    writer.WriteLine("- "+dictionaryName);
                    foreach (var pair in dictionaries[dictionaryName])
                    {
                        writer.WriteLine(pair.Key + ": " + string.Join(", ", pair.Value));
                    }
                    writer.Close();
                }
            }
            private void ExportDictionary()
            {
                Console.Write("Введите название словаря, который нужно экспортировать: ");
                string dictionaryName = Console.ReadLine();
                LoadDictionaryFromFile(dictionaryName);
                if (dictionaries.ContainsKey(dictionaryName))
                {
                    Console.Write("Введите путь для экспорта словаря: ");
                    string exportPath = Console.ReadLine();
                    StreamWriter writer = new StreamWriter(exportPath);
                    writer.WriteLine("- " + dictionaryName);
                    foreach (var pair in dictionaries[dictionaryName])
                    {
                        writer.WriteLine(pair.Key + ": " + string.Join(", ", pair.Value));
                    }
                    writer.Close ();
                    Console.WriteLine($"Словарь \"{dictionaryName}\" успешно экспортирован по пути: {exportPath}");
                }
                else
                {
                    Console.WriteLine($"Словарь с названием \"{dictionaryName}\" не найден.");
                }
            }

            private void LoadDictionaryFromFile(string filePath)
            {
                try
                {
                    StreamReader reader = new StreamReader(filePath+".txt");
                    
                    string line;
                    string dict = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("-"))
                        {      
                            dict = line.Substring(2); // Имя словаря без "- "
                            if (!dictionaries.ContainsKey(dict)) {
                                dictionaries.Add(dict,new Dictionary<string, List<string>>());
                            }
                        }
                        else
                        {
                            string[] parts = line.Split(':');
                            if (parts.Length == 2)
                            {
                                string word = parts[0].Trim();
                                string[] translations = parts[1].Split(',').Select(t => t.Trim()).ToArray();
                                if (!dictionaries[dict].ContainsKey(word))
                                {
                                    dictionaries[dict].Add(word, translations.ToList<string>());
                                }
                            }
                        }
                    }
                    
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при загрузке словаря из файла: {ex.Message}");
                }
            }

        }
        static void Main(string[] args)
        {
            DictionaryApp app = new DictionaryApp();
            app.Run();
        }
    }
}


