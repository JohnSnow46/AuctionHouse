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
using System.Security.Cryptography;
using System.Security.Claims;

namespace AuctionHouse.Controllers
{
    public class ListingsController : Controller
    {
        private readonly IListingsService _listingsService;
        private readonly IBidService _bidService;
        private readonly ICommentService _commentService;
        private readonly IWebHostEnvironment _webHostEnviroment;

        private static readonly int PageSize = 5;

        public ListingsController(IListingsService listingsService, IBidService bidService, IWebHostEnvironment webHostEnvironment, ICommentService commentService)
        {
            _listingsService = listingsService;
            _bidService = bidService;
            _webHostEnviroment = webHostEnvironment;
            _commentService = commentService;
        }

        public async Task<IActionResult> Index(int? pageNumber, string searchString)
        {
            var listings = _listingsService.GetAll();

            if (!string.IsNullOrEmpty(searchString))
            {
                listings = listings.Where(a => a.Title.Contains(searchString));
            }

            return View(await PaginatedList<Listing>.CreateAsync(listings.Where(l => l.IsSold == false), pageNumber ?? 1, PageSize));
        }

        public async Task<IActionResult> MyListings(int? pageNumber)
        {
            var listings = _listingsService.GetAll();

            return View("Index", await PaginatedList<Listing>.CreateAsync(listings.Where(l => l.IdentityUserId == User.FindFirstValue(ClaimTypes.NameIdentifier)), pageNumber ?? 1, PageSize));
        }

        public async Task<IActionResult> MyBids(int? pageNumber)
        {
            var listings = _bidService.GetAll();

            return View(await PaginatedList<Bid>.CreateAsync(listings.Where(l => l.IdentityUserId == User.FindFirstValue(ClaimTypes.NameIdentifier)), pageNumber ?? 1, PageSize));
        }

        [HttpPost]
        public async Task<IActionResult> AddBid([Bind("Id, Price, ListingId, IdentityUserId")] Bid bid)
        {
            if (ModelState.IsValid)
            {
                await _bidService.Add(bid);
            }
            var listing = await _listingsService.GetById(bid.ListingId);
            listing.Price = bid.Price;
            await _listingsService.SaveChanges();

            return View("Details", listing);
        }

        public async Task<IActionResult> CloseBid(int id)
        {
            var listing = await _listingsService.GetById(id);
            listing.IsSold = true;
            await _listingsService.SaveChanges();
            return View("Details", listing);
        }

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

        [HttpPost]
        public async Task<IActionResult> AddComment([Bind("Id, Content, ListingId, IdentityUserId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                await _commentService.AddComment(comment);

                return RedirectToAction("Details", "Listings", new { id = comment.ListingId });
            }

            var listing = await _listingsService.GetById(comment.ListingId);
            return View("Details", listing);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ListingVM listing)
        {
            if (ModelState.IsValid)
            {
                if (listing.Image != null && listing.Image.Length > 0)
                {
                    string uploadDir = Path.Combine(_webHostEnviroment.WebRootPath, "images");

                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(listing.Image.FileName);
                    string filePath = Path.Combine(uploadDir, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await listing.Image.CopyToAsync(fileStream);
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
