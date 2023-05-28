using System.Net;
using DTL23_API.Attributes;
using DTL23_API.Utilities;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using DTL23_API.DataClasses;
using DTL23_API.DataHandlers;

namespace DTL23_API.Controllers
{
    /// <summary>
    /// Загрузка данных в БД
    /// </summary>
    [Route("/[controller]")]
    [ApiController]
    public class UploadFileController : Controller {
		private readonly long _fileSizeLimit = 30000000;
		private readonly string[] _permittedExtensions = { ".xls", ".xlsx" };
        private readonly string _targetFilePath = Logging.assemblyDirectory + "Uploaded\\";

		// Get the default form options so that we can use them to set the default 
		// limits for request body data.
		private static readonly FormOptions _defaultFormOptions = new FormOptions();


		/// <summary>
		/// Загрузка в БД данных из файла *.xls, *.xlsx. Файл должен содержать столбцы 
        /// 'Пол пациента', 'Дата рождения пациента', 'ID пациента', 'Код МКБ-10', 
        /// 'Диагноз', 'Дата оказания услуги', 'Должность', 'Назначения'. 
        /// Размер файла не должен превышать 28МБ
		/// </summary>
		/// <returns>Список с результатами загрузки файлов в формате JSON</returns>
		[HttpPost]
        [DisableFormValueModelBinding]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<List<UploadFileResult>>> UploadPhysical() {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType)) {
                ModelState.AddModelError("File",
                    $"The request couldn't be processed (Error 1).");
                // Log error

                return BadRequest(ModelState);
            }

            var boundary = MultipartRequestHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(Request.ContentType),
                _defaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            var section = await reader.ReadNextSectionAsync();

            List<UploadFileResult> uploadFileResults = new List<UploadFileResult>();

			while (section != null) {
                var hasContentDispositionHeader =
                    ContentDispositionHeaderValue.TryParse(
                        section.ContentDisposition, out var contentDisposition);

                if (hasContentDispositionHeader) {
                    // This check assumes that there's a file
                    // present without form data. If form data
                    // is present, this method immediately fails
                    // and returns the model error.
                    if (!MultipartRequestHelper.HasFileContentDisposition(contentDisposition)) {
                        ModelState.AddModelError("File",
                            $"The request couldn't be processed (Error 2).");
                        // Log error

                        return BadRequest(ModelState);

                    } else {
                        // Don't trust the file name sent by the client. To display
                        // the file name, HTML-encode the value.
                        var trustedFileNameForDisplay = WebUtility.HtmlEncode(
                                contentDisposition.FileName.Value);
						var trustedFileNameForFileStorage = 
                            Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) +
                            Path.GetExtension(trustedFileNameForDisplay);

                        // **WARNING!**
                        // In the following example, the file is saved without
                        // scanning the file's contents. In most production
                        // scenarios, an anti-virus/anti-malware scanner API
                        // is used on the file before making the file available
                        // for download or for use by other systems. 
                        // For more information, see the topic that accompanies 
                        // this sample.

                        var streamedFileContent = await FileHelpers.ProcessStreamedFile(
                            section, contentDisposition, ModelState,
                            _permittedExtensions, _fileSizeLimit);

                        if (!ModelState.IsValid)
                            return BadRequest(ModelState);

                        if (!Directory.Exists(_targetFilePath))
                            Directory.CreateDirectory(_targetFilePath);

                        string filePath = Path.Combine(_targetFilePath, trustedFileNameForFileStorage);
                        using (var targetStream = System.IO.File.Create(filePath)) {
                            targetStream.Write(streamedFileContent);
                            targetStream.Close();
                        }

						string toLog = "Uploaded file '" + trustedFileNameForDisplay + "' saved to " +
							"'" + _targetFilePath + "' as " + trustedFileNameForFileStorage;

						Logging.ToLog(toLog);

						uploadFileResults.Add(UploadFileToDb.ParseAndUploadFileToDb(
                            trustedFileNameForFileStorage,
                            trustedFileNameForDisplay,
                            _targetFilePath));

                        System.IO.File.Delete(filePath);
					}
                }

                // Drain any remaining section body that hasn't been consumed and
                // read the headers for the next section.
                section = await reader.ReadNextSectionAsync();
            }

            return uploadFileResults;
        }
	}
}
