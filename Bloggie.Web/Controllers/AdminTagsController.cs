using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tr;

        public AdminTagsController(ITagRepository tr)
        {
            this.tr = tr;
        }

        
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest atr)
        {
            Tag t = new Tag
            {
                Name = atr.Name,
                DisplayName = atr.DisplayName
            };

            await tr.AddAsync(t);

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            //use the DbContext to get all tags
            var tags = await tr.GetAllAsync();

            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {

            Tag? tag = await tr.GetAsync(id);

            if (tag != null)
            {
                EditTagRequest etr = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(etr);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest etr)
        {
            Tag tag = new Tag
            {
                Id = etr.Id,
                Name = etr.Name,
                DisplayName = etr.DisplayName
            };

            var updatedTag = await tr.UpdateAsync(tag);

            if (updatedTag != null)
            {
                // TODO - Show Success notification
            }
            else
            {
                // TODO - Show Failure notification
            }

            return RedirectToAction("Edit", new { id = etr.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest etr)
        {

            Tag? deletedTag = await tr.DeleteAsync(etr.Id);
            if (deletedTag != null)
            {
                // TODO - Show Success notification
                return RedirectToAction("List");

            }
            // TODO - Show Failure notification
            return RedirectToAction("Edit", new { id=etr.Id });
        }
    }
}
