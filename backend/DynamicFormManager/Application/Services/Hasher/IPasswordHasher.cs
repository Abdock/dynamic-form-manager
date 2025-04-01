﻿namespace Application.Services.Hasher;

public interface IPasswordHasher
{
    string ComputeHash(string password);
    bool Verify(string password, string passwordHash);
}