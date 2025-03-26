﻿using Dal.Models;

namespace Dal.Api
{
    public interface IOrder : ICrud<Order>
    {
        Task<List<Order>> GetOrdersByIdCustomer(int idC);
        Task<List<Order>> GetOrdersToday();
        Task<List<Order>> GetOrdersByStatusFalse();
        Task<List<Order>> GetOrdersByDateRange(DateTime startDate, DateTime endDate);

    }
}
