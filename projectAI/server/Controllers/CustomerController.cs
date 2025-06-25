using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        //private readonly ICustomerService _customerService;

        //public CustomersController(ICustomerService customerService)
        //{
        //    _customerService = customerService;
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    try
        //    {
        //        var customers = await _customerService.GetAll();
        //        return Ok(customers);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    try
        //    {
        //        var customer = await _customerService.GetCustomerById(id);
        //        if (customer == null)
        //            return NotFound();

        //        return Ok(customer);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] Customer customer)
        //{
        //    try
        //    {
        //        var createdCustomer = await _customerService.Create(customer);
        //        return CreatedAtAction(nameof(GetById), new { id = createdCustomer.UserId }, createdCustomer);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, [FromBody] Customer customer)
        //{
        //    if (id != customer.UserId)
        //        return BadRequest("ID mismatch");

        //    try
        //    {
        //        var updatedCustomer = await _customerService.Update(customer);
        //        return Ok(updatedCustomer);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    try
        //    {
        //        var customer = await _customerService.GetCustomerById(id);
        //        if (customer == null)
        //            return NotFound();

        //        await _customerService.Delete(customer);
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
