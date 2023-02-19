using Microsoft.AspNetCore.Mvc;
using MoveShareApplication.Data;
using System.Security.Claims;
using MoveShareApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MoveShareApplication.Authorization;
using Microsoft.CodeAnalysis;

namespace MoveShareApplication.Controllers
{

    /// <summary>
    /// _logger categories{
    ///     Exception,
    ///     DbUpdateException,
    ///     Sensitive Action Failed,
    ///     Sensitive Action Succeed,     
    /// }
    /// To Do: 
    ///     1, To save log into DB (separate Database, JSON format).
    ///     2, To add new log catetory : All Admin Action.
    /// </summary>

    [Authorize]
    public class MoveShareController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<MoveShareController> _logger;
        public MoveShareController(ApplicationDbContext db, IAuthorizationService authorizationService, ILogger<MoveShareController> logger)
        {
            _db = db;
            _authorizationService = authorizationService;
            _logger = logger;
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            try {
                IEnumerable<Item> available_item_list = _db.Item.Where(i => i.Available == true);
                return View(available_item_list);
                
            }
            catch (Exception ex) {
                _logger.LogInformation("Exception :" + ex);

            }
            return RedirectToAction("Index", "Home");
        }

        //Create function
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [Authorize(Roles = "Administrator,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Item obj)
        {

            /*
            //ModelState validation method report error, as it apply to associate Model's full property.
            //No elegant solution found yet for partial evaluation, besides create a mock ViewModel. Leave this this moment.
            
              [Bind(Include = "Description,Quantity,PickUpNote,Location")]

              if (!ModelState.IsValid)
              {
                  var errors = ModelState.Values.SelectMany(v => v.Errors);
                  return View(obj);
              }
           */

            obj.Item_id = Guid.NewGuid().ToString();
            obj.Owner_id = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            obj.Created_at = DateTime.Now;
            obj.LastUpdate_at = obj.Created_at;
            obj.Available = true;

            try
            {
                ModelState.Clear();
                if (!TryValidateModel(obj))
                {
                    //Debug: var errors = ModelState.Values.SelectMany(v => v.Errors);
                    _logger.LogInformation("Sensitive Action Failed : Create() : by user " + User.Identity + " : at " + obj.Created_at + " : issue - input validation failed " );
                    TempData["Fail"] = "Pls double check input";
                    return View();
                }

                _db.Item.Add(obj);
                await _db.SaveChangesAsync();
                
                _logger.LogInformation("Sensitive Action Succeed : Create() : by user " + User.Identity + " : at " + obj.Created_at);
                TempData["Success"] = "Item created successfully!";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogInformation("DB exceptoin : Create() : " + ex);
                TempData["Fail"] = "Failed!";
                return RedirectToAction("Index");
            }

        }


        [HttpGet]
        public IActionResult Edit(string id)
        {
            //consider security text: scenario MITM attacker with proxy
            if (id == null) {
                return NotFound();
            }

            var findItem = _db.Item.Find(id);

            if (findItem == null) {
                return NotFound();
            }
            return View(findItem);
        }

  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(Item obj)
        {
            /*
            //ModelState validation method report error, as it apply to associate Model's full property.
            //No elegant solution found yet for partial evaluation, besides create a mock ViewModel. Leave this this moment.

              [Bind(Include = "Description,Quantity,PickUpNote,Location")]

              if (!ModelState.IsValid)
              {
                  var errors = ModelState.Values.SelectMany(v => v.Errors);
                  return View(obj);
              }
           */

            if (obj.Item_id == null)
            {
                return NotFound();
            }

            Item originalItem = _db.Item.Find(obj.Item_id);
            
            if (originalItem == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, originalItem, MoveShareItemOperation.Edit);
            
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            ModelState.Clear(); //important.tricky. Otherwise, following TryUpdateModelAsync wouldn't execute.(as modelstate evaluation would fail, due to we only bind to partial of the Item model)
            if (await TryUpdateModelAsync<Item>(originalItem, "", i => i.Name, i => i.Description, i => i.Quantity, i => i.PickUpNote, i => i.Location))
            {
                originalItem.LastUpdate_at = DateTime.Now;

                try
                {
                    await _db.SaveChangesAsync();
                }

                catch (DbUpdateException ex)
                {
                    _logger.LogInformation("DB exceptoin : EditPost() : " + ex);
                    return RedirectToAction("Index");
                }

                _logger.LogInformation("Sensitive Action Succeed : EditPost() : by user " + User.Identity + " : update at " + originalItem.LastUpdate_at);
                TempData["Success"] = "Item edited successfully!";
                return RedirectToAction("Index");
            }

            _logger.LogInformation("Sensitive Action Failed : EditPost() : by user " + User.Identity + " : at " + obj.Created_at + " : possible issue - input validation failed ");
            TempData["Fail"] = "Failed!";
            return RedirectToAction("Index");

        }


        [HttpGet]
        public IActionResult Delete(string id)
        {
            // to perform security test: fuzz, no int
            if (id == null)
            {
                return NotFound();
            }

            Item finditem = _db.Item.Find(id);

            if (finditem == null)
            {
                return NotFound();
            }

            return View(finditem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItem(string Item_id)
        {


            //add logic: to handle not yet completed order, associated to the to be deleted item.

            if (Item_id == null)
            {
                return NotFound();
            }

            Item originalItem = _db.Item.Find(Item_id);

            if (originalItem == null)
            {
                return NotFound();
            }

           
            var isAuthorized = await _authorizationService.AuthorizeAsync(this.User, originalItem, MoveShareItemOperation.Delete);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            originalItem.Available = false;
            originalItem.LastUpdate_at = DateTime.Now;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {
                _logger.LogInformation("DB exceptoin : DeleteItem() : " + ex);
                return RedirectToAction("Index");
            }

            _logger.LogInformation("Sensitive Action Succeed : DeleteItem() : by user " + User.Identity + " : update at " + originalItem.LastUpdate_at);
            TempData["Success"] = "Item deleted successfully!";
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Order(string id)
        {
            return View();

        }


        //Order & Picture upload, relevant logic to be added

        /*
        [HttpPost]
        public IActionResult OrderItem(string id)
        {
            //Add logic to specify order quantity.

            var new_order = new Order();
            new_order.Order_id = Guid.NewGuid().ToString();
            new_order.Item_id = id;

            //new_order.Order_quantity =
            new_order.Created_at = DateTime.Now;
            new_order.LastUpdate_at = new_order.Created_at;
            new_order.Status = "Created";
            new_order.Customer_id = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            _db.Order.Add(new_order);
            _db.SaveChanges();
            TempData["Success"] = "Item Ordered successfully!";
            return RedirectToAction("Index");
        }
        */

    }
}

