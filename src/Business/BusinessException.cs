using System;
using System.Collections.Generic;

namespace Business
{
    public class BusinessException : Exception
    { 
        private static readonly Dictionary<ErrorCodes, string> ErrorDescriptions;
        static BusinessException()
        {
            ErrorDescriptions = new Dictionary<ErrorCodes, string>()
            {
                { ErrorCodes.ORDER_CANNOT_BE_EMPTY, "The order cannot be empty" },
                { ErrorCodes.WHOLESALER_MUST_EXIST, "The wholesaler must exist" },
                { ErrorCodes.BREWERY_MUST_EXIST, "The brewery must exist" },
                { ErrorCodes.NO_DUPLICATE_IN_ORDER, "There must not be duplicates in the order" },
                { ErrorCodes.NOT_GREATER_NUMBER_THAN_IN_STOCK, "The number of beers ordered cannot be greater than the wholesaler's stock" },
                { ErrorCodes.BEER_NOT_SOLD_BY_WHOLESALER, "The beer must be sold by the wholesaler" },
                { ErrorCodes.BEER_ALREADY_SOLD_BY_WHOLESALER, "The beer must not be already sold by the wholesaler" }
            };
        }

        public ClientSideError Error { get; private set; }

        public BusinessException(ErrorCodes errorCode, string source) : base(errorCode.ToString())
        {
            Error = new ClientSideError()
            {
                Code = errorCode,
                Source = source,
                Type = errorCode.ToString(),
                Message = FindMessage(errorCode)
            };
        }

        private string FindMessage(ErrorCodes errorCode)
        {
            return ErrorDescriptions.ContainsKey(errorCode) 
                ? ErrorDescriptions[errorCode] 
                : "No message set for this error code";
        }
    }
}