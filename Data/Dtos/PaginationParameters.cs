﻿namespace PersonalAccount.API.Models.Dtos;
public class PaginationParameters
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}