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
using Microsoft.IdentityModel.Tokens;

namespace AuctionHouse.Controllers
{
    public class ListingsController : Controller
    {
        private readonly IListingsService _listingsService;
        private readonly IBidService _bidService;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public ListingsController(IListingsService listingsService,IBidService bidService, IWebHostEnvironment webHostEnvironment)
        {
            _listingsService = listingsService;
            _bidService = bidService;
            _webHostEnviroment = webHostEnvironment;
        }

        // GET: Listings with Pagination
        public async Task<IActionResult> Index(int? pageNumber, string searchString)
        {
            int pageSize = 3; // elements numbers on page
            var listings = _listingsService.GetAll();

            if (!string.IsNullOrEmpty(searchString))
            {
                listings = listings.Where(a => a.Title.Contains(searchString));
            }

            // Tworzymy stronicowaną listę
            return View(await PaginatedList<Listing>.CreateAsync(listings, pageNumber ?? 1, pageSize));
        }

        [HttpPost]
        public async Task<IActionResult> AddBid([Bind("Id, Price, ListingId, Identityuser")] Bid bid)
        {
            if (ModelState.IsValid)
            {
                await _bidService.AddAsync(bid);
            }
            var listing = await _listingsService.GetById(bid.ListingId);
            listing.Price = bid.Price;
            await _listingsService.SaveChanges();

            return View("Details", listing);
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
