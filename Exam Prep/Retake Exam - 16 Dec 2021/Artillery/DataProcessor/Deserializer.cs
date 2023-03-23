namespace Artillery.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage =
                "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            var countriesDtos = Deserialize<ImportCountriesDto[]>(xmlString, "Countries");

            List<Country> validCountries = new List<Country>();

            foreach ( var cDto in countriesDtos ) 
            {
                if (!IsValid(cDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Country country = new Country() 
                {
                    CountryName = cDto.CountryName,
                    ArmySize = cDto.ArmySize
                };

                validCountries.Add(country);
                sb.AppendLine(string.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
            }
            context.Countries.AddRange(validCountries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            var manufacturerDto = Deserialize<ImportManufacturersDto[]>(xmlString, "Manufacturers");

            List<Manufacturer> validManufacturers = new List<Manufacturer>();

            foreach (var mDto in manufacturerDto)
            {
                if (!IsValid(mDto) || validManufacturers.Any(x => x.ManufacturerName == mDto.ManufacturerName))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Manufacturer manufacturer = new Manufacturer() 
                {
                    ManufacturerName = mDto.ManufacturerName,
                    Founded = mDto.Founded
                };
                validManufacturers.Add(manufacturer);
                string[] foundedInfo = manufacturer.Founded.Split(", ").Reverse().ToArray();
                string fCountry = foundedInfo[0];
                string fTown = foundedInfo[1];
                string output = fTown + ", " + fCountry;

                sb.AppendLine(string.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, output));
            }
            context.Manufacturers.AddRange(validManufacturers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            var shellDtos = Deserialize<ImportShellsDto[]>(xmlString, "Shells");

            List<Shell> validShells = new List<Shell>();
            
            foreach (var sDto in shellDtos) 
            {
                if (!IsValid(sDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Shell shell = new Shell() 
                {
                    ShellWeight = sDto.ShellWeight,
                    Caliber = sDto.Caliber
                };
                validShells.Add(shell);
                sb.AppendLine(string.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
            }

            context.Shells.AddRange(validShells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            var gunsDtos = JsonConvert.DeserializeObject<ImportGunsDto[]>(jsonString);

            List<Gun> validGuns = new List<Gun>();

            foreach (var gDto in gunsDtos)
            {
                bool isGunTypeValid = Enum.TryParse<GunType>(gDto.GunType, out GunType gunTypeParsed);

                if (!IsValid(gDto) || !isGunTypeValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Gun gun = new Gun() 
                {
                    ManufacturerId = gDto.ManufacturerId,
                    GunWeight = gDto.GunWeight,
                    BarrelLength = gDto.BarrelLength,
                    NumberBuild = gDto.NumberBuild,
                    Range = gDto.Range,
                    GunType = gunTypeParsed,
                    ShellId = gDto.ShellId
                };
                validGuns.Add(gun);

                foreach (var cDto in gDto.Countries)
                {
                    Country country = context.Countries.Find(cDto.Id);

                    CountryGun countryGun = new CountryGun() 
                    {
                        Country = country,
                        Gun = gun
                    };
                    gun.CountriesGuns.Add(countryGun);
                }
                sb.AppendLine(string.Format(SuccessfulImportGun, gun.GunType, gun.GunWeight, gun.BarrelLength));
            }
            context.Guns.AddRange(validGuns);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }

        private static T Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute root = new XmlRootAttribute(rootName);
            XmlSerializer serializer = new XmlSerializer(typeof(T), root);

            using StringReader reader = new StringReader(inputXml);

            T dtos = (T)serializer.Deserialize(reader);
            return dtos;
        }
    }
}
