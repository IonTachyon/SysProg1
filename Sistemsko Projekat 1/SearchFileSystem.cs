using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistemsko_Projekat_1
{
    struct SearchData
    {
        public SearchData(string dir, string search)
        {
            currentDirectory = dir;
            searchedFileName = search;
        }

        public string currentDirectory { get; set; }
        public string searchedFileName { get; set; }

    }

    internal class SearchFileSystem
    {
        private object filelocker;
        private object dirlocker;
        private object listlocker;
        private int dirs;

        public List<string> Files { get; }
        public int[] Palindromes { get; set; }

        public SearchFileSystem()
        {
            filelocker = new object();
            dirlocker = new object();
            listlocker = new object();
            dirs = 0;
            Files = new List<string>();
            Palindromes = new int[1];
            Palindromes[0] = 0;
        }
        // zapocni pretragu
        public void Start(string dir, string word)
        {
            int workerThreads;
            SearchData startData = new SearchData(dir, word);

            // pribavi sve dostupne threadove
            ThreadPool.GetAvailableThreads(out workerThreads, out int irr);

            // zapocni pretragu 
            ThreadPool.QueueUserWorkItem(SearchFolder, startData);

            // dok se ne zavrsi pretraga - cekaj.
            // dirs je brojac za broj tekucih pretraga
            lock(dirlocker)
            {
                dirs++;
            }
            while(dirs > 0)
            {
                Thread.Sleep(1);
            }

            SearchFilesForPalindromes();

        }

        
        // pretraga fajl sistema za trazenim fajlom
        private void SearchFolder(object? data)
        {
            try
            {
                // QueueUserWorkItem formatiranje
                SearchData searchData = (SearchData)data;
                string currentDirectory = searchData.currentDirectory;
                string searchedFileName = searchData.searchedFileName;

                if (currentDirectory == null)
                {
                    throw new Exception("SearchFileSystem: Current directory not found!");
                }

                // nadji sve fajlove i pod-direktorije
                DirectoryInfo dirInfo = new DirectoryInfo(currentDirectory);
                var files = dirInfo.EnumerateFiles();
                var directories = dirInfo.EnumerateDirectories();

                // za svaki fajl, ako se poklapa - dodaj u listu
                foreach (FileInfo file in files)
                {
                    var fileName = file.Name;
                    if (fileName == searchedFileName)
                    {
                        addFileToList(file);
                    }
                }

                // za svaku direktoriju, dodeliti zasebnom threadu posao da pretrazi tu direktoriju

                SearchData newSearchData;
                foreach (DirectoryInfo directory in directories)
                {
                    newSearchData = new SearchData(directory.FullName, searchedFileName);
                    if(directory.FullName != null)
                    {
                        ThreadPool.QueueUserWorkItem(SearchFolder, newSearchData);
                    }

                    lock (dirlocker)
                    {
                        dirs++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                lock(dirlocker)
                {
                    dirs--;
                }
            }
            
        }

        private void addFileToList(FileInfo file)
        {
            lock(filelocker)
            {
                Files.Add(file.FullName);
            }
        }

        // pretraga nadjenih fajlova za palindrome
        public void SearchFilesForPalindromes()
        {
            Palindromes = new int[Files.Count];
            List<Thread> threads = new List<Thread>();

            foreach(string file in Files)
            {
                int index = Files.IndexOf(file);
                Palindromes[index] = 0;
                Thread thread = new Thread(() =>
                {
                    SearchFile(file, index);
                });
                threads.Add(thread);
                thread.Start();
                thread.Join();
            }
        }

        private void SearchFile(string file, int index)
        {
            string fileContents = File.ReadAllText(file);
            string[] words = fileContents.Split(' ');
            foreach(string word in words)
            {
                char[] letters = word.ToCharArray();
                char[] reverseLetters = new char[letters.Length];
                Array.Copy(letters, reverseLetters, letters.Length);
                Array.Reverse(reverseLetters);
                bool isPalindrome = reverseLetters.SequenceEqual(letters);
                if(isPalindrome)
                {
                    lock(listlocker)
                    {
                        Palindromes[index]++;
                    }
                }
            }
        }

        public PalindromeResponse GetResponse()
        {
            return new PalindromeResponse(Files.ToArray(), Palindromes);
        }
    }
}
