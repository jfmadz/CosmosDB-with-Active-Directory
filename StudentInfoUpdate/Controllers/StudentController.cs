using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Threading.Tasks;
using StudentInfoUpdate.Models;
//using PagedList;
//using PagedList.Mvc;

namespace StudentInfoUpdate.Controllers
{

    [System.Web.Mvc.Authorize]

    public class StudentController : Controller
    {
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync(string searchString/*,int? page*/)
        {  
            var items = await DocumentDBRepository<Student>.GetItemsAsync(d => !d.Completed);
            if ((!string.IsNullOrEmpty(searchString)))
            {
                items = items.Where(s => s.Name.Contains(searchString) || s.Surname.Contains(searchString) || s.Student_No.Contains(searchString));
            }
            //{
            //    var blob = new StudentInfoUpdate.Blob();
            //}
            return View(items.ToList()/*.ToPagedList(page??1,3)*/);
        }

        [ActionName("Create")]
        public async Task<ActionResult>CreateAsync()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Student_No,Name,Surname,Email,Address,Mobile_No,Completed,ImageURri,ThumbnailUri,Caption")]Student item, HttpPostedFileBase uploadFile)
        {
            if (ModelState.IsValid)
            {
                foreach (string file in Request.Files)
                {
                    uploadFile = Request.Files[file];
                }
                // Container Name - picture
                BlobManager BlobManagerObj = new BlobManager("picture");
                string FileAbsoluteUri = BlobManagerObj.UploadFile(uploadFile);

               


                {
                    await DocumentDBRepository<Student>.CreateItemAsync(item);
                    return RedirectToAction("Index");
                }
            }
                
            
                 return View(item);
            
        }

        public ActionResult Get()
        {
            // Container Name - picture
            BlobManager BlobManagerObj = new BlobManager("picture");
            List<string> fileList = BlobManagerObj.BlobList();

            return View(fileList);
        }



        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Student_No,Name,Surname,Email,Address,Mobile_No,Completed,FileUpload")] Student item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<Student>.UpdateItemAsync(item.Id, item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Student item = await DocumentDBRepository<Student>.GetItemAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }





        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(string id, string category)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Student item = await DocumentDBRepository<Student>.GetItemAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind(Include = "Id,Student_No,Name,Surname,Email,Address,Mobile_No,Completed")] string id, string category, string uri)
        {
            // Container Name - picture
            BlobManager BlobManagerObj = new BlobManager("picture");
            BlobManagerObj.DeleteBlob(uri);

            await DocumentDBRepository<Student>.DeleteItemAsync(id, category);
            return RedirectToAction("Index");
        }



        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id, string category)
        {
            Student item = await DocumentDBRepository<Student>.GetItemAsync(id);
            return View(item);
        }
    }
}