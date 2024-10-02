using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuctionHouse.Data;
using AuctionHouse.Models;
using AuctionHouse.Data.Services;
using Microsoft.AspNetCore.Hosting;

namespace AuctionHouse.Controllers
{
    public class ListingsController : Controller
    {
        private readonly IListingsService _listingsService;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public ListingsController(IListingsService listingsService, IWebHostEnvironment webHostEnvironment)
        {
            _listingsService = listingsService;
            _webHostEnviroment = webHostEnvironment;
        }

        // GET: Listings with Pagination
        public async Task<IActionResult> Index(int? pageNumber)
        {
            int pageSize = 2; // Liczba elementów na stronie
            var listings = _listingsService.GetAll();

            // Tworzymy stronicowaną listę
            return View(await PaginatedList<Listing>.CreateAsync(listings, pageNumber ?? 1, pageSize));
        }

        // GET: Listings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await _listingsService.GetById(id);

            if (listing == null)
            {
                return NotFound();
            }

            return View(listing);
        }

        // GET: Listings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Listings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ListingVM listing)
        {
            if (ModelState.IsValid)
            {
                if (listing.Image != null)
                {
                    //string uploadDir = Path.Combine(_webHostEnviroment.WebRootPath, "Images");
                    string fileName = listing.Image.FileName;
                    //string filePath = Path.Combine(uploadDir, fileName);

                    using (var fileStream = new FileStream(fileName, FileMode.Create))
                    {
                        listing.Image.CopyTo(fileStream);
                    }

                    var listObj = new Listing
                    {
                        Title = listing.Title,
                        Description = listing.Description,
                        Price = listing.Price,
                        IdentityUserId = listing.IdentityUserId,
                        ImgPath = fileName,
                    };

                    await _listingsService.Add(listObj);
                    return RedirectToAction("Index");
                }
            }

            return View(listing);
        }
    }
}
