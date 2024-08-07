﻿using BusinessObjects.DTOs;

namespace CarRentingWebClient.AccessAPIs.Interfaces;

public interface IAuthAPIs
{
    public Task<bool> CustomerLoginAsync(LoginDTO loginDTO);
    public Task<bool> AdminLoginAsync(LoginDTO loginDTO);
    public Task<LoginResponse> LoginAsync(LoginDTO loginDTO);
}
