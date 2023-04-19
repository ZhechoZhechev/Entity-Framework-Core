using Microsoft.EntityFrameworkCore.Query.Internal;

namespace PetStoreWorkshop.Data.Models.Common
{
    public static class ModelConstants
    {
        // Pet constants
        public const int PetNameLength = 50;
        public const int PetBreedLength = 30;

        // Product constants
        public const int ProductNameLength = 50;

        // Store constants
        public const int StoreNameLength = 50;
        public const int StoreAddressLength = 100;
        public const int StoreDescriptionLength = 500;

        // Address constants
        public const int AddressTextLength = 100;
        public const int AddressTownNameLength = 50;

        // Client constants
        public const int ClientNameLength = 50;

        // CardInfo constants
        public const int CardNumberLength = 19;
        public const int CardExpirationDateLength = 5;
        public const int CardOwnerNameLength = 50;
        public const int CardSecurityNumberLength = 4;

        // ClientCard constants
        public const int ClientCardNumberLength = 19;
        public const int ClientCardExpirationDateLength = 5;

        // Service constants
        public const int ServiceNameLength = 50;
        public const int ServiceDescriptionLength = 500;

        // Category constants
        public const int CategoryNameLength = 50;

    }
}
