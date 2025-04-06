namespace Application.DTO.Enums;

public enum CustomStatusCodes
{
    // Success
    Ok = 200,
    Created = 201,
    // Bad request
    InvalidEmailFormat = 400_01,
    WeakPassword = 400_02,
    EmailAlreadyUsing = 400_03,
    // Not found
    InvalidUserCredentials = 404_01,
    // Internal server error
    Unknown = 500_01
}