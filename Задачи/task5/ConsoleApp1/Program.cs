using System;
using System.Linq;
using System.Reflection;
using A;

namespace A
{
    static class Program
    {
        static void ListTypesInAssembly(Assembly assembly)
        {

            var types = assembly.GetTypes().Where(t => t.IsPublic);//получаем класса, с public
           
            var list=new List<(string namespaceName, string className)>();//получение листа с именем namepspace, именем класса

            foreach (var type in types)//заполенение листа
            {
                list.Add((type.Namespace, type.FullName));
            }

            var list2 = list.OrderBy(element => element.namespaceName);//сорировка по пространству имен

            foreach (var (namespaceName, publicTypes) in list2)//вывод на экран
            {
                if (publicTypes.Count() == 0)
                    continue;

                Console.WriteLine("NAMESPACE: " + namespaceName);
                Console.WriteLine("   " + publicTypes);
                
            }
        }
        static void Main(string[] args)
        {
            var assemblyPath = "D:\\СПП\\Lab3\\TestProject\\bin\\Debug\\TestProject.exe";//путь к exe
            var assembly = Assembly.LoadFrom(assemblyPath);//получаем самой disassemmbly части
            ListTypesInAssembly(assembly);//вывод на экран наших классов
            Console.ReadKey();
        }
    }

}