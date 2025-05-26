using ClientManagment.Models;
using System.Text.Json;

namespace ClientManagment
{
    public class Program
    {

        private static readonly string filePathData = "C:\\System programming\\Homework\\ClientManagment\\data.json";

        
        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("----------Managment clients------");
                Console.WriteLine();
                Console.WriteLine("Menu");
                Console.WriteLine("1. Add client");
                Console.WriteLine("2. Get all clients");
                Console.WriteLine("3. Get client by first name");
                Console.WriteLine("4. Exit");

                Console.Write("Enter option: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.Clear();
                        await AddClientProcessAsync();
                        break;
                    case "2":
                        Console.Clear();
                        await GetAllClientsProcessAsync();
                        break;
                    case "3":
                        Console.Clear();
                        await GetClientByFirstNameProcces();
                        break;
                    case "4":
                        Environment.Exit(0);
                        break;
                }

            }
        }

        static async Task AddClientToFileAsync(string filePath, Client client)
        {
            List<Client> clients = new List<Client>();

            if (File.Exists(filePath))
            {
                string existingData = await File.ReadAllTextAsync(filePath);
                if (!string.IsNullOrWhiteSpace(existingData))
                {
                    clients = JsonSerializer.Deserialize<List<Client>>(existingData) ?? new List<Client>();
                }
            }

            clients.Add(client);

            string jsonData = JsonSerializer.Serialize(clients, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, jsonData);
        }


        static async Task<List<Client>> GetAllClientsWithFileAsync(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string content = await sr.ReadToEndAsync();
                return JsonSerializer.Deserialize<List<Client>>(content) ?? new List<Client>();
            }
        }



        static async Task<string> GetClientByFirstNameWithFileAsync(string filePath, string firstName)
        {
            var clients = await GetAllClientsWithFileAsync(filePath);
            var client = clients.FirstOrDefault(c => c.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase));

            return client != null ? JsonSerializer.Serialize(client, new JsonSerializerOptions { WriteIndented = true }) : "Client not found";
        }


        static async Task AddClientProcessAsync()
        {
            Console.Write("Enter first name: ");
            string firstName = Console.ReadLine()?.Trim() ?? string.Empty;

            Console.Write("Enter last name: ");
            string lastName = Console.ReadLine()?.Trim() ?? string.Empty;

            int age;
            while (true)
            {
                Console.Write("Enter age: ");
                if (int.TryParse(Console.ReadLine(), out age) && age > 0)
                    break;

                Console.WriteLine("Invalid input. Age must be a positive integer. Try again.");
            }


            Console.Write("Enter address: ");
            string address = Console.ReadLine()?.Trim() ?? string.Empty;

            var client = new Client()
            {
                FirstName = firstName,
                LastName = lastName,
                Age = age,
                Address = address
            };

            await AddClientToFileAsync(filePathData, client);
            Console.WriteLine("Client added successfully!");

            Console.WriteLine();
            Console.WriteLine("Press enter to return to the menu.");
            Console.ReadKey();
            Console.Clear();
        }


        static async Task GetAllClientsProcessAsync()
        {
            var clients = await GetAllClientsWithFileAsync(filePathData);

            if (clients.Count == 0)
            {
                Console.WriteLine("No clients found.");
                return;
            }

            Console.WriteLine("List of Clients:");
            foreach (var client in clients)
            {
                Console.WriteLine($"{client.FirstName} {client.LastName}, Age: {client.Age}, Address: {client.Address}");
            }

            Console.WriteLine();
            Console.WriteLine("Press enter to return to the menu.");
            Console.ReadKey();
            Console.Clear();
        }


        static async Task GetClientByFirstNameProcces()
        {
            Console.Write("Enter first name: ");
            string firstName = Console.ReadLine()?.Trim() ?? string.Empty;

            var clientJson = await GetClientByFirstNameWithFileAsync(filePathData, firstName);

            var client = JsonSerializer.Deserialize<Client>(clientJson);

            if (client == null)
            {
                Console.WriteLine("Client not found.");
            }

            Console.WriteLine("Client Info:");
            Console.WriteLine($"First Name: {client.FirstName}");
            Console.WriteLine($"Last Name: {client.LastName}");
            Console.WriteLine($"Age: {client.Age}");
            Console.WriteLine($"Address: {client.Address}");

            Console.WriteLine();
            Console.WriteLine("Press enter to return to the menu.");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
