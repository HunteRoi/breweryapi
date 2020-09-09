namespace API.Infrastructure
{
    public enum ErrorCodes
    {
        ORDER_CANNOT_BE_EMPTY,
        WHOLESALER_MUST_EXIST,
        NO_DUPLICATE_IN_ORDER,
        NO_GREATER_NUMBER_THAN_IN_STOCK,
        BEER_NOT_SOLD_BY_WHOLESALER
    }
}
