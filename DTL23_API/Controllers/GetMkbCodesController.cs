using DTL23_API.DataClasses;
using DTL23_API.DataHandlers;
using Microsoft.AspNetCore.Mvc;

namespace DTL23_API.Controllers {
	/// <summary>
	/// Получение справочника МКБ-кодов
	/// </summary>
	[Route("/[controller]")]
	[ApiController]
	public class GetMkbCodesController : ControllerBase {
		/// <summary>
		/// Получение справочника МКБ-кодов из ДБ
		/// </summary>
		/// <returns>Список объектов с описанием МКБ-кодов</returns>
		[HttpGet]
		public async Task<ActionResult<List<MkbCode>>> Index() {
			List<MkbCode> list = null;
			await Task.Run(() => { list = GetListOfMkbCodes.GetList(); });
			return list;
		}
	}
}
