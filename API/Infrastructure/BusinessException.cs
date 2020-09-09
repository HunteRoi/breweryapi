using System;
using System.Collections.Generic;

namespace API.Infrastructure
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
                { ErrorCodes.NO_DUPLICATE_IN_ORDER, "There must not be duplicates in the order" },
                { ErrorCodes.NO_GREATER_NUMBER_THAN_IN_STOCK, "The number of beers ordered cannot be greater than the wholesaler's stock" },
                { ErrorCodes.BEER_NOT_SOLD_BY_WHOLESALER, "The beer must be sold by the wholesaler" }
            };
        }

        public ClientSideError Error { get; private set; }

        public BusinessException(ErrorCodes errorCode) : base(errorCode.ToString())
        {
            Error = new ClientSideError()
            {
                Code = errorCode,
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