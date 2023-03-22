namespace Trucks.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    using Data;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            var DispacherDtos = Deserialize<ImportDespatchersDto[]>(xmlString, "Despatchers");

            List<Despatcher> validDespachers = new List<Despatcher>();
            //List<Truck> validTrucks = new List<Truck>();

            foreach (var dDto in DispacherDtos)
            {
                if (!IsValid(dDto) || string.IsNullOrEmpty(dDto.Position))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Despatcher despatcher = new Despatcher() 
                {
                    Name = dDto.Name,
                    Position = dDto.Position
                };
                validDespachers.Add(despatcher);

                foreach (var tDto in dDto.Trucks)
                {
                    if (!IsValid(tDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Truck truck = new Truck() 
                    {
                        RegistrationNumber = tDto.RegistrationNumber,
                        VinNumber = tDto.VinNumber,
                        TankCapacity = tDto.TankCapacity,
                        CargoCapacity = tDto.CargoCapacity,
                        CategoryType = (CategoryType)tDto.CategoryType,
                        MakeType = (MakeType)tDto.MakeType
                    };
                    //validTrucks.Add(truck);
                    despatcher.Trucks.Add(truck);
                }
                sb.AppendLine(string.Format(SuccessfullyImportedDespatcher, despatcher.Name, despatcher.Trucks.Count()));
            }
            context.Despatchers.AddRange(validDespachers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var clietntDtos = JsonConvert.DeserializeObject<ImportClientsDto[]>(jsonString);

            List<Client> validClients = new List<Client>();
            //List<ClientTruck> validClientTrucks = new List<ClientTruck>();

            foreach(var cDto in clietntDtos) 
            {
                if (!IsValid(cDto) || cDto.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client client = new Client() 
                {
                    Name = cDto.Name,
                    Nationality = cDto.Nationality,
                    Type = cDto.Type,
                };

                foreach (int truckId in cDto.Trucks.Distinct()) 
                {
                    Truck truck = context.Trucks.Find(truckId);
                    if (truck == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ClientTruck clientTruck = new ClientTruck() 
                    {
                        Client = client,
                        Truck = truck
                    };
                    client.ClientsTrucks.Add(clientTruck);
                }
                validClients.Add(client);
                sb.AppendLine(string.Format(SuccessfullyImportedClient, client.Name, client.ClientsTrucks.Count()));
            }
            context.Clients.AddRange(validClients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

        private static T Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute rootAttribute = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), rootAttribute);

            using StringReader input = new StringReader(inputXml);

            T output = (T)xmlSerializer.Deserialize(input)!;

            return output;
        }
    }
}
