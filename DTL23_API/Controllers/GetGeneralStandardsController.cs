using Microsoft.AspNetCore.Mvc;
using DTL23_API.DataClasses;
using DTL23_API.DataHandlers;
using DTL23_API.Helpers;

namespace DTL23_API.Controllers {
	/// <summary>
	/// Получение данных по стандартам минздрава
	/// </summary>
	[Route("/[controller]")]
	[ApiController]
	public class GetGeneralStandardsController : ControllerBase {
		/// <summary>
		/// Получение данных по стандартам минздрава из БД
		/// </summary>
		/// <param name="mkbCode">Код МКБ-10 (опционально)</param>
		/// <returns>Список, содержащий объекты со стандартами минздрава в формате JSON</returns>
		/// <exception cref="ArgumentException">mkbCode должен соответствовать одному 
		/// из кодов справочника МКБ-10</exception>
		[HttpGet]
		public async Task<ActionResult<List<GeneralStandard>>> Index(string? mkbCode) {
			if (!string.IsNullOrEmpty(mkbCode) && !MKB10.IsMkb10StringCorrect(mkbCode))
				throw new ArgumentException("МКБ-10 код не соответствует формату");

			List<GeneralStandard> list = null;
			await Task.Run(() => { list = GetListOfGeneralStandards.GetList(mkbCode); });
			return list;
		}
	}
}
