﻿using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;


namespace Dal.Services
{
    public class OrderService : IOrder
    {
        mycontext db;
        public OrderService(mycontext? m)
        {
            db = m;
        }
        public async Task<List<Order>> GetAll()
        {
            try
            {
                return  db.Orders.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving orders", ex);

            }
        }
       //החזרת הזמנות של לקוח ספציפי
       //ביצוע בדיקה בביאל שקיים באמת לקוח כזה במאגר
        public async Task<List<Order>> GetOrdersByIdCustomer(int idC)
        {
            try
            {
                return db.Orders.Where(o => o.IdCustomer == idC).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving orders", ex);

            }
        }
        //החזרת רשימת ההזמנות שהתבצעה היום
        public async Task<List<Order>> GetOrdersToday()
        {
            try
            {
                return db.Orders.Where(o => o.DateOrder==DateTime.Today).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving orders", ex);
            }
        }
        public async Task<List<Order>> GetOrdersByStatusFalse()
        {
            try
            {
                return db.Orders.Where(o => o.Status == false).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving orders", ex);
            }
        }
        public async Task<List<Order>> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await db.Orders.Where(o => o.DateOrder >= startDate && o.DateOrder <= endDate).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving orders", ex);
            }
        }
    }
}
