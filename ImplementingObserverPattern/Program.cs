using System;

namespace ImplementingObserverPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begin program");
            var action1 = new Action<string>(NewUserWasCreated);
            var action2 = new Action<string,string>(NewUserCreated);
            var emitter = Emitter.Instance;
            emitter.On("Create User", action1, 1).Once();
            //emitter.On("Create User", action1, 2);
            emitter.On("Create User", action2, 3);
            emitter.Emit("Create User","John","Doe");
            //emitter.Emit("Create User", "John", "Doe");
            Console.ReadLine();
        }

        private static void NewUserWasCreated(string firstname)
        {
            Console.WriteLine("Welcome {0}", firstname);
        }

        private static void NewUserCreated(string firstname,string lastname)
        {
            Console.WriteLine("Thanks for sign up {0} {1}", firstname, lastname);
        }
    }
}